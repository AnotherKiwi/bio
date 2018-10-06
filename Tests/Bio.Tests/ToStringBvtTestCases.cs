using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using Bio.Algorithms.Alignment;
using Bio.Algorithms.Assembly;
using Bio.Algorithms.SuffixTree;
using Bio.IO.FastA;
using Bio.SimilarityMatrices;
using Bio.TestAutomation.Util;
using Bio.Util;

using NUnit.Framework;

namespace Bio.Tests
{
    /// <summary>
    ///     Bvt Test cases for ToString and ConvertToString of all classes
    /// </summary>
    [TestFixture]
    public class ToStringBvtTestCases
    {
        private readonly Utility utilityObj = new Utility(@"TestUtils\TestsConfig.xml");

        #region BVT Test Cases

        /// <summary>
        ///     Validates Sequence ToString()
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateSequenceToString()
        {
            ISequence seqSmall = new Sequence(Alphabets.DNA, "ATCG");
            var seqLargeString = utilityObj.xmlUtil.GetTextValue(Constants.ToStringNodeName,
                                                                    Constants.seqLargeStringNode);
            ISequence seqLarge = new Sequence(Alphabets.DNA, seqLargeString);
            var ActualSmallString = seqSmall.ToString();
            var ActualLargeString = seqLarge.ToString();
            var ExpectedSmallString = "ATCG";
            var seqLargeExpected = utilityObj.xmlUtil.GetTextValue(Constants.ToStringNodeName,
                                                                      Constants.seqLargeExpected2Node);
            var expectedLargeString = string.Format(CultureInfo.CurrentCulture,
                                                       seqLargeExpected,
                                                       (seqLarge.Count - Helper.AlphabetsToShowInToString));
            Assert.AreEqual(ExpectedSmallString, ActualSmallString);
            Assert.AreEqual(expectedLargeString, ActualLargeString);

            //check with blank sequence
            var seqBlank = new Sequence(Alphabets.DNA, "");
            var blankString = seqBlank.ToString();
            Assert.AreEqual(string.Empty, blankString);

            // Gets the expected sequence from the Xml
            var filePath = utilityObj.xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                                                              Constants.FilePathNode).TestDir();
            //read sequence from file
            var parser = new FastAParser { Alphabet = Alphabets.Protein };
            var seqsList = parser.Parse(filePath).ToList();

            var seqString = new string(seqsList[0].Select(a => (char) a).ToArray());
            if (seqString.Length > Helper.AlphabetsToShowInToString)
            {
                //check if the whole sequence string contains the string retrieved from ToString
                Assert.IsTrue(seqString.Contains(seqsList[0].ToString().Substring(0, Helper.AlphabetsToShowInToString)));
                Assert.IsTrue(seqsList[0].ToString().Contains("... +["));
            }
            else
            {
                Assert.AreEqual(seqString, seqsList[0].ToString());
            }
        }


        /// <summary>
        ///     Validates DerivedSequence ToString()
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateDerivedSequenceToString()
        {
            ISequence seqSmall = new Sequence(Alphabets.DNA, "ATCG");
            var seqLargeStr = utilityObj.xmlUtil.GetTextValue(Constants.ToStringNodeName,
                                                                 Constants.seqLargeStringNode);
            ISequence seqLarge = new Sequence(Alphabets.DNA, seqLargeStr);
            ISequence DeriveSeqSmall = new DerivedSequence(seqSmall, false, true);
            ISequence DeriveSeqLarge = new DerivedSequence(seqLarge, false, true);

            var ActualSmallString = DeriveSeqSmall.ToString();
            var ActualLargeString = DeriveSeqLarge.ToString();
            var ExpectedSmallString = "TAGC";
            var seqLargeExpected = utilityObj.xmlUtil.GetTextValue(Constants.ToStringNodeName,
                                                                      Constants.seqLargeExpectedNode);
            var expectedLargeString = string.Format(CultureInfo.CurrentCulture,
                                                       seqLargeExpected,
                                                       (seqLarge.Count - Helper.AlphabetsToShowInToString));

            Assert.AreEqual(ExpectedSmallString, ActualSmallString);
            Assert.AreEqual(expectedLargeString, ActualLargeString);

            //read sequences from file
            // Get input and expected values from xml
            var expectedSequence = utilityObj.xmlUtil.GetTextValue(
                Constants.ProteinDerivedSequenceNode, Constants.ExpectedSequence);
            var alphabetName = utilityObj.xmlUtil.GetTextValue(
                Constants.ProteinDerivedSequenceNode, Constants.AlphabetNameNode);
            var alphabet = Utility.GetAlphabet(alphabetName);

            // Create derived Sequence
            ISequence seq = new Sequence(alphabet, expectedSequence);
            var derSequence = new DerivedSequence(seq, false, false);

            var actualDerivedSeqStr = derSequence.ToString();
            if (actualDerivedSeqStr.Length > Helper.AlphabetsToShowInToString)
            {
                //check if the whole sequence string contains the string retrieved from ToString
                Assert.IsTrue(
                    expectedSequence.Contains(derSequence.ToString().Substring(0, Helper.AlphabetsToShowInToString)));
                Assert.IsTrue(derSequence.ToString().Contains("... +["));
            }
            else
            {
                Assert.AreEqual(expectedSequence, derSequence.ToString());
            }
        }

