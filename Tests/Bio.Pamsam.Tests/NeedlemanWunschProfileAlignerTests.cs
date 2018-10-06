using System;
using System.Collections.Generic;
using System.Linq;
using Bio;
using Bio.Algorithms.Alignment.MultipleSequenceAlignment;
using Bio.IO.FastA;
using Bio.SimilarityMatrices;
using System.IO;
using NUnit.Framework;
using Bio.Tests;

namespace Bio.Pamsam.Tests
{
    /// <summary>
    /// Test for NeedlemanWunschProfileAligner class
    /// </summary>
    [TestFixture]
    public class NeedlemanWunschProfileAlignerTests
    {

        /// <summary>
        /// Test NeedlemanWunschProfileAligner class
        /// </summary>
        [Test]
        public void TestNeedlemanWunschProfileAligner()
        {
            ISequence templateSequence = new Sequence(Alphabets.AmbiguousDNA, "ATGCSWRYKMBVHDN-");
            var itemSet = new Dictionary<byte, int>();
            for (var i = 0; i < templateSequence.Count; ++i)
            {
                itemSet.Add(templateSequence[i], i);
                if (char.IsLetter((char)templateSequence[i]))
                    itemSet.Add((byte)char.ToLower((char)templateSequence[i]), i);
            }
            Profiles.ItemSet = itemSet;

            var similarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.AmbiguousDna);
            var gapOpenPenalty = -3;
            var gapExtendPenalty = -1;

            IProfileAligner profileAligner = new NeedlemanWunschProfileAlignerSerial(similarityMatrix, ProfileScoreFunctionNames.WeightedInnerProduct,
                                                                                gapOpenPenalty, gapExtendPenalty, Environment.ProcessorCount);

            ISequence seqA = new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT");
            ISequence seqB = new Sequence(Alphabets.DNA, "GGGAATCAAAATCAG");

            var sequences = new List<ISequence>
            {
                seqA,
                seqB
            };

            var profileAlignmentA = ProfileAlignment.GenerateProfileAlignment(sequences[0]);
            var profileAlignmentB = ProfileAlignment.GenerateProfileAlignment(sequences[1]);
            profileAligner.Align(profileAlignmentA, profileAlignmentB);


            var eStringSubtree = profileAligner.GenerateEString(profileAligner.AlignedA);
            var eStringSubtreeB = profileAligner.GenerateEString(profileAligner.AlignedB);

            var alignedSequences = new List<ISequence>();

            ISequence seq = profileAligner.GenerateSequenceFromEString(eStringSubtree, sequences[0]);
            alignedSequences.Add(seq);
            seq = profileAligner.GenerateSequenceFromEString(eStringSubtreeB, sequences[1]);
            alignedSequences.Add(seq);

            var profileScore = MsaUtils.MultipleAlignmentScoreFunction(alignedSequences, similarityMatrix, gapOpenPenalty, gapExtendPenalty);

            Console.WriteLine("alignment score is: {0}", profileScore);

            Console.WriteLine("the aligned sequences are:");
            for (var i = 0; i < alignedSequences.Count; ++i)
                Console.WriteLine(alignedSequences[i]);

            // Test on case 3: 36 sequences
            var filepath = @"TestUtils\RV11_BBS_allSmall.afa";
            var filePathObj = filepath.TestDir();
            IList<ISequence> orgSequences = new FastAParser() { Alphabet = AmbiguousDnaAlphabet.Instance }.Parse(filePathObj).ToList();

            sequences = MsaUtils.UnAlign(orgSequences);
            var numberOfSequences = orgSequences.Count;

            Console.WriteLine("Original unaligned sequences are:");
            for (var i = 0; i < numberOfSequences; ++i)
            {
                Console.WriteLine(">");
                Console.WriteLine(sequences[i]);
            }

            for (var i = 1; i < numberOfSequences - 1; ++i)
            {
                for (var j = i + 1; j < numberOfSequences; ++j)
                {
                    profileAlignmentA = ProfileAlignment.GenerateProfileAlignment(sequences[i]);
                    profileAlignmentB = ProfileAlignment.GenerateProfileAlignment(sequences[j]);

                    profileAligner = new NeedlemanWunschProfileAlignerSerial(similarityMatrix, ProfileScoreFunctionNames.WeightedInnerProduct,
                                                                                gapOpenPenalty, gapExtendPenalty, Environment.ProcessorCount);
                    profileAligner.Align(profileAlignmentA, profileAlignmentB);

                    eStringSubtree = profileAligner.GenerateEString(profileAligner.AlignedA);
                    eStringSubtreeB = profileAligner.GenerateEString(profileAligner.AlignedB);

                    Console.WriteLine("Sequences lengths are: {0}-{1}", sequences[i].Count, sequences[j].Count);
                    Console.WriteLine("estring 1:");
                    for (var k = 0; k < eStringSubtree.Count; ++k)
                    {
                        Console.Write("{0}\t", eStringSubtree[k]);
                    }
                    Console.WriteLine("\nestring 2:");
                    for (var k = 0; k < eStringSubtreeB.Count; ++k)
                    {
                        Console.Write("{0}\t", eStringSubtreeB[k]);
                    }

                    alignedSequences = new List<ISequence>();

                    seq = profileAligner.GenerateSequenceFromEString(eStringSubtree, sequences[i]);
                    alignedSequences.Add(seq);
                    seq = profileAligner.GenerateSequenceFromEString(eStringSubtreeB, sequences[j]);
                    alignedSequences.Add(seq);

                    profileScore = MsaUtils.MultipleAlignmentScoreFunction(alignedSequences, similarityMatrix, gapOpenPenalty, gapExtendPenalty);

                    Console.WriteLine("\nalignment score is: {0}", profileScore);

                    Console.WriteLine("the aligned sequences are:");
                    for (var k = 0; k < alignedSequences.Count; ++k)
                    {
                        Console.WriteLine(new string(alignedSequences[k].Select(a => (char)a).ToArray()));
                    }
                }
            }
        }
    }
}
