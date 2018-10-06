using System;
using System.Collections.Generic;
using Bio.Extensions;
using NUnit.Framework;
using Bio.Util;
using System.Globalization;
using Bio.Algorithms.Alignment;
using Bio.Algorithms.SuffixTree;
using Bio.Algorithms.Assembly;
using Bio.SimilarityMatrices;

namespace Bio.Tests
{
    /// <summary>
    /// Summary description for TestToStringForBio
    /// </summary>
    [TestFixture]
    public class TestToStringForBio
    {
        /// <summary>
        /// Test for Sequence ConvertToString method.
        /// </summary>
        [Test]
        public void TestSequenceConvertToString()
        {
            ISequence seq = new Sequence(Alphabets.DNA, "ATCGATCGGATCGATCGGCTACTAATATCGATCGGCTACGATCGGCTAATCGATCGATCGGCTAATCGATCGATCGGCTAGCTA");
            const string expectedValue = "TCGATCGGCT";
            var actualValue = seq.ConvertToString(10, 10);
            Assert.AreEqual(expectedValue, actualValue);
        }

        /// <summary>
        /// Test for Sequence ConvertToString method.
        /// </summary>
        [Test]
        public void TestDerivedSequenceConvertToString()
        {
            var baseSeq = new Sequence(Alphabets.DNA, "ATCGATCGGATCGATCGGCTACTAATATCGATCGGCTACGATCGGCTAATCGATCGATCGGCTAATCGATCGATCGGCTAGCTA");
            var seq = new DerivedSequence(baseSeq, false, false);
            const string expectedValue = "TCGATCGGCT";
            var actualValue = seq.ConvertToString(10, 10);
            Assert.AreEqual(expectedValue, actualValue);
        }

        /// <summary>
        /// Test for Sequence ConvertToString method.
        /// </summary>
        [Test]
        public void TestSequenceConvertToStringWithException()
        {
            var seq = new Sequence(Alphabets.DNA, "ATCGATCGGATCGATCGGCTACTAATATCGATCGGCTACGATCGGCTAATCGATCGATCGGCTAATCGATCGATCGGCTAGCTA");
            try
            {
                seq.ConvertToString(80, 10);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.AreEqual("length", ex.ParamName);
            }
        }

        /// <summary>
        /// Test for Sequence ToString() Function.
        /// </summary>
        [Test]
        public void TestSequenceToString()
        {
            ISequence seqSmall = new Sequence(Alphabets.DNA, "ATCG");
            ISequence seqLarge = new Sequence(Alphabets.DNA, "ATCGGGGGGGGGGGGGGGGGGGGGGGGCCCCCCCCCCCCCCCCCCCCCCGGGGGGGGGGTTTTTTTTTTTTTTT");
            var ActualSmallString = seqSmall.ToString();
            var ActualLargeString = seqLarge.ToString();
            var ExpectedSmallString = "ATCG";
            var expectedLargeString = string.Format(CultureInfo.CurrentCulture, "ATCGGGGGGGGGGGGGGGGGGGGGGGGCCCCCCCCCCCCCCCCCCCCCCGGGGGGGGGGTTTTT... +[{0}]", (seqLarge.Count - Helper.AlphabetsToShowInToString));

            Assert.AreEqual(ExpectedSmallString, ActualSmallString);
            Assert.AreEqual(expectedLargeString, ActualLargeString);
        }

        /// <summary>
        /// Test for DeriveSequence ToString() Function.
        /// </summary>
        [Test]
        public void TestDerivedSequenceToString()
        {
            ISequence seqSmall = new Sequence(Alphabets.DNA, "ATCG");
            ISequence seqLarge = new Sequence(Alphabets.DNA, "ATCGGGGGGGGGGGGGGGGGGGGGGGGCCCCCCCCCCCCCCCCCCCCCCGGGGGGGGGGTTTTTTTTTTTTTTT");
            ISequence DeriveSeqSmall = new DerivedSequence(seqSmall, false, true);
            ISequence DeriveSeqLarge = new DerivedSequence(seqLarge, false, true);

            var ActualSmallString = DeriveSeqSmall.ToString();
            var ActualLargeString = DeriveSeqLarge.ToString();
            var ExpectedSmallString = "TAGC";
            var expectedLargeString = string.Format(CultureInfo.CurrentCulture, "TAGCCCCCCCCCCCCCCCCCCCCCCCCGGGGGGGGGGGGGGGGGGGGGGCCCCCCCCCCAAAAA... +[{0}]", (seqLarge.Count - Helper.AlphabetsToShowInToString));

            Assert.AreEqual(ExpectedSmallString, ActualSmallString);
            Assert.AreEqual(expectedLargeString, ActualLargeString);
        }

