using System;
using System.Collections.Generic;
using System.Linq;
using Bio;
using Bio.Algorithms.Alignment.MultipleSequenceAlignment;
using Bio.IO.FastA;
using Bio.SimilarityMatrices;
using NUnit.Framework;
using Bio.Tests;

namespace Bio.Pamsam.Tests
{
    /// <summary>
    /// Test for MuscleMultipleSequenceAlignment class
    /// </summary>
    [TestFixture]
    public class PAMSAMMultipleSequenceAlignmentTests
    {
        [Test]
        public void TestName()
        {
            var aligner = new PAMSAMMultipleSequenceAligner();
            Assert.AreEqual("PAMSAM (MUSCLE)", aligner.Name);
        }

        /// <summary>
        /// Test MuscleMultipleSequenceAlignment class
        /// </summary>
        [Test]
        public void TestMuscleMultipleSequenceAlignment()
        {

            var similarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.AmbiguousDna);
            var gapOpenPenalty = -4;
            var gapExtendPenalty = -1;
            var kmerLength = 3;

            ISequence seqA = new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT");
            ISequence seqB = new Sequence(Alphabets.DNA, "GGGAATCAAAATCAG");
            ISequence seqC = new Sequence(Alphabets.DNA, "GGGACAAAATCAG");
            var sequences = new List<ISequence>
            {
                seqA,
                seqB,
                seqC
            };

            var distanceFunctionName = DistanceFunctionTypes.EuclideanDistance;
            var hierarchicalClusteringMethodName = UpdateDistanceMethodsTypes.Average;
            var profileAlignerName = ProfileAlignerNames.NeedlemanWunschProfileAligner;
            var profileProfileFunctionName = ProfileScoreFunctionNames.WeightedInnerProduct;

            var msa = new PAMSAMMultipleSequenceAligner
                (sequences, kmerLength, distanceFunctionName, hierarchicalClusteringMethodName,
                profileAlignerName, profileProfileFunctionName, similarityMatrix, gapOpenPenalty, gapExtendPenalty,
                Environment.ProcessorCount * 2, Environment.ProcessorCount);

            Console.WriteLine("Aligned sequences in stage 1: {0}", msa.AlignmentScoreA);
            for (var i = 0; i < msa.AlignedSequencesA.Count; ++i)
            {
                Console.WriteLine(new string(msa.AlignedSequencesA[i].Select(a => (char)a).ToArray()));
            }
            
            Console.WriteLine("Aligned sequences in stage 3: {0}", msa.AlignmentScoreC);
            for (var i = 0; i < msa.AlignedSequencesC.Count; ++i)
            {
                Console.WriteLine(new string(msa.AlignedSequencesC[i].Select(a => (char)a).ToArray()));
            }
            Console.WriteLine("Aligned sequences final: {0}", msa.AlignmentScore);

            for (var i = 0; i < msa.AlignedSequences.Count; ++i)
            {
                Console.WriteLine(new string(msa.AlignedSequences[i].Select(a => (char)a).ToArray()));
            }

            // Test case 2
            Console.WriteLine("Example 2");
            sequences = new List<ISequence>
            {
                new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT"),
                new Sequence(Alphabets.DNA, "GGGAATCAAAATCAG"),
                new Sequence(Alphabets.DNA, "GGGAATCAAAATCAG"),
                new Sequence(Alphabets.DNA, "GGGAAATCG"),
                new Sequence(Alphabets.DNA, "GGGAATCAATCAG"),
                new Sequence(Alphabets.DNA, "GGGAATCTTATCAG"),
                new Sequence(Alphabets.DNA, "GGGACAAAATCAG"),
                new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT"),
                new Sequence(Alphabets.DNA, "GGGAATCAAAATCAG"),
                new Sequence(Alphabets.DNA, "GGGACAAAATCAG")
            };


            msa = new PAMSAMMultipleSequenceAligner
                (sequences, kmerLength, distanceFunctionName, hierarchicalClusteringMethodName,
                profileAlignerName, profileProfileFunctionName, similarityMatrix, gapOpenPenalty, gapExtendPenalty,
                Environment.ProcessorCount * 2, Environment.ProcessorCount);

            Console.WriteLine("Aligned sequences in stage 1: {0}", msa.AlignmentScoreA);
            for (var i = 0; i < msa.AlignedSequencesA.Count; ++i)
            {
                Console.WriteLine(new string(msa.AlignedSequencesA[i].Select(a => (char)a).ToArray()));
            }
            
