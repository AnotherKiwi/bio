using System;
using System.Collections.Generic;
using System.Linq;

using Bio.Algorithms.Alignment;
using Bio.Algorithms.Assembly;
using Bio.SimilarityMatrices;
using Bio.TestAutomation.Util;
using Bio.Util.Logging;

using NUnit.Framework;

namespace Bio.Tests.Algorithms.Assembly
{
    /// <summary>
    ///     Assembly Bvt Test case implementation.
    /// </summary>
    [TestFixture]
    public class AssemblyBvtTestCases
    {
        private readonly Utility utilityObj = new Utility(@"TestUtils\TestsConfig.xml");

        #region Sequence Assembly BVT Test cases

        /// <summary>
        ///     Validates if the Assemble() method assembles the
        ///     sequences on passing valid sequences as parameter..
        ///     Input: Sequences with Alphabets, matchscore, mismatch score,
        ///     gap cost, merge threshold, consensus threshold.
        ///     Validation: validates unmerged sequences count, contigs count,
        ///     contig sequences count and concensus.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void SequenceAssemblerWithAssembleMethod()
        {
            var assembly = GetSequenceAssembly("assemble");
            var contigConsensus = utilityObj.xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName,
                                                                     Constants.ContigConsensusNode);
            var contigSequencesCount = int.Parse(utilityObj.xmlUtil.GetTextValue(
                Constants.AssemblyAlgorithmNodeName,
                Constants.ContigSequencesCountNode), null);

            // Get the parameters from Xml for Assemble() method test cases.
            var unMergedCount = int.Parse(utilityObj.xmlUtil.GetTextValue(
                Constants.AssemblyAlgorithmNodeName,
                Constants.UnMergedSequencesCountNode), null);
            var contigsCount = int.Parse(utilityObj.xmlUtil.GetTextValue(
                Constants.AssemblyAlgorithmNodeName,
                Constants.ContigsCountNode), null);

            Assert.AreEqual(unMergedCount, assembly.UnmergedSequences.Count);
            Assert.AreEqual(contigsCount, assembly.Contigs.Count);
            var contigRead = assembly.Contigs[0];

            Assert.AreEqual(contigConsensus, new String(contigRead.Consensus.Select(a => (char) a).ToArray()));
            Assert.AreEqual(contigSequencesCount, contigRead.Sequences.Count);

            // Logs the concensus
            ApplicationLog.WriteLine(string.Format(null,
                                                   "SequenceAssembly BVT : Un Merged Sequences Count is '{0}'.",
                                                   assembly.UnmergedSequences.Count.ToString((IFormatProvider) null)));
            ApplicationLog.WriteLine(string.Format(null,
                                                   "SequenceAssembly BVT : Contigs Count is '{0}'.",
                                                   assembly.Contigs.Count.ToString((IFormatProvider) null)));
            ApplicationLog.WriteLine(string.Format(null,
                                                   "SequenceAssembly BVT : Contig Sequences Count is '{0}'.",
                                                   contigRead.Sequences.Count.ToString((IFormatProvider) null)));
            ApplicationLog.WriteLine(string.Format(null,
                                                   "SequenceAssembly BVT : Consensus read is '{0}'.",
                                                   contigRead.Consensus));
        }

        /// <summary>
        ///     Validate if the contig() method is retrieves a valid Contig
        ///     once a valid index is passed as parameter.
        ///     Input: Sequences with Alphabets, matchscore, mismatch score,
        ///     gap cost, merge threshold, consensus threshold.
        ///     Validation: Validates valid Contig is read.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void SequenceAssemblerWithContigMethod()
        {
            var assembly = GetSequenceAssembly("contig");
            var contigConsensus = utilityObj.xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName,
                                                                     Constants.ContigConsensusNode);
            var contigSequencesCount = int.Parse(utilityObj.xmlUtil.GetTextValue(
                Constants.AssemblyAlgorithmNodeName,
                Constants.ContigSequencesCountNode), null);

            // Read the contig from Contig method.
            var contigsRead = assembly.Contigs[0];

            // Log the required info.
            ApplicationLog.WriteLine(string.Format(null,
                                                   "SequenceAssembly BVT : Consensus read is '{0}'.",
                                                   contigsRead.Consensus));
            Assert.AreEqual(contigConsensus, new String(contigsRead.Consensus.Select(a => (char) a).ToArray()));
            Assert.AreEqual(contigSequencesCount, contigsRead.Sequences.Count);