        /// <summary>
        /// Test for QualitativeSequence ToString() Function.
        /// </summary>
        [Test]
        public void TestQualitativeSequenceToString()
        {
            var seqData = new byte[4];
            seqData[0] = (byte)'A';
            seqData[1] = (byte)'T';
            seqData[2] = (byte)'C';
            seqData[3] = (byte)'G';
            var qualityScores = new byte[4];
            qualityScores[0] = (byte)'A';
            qualityScores[1] = (byte)'A';
            qualityScores[2] = (byte)'A';
            qualityScores[3] = (byte)'B';
            var seq = new QualitativeSequence(Alphabets.DNA, FastQFormatType.Illumina_v1_3, seqData, qualityScores);
            var actualString = seq.ToString().Replace("\r\n", "\n");
            var expectedString = "ATCG\nAAAB";
            Assert.AreEqual(actualString, expectedString);
        }

        /// <summary>
        /// Test for All Alphabets ToString() Function.
        /// </summary>
        [Test]
        public void TestAllAlphabetsToString()
        {
            var dna = DnaAlphabet.Instance;
            var rna = RnaAlphabet.Instance;
            var protein = ProteinAlphabet.Instance;
            var dnaAmbiguous = AmbiguousDnaAlphabet.Instance;
            var rnaAmbiguous = AmbiguousRnaAlphabet.Instance;
            var proteinAmbiguous = AmbiguousProteinAlphabet.Instance;

            var dnaStringActual = dna.ToString();
            var rnaStringActual = rna.ToString();
            var proteinStringActual = protein.ToString();
            var dnaAmbiguousStringActual = dnaAmbiguous.ToString();
            var rnaAmbiguousStringActual = rnaAmbiguous.ToString();
            var proteinAmbiguousStringActual = proteinAmbiguous.ToString();

            var dnaStringExpected = "ACGT-";
            var rnaStringExpected = "ACGU-";
            var proteinStringExpected = "ACDEFGHIKLMNOPQRSTUVWY-*";
            var dnaAmbiguousStringExpected = "ACGT-MRSWYKVHDBN";
            var rnaAmbiguousStringExpected = "ACGU-NMRSWYKVHDB";
            var proteinAmbiguousStringExpected = "ACDEFGHIKLMNOPQRSTUVWY-*XZBJ";

            Assert.AreEqual(dnaStringExpected, dnaStringActual);
            Assert.AreEqual(rnaStringExpected, rnaStringActual);
            Assert.AreEqual(proteinStringExpected, proteinStringActual);
            Assert.AreEqual(dnaAmbiguousStringExpected, dnaAmbiguousStringActual);
            Assert.AreEqual(rnaAmbiguousStringExpected, rnaAmbiguousStringActual);
            Assert.AreEqual(proteinAmbiguousStringExpected, proteinAmbiguousStringActual);
        }

        /// <summary>
        /// Test for SequenceRange ToString() Function.
        /// </summary>
        [Test]
        public void TestSequenceRangeToString()
        {
            var range = new SequenceRange("chr20", 0, 3);
            var actualString = range.ToString();
            var expectedString = "ID=chr20 Start=0 End=3";
            Assert.AreEqual(actualString, expectedString);
        }

        /// <summary>
        /// Test for SequenceRangeGrouping ToString() Function.
        /// </summary>
        [Test]
        public void TestSequenceRangeGroupingToString()
        {
            ISequenceRange range1 = new SequenceRange("chr20", 0, 3);
            ISequenceRange range2 = new SequenceRange("chr21", 0, 4);
            IList<ISequenceRange> ranges = new List<ISequenceRange>
            {
                range1,
                range2
            };

            var rangegrouping = new SequenceRangeGrouping(ranges);
            var actualString = rangegrouping.ToString();

            var expectedString = "ID=chr20 Start=0 End=3\r\nID=chr21 Start=0 End=4\r\n".Replace("\r\n", Environment.NewLine);
            Assert.AreEqual(actualString, expectedString);
        }