            Console.WriteLine("Aligned sequences in stage 3: {0}", msa.AlignmentScoreC);
            for (var i = 0; i < msa.AlignedSequencesC.Count; ++i)
            {
                Console.WriteLine(new string(msa.AlignedSequencesC[i].Select(a => (char)a).ToArray()));
            }
            Console.WriteLine("Aligned sequences final: {0}", msa.AlignmentScore);
            for (var i = 0; i < msa.AlignedSequences.Count; ++i)
            {
                Console.WriteLine(new string(msa.AlignedSequences[i].Select(a => (char)a).ToArray()));
            }

            // Test case e
            Console.WriteLine("Example 2");
            sequences = new List<ISequence>
            {
                new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT"),
                new Sequence(Alphabets.DNA, "GGGAATCAAAATCAG"),
                new Sequence(Alphabets.DNA, "GGGAATCAAAATCAG"),
                new Sequence(Alphabets.DNA, "GGGAAATCG"),
                new Sequence(Alphabets.DNA, "GGGAATCAATCAG"),
                new Sequence(Alphabets.DNA, "GGGACAAAATCAG"),
                new Sequence(Alphabets.DNA, "GGGAATCTTATCAG")
            };


            msa = new PAMSAMMultipleSequenceAligner
                (sequences, kmerLength, distanceFunctionName, hierarchicalClusteringMethodName,
                profileAlignerName, profileProfileFunctionName, similarityMatrix, gapOpenPenalty, gapExtendPenalty,
                Environment.ProcessorCount * 2, Environment.ProcessorCount);

            Console.WriteLine("Aligned sequences in stage 1: {0}", msa.AlignmentScoreA);
            for (var i = 0; i < msa.AlignedSequencesA.Count; ++i)
            {
                Console.WriteLine(new string(msa.AlignedSequencesA[i].Select(a => (char)a).ToArray()));
            }
            
            Console.WriteLine("Aligned sequences in stage 3: {0}", msa.AlignmentScoreC);
            for (var i = 0; i < msa.AlignedSequencesC.Count; ++i)
            {
                Console.WriteLine(new string(msa.AlignedSequencesC[i].Select(a => (char)a).ToArray()));
            }
            Console.WriteLine("Aligned sequences final: {0}", msa.AlignmentScore);
            for (var i = 0; i < msa.AlignedSequences.Count; ++i)
            {
                Console.WriteLine(new string(msa.AlignedSequences[i].Select(a => (char)a).ToArray()));
            }

        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void TestMuscleMultipleSequenceAlignmentRunningTime()
        {
            var filepath = @"TestUtils\Fasta\RunningTime\BOX246.xml.afa".TestDir();

            // Test on DNA benchmark dataset
            var parser = new FastAParser();

            IList<ISequence> orgSequences = parser.Parse(filepath).ToList();

            var sequences = MsaUtils.UnAlign(orgSequences);

            //filepath = @"TestUtils\Fasta\RunningTime\12_raw.afa";
            //List<ISequence> sequences = parser.Parse(filepath);

            var numberOfSequences = orgSequences.Count;

            Console.WriteLine("Original sequences are:");
            for (var i = 0; i < numberOfSequences; ++i)
            {
                Console.WriteLine(new string(sequences[i].Select(a => (char)a).ToArray()));
            }

            Console.WriteLine("Benchmark sequences are:");
            for (var i = 0; i < numberOfSequences; ++i)
            {
                Console.WriteLine(new string(orgSequences[i].Select(a => (char)a).ToArray()));
            }

            PAMSAMMultipleSequenceAligner.FasterVersion = true;
            PAMSAMMultipleSequenceAligner.UseWeights = false;
            PAMSAMMultipleSequenceAligner.UseStageB = false;
            PAMSAMMultipleSequenceAligner.NumberOfCores = 2;

            var gapOpenPenalty = -13;
            var gapExtendPenalty = -5;
            var kmerLength = 2;

            var numberOfDegrees = 2;//Environment.ProcessorCount;
            var numberOfPartitions = 16;// Environment.ProcessorCount * 2;


            var distanceFunctionName = DistanceFunctionTypes.EuclideanDistance;
            var hierarchicalClusteringMethodName = UpdateDistanceMethodsTypes.Average;
            var profileAlignerName = ProfileAlignerNames.NeedlemanWunschProfileAligner;
            var profileProfileFunctionName = ProfileScoreFunctionNames.InnerProductFast;

            var similarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.Blosum62);