            ApplicationLog.WriteLine("SequenceAssembly BVT : Successfully read the Contig.");
        }

        #endregion Sequence Assembly BVT Test cases

        #region Consensus BVT Test cases

        /// <summary>
        ///     Validate MakeConsensus() method.
        ///     Input: Sequences with Alphabets, matchscore, mismatch score,
        ///     gap cost, merge threshold, consensus threshold.
        ///     Validation: Validates valid Contig is read.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void SimpleConsensusWithMakeConsensusMethod()
        {
            var assembly = GetSequenceAssembly("consensus");

            var contigConsensus = utilityObj.xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName,
                                                                     Constants.ContigConsensusNode);
            var consensusThreshold = double.Parse(utilityObj.xmlUtil.GetTextValue(
                Constants.AssemblyAlgorithmNodeName,
                Constants.ConsensusThresholdNode), null);
            var alphabet = Utility.GetAlphabet(utilityObj.xmlUtil.GetTextValue(
                Constants.AssemblyAlgorithmNodeName,
                Constants.AlphabetNameNode));
            // Read the contig from Contig method.
            var contigReadForConsensus = assembly.Contigs[0];
            contigReadForConsensus.Consensus = null;
            var simpleSeqAssembler = new OverlapDeNovoAssembler();
            simpleSeqAssembler.ConsensusResolver = new SimpleConsensusResolver(consensusThreshold);
            simpleSeqAssembler.MakeConsensus(alphabet, contigReadForConsensus);

            Assert.AreEqual(contigConsensus,
                            new String(contigReadForConsensus.Consensus.Select(a => (char) a).ToArray()));

            // Log the required info.
            ApplicationLog.WriteLine(string.Format(null,
                                                   "SimpleConsensusMethod BVT : Consensus read is '{0}'.",
                                                   contigReadForConsensus.Consensus));
        }

        #endregion Consensus BVT Test cases

        #region Supported methods

        /// <summary>
        ///     Validate Sequence Assembler Test cases based on additional parameter values
        /// </summary>
        /// <param name="additionalParameter">Additional parameters</param>
        private IOverlapDeNovoAssembly GetSequenceAssembly(string additionalParameter)
        {
            // Get the parameters from Xml
            var matchScore = int.Parse(utilityObj.xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName,
                                                                       Constants.MatchScoreNode), null);
            var mismatchScore = int.Parse(utilityObj.xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName,
                                                                          Constants.MisMatchScoreNode), null);
            var gapCost = int.Parse(utilityObj.xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName,
                                                                    Constants.GapCostNode), null);
            var mergeThreshold = double.Parse(utilityObj.xmlUtil.GetTextValue(
                Constants.AssemblyAlgorithmNodeName,
                Constants.MergeThresholdNode), null);
            var consensusThreshold = double.Parse(utilityObj.xmlUtil.GetTextValue(
                Constants.AssemblyAlgorithmNodeName,
                Constants.ConsensusThresholdNode), null);
            var sequence1 = utilityObj.xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName,
                                                               Constants.SequenceNode1);
            var sequence2 = utilityObj.xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName,
                                                               Constants.SequenceNode2);
            var sequence3 = utilityObj.xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName,
                                                               Constants.SequenceNode3);
            var alphabet = Utility.GetAlphabet(utilityObj.xmlUtil.GetTextValue(
                Constants.AssemblyAlgorithmNodeName,
                Constants.AlphabetNameNode));

            // Log based on the test cases
            switch (additionalParameter)
            {
                case "consensus":
                    // Logs the sequences
                    ApplicationLog.WriteLine(string.Format(null,
                                                           "SimpleConsensusMethod BVT : Sequence 1 used is '{0}'.",
                                                           sequence1));
                    ApplicationLog.WriteLine(string.Format(null,
                                                           "SimpleConsensusMethod BVT : Sequence 2 used is '{0}'.",
                                                           sequence2));
                    ApplicationLog.WriteLine(string.Format(null,
                                                           "SimpleConsensusMethod BVT : Sequence 3 used is '{0}'.",
                                                           sequence3));
                    break;
                default:
                    // Logs the sequences
                    ApplicationLog.WriteLine(string.Format(null,
                                                           "SequenceAssembly BVT : Sequence 1 used is '{0}'.", sequence1));
                    ApplicationLog.WriteLine(string.Format(null,
                                                           "SequenceAssembly BVT : Sequence 2 used is '{0}'.", sequence2));
                    ApplicationLog.WriteLine(string.Format(null,
                                                           "SequenceAssembly BVT : Sequence 3 used is '{0}'.", sequence3));
                    break;
            }

            var seq1 = new Sequence(alphabet, sequence1);
            var seq2 = new Sequence(alphabet, sequence2);
            var seq3 = new Sequence(alphabet, sequence3);

            // here is how the above sequences should align:
            // TATAAAGCGCCAA
            //         GCCAAAATTTAGGC
            //                   AGGCACCCGCGGTATT   <= reversed
            // 
            // TATAAAGCGCCAAAATTTAGGCACCCGCGGTATT

            var assembler = new OverlapDeNovoAssembler();
            assembler.MergeThreshold = mergeThreshold;
            assembler.OverlapAlgorithm = new PairwiseOverlapAligner();
            (assembler.OverlapAlgorithm).SimilarityMatrix =
                new DiagonalSimilarityMatrix(matchScore, mismatchScore);
            (assembler.OverlapAlgorithm).GapOpenCost = gapCost;
            assembler.ConsensusResolver = new SimpleConsensusResolver(consensusThreshold);
            assembler.AssumeStandardOrientation = false;

            var inputs = new List<ISequence>
            {
                seq1,
                seq2,
                seq3
            };

            // Assembles all the sequences.
            return (IOverlapDeNovoAssembly) assembler.Assemble(inputs);
        }

        #endregion Supported methods
    }
}