         /// <summary>
        /// Test for SequenceStatistics ToString() Function.
        /// </summary>
        [Test]
        public void TestSequenceStatisticsToString()
        {
            ISequence seq = new Sequence(Alphabets.DNA, "ATCGATCG");
            var seqStats = new SequenceStatistics(seq);
            var actualString = seqStats.ToString();
            var expectedString = "A - 2\r\nC - 2\r\nG - 2\r\nT - 2\r\n".Replace("\r\n", Environment.NewLine);
            Assert.AreEqual(actualString, expectedString);
        }

        /// <summary>
        /// Test for AlignedSequence ToString() Function.
        /// </summary>
        [Test]
        public void TestAlignedSequenceToString()
        {
            var seqList = new List<ISequence>
            {
                new Sequence(Alphabets.DNA,"CAAAAGGGATTGC---TGTTGGAGTGAATGCCATTACCTACCGGCTAGGAGGAGTAGTACAAAGGAGC"),
                new Sequence(Alphabets.DNA, "CAAAAGGGATTGC---"),
                new Sequence(Alphabets.DNA, "TAGTAGTTCTGCTATATACATTTG"),
                new Sequence(Alphabets.DNA, "GTTATCATGCGAACAATTCAACAGACACTGTAGA")
            };

            var num = new NucmerPairwiseAligner
            {
                BreakLength = 8,
                FixedSeparation = 0,
                MinimumScore = 0,
                MaximumSeparation = 0,
                SeparationFactor = 0,
                LengthOfMUM = 8
            };

            IList<ISequence> sequenceList = seqList;
            var alignmentObj = num.Align(sequenceList);
            Assert.AreNotEqual(0, alignmentObj.Count);

            var alignedSeqs = (AlignedSequence) alignmentObj[0].AlignedSequences[0];

            var actualString = alignedSeqs.ToString();
            var ExpectedString = "CAAAAGGGATTGC---\r\nCAAAAGGGATTGC---\r\nCAAAAGGGATTGC---\r\n".Replace("\r\n", Environment.NewLine);
            Assert.AreEqual(ExpectedString, actualString);
        }

        /// <summary>
        /// Test for Cluster ToString() Function.
        /// </summary>
        [Test]
        public void TestClusterToString()
        {
            var match = new Match();

            var matchExtn1 = new MatchExtension(match);
            matchExtn1.ID = 1;
            matchExtn1.Length = 20;
            var matchExtn2 = new MatchExtension(match);
            matchExtn2.ID = 2;
            matchExtn2.Length = 30;
            IList<MatchExtension> extnList = new List<MatchExtension>
            {
                matchExtn1,
                matchExtn2
            };

            var clust = new Cluster(extnList);
            var actualString = clust.ToString();
            var expectedString = "RefStart=0 QueryStart=0 Length=20 Score=0 WrapScore=0 IsGood=False\r\nRefStart=0 QueryStart=0 Length=30 Score=0 WrapScore=0 IsGood=False\r\n".Replace ("\r\n", Environment.NewLine);
            Assert.AreEqual(actualString, expectedString);
        }

        /// <summary>
        /// Test for DeltaAlignment ToString() Function.
        /// </summary>
        [Test]
        public void TestDeltaAlignmentToString()
        {
            ISequence refSeq = new Sequence(Alphabets.DNA, "ATCGGGGGGGGAAAAAAATTTTCCCCGGGGG");
            ISequence qrySeq = new Sequence(Alphabets.DNA, "GGGGG");
            var delta = new DeltaAlignment(refSeq, qrySeq);
            delta.FirstSequenceEnd = 21;
            delta.SecondSequenceEnd = 20;
            var actualString = delta.ToString();
            var expectedString = "Ref ID= Query Id= Ref start=0 Ref End=21 Query start=0 Query End=20, Direction=FORWARD";
            Assert.AreEqual(actualString, expectedString);
        }

         /// <summary>
        /// Test for PairwiseAlignedSequence ToString() Function.
        /// </summary>
        [Test]
        public void TestPairwiseAlignedSequenceToString()
        {
            var alignedSeq = new PairwiseAlignedSequence();
            alignedSeq.FirstSequence = new Sequence(Alphabets.Protein, "AWGHE");
            alignedSeq.SecondSequence = new Sequence(Alphabets.Protein, "AW-HE");
            alignedSeq.Consensus = new Sequence(Alphabets.Protein, "AWGHE");
            alignedSeq.Score = 28;
            alignedSeq.FirstOffset = 0;
            alignedSeq.SecondOffset = 3;

            var actualString = alignedSeq.ToString();
            var expectedString = "AWGHE\r\nAWGHE\r\nAW-HE\r\n".Replace("\r\n", Environment.NewLine);
            Assert.AreEqual(actualString, expectedString);
        }