            var msa = new PAMSAMMultipleSequenceAligner
               (sequences, kmerLength, distanceFunctionName, hierarchicalClusteringMethodName,
               profileAlignerName, profileProfileFunctionName, similarityMatrix, gapOpenPenalty, gapExtendPenalty,
               numberOfPartitions, numberOfDegrees);
            Console.WriteLine("The number of partitions is: {0}", numberOfPartitions);
            Console.WriteLine("The number of degrees is: {0}", numberOfDegrees);
            Console.WriteLine("Alignment score Q is: {0}", MsaUtils.CalculateAlignmentScoreQ(msa.AlignedSequences, orgSequences));
            Console.WriteLine("Alignment score TC is: {0}", MsaUtils.CalculateAlignmentScoreTC(msa.AlignedSequences, orgSequences));



            Console.WriteLine("Benchmark SPS score is: {0}", MsaUtils.MultipleAlignmentScoreFunction(orgSequences, similarityMatrix, gapOpenPenalty, gapExtendPenalty));
            Console.WriteLine("Aligned sequences in stage 1: {0}", msa.AlignmentScoreA);
            for (var i = 0; i < msa.AlignedSequencesA.Count; ++i)
            {
                Console.WriteLine(new string(msa.AlignedSequencesA[i].Select(a => (char)a).ToArray()));
            }
            Console.WriteLine("Alignment score Q is: {0}", MsaUtils.CalculateAlignmentScoreQ(msa.AlignedSequencesA, orgSequences));
            Console.WriteLine("Alignment score TC is: {0}", MsaUtils.CalculateAlignmentScoreTC(msa.AlignedSequencesA, orgSequences));
            Console.WriteLine("Aligned sequences in stage 2: {0}", msa.AlignmentScoreB);
            for (var i = 0; i < msa.AlignedSequencesB.Count; ++i)
            {
                Console.WriteLine(new string(msa.AlignedSequencesB[i].Select(a => (char)a).ToArray()));
            }
            Console.WriteLine("Alignment score Q is: {0}", MsaUtils.CalculateAlignmentScoreQ(msa.AlignedSequencesB, orgSequences));
            Console.WriteLine("Alignment score TC is: {0}", MsaUtils.CalculateAlignmentScoreTC(msa.AlignedSequencesB, orgSequences));
            Console.WriteLine("Aligned sequences in stage 3: {0}", msa.AlignmentScoreC);
            for (var i = 0; i < msa.AlignedSequencesC.Count; ++i)
            {
                Console.WriteLine(new string(msa.AlignedSequencesC[i].Select(a => (char)a).ToArray()));
            }
            Console.WriteLine("Alignment score Q is: {0}", MsaUtils.CalculateAlignmentScoreQ(msa.AlignedSequencesC, orgSequences));
            Console.WriteLine("Alignment score TC is: {0}", MsaUtils.CalculateAlignmentScoreTC(msa.AlignedSequencesC, orgSequences));
            Console.WriteLine("Aligned sequences final: {0}", msa.AlignmentScore);
            for (var i = 0; i < msa.AlignedSequences.Count; ++i)
            {
                Console.WriteLine(new string(msa.AlignedSequences[i].Select(a => (char)a).ToArray()));
            }
            Console.WriteLine("Alignment score Q is: {0}", MsaUtils.CalculateAlignmentScoreQ(msa.AlignedSequences, orgSequences));
            Console.WriteLine("Alignment score TC is: {0}", MsaUtils.CalculateAlignmentScoreTC(msa.AlignedSequences, orgSequences));
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void testBug()
        {
            ISequence seq1 = new Sequence(Alphabets.Protein, "MQEPQSELNIDPPLSQETFSELWNLLPENNVLSSELCPAVDELLLPESVVNWLDEDSDDAPRMPATSAP");
            ISequence seq2 = new Sequence(Alphabets.Protein, "PLSQETFSDLWNLLPENNLLSSELSAPVDDLLPYTDVATWLDECPNEAPQMPEPSAPAAPPPATPAPATSWPLSSFVPSQKTYPGNYGFRLGF");
            ISequence seq3 = new Sequence(Alphabets.Protein, "MEPSSETGMDPPLSQETFEDLWSLLPDPLQTVTCRLDNLSEFPDYPLAADMSVLQEGLMGNAVPTVTSCAPSTDDYAGKYGLQLDFQQNGTAKS");
            ISequence seq4 = new Sequence(Alphabets.Protein, "MEEPQSDPSVEPPLSQETFSDLWKLLPENNVLSPLPSQAMDDLMLSPDDIEQWFTEDPGPDEAPRMPEAAPRVAPAPAAPTPAAPAPAPSWPLS");
            ISequence seq5 = new Sequence(Alphabets.Protein, "MEESQAELGVEPPLSQETFSDLWKLLPENNLLSSELSPAVDDLLLSPEDVANWLDERPDEAPQMPEPPAPAAPTPAAPAPATSWPLSSFVPSQK");
            ISequence seq6 = new Sequence(Alphabets.Protein, "MTAMEESQSDISLELPLSQETFSGLWKLLPPEDILPSPHCMDDLLLPQDVEEFFEGPSEALRVSGAPAAQDPVTETPGPVAPAPATPWPLSSFVPSQKTYQGNYGFHLGFLQ");
            ISequence seq7 = new Sequence(Alphabets.Protein, "FRLGFLHSGTAKSVTWTYSPLLNKLFCQLAKTCPVQLWVSSPPPPNTCVRAMAIYKKSEFVTEVVRRCPHHERCSDSSDGLAPPQHLIRVEGNLRAKYLDDRNTFRHSVV");

            var sequences = new List<ISequence>
            {
                seq1,
                seq2,
                seq3,
                seq4,
                seq5,
                seq6,
                seq7
            };

            var msa = new PAMSAMMultipleSequenceAligner(sequences,
                2, DistanceFunctionTypes.EuclideanDistance, 
                UpdateDistanceMethodsTypes.Average, 
                ProfileAlignerNames.NeedlemanWunschProfileAligner,
                ProfileScoreFunctionNames.WeightedEuclideanDistance,
                new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.Blosum50), 
                -8, -1, 2, 16);