        /// <summary>
        ///     Validates QualitativeSequence ToString
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateQualitativeSequenceToString()
        {
            var seqData = new byte[4];
            seqData[0] = (byte) 'A';
            seqData[1] = (byte) 'T';
            seqData[2] = (byte) 'C';
            seqData[3] = (byte) 'G';
            var qualityScores = new byte[4];
            qualityScores[0] = (byte) 'A';
            qualityScores[1] = (byte) 'A';
            qualityScores[2] = (byte) 'A';
            qualityScores[3] = (byte) 'B';
            var seq = new QualitativeSequence(Alphabets.DNA,
                                              FastQFormatType.Illumina_v1_3, seqData, qualityScores);
            var actualString = seq.ToString().Replace("\r","").Replace("\n","");
            var expectedString = "ATCGAAAB"; // This is dangerously platform specific
            Assert.AreEqual(expectedString, actualString);
        }

        /// <summary>
        ///     Validates All Alphabets ToString
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateAllAlphabetsToString()
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

            Assert.AreEqual("ACGT-", dnaStringActual);
            Assert.AreEqual("ACGU-", rnaStringActual);
            Assert.AreEqual("ACDEFGHIKLMNOPQRSTUVWY-*", proteinStringActual);
            Assert.AreEqual("ACGT-MRSWYKVHDBN", dnaAmbiguousStringActual);
            Assert.AreEqual("ACGU-NMRSWYKVHDB", rnaAmbiguousStringActual);
            Assert.AreEqual("ACDEFGHIKLMNOPQRSTUVWY-*XZBJ", proteinAmbiguousStringActual);
        }

        /// <summary>
        ///     Validates SequenceRange ToString
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateSequenceRangeToString()
        {
            var range = new SequenceRange("chr20", 0, 3);
            var actualString = range.ToString();
            Assert.AreEqual("ID=chr20 Start=0 End=3", actualString);
        }

        /// <summary>
        ///     Validates SequenceRangeGrouping ToString
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateSequenceRangeGroupingToString()
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
            var expectedStr = utilityObj.xmlUtil.GetTextValue(Constants.ToStringNodeName,
                                                                 Constants.SequenceRangeGroupingExpectedNode);
            Assert.AreEqual(expectedStr.Replace("\\r\\n", ""), actualString.Replace(System.Environment.NewLine, ""));
        }

        /// <summary>
        ///     Validates SequenceStatistics ToString
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateSequenceStatisticsToString()
        {
            ISequence seq = new Sequence(Alphabets.DNA, "ATCGATCG");
            var seqStats = new SequenceStatistics(seq);
            var actualString = seqStats.ToString();
            var expectedString = "A - 2\r\nC - 2\r\nG - 2\r\nT - 2\r\n".Replace("\r\n", System.Environment.NewLine);
            Assert.AreEqual(actualString, expectedString);

            // Gets the expected sequence from the Xml
            List<ISequence> seqsList;
            IEnumerable<ISequence> sequences = null;
            var filePath = utilityObj.xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                                                              Constants.FilePathNode).TestDir();
            using (var reader = File.OpenRead(filePath))
            {
                var parser = new FastAParser();
                {
                    parser.Alphabet = Alphabets.Protein;
                    sequences = parser.Parse(reader);

                    //Create a list of sequences.
                    seqsList = sequences.ToList();
                }
            }

            foreach (var Sequence in seqsList)
            {
                seqStats = new SequenceStatistics(Sequence);
                var seqStatStr = seqStats.ToString();
                Assert.IsTrue(seqStatStr.Contains(" - "));
            }
        }

        /// <summary>
        ///     Validates AlignedSequence ToString
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateAlignedSequenceToString()
        {
            IList<ISequence> seqList = new List<ISequence>();
            var actualAlignedSeqString = utilityObj.xmlUtil.GetTextValue(Constants.ToStringNodeName,
                                                                            Constants.AlignedSeqActualNode);
            seqList.Add(new Sequence(Alphabets.DNA,
                                     actualAlignedSeqString));
            seqList.Add(new Sequence(Alphabets.DNA, "CAAAAGGGATTGC---"));
            seqList.Add(new Sequence(Alphabets.DNA, "TAGTAGTTCTGCTATATACATTTG"));
            seqList.Add(new Sequence(Alphabets.DNA, "GTTATCATGCGAACAATTCAACAGACACTGTAGA"));
            var num = new NucmerPairwiseAligner();
            num.BreakLength = 8;
            num.FixedSeparation = 0;
            num.MinimumScore = 0;
            num.MaximumSeparation = 0;
            num.SeparationFactor = 0;
            num.LengthOfMUM = 8;
            var sequenceList = seqList;
            var alignmentObj = num.Align(sequenceList);
            var alignedSeqs = (AlignedSequence) alignmentObj[0].AlignedSequences[0];

            var actualString = alignedSeqs.ToString();
            var expectedString = utilityObj.xmlUtil.GetTextValue(Constants.ToStringNodeName,
                                                                    Constants.AlignedSeqExpectedNode);
            Assert.AreEqual(actualString, expectedString.Replace("\\r\\n", System.Environment.NewLine));
        }

        /// <summary>
        ///     Validates Cluster ToString
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateClusterToString()
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
            var expectedString = utilityObj.xmlUtil.GetTextValue(Constants.ToStringNodeName,
                                                                    Constants.ClusterExpectedNode);
            Assert.AreEqual(actualString, expectedString.Replace("\\r\\n", System.Environment.NewLine));
        }

        /// <summary>
        ///     Validates DeltaAlignment ToString
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateDeltaAlignmentToString()
        {
            ISequence refSeq = new Sequence(Alphabets.DNA, "ATCGGGGGGGGAAAAAAATTTTCCCCGGGGG");
            ISequence qrySeq = new Sequence(Alphabets.DNA, "GGGGG");
            var delta = new DeltaAlignment(refSeq, qrySeq) {FirstSequenceEnd = 21, SecondSequenceEnd = 20};
            
            var actualString = delta.ToString();
            var expectedString = utilityObj.xmlUtil.GetTextValue(Constants.ToStringNodeName, Constants.DeltaAlignmentExpectedNode);
            Assert.AreEqual(expectedString, actualString);

            // Gets the expected sequence from the Xml
            List<ISequence> seqsList;
            var filePath = utilityObj.xmlUtil.GetTextValue(Constants.SimpleFastaNodeName, Constants.FilePathNode).TestDir();
            using (var reader = File.OpenRead(filePath))
            {
                var parser = new FastAParser();
                {
                    parser.Alphabet = Alphabets.Protein;
                    seqsList = parser.Parse(reader).ToList();
                }
            }

            delta = new DeltaAlignment(seqsList[0], qrySeq) {FirstSequenceEnd = 21, SecondSequenceStart = 20, QueryDirection = Cluster.ReverseDirection};
            actualString = delta.ToString();
            expectedString = utilityObj.xmlUtil.GetTextValue(Constants.ToStringNodeName, Constants.DeltaAlignmentExpected2Node);
            Assert.AreEqual(expectedString, actualString);
        }

        /// <summary>
        ///     Validates PairwiseAlignedSequence ToString
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidatePairwiseAlignedSequenceToString()
        {
            var alignedSeq = new PairwiseAlignedSequence();
            alignedSeq.FirstSequence = new Sequence(Alphabets.Protein, "AWGHE");
            alignedSeq.SecondSequence = new Sequence(Alphabets.Protein, "AW-HE");
            alignedSeq.Consensus = new Sequence(Alphabets.Protein, "AWGHE");
            alignedSeq.Score = 28;
            alignedSeq.FirstOffset = 0;
            alignedSeq.SecondOffset = 3;

            var actualString = alignedSeq.ToString();
            var expectedString = "AWGHE\r\nAWGHE\r\nAW-HE\r\n".Replace("\r\n", System.Environment.NewLine);
            Assert.AreEqual(actualString, expectedString);
        }

        /// <summary>
        ///     Validates PairwiseSequenceAlignment ToString
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidatePairwiseSequenceAlignmentToString()
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
            var expectedString = "AWGHE\r\nAWGHE\r\nAW-HE\r\n\r\n".Replace("\r\n", System.Environment.NewLine);
            Assert.AreEqual(actualString, expectedString);
        }

        /// <summary>
        ///     Validates Match And MatchExtension ToString
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateMatchAndMatchExtensionToString()
        {
            var match = new Match();
            match.Length = 20;
            match.QuerySequenceOffset = 33;

            var matchExtn = new MatchExtension(match);
            matchExtn.ID = 1;
            matchExtn.Length = 20;

            var actualMatchExtnString = matchExtn.ToString();
            var actualMatchstring = match.ToString();
            var ExpectedMatchExtnString = utilityObj.xmlUtil.GetTextValue(Constants.ToStringNodeName,
                                                                             Constants.ExpectedMatchExtnStringNode);
            var ExpectedMatchString = utilityObj.xmlUtil.GetTextValue(Constants.ToStringNodeName,
                                                                         Constants.ExpectedMatchStringNode);

            Assert.AreEqual(ExpectedMatchExtnString, actualMatchExtnString);
            Assert.AreEqual(actualMatchstring, ExpectedMatchString);
        }

        /// <summary>
        ///     Validates SequenceAlignment ToString
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateSequenceAlignmentToString()
        {
            ISequenceAligner aligner = SequenceAligners.NeedlemanWunsch;
            IAlphabet alphabet = Alphabets.Protein;
            var origSequence1 = "KRIPKSQNLRSIHSIFPFLEDKLSHLN";
            var origSequence2 = "LNIPSLITLNKSIYVFSKRKKRLSGFLHN";

            // Create input sequences
            var inputSequences = new List<ISequence>
            {
                new Sequence(alphabet, origSequence1),
                new Sequence(alphabet, origSequence2)
            };

            // Get aligned sequences
            var alignments = aligner.Align(inputSequences);
            ISequenceAlignment alignment = new SequenceAlignment();
            for (var ialigned = 0; ialigned < alignments[0].AlignedSequences.Count; ialigned++)
            {
                alignment.AlignedSequences.Add(alignments[0].AlignedSequences[ialigned]);
            }

            foreach (var key in alignments[0].Metadata.Keys)
            {
                alignment.Metadata.Add(key, alignments[0].Metadata[key]);
            }

            var actualSequenceAlignmentString = alignment.ToString();
            var ExpectedSequenceAlignmentString = utilityObj.xmlUtil.GetTextValue(Constants.ToStringNodeName,
                                                                                     Constants
                                                                                         .SequenceAlignmentExpectedNode);

            Assert.AreEqual(ExpectedSequenceAlignmentString.Replace("\\r\\n", ""),
                actualSequenceAlignmentString.Replace(System.Environment.NewLine, ""));
        }

        /// <summary>
        ///     Validates Contig ToString
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateContigToString()
        {
            const int matchScore = 5;
            const int mismatchScore = -4;
            const int gapCost = -10;
            const double mergeThreshold = 4;
            const double consensusThreshold = 66;
            var seq2Str = utilityObj.xmlUtil.GetTextValue(Constants.ToStringNodeName, Constants.Seq2StrNode);
            var seq1Str = utilityObj.xmlUtil.GetTextValue(Constants.ToStringNodeName, Constants.Seq1StrNode);

            ISequence seq1 = new Sequence(Alphabets.DNA, seq1Str);
            ISequence seq2 = new Sequence(Alphabets.DNA, seq2Str);

            var assembler = new OverlapDeNovoAssembler
                                {
                                    MergeThreshold = mergeThreshold,
                                    OverlapAlgorithm = new NeedlemanWunschAligner
                                                           {
                                                               SimilarityMatrix =
                                                                   new DiagonalSimilarityMatrix(matchScore,
                                                                                                mismatchScore),
                                                               GapOpenCost = gapCost
                                                           },
                                    ConsensusResolver = new SimpleConsensusResolver(consensusThreshold),
                                    AssumeStandardOrientation = false
                                };

            var seqAssembly = (IOverlapDeNovoAssembly) assembler.Assemble(new List<ISequence> {seq1, seq2});

            var contig0 = seqAssembly.Contigs[0];
            var actualString = contig0.ToString();
            var expectedString = utilityObj.xmlUtil.GetTextValue(Constants.ToStringNodeName,
                                                                    Constants.OverlapDenovoExpectedNode);
            Assert.AreEqual(expectedString.Replace("\\r\\n", ""), actualString.Replace("\r\n", ""));

            // Get the parameters from Xml
            var matchScore1 =
                int.Parse(
                    utilityObj.xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName, Constants.MatchScoreNode), null);
            var mismatchScore1 =
                int.Parse(
                    utilityObj.xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName, Constants.MisMatchScoreNode),
                    null);
            var gapCost1 =
                int.Parse(utilityObj.xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName, Constants.GapCostNode),
                          null);
            var mergeThreshold1 =
                double.Parse(
                    utilityObj.xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName, Constants.MergeThresholdNode),
                    null);
            var consensusThreshold1 =
                double.Parse(
                    utilityObj.xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName,
                                                    Constants.ConsensusThresholdNode), null);
            var sequence1 = utilityObj.xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName,
                                                               Constants.SequenceNode1);
            var sequence2 = utilityObj.xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName,
                                                               Constants.SequenceNode2);
            var sequence3 = utilityObj.xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName,
                                                               Constants.SequenceNode3);
            var alphabet =
                Utility.GetAlphabet(utilityObj.xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName,
                                                                    Constants.AlphabetNameNode));

            ISequence seq4 = new Sequence(alphabet, sequence1);
            ISequence seq5 = new Sequence(alphabet, sequence2);
            ISequence seq6 = new Sequence(alphabet, sequence3);

            var assembler1 = new OverlapDeNovoAssembler
                                 {
                                     MergeThreshold = mergeThreshold1,
                                     OverlapAlgorithm = new PairwiseOverlapAligner
                                                            {
                                                                SimilarityMatrix =
                                                                    new DiagonalSimilarityMatrix(matchScore1,
                                                                                                 mismatchScore1),
                                                                GapOpenCost = gapCost1
                                                            },
                                     ConsensusResolver = new SimpleConsensusResolver(consensusThreshold1),
                                     AssumeStandardOrientation = false
                                 };

            // Assembles all the sequences.
            var seqAssembly1 = (IOverlapDeNovoAssembly) assembler1.Assemble(new List<ISequence> {seq4, seq5, seq6});
            var contig1 = seqAssembly1.Contigs[0];
            var actualString1 = contig1.ToString();
            const string expectedString1 = "TATAAAGCGCCAAAATTTAGGCACCCGCGGTATT";

            Assert.AreEqual(expectedString1, actualString1);
        }

        /// <summary>
        ///     Validates MatePair ToString
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateMatePairToString()
        {
            var p = new MatePair("2K") {ForwardReadID = "F", ReverseReadID = "R"};
            var actualString = p.ToString();
            var expectedString = utilityObj.xmlUtil.GetTextValue(Constants.ToStringNodeName,
                                                                    Constants.MatePairExpectedNode);
            Assert.AreEqual(actualString, expectedString);
        }

        /// <summary>
        ///     Validates OverlapDenovoAssembly ToString
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateOverlapDenovoAssemblyToString()
        {
            const int matchScore = 5;
            const int mismatchScore = -4;
            const int gapCost = -10;
            const double mergeThreshold = 4;
            const double consensusThreshold = 66;

            ISequence seq1 = new Sequence(Alphabets.DNA,
                                          utilityObj.xmlUtil.GetTextValue(Constants.ToStringNodeName,
                                                                          Constants.Seq1StrNode));
            ISequence seq2 = new Sequence(Alphabets.DNA,
                                          utilityObj.xmlUtil.GetTextValue(Constants.ToStringNodeName,
                                                                          Constants.Seq2StrNode));

            var assembler = new OverlapDeNovoAssembler
                                {
                                    MergeThreshold = mergeThreshold,
                                    OverlapAlgorithm = new NeedlemanWunschAligner
                                                           {
                                                               SimilarityMatrix =
                                                                   new DiagonalSimilarityMatrix(matchScore,
                                                                                                mismatchScore),
                                                               GapOpenCost = gapCost
                                                           },
                                    ConsensusResolver = new SimpleConsensusResolver(consensusThreshold),
                                    AssumeStandardOrientation = false
                                };

            var inputs = new List<ISequence> {seq1, seq2};
            var seqAssembly = (IOverlapDeNovoAssembly) assembler.Assemble(inputs);

            Assert.AreEqual(0, seqAssembly.UnmergedSequences.Count);
            Assert.AreEqual(1, seqAssembly.Contigs.Count);

            assembler.OverlapAlgorithm = new SmithWatermanAligner
                                             {
                                                 SimilarityMatrix =
                                                     new DiagonalSimilarityMatrix(matchScore, mismatchScore),
                                                 GapOpenCost = gapCost
                                             };
            seqAssembly = (OverlapDeNovoAssembly) assembler.Assemble(inputs);

            var actualString = seqAssembly.ToString();
            const string expectedString = "ACAAAAGCAACAAAAATGAAGGCAATACTAGTAGTTCTGCTATATACATTTGCAACCGCAAATG... +[1678]";
            Assert.AreEqual(expectedString, actualString.Replace(System.Environment.NewLine, ""));

            // Get the parameters from Xml
            var matchScore1 =
                int.Parse(
                    utilityObj.xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName, Constants.MatchScoreNode), null);
            var mismatchScore1 =
                int.Parse(
                    utilityObj.xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName, Constants.MisMatchScoreNode),
                    null);
            var gapCost1 =
                int.Parse(utilityObj.xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName, Constants.GapCostNode),
                          null);
            var mergeThreshold1 =
                double.Parse(
                    utilityObj.xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName, Constants.MergeThresholdNode),
                    null);
            var consensusThreshold1 =
                double.Parse(
                    utilityObj.xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName,
                                                    Constants.ConsensusThresholdNode), null);

            var sequence1 = utilityObj.xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName,
                                                               Constants.SequenceNode1);
            var sequence2 = utilityObj.xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName,
                                                               Constants.SequenceNode2);
            var sequence3 = utilityObj.xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName,
                                                               Constants.SequenceNode3);
            var alphabet =
                Utility.GetAlphabet(utilityObj.xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName,
                                                                    Constants.AlphabetNameNode));

            var seq4 = new Sequence(alphabet, sequence1);
            var seq5 = new Sequence(alphabet, sequence2);
            var seq6 = new Sequence(alphabet, sequence3);

            var assembler1 = new OverlapDeNovoAssembler
                                 {
                                     MergeThreshold = mergeThreshold1,
                                     OverlapAlgorithm = new PairwiseOverlapAligner
                                                            {
                                                                SimilarityMatrix =
                                                                    new DiagonalSimilarityMatrix(matchScore1,
                                                                                                 mismatchScore1),
                                                                GapOpenCost = gapCost1,
                                                            },
                                     ConsensusResolver = new SimpleConsensusResolver(consensusThreshold1),
                                     AssumeStandardOrientation = false,
                                 };

            var inputs1 = new List<ISequence> {seq4, seq5, seq6};

            // Assembles all the sequences.
            seqAssembly = (OverlapDeNovoAssembly) assembler1.Assemble(inputs1);

            Assert.AreEqual(0, seqAssembly.UnmergedSequences.Count);
            Assert.AreEqual(1, seqAssembly.Contigs.Count);

            assembler1.OverlapAlgorithm = new SmithWatermanAligner();
            seqAssembly = (OverlapDeNovoAssembly) assembler1.Assemble(inputs1);

            var expectedString1 = "TYMKWRRGCGCCAAAATTTAGGC" + System.Environment.NewLine;
            actualString = seqAssembly.ToString();
            Assert.AreEqual(expectedString1, actualString);
        }