        /// <summary>
        /// Test for PairwiseSequenceAlignment ToString() Function.
        /// </summary>
        [Test]
        public void TestPairwiseSequenceAlignmentToString()
        {
            IPairwiseSequenceAlignment align = new PairwiseSequenceAlignment();
            var alignedSeq = new PairwiseAlignedSequence();
            alignedSeq.FirstSequence = new Sequence(Alphabets.Protein, "AWGHE");
            alignedSeq.SecondSequence = new Sequence(Alphabets.Protein, "AW-HE");
            alignedSeq.Consensus = new Sequence(Alphabets.Protein, "AWGHE");
            alignedSeq.Score = 28;
            alignedSeq.FirstOffset = 0;
            alignedSeq.SecondOffset = 3;
            align.PairwiseAlignedSequences.Add(alignedSeq);

            var actualString = align.ToString();
            var expectedString = "AWGHE\r\nAWGHE\r\nAW-HE\r\n\r\n".Replace("\r\n", Environment.NewLine);
            Assert.AreEqual(actualString, expectedString); 
        }

         /// <summary>
        /// Test for Match And MatchExtension ToString() Function.
        /// </summary>
        [Test]
        public void TestMatchAndMatchExtensionToString()
        {
            var match = new Match();
            match.Length = 20;
            match.QuerySequenceOffset = 33;

            var matchExtn = new MatchExtension(match);
            matchExtn.ID = 1;
            matchExtn.Length = 20;

            var actualMatchExtnString = matchExtn.ToString();
            var actualMatchstring = match.ToString();
            var ExpectedMatchExtnString = "RefStart=0 QueryStart=33 Length=20 Score=0 WrapScore=0 IsGood=False";
            var ExpectedMatchString = "RefStart=0 QueryStart=33 Length=20";

            Assert.AreEqual(ExpectedMatchExtnString, actualMatchExtnString);
            Assert.AreEqual(actualMatchstring, ExpectedMatchString);
        }

        /// <summary>
        /// Test for SequenceAlignment ToString() Function.
        /// </summary>
        [Test]
        public void TestSequenceAlignmentToString()
        {
            ISequenceAligner aligner = SequenceAligners.NeedlemanWunsch;
            IAlphabet alphabet = Alphabets.Protein;
            const string origSequence1 = "KRIPKSQNLRSIHSIFPFLEDKLSHLN";
            const string origSequence2 = "LNIPSLITLNKSIYVFSKRKKRLSGFLHN";

            // Create input sequences
            var inputSequences = new List<ISequence>
                {
                    new Sequence(alphabet, origSequence1),
                    new Sequence(alphabet, origSequence2)
                };

            // Get aligned sequences
            var alignments = aligner.Align(inputSequences);
            ISequenceAlignment alignment = new SequenceAlignment();
            foreach (var alignedSequence in alignments[0].AlignedSequences)
                alignment.AlignedSequences.Add(alignedSequence);

            const string expected = "XXIPXXXXLXXXXXXFXXXXXXLSXXLHN\r\n" +
                                    "KRIPKSQNLRSIHSIFPFLEDKLSHL--N\r\n" +
                                    "LNIPSLITLNKSIYVFSKRKKRLSGFLHN\r\n\r\n";
            Assert.AreEqual(expected.Replace("\r\n", Environment.NewLine), alignment.ToString());
        }