            Assert.IsNotNull(msa.AlignedSequences);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void testBug2()
        {
            //Test on DNA benchmark dataset
            var filepath = @"TestUtils\122_raw.afa".TestDir();
            var parser = new FastAParser();

            IList<ISequence> orgSequences = parser.Parse(filepath).ToList();

            var sequences = MsaUtils.UnAlign(orgSequences);

            PAMSAMMultipleSequenceAligner.FasterVersion = false;
            PAMSAMMultipleSequenceAligner.UseWeights = false;
            PAMSAMMultipleSequenceAligner.UseStageB = false;
            PAMSAMMultipleSequenceAligner.NumberOfCores = 2;

            var gapOpenPenalty = -13;
            var gapExtendPenalty = -5;
            var kmerLength = 2;
            var numberOfDegrees = 2;//Environment.ProcessorCount;
            var numberOfPartitions = 16;// Environment.ProcessorCount * 2;

            var distanceFunctionName = DistanceFunctionTypes.EuclideanDistance;
            var hierarchicalClusteringMethodName = UpdateDistanceMethodsTypes.Average;
            var profileAlignerName = ProfileAlignerNames.NeedlemanWunschProfileAligner;
            var profileProfileFunctionName = ProfileScoreFunctionNames.InnerProductFast;

            var similarityMatrix =  new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.AmbiguousDna);

            var msa = new PAMSAMMultipleSequenceAligner
               (sequences, kmerLength, distanceFunctionName, hierarchicalClusteringMethodName,
               profileAlignerName, profileProfileFunctionName, similarityMatrix, gapOpenPenalty, gapExtendPenalty,
               numberOfPartitions, numberOfDegrees);

            Assert.IsNotNull(msa.AlignedSequences);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void testBug3()
        {
            //Test on DNA benchmark dataset
            var filepath = @"TestUtils\122_raw.afa".TestDir();
            var parser = new FastAParser();

            IList<ISequence> orgSequences = parser.Parse(filepath).ToList();

            var sequences = MsaUtils.UnAlign(orgSequences);

            PAMSAMMultipleSequenceAligner.FasterVersion = false;
            PAMSAMMultipleSequenceAligner.UseWeights = false;
            PAMSAMMultipleSequenceAligner.UseStageB = false;
            PAMSAMMultipleSequenceAligner.NumberOfCores = 2;

            var gapOpenPenalty = -13;
            var gapExtendPenalty = -5;
            var kmerLength = 2;

            var numberOfDegrees = 2;//Environment.ProcessorCount;
            var numberOfPartitions = 16;// Environment.ProcessorCount * 2;


            var distanceFunctionName = DistanceFunctionTypes.EuclideanDistance;
            var hierarchicalClusteringMethodName = UpdateDistanceMethodsTypes.Average;
            var profileAlignerName = ProfileAlignerNames.NeedlemanWunschProfileAligner;
            var profileProfileFunctionName = ProfileScoreFunctionNames.InnerProductFast;

            var similarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.AmbiguousDna);
                  
            //DateTime startTime = DateTime.Now;
            var msa = new PAMSAMMultipleSequenceAligner
               (sequences, kmerLength, distanceFunctionName, hierarchicalClusteringMethodName,
               profileAlignerName, profileProfileFunctionName, similarityMatrix, gapOpenPenalty, gapExtendPenalty,
               numberOfPartitions, numberOfDegrees);
            Assert.IsNotNull(msa.AlignedSequences);
        }
    }
}