#if FALSE
        /// <summary>
        ///     Validates PadenaAssembly ToString
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidatePadenaAssemblyToString()
        {
            ISequence seq2 = new Sequence(Alphabets.DNA, "ACAAAAGCAAC");
            ISequence seq1 = new Sequence(Alphabets.DNA, "ATGAAGGCAATACTAGTAGT");
            IList<ISequence> contigList = new List<ISequence>();
            contigList.Add(seq1);
            contigList.Add(seq2);
            var denovoAssembly = new PadenaAssembly();
            denovoAssembly.AddContigs(contigList);

            string actualString = denovoAssembly.ToString();
            string expectedString = "ATGAAGGCAATACTAGTAGT\r\nACAAAAGCAAC\r\n";
            Assert.AreEqual(actualString, expectedString);

            // read sequences from xml
            string sequence1 = this.utilityObj.xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName,
                                                               Constants.SequenceNode1);
            string sequence2 = this.utilityObj.xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName,
                                                               Constants.SequenceNode2);
            IAlphabet alphabet =
                Utility.GetAlphabet(this.utilityObj.xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName,
                                                                    Constants.AlphabetNameNode));

            var seq3 = new Sequence(alphabet, sequence1);
            var seq4 = new Sequence(alphabet, sequence2);
            IList<ISequence> contigList1 = new List<ISequence>();
            contigList1.Add(seq3);
            contigList1.Add(seq4);
            var denovoAssembly1 = new PadenaAssembly();
            denovoAssembly1.AddContigs(contigList1);

            string actualString1 = denovoAssembly1.ToString();
            string expectedString1 = "GCCAAAATTTAGGC\r\nTTATGGCGCCCACGGA\r\n";
            Assert.AreEqual(expectedString1, actualString1);
        }