        /// <summary>
        /// Test for Contig ToString() Function.
        /// </summary>
        [Test]
        public void TestContigToString()
        {
            // test parameters
            const int matchScore = 5;
            const int mismatchScore = -4;
            const int gapCost = -10;
            const double mergeThreshold = 4;
            const double consensusThreshold = 66;

            var seq2 = new Sequence(Alphabets.DNA, "ACAAAAGCAACAAAAATGAAGGCAATACTAGTAGTTCTGCTATATACATTTGCAACCGCAAATGCAGACACATTATGTATAGGTTATCATGCGAACAATTCAACAGACACTGTAGACACAGTACTAGAAAAGAATGTAACAGTAACACACTCTGTTAACCTTCTAGAAGACAAGCATAACGGGAAACTATGCAAACTAAGAGGAGTAGCCCCATTGCATTTGGGTAAATGTAACATTGCTGGCTGGATCCTGGGAAATCCAGAGTGTGAATCACTCTCCACAGCAAGCTCATGGTCCTACATTGTGGAAACATCTAGTTCAGACAATGGAACGTGTTACCCAGGAGATTTCATCGATTATGAGGAGCTAAGAGAGCAATTGAGCTCAGTGTCATCATTTGAAAGGTTTGAGATATTCCCCAAGACAAGTTCATGGCCCAATCATGACTCGAACAAAGGTGTAACGGCAGCATGTCCTCATGCTGGAGCAAAAAGCTTCTACAAAAATTTAATATGGCTAGTTAAAAAAGGAAATTCATACCCAAAGCTCAGCAAATCCTACATTAATGATAAAGGGAAAGAAGTCCTCGTGCTATGGGGCATTCACCATCCATCTACTAGTGCTGACCAACAAAGTCTCTATCAGAATGCAGATGCATATGTTTTTGTGGGGTCATCAAGATATAGCAAGAAGTTCAAGCCGGAAATAGCAATAAGACCCAAAGTGAGGGATCAAGAAGGGAGAATGAACTATTACTGGACACTAGTAGAGCCGGGAGACAAAATAACATTCGAAGCAACTGGAAATCTAGTGGTACCGAGATATGCATTCGCAATGGAAAGAAATGCTGGATCTGGTATTATCATTTCAGATACACCAGTCCACGATTGCAATACAACTTGTCAGACACCCAAGGGTGCTATAAACACCAGCCTCCCATTTCAGAATATACATCCGATCACAATTGGAAAATGTCCAAAATATGTAAAAAGCACAAAATTGAGACTGGCCACAGGATTGAGGAATGTCCCGTCTATTCAATCTAGAGGCCTATTTGGGGCCATTGCCGGTTTCATTGAAGGGGGGTGGACAGGGATGGTAGATGGATGGTACGGTTATCACCATCAAAATGAGCAGGGGTCAGGATATGCAGCCGACCTGAAGAGCACACAGAATGCCATTGACGAGATTACTAACAAAGTAAATTCTGTTATTGAAAAGATGAATATACAGTTCACAGCAGTAGGTAAAGAGTTCAACCACCTGGAAAAAAGAATAGAGAATTTAAATAAAAAAGTTGATGATGGTTTCCTGGACATTTGGACTTACAATGCCGAACTGTTGGTTCTATTGGAAAATGAAAGAACTTTGGACTACCACGATTCGAATGTGAAGAACTTATATGAAAAGGTAAGAAGCCAGCTAAAAAACAATGCCAAGGAAATTGGAAACGGCTGCTTTGAATTTTACCACAAATGCGATAACACGTGCATGGAAAGTGTCAAAAATGGGACTTATGACTACCCAAAATACTCAGAGGAAGCAAAATTAAACAGAGAAGAAATAGATGGGGTAAAGCTGGAATCAACAAGGATTTACCAGATTTTGGCGATCTATTCAACTGTCGCCAGTTCATTGGTACTGGTAGTCTCCCTGGGGGCAATCAGTTTCTGGATGTGCTCTAATGGGTCTCTACAGTGTAGAATATGTATTTAACATTAGGATTTCAGAAGCATGAGAAA");
            var seq1 = new Sequence(Alphabets.DNA, "ATGAAGGCAATACTAGTAGTTCTGCTATATACATTTGCAACCGCAAATGCAGACACATTATGTATAGGTTATCATGCGAACAATTCAACAGACACTGTAGACACAGTACTAGAAAAGAATGTAACAGTAACACACTCTGTTAACCTTCTAGAAGACAAGCATAACGGGAAACTATGCAAACTAAGAGGGGTAGCCCCATTGCATTTGGGTAAATGTAACATTGCTGGCTGGATCCTGGGAAATCCAGAGTGTGAATCACTCTCCACAGCAAGCTCATGGTCCTACATTGTGGAAACATCTAGTTCAGACAATGGAACGTGTTACCCAGGAGATTTCATCGATTATGAGGAGCTAAGAGAGCAATTGAGCTCAGTGTCATCATTTGAAAGGTTTGAGATATTCCCCAAGACAAGTTCATGGCCCAATCATGACTCGAACAAAGGTGTAACGGCAGCATGTCCTCATGCTGGAGCAAAAAGCTTCTACAAAAATTTAATATGGCTAGTTAAAAAAGGAAATTCATACCCAAAGCTCAGCAAATCCTACATTAATGATAAAGGGAAAGAAGTCCTCGTGCTATGGGGCATTCACCATCCATCTACTAGTGCTGACCAACAAAGTCTCTATCAGAATGCAGATGCATATGTTTTTGTGGGGACATCAAGATACAGCAAGAAGTTCAAGCCGGAAATAGCAATAAGACCCAAAGTGAGGGATCAAGAAGGGAGAATGAACTATTACTGGACACTAGTAGAGCCGGGAGACAAAATAACATTCGAAGCAACTGGAAATCTAGTGGTACCGAGATATGCATTCGCAATGGAAAGAAATGCTGGATCTGGTATTATCATTTCAGATACACCAGTCCACGATTGCAATACAACTTGTCAGACACCCAAGGGTGCTATAAACACCAGCCTCCCATTTCAGAATATACATCCGATCACAATTGGAAAATGTCCAAAATATGTAAAAAGCACAAAATTGAGACTGGCCACAGGATTGAGGAATGTCCCGTCTATTCAATCTAGAGGCCTATTTGGGGCCATTGCCGGTTTCATTGAAGGGGGGTGGACAGGGATGGTAGATGGATGGTACGGTTATCACCATCAAAATGAGCAGGGGTCAGGATATGCAGCCGACCTGAAGAGCACACAGAATGCCATTGACGAGATTACTAACAAAGTAAATTCTGTTATTGAAAAGATGAATACACAGTTCACAGCAGTAGGTAAAGAGTTCAACCACCTGGAAAAAAGAATAGAGAATTTAAATAAAAAAATTGATGATGGTTTCCTGGACATTTGGACTTACAATGCCGAACTGTTGGTTCTATTGGAAAATGAAAGAACTTTGGACTACCACGATTCAAATGTGAAGAACTTATATGAAAAGGTAAGAAGCCAGTTAAAAAACAATGCCAAGGAAATTGGAAACGGCTGCTTTGAATTTTACCACAAATGCGATAACACGTGCATGGAAAGTGTCAAAAATGGGACTTATGACTACCCAAAATACTCAGAGGAAGCAAAATTAAACAGAGAAGAAATAGATGGGGTAAAGCTGGAATCAACAAGGATTTACCAGATTTTGGCGATCTATTCAACTGTCGCCAGTTCATTGGTACTGGTAGTCTCCCTGGGGGCAATCAGTTTCTGGATGTGCTCTAATGGGTCTCTACAGTGTAGAATATGTATTTAA");

            var assembler = new OverlapDeNovoAssembler
            {
                MergeThreshold = mergeThreshold,
                OverlapAlgorithm = new NeedlemanWunschAligner 
                {
                    SimilarityMatrix = new DiagonalSimilarityMatrix(matchScore, mismatchScore),
                    GapOpenCost = gapCost
                },
                ConsensusResolver = new SimpleConsensusResolver(consensusThreshold),
                AssumeStandardOrientation = false,
            };

            var seqAssembly = (IOverlapDeNovoAssembly) assembler.Assemble(new List<ISequence> {seq1, seq2});
            var contig0 = seqAssembly.Contigs[0];
            var actualString = contig0.ToString();
            //string expectedString = "ACAAAAGCAACAAAAATGAAGGCAATACTAGTAGTTCTGCTATATACATTTGCAACCGCAAATG... +[1678]";
            var expectedString = "AYRAARGCAAYAMWARTRRWKSYRMTAYWWRYAKTTSYRMYMKMWAMWKYWGMMACMKYAWRTR... +[1678]";
            Assert.AreEqual(actualString, expectedString); 
        }