#endif

        /// <summary>
        ///     Validates Sequence ConvertToString()
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateSequenceConvertToString()
        {
            var seqLargeString = utilityObj.xmlUtil.GetTextValue(Constants.ToStringNodeName,
                                                                    Constants.seqLargeStringNode);
            ISequence seqLarge = new Sequence(Alphabets.DNA, seqLargeString);
            var ActualLargeString = seqLarge.ToString();
            var seqLargeExpected = utilityObj.xmlUtil.GetTextValue(Constants.ToStringNodeName,
                                                                      Constants.seqLargeExpected2Node);
            var expectedLargeString = string.Format(CultureInfo.CurrentCulture,
                                                       seqLargeExpected,
                                                       (seqLarge.Count - Helper.AlphabetsToShowInToString));
            Assert.AreEqual(expectedLargeString, ActualLargeString);

            List<ISequence> seqsList;
            // Gets the expected sequence from the Xml
            var filePath = utilityObj.xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                                                              Constants.FilePathNode).TestDir();

            var parser = new FastAParser { Alphabet = Alphabets.Protein };
            var seq = parser.Parse(filePath);

            //Create a list of sequences.
            seqsList = seq.ToList();

            var seqString = new string(seqsList[0].Select(a => (char) a).ToArray());
            Assert.AreEqual(seqString.Substring(2, 5), ((Sequence) seqsList[0]).ConvertToString(2, 5));
        }

        /// <summary>
        ///     Validates DerivedSequence ConvertToString()
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateDerivedSequenceConvertToString()
        {
            var seqLargeStr = utilityObj.xmlUtil.GetTextValue(Constants.ToStringNodeName,
                                                                 Constants.seqLargeStringNode);
            ISequence seqLarge = new Sequence(Alphabets.DNA, seqLargeStr);
            ISequence DeriveSeqLarge = new DerivedSequence(seqLarge, false, true);

            var ActualLargeString = DeriveSeqLarge.ToString();
            var seqLargeExpected = utilityObj.xmlUtil.GetTextValue(Constants.ToStringNodeName,
                                                                      Constants.seqLargeExpectedNode);
            var expectedLargeString = string.Format(CultureInfo.CurrentCulture,
                                                       seqLargeExpected,
                                                       (seqLarge.Count - Helper.AlphabetsToShowInToString));

            Assert.AreEqual(expectedLargeString, ActualLargeString);

            // Get input and expected values from xml
            var expectedSequence = utilityObj.xmlUtil.GetTextValue(
                Constants.ProteinDerivedSequenceNode, Constants.ExpectedSequence);
            var alphabetName = utilityObj.xmlUtil.GetTextValue(
                Constants.ProteinDerivedSequenceNode, Constants.AlphabetNameNode);
            var alphabet = Utility.GetAlphabet(alphabetName);

            // Create derived Sequence
            ISequence seq = new Sequence(alphabet, expectedSequence);
            var derSequence = new DerivedSequence(seq, false, false);

            Assert.AreEqual(expectedSequence.Substring(2, 5), derSequence.ConvertToString(2, 5));
        }

        #endregion BVT Test Cases
    }
}