        /// <summary>
        /// Test for MatePair ToString() Function.
        /// </summary>
        [Test]
        public void TestMatePairToString()
        {
            var p = new MatePair("2K") {ForwardReadID = "F", ReverseReadID = "R"};
            var actualString = p.ToString();
            const string expectedString = "ForwardReadID=F, ReverseReadID=R, MeanLength=2000, Standard Deviation=100";
            Assert.AreEqual(actualString, expectedString); 
        }

        /// <summary>
        /// Test for OverlapDenovoAssembly ToString() Function.
        /// </summary>
        [Test]
        public void TestOverlapDenovoAssemblyToString()
        {
            const int matchScore = 5;
            const int mismatchScore = -4;
            const int gapCost = -10;
            const double mergeThreshold = 4;
            const double consensusThreshold = 66;

            ISequence seq2 = new Sequence(Alphabets.DNA, "ACAAAAGCAACAAAAATGAAGGCAATACTAGTAGTTCTGCTATATACATTTGCAACCGCAAATGCAGACACATTATGTATAGGTTATCATGCGAACAATTCAACAGACACTGTAGACACAGTACTAGAAAAGAATGTAACAGTAACACACTCTGTTAACCTTCTAGAAGACAAGCATAACGGGAAACTATGCAAACTAAGAGGAGTAGCCCCATTGCATTTGGGTAAATGTAACATTGCTGGCTGGATCCTGGGAAATCCAGAGTGTGAATCACTCTCCACAGCAAGCTCATGGTCCTACATTGTGGAAACATCTAGTTCAGACAATGGAACGTGTTACCCAGGAGATTTCATCGATTATGAGGAGCTAAGAGAGCAATTGAGCTCAGTGTCATCATTTGAAAGGTTTGAGATATTCCCCAAGACAAGTTCATGGCCCAATCATGACTCGAACAAAGGTGTAACGGCAGCATGTCCTCATGCTGGAGCAAAAAGCTTCTACAAAAATTTAATATGGCTAGTTAAAAAAGGAAATTCATACCCAAAGCTCAGCAAATCCTACATTAATGATAAAGGGAAAGAAGTCCTCGTGCTATGGGGCATTCACCATCCATCTACTAGTGCTGACCAACAAAGTCTCTATCAGAATGCAGATGCATATGTTTTTGTGGGGTCATCAAGATATAGCAAGAAGTTCAAGCCGGAAATAGCAATAAGACCCAAAGTGAGGGATCAAGAAGGGAGAATGAACTATTACTGGACACTAGTAGAGCCGGGAGACAAAATAACATTCGAAGCAACTGGAAATCTAGTGGTACCGAGATATGCATTCGCAATGGAAAGAAATGCTGGATCTGGTATTATCATTTCAGATACACCAGTCCACGATTGCAATACAACTTGTCAGACACCCAAGGGTGCTATAAACACCAGCCTCCCATTTCAGAATATACATCCGATCACAATTGGAAAATGTCCAAAATATGTAAAAAGCACAAAATTGAGACTGGCCACAGGATTGAGGAATGTCCCGTCTATTCAATCTAGAGGCCTATTTGGGGCCATTGCCGGTTTCATTGAAGGGGGGTGGACAGGGATGGTAGATGGATGGTACGGTTATCACCATCAAAATGAGCAGGGGTCAGGATATGCAGCCGACCTGAAGAGCACACAGAATGCCATTGACGAGATTACTAACAAAGTAAATTCTGTTATTGAAAAGATGAATATACAGTTCACAGCAGTAGGTAAAGAGTTCAACCACCTGGAAAAAAGAATAGAGAATTTAAATAAAAAAGTTGATGATGGTTTCCTGGACATTTGGACTTACAATGCCGAACTGTTGGTTCTATTGGAAAATGAAAGAACTTTGGACTACCACGATTCGAATGTGAAGAACTTATATGAAAAGGTAAGAAGCCAGCTAAAAAACAATGCCAAGGAAATTGGAAACGGCTGCTTTGAATTTTACCACAAATGCGATAACACGTGCATGGAAAGTGTCAAAAATGGGACTTATGACTACCCAAAATACTCAGAGGAAGCAAAATTAAACAGAGAAGAAATAGATGGGGTAAAGCTGGAATCAACAAGGATTTACCAGATTTTGGCGATCTATTCAACTGTCGCCAGTTCATTGGTACTGGTAGTCTCCCTGGGGGCAATCAGTTTCTGGATGTGCTCTAATGGGTCTCTACAGTGTAGAATATGTATTTAACATTAGGATTTCAGAAGCATGAGAAA");
            ISequence seq1 = new Sequence(Alphabets.DNA, "ATGAAGGCAATACTAGTAGTTCTGCTATATACATTTGCAACCGCAAATGCAGACACATTATGTATAGGTTATCATGCGAACAATTCAACAGACACTGTAGACACAGTACTAGAAAAGAATGTAACAGTAACACACTCTGTTAACCTTCTAGAAGACAAGCATAACGGGAAACTATGCAAACTAAGAGGGGTAGCCCCATTGCATTTGGGTAAATGTAACATTGCTGGCTGGATCCTGGGAAATCCAGAGTGTGAATCACTCTCCACAGCAAGCTCATGGTCCTACATTGTGGAAACATCTAGTTCAGACAATGGAACGTGTTACCCAGGAGATTTCATCGATTATGAGGAGCTAAGAGAGCAATTGAGCTCAGTGTCATCATTTGAAAGGTTTGAGATATTCCCCAAGACAAGTTCATGGCCCAATCATGACTCGAACAAAGGTGTAACGGCAGCATGTCCTCATGCTGGAGCAAAAAGCTTCTACAAAAATTTAATATGGCTAGTTAAAAAAGGAAATTCATACCCAAAGCTCAGCAAATCCTACATTAATGATAAAGGGAAAGAAGTCCTCGTGCTATGGGGCATTCACCATCCATCTACTAGTGCTGACCAACAAAGTCTCTATCAGAATGCAGATGCATATGTTTTTGTGGGGACATCAAGATACAGCAAGAAGTTCAAGCCGGAAATAGCAATAAGACCCAAAGTGAGGGATCAAGAAGGGAGAATGAACTATTACTGGACACTAGTAGAGCCGGGAGACAAAATAACATTCGAAGCAACTGGAAATCTAGTGGTACCGAGATATGCATTCGCAATGGAAAGAAATGCTGGATCTGGTATTATCATTTCAGATACACCAGTCCACGATTGCAATACAACTTGTCAGACACCCAAGGGTGCTATAAACACCAGCCTCCCATTTCAGAATATACATCCGATCACAATTGGAAAATGTCCAAAATATGTAAAAAGCACAAAATTGAGACTGGCCACAGGATTGAGGAATGTCCCGTCTATTCAATCTAGAGGCCTATTTGGGGCCATTGCCGGTTTCATTGAAGGGGGGTGGACAGGGATGGTAGATGGATGGTACGGTTATCACCATCAAAATGAGCAGGGGTCAGGATATGCAGCCGACCTGAAGAGCACACAGAATGCCATTGACGAGATTACTAACAAAGTAAATTCTGTTATTGAAAAGATGAATACACAGTTCACAGCAGTAGGTAAAGAGTTCAACCACCTGGAAAAAAGAATAGAGAATTTAAATAAAAAAATTGATGATGGTTTCCTGGACATTTGGACTTACAATGCCGAACTGTTGGTTCTATTGGAAAATGAAAGAACTTTGGACTACCACGATTCAAATGTGAAGAACTTATATGAAAAGGTAAGAAGCCAGTTAAAAAACAATGCCAAGGAAATTGGAAACGGCTGCTTTGAATTTTACCACAAATGCGATAACACGTGCATGGAAAGTGTCAAAAATGGGACTTATGACTACCCAAAATACTCAGAGGAAGCAAAATTAAACAGAGAAGAAATAGATGGGGTAAAGCTGGAATCAACAAGGATTTACCAGATTTTGGCGATCTATTCAACTGTCGCCAGTTCATTGGTACTGGTAGTCTCCCTGGGGGCAATCAGTTTCTGGATGTGCTCTAATGGGTCTCTACAGTGTAGAATATGTATTTAA");

            IOverlapDeNovoAssembler assembler = new OverlapDeNovoAssembler
            {
                MergeThreshold = mergeThreshold,
                OverlapAlgorithm = new NeedlemanWunschAligner 
                { 
                    SimilarityMatrix = new DiagonalSimilarityMatrix(matchScore, mismatchScore),
                    GapOpenCost = gapCost
                },
                ConsensusResolver = new SimpleConsensusResolver(consensusThreshold),
                AssumeStandardOrientation = false
            };

            var inputs = new List<ISequence> {seq1, seq2};
            var seqAssembly = (IOverlapDeNovoAssembly) assembler.Assemble(inputs);

            Assert.AreEqual(0, seqAssembly.UnmergedSequences.Count);
            Assert.AreEqual(1, seqAssembly.Contigs.Count);
            
            assembler.OverlapAlgorithm = new SmithWatermanAligner();
            seqAssembly = (IOverlapDeNovoAssembly) assembler.Assemble(inputs);

            var expectedString = "ACAAAAGCAACAAAAATGAAGGCAATACTAGTAGTTCTGCTATATACATTTGCAACCGCAAATG... +[1678]\r\n".Replace("\r\n",Environment.NewLine);
            var actualString = seqAssembly.ToString();
            Assert.AreEqual(expectedString, actualString);
        }
    }
}
