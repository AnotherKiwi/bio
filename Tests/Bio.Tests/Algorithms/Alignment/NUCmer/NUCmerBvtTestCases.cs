using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using Bio.Algorithms.Alignment;
using Bio.Algorithms.MUMmer;
using Bio.Algorithms.SuffixTree;
using Bio.Extensions;
using Bio.IO.FastA;
using Bio.TestAutomation.Util;
using Bio.Util.Logging;

using NUnit.Framework;

namespace Bio.Tests.Algorithms.Alignment.NUCmer
{
    /// <summary>
    /// NUCmer Bvt Test case implementation.
    /// </summary>
    [TestFixture]
    public class NUCmerBvtTestCases
    {
        /// <summary>
        /// Lis Parameters which are used for different test cases 
        /// based on which the test cases are executed.
        /// </summary>
        enum AdditionalParameters
        {
            FindUniqueMatches,
            PerformClusterBuilder
        };

        readonly Utility utilityObj = new Utility(@"TestUtils\NUCmerTestsConfig.xml");
        readonly ASCIIEncoding encodingObj = new ASCIIEncoding();

        #region Suffix Tree Test Cases

        /// <summary>
        /// Validate FindMatches() method with one line sequences
        /// for both reference and query parameter and validate
        /// the unique matches
        /// Input : One line sequence for both reference and query parameter
        /// Validation : Validate the unique matches
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void SuffixTreeFindMatchesOneLineSequence()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.OneLineSequenceNodeName,
                false, AdditionalParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate FindMatches() method with small size (less than 35kb) sequences 
        /// for reference and query parameter and validate
        /// the unique matches
        /// Input : Small size sequence for both reference and query parameter
        /// Validation : Validate the unique matches
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void SuffixTreeFindMatchesSmallSizeSequence()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.SmallSizeSequenceNodeName, true,
                AdditionalParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate BuildCluster() method with one unique match
        /// and validate the clusters
        /// Input : one unique matches
        /// Validation : Validate the unique matches
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ClusterBuilderOneUniqueMatches()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.OneUniqueMatchSequenceNodeName,
                false, AdditionalParameters.PerformClusterBuilder);
        }

        /// <summary>
        /// Validate BuildCluster() method with two unique match
        /// without cross overlap and validate the clusters
        /// Input : two unique matches with out cross overlap
        /// Validation : Validate the unique matches
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ClusterBuilderTwoUniqueMatchesWithoutCrossOverlap()
        {
            ValidateFindMatchSuffixGeneralTestCases(
                Constants.TwoUniqueMatchWithoutCrossOverlapSequenceNodeName,
                false, AdditionalParameters.PerformClusterBuilder);
        }

        /// <summary>
        /// Validate BuildCluster() method with two unique match
        /// with cross overlap and validate the clusters
        /// Input : two unique matches with cross overlap
        /// Validation : Validate the unique matches
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ClusterBuilderTwoUniqueMatchesWithCrossOverlap()
        {
            ValidateFindMatchSuffixGeneralTestCases(
                Constants.TwoUniqueMatchWithCrossOverlapSequenceNodeName,
                false, AdditionalParameters.PerformClusterBuilder);
        }

        #endregion Suffix Tree Test Cases

        #region NUCmer Align Test Cases

        /// <summary>
        /// Validate Align() method with one line sequence 
        /// and validate the aligned sequences
        /// Input : One line sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void NUCmerAlignOneLineSequence()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.OneLineSequenceNodeName, false);
        }

        /// <summary>
        /// Validate Align() method with small size (less than 35kb) sequence 
        /// and validate the aligned sequences
        /// Input : small size sequence file
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void NUCmerAlignSmallSizeSequence()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.SmallSizeSequenceNodeName, true);
        }

        /// <summary>
        /// Validate Align(seq, seqList) method with small size (less than 35kb) sequence 
        /// and validate the aligned sequences
        /// Input : small size sequence file
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void NUCmerAlignSmallSizeAlignSequence()
        {
            // Gets the reference sequence from the FastA file
            var filePath = utilityObj.xmlUtil.GetTextValue(Constants.SmallSizeSequenceNodeName,
                Constants.FilePathNode).TestDir();

            Assert.IsNotNull(filePath);

            var parser = new FastAParser();
            var refSeqList = parser.Parse(filePath);

            // Gets the query sequence from the FastA file
            var queryFilePath = utilityObj.xmlUtil.GetTextValue(Constants.SmallSizeSequenceNodeName,
                Constants.SearchSequenceFilePathNode).TestDir();

            Assert.IsNotNull(queryFilePath);

            var queryParser = new FastAParser();
            var searchSeqList = queryParser.Parse(queryFilePath);

            var mumLength = utilityObj.xmlUtil.GetTextValue(Constants.SmallSizeSequenceNodeName, Constants.MUMAlignLengthNode);

            var nucmerObj = new NucmerPairwiseAligner
                {
                    MaximumSeparation = 0,
                    MinimumScore = 2,
                    SeparationFactor = 0.12f,
                    BreakLength = 2,
                    LengthOfMUM = long.Parse(mumLength, null)
                };

            var align = nucmerObj.Align(refSeqList.ElementAt(0), searchSeqList);

            var expectedSequences = utilityObj.xmlUtil.GetFileTextValue(Constants.SmallSizeSequenceNodeName,
                Constants.ExpectedSequencesNode);

            var expSeqArray = expectedSequences.Split(',');

            var j = 0;

            // Gets all the aligned sequences in comma separated format
            foreach (IPairwiseSequenceAlignment seqAlignment in align)
            {
                foreach (var alignedSeq in seqAlignment)
                {
                    Assert.AreEqual(expSeqArray[j], alignedSeq.FirstSequence.ConvertToString());
                    ++j;
                    Assert.AreEqual(expSeqArray[j], alignedSeq.SecondSequence.ConvertToString());
                    j++;
                }
            }
        }

        /// <summary>
        /// Validate Align() method with one line sequence 
        /// with cross over lap and validate the aligned sequences
        /// Input : One line sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void NUCmerAlignSequenceWithCrossOverlap()
        {
            ValidateNUCmerAlignGeneralTestCases(
                Constants.TwoUniqueMatchWithCrossOverlapSequenceNodeName, false);
        }

        /// <summary>
        /// Validate All properties in NUCmer class
        /// Input : Create a NUCmer object.
        /// Validation : Validate the properties
        /// </summary>
        [Category("Priority0")]
        public void NUCmerAlignerProperties()
        {
            var nucmerObj = new NucmerPairwiseAligner();
            Assert.AreEqual(Constants.NUCLength, nucmerObj.LengthOfMUM.ToString((IFormatProvider)null));
            Assert.AreEqual(200, nucmerObj.BreakLength);
            Assert.AreEqual(-8, nucmerObj.GapExtensionCost);
            Assert.AreEqual(-13, nucmerObj.GapOpenCost);
            Assert.AreEqual(Constants.NUCFixedSeperation, nucmerObj.FixedSeparation.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(Constants.NUCMaximumSeparation, nucmerObj.MaximumSeparation.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(Constants.NUCMinimumScore, nucmerObj.MinimumScore.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(Constants.NUCSeparationFactor, nucmerObj.SeparationFactor.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Validate GetClusters() method by passing valid values.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateNUCmerGetClusters()
        {
            // NOTE: Nigel ran this test with the same data through mmummer and mgaps and got the same result.

            // Gets the reference sequence from the FastA file
            var filePath = utilityObj.xmlUtil.GetTextValue(Constants.MediumSizeSequenceNodeName,
                Constants.FilePathNode).TestDir();

            // Gets the query sequence from the FastA file
            var queryFilePath = utilityObj.xmlUtil.GetTextValue(Constants.MediumSizeSequenceNodeName,
                Constants.SearchSequenceFilePathNode).TestDir();

            var parser = new FastAParser();
            var seqs1 = parser.Parse(filePath);
            var seqs2 = parser.Parse(queryFilePath);
            var nuc = new Bio.Algorithms.Alignment.NUCmer(seqs1.First()) {
                LengthOfMUM = 5,
                MinimumScore = 0,
            };
            var clusts = nuc.GetClusters(seqs2.First());
            var clustCount1 = utilityObj.xmlUtil.GetTextValue(
                Constants.MediumSizeSequenceNodeName, Constants.ClustCount1Node);

            Assert.AreEqual(clustCount1, clusts.Count.ToString(CultureInfo.InvariantCulture));
        }

        #endregion NUCmer Align Test Cases

        #region NUCmer Simple Align Test Cases

        /// <summary>
        /// Validate SimpleAlign() method with one line Dna list of sequence 
        /// and validate the aligned sequences
        /// Input : One line Dna list of sequence
        /// Validation : Validate the aligned sequences
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void NUCmerAlignSimpleOneLineDnaListOfSequence()
        {
            ValidateNUCmerAlignSimpleGeneralTestCases(Constants.SingleDnaNucmerSequenceNodeName, true);
        }

        /// <summary>
        /// Validate SimpleAlign() method with one line Rna list of sequence 
        /// and validate the aligned sequences
        /// Input : One line Rna list of sequence
        /// Validation : Validate the aligned sequences
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void NUCmerAlignSimpleOneLineRnaListOfSequence()
        {
            ValidateNUCmerAlignSimpleGeneralTestCases(Constants.SingleRnaNucmerSequenceNodeName, true);
        }

        #endregion NUCmer Simple Align Test Cases

        #region Supported Methods

        /// <summary>
        /// Validates most of the find matches suffix tree test cases with varying parameters.
        /// </summary>
        /// <param name="nodeName">Node name which needs to be read for execution.</param>
        /// <param name="isFilePath">Is File Path?</param>
        /// <param name="additionalParam">LIS action type enum</param>
        void ValidateFindMatchSuffixGeneralTestCases(string nodeName, bool isFilePath,
            AdditionalParameters additionalParam)
        {
            ISequence referenceSeqs;
            var searchSeqList = new List<ISequence>();

            if (isFilePath)
            {
                // Gets the reference sequence from the FastA file
                var filePath = utilityObj.xmlUtil.GetTextValue(nodeName,
                    Constants.FilePathNode).TestDir();

                Assert.IsNotNull(filePath);
                
                var parser = new FastAParser();
                var referenceSeqList = parser.Parse(filePath);
                var byteList = new List<Byte>();
                foreach (var seq in referenceSeqList)
                {
                    byteList.AddRange(seq);
                    byteList.Add((byte)'+');
                }
                referenceSeqs = new Sequence(referenceSeqList.First().Alphabet.GetMummerAlphabet(), byteList.ToArray());

                // Gets the query sequence from the FastA file
                var queryFilePath = utilityObj.xmlUtil.GetTextValue(nodeName,
                    Constants.SearchSequenceFilePathNode).TestDir();

                Assert.IsNotNull(queryFilePath);

                var querySeqList = parser.Parse(queryFilePath);
                searchSeqList.AddRange(querySeqList);
            }
            else
            {
                // Gets the reference & search sequences from the configuration file
                var referenceSequences = utilityObj.xmlUtil.GetTextValues(nodeName,
                    Constants.ReferenceSequencesNode);
                var searchSequences = utilityObj.xmlUtil.GetTextValues(nodeName,
                    Constants.SearchSequencesNode);

                var seqAlphabet = Utility.GetAlphabet(utilityObj.xmlUtil.GetTextValue(nodeName,
                       Constants.AlphabetNameNode));

                var refSeqList = referenceSequences.Select(t => new Sequence(seqAlphabet, encodingObj.GetBytes(t))).Cast<ISequence>().ToList();

                var byteListQuery = new List<Byte>();
                foreach (var seq in refSeqList)
                {
                    byteListQuery.AddRange(seq);
                    byteListQuery.Add((byte)'+');
                }
                referenceSeqs = new Sequence(refSeqList.First().Alphabet.GetMummerAlphabet(),
                    byteListQuery.ToArray());

                searchSeqList.AddRange(searchSequences.Select(t => new Sequence(seqAlphabet, encodingObj.GetBytes(t))).Cast<ISequence>());
            }

            var mumLength = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.MUMLengthNode);

            // Builds the suffix for the reference sequence passed.           
            var suffixTreeBuilder = new MultiWaySuffixTree(referenceSeqs as Sequence)
                {
                    MinLengthOfMatch = long.Parse(mumLength, null)
                };

            var matches = new Dictionary<ISequence, IEnumerable<Match>>();
            foreach (var sequence in searchSeqList)
            {
                matches.Add(sequence,
                    suffixTreeBuilder.SearchMatchesUniqueInReference(sequence));
            }

            var mums = new List<Match>();
            foreach (var a in matches.Values)
            {
                mums.AddRange(a);
            }

            switch (additionalParam)
            {
                case AdditionalParameters.FindUniqueMatches:
                    // Validates the Unique Matches.
                    Assert.IsTrue(ValidateUniqueMatches(mums, nodeName, additionalParam, isFilePath));
                    break;
                case AdditionalParameters.PerformClusterBuilder:
                    // Validates the Unique Matches.
                    Assert.IsTrue(ValidateUniqueMatches(mums, nodeName, additionalParam, isFilePath));
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Validates the NUCmer align method for several test cases for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node name to be read from xml</param>
        /// <param name="isFilePath">Is Sequence saved in File</param>
        void ValidateNUCmerAlignGeneralTestCases(string nodeName, bool isFilePath)
        {
            IList<ISequence> refSeqList, searchSeqList;

            if (isFilePath)
            {
                // Gets the reference sequence from the FastA file
                var filePath = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.FilePathNode).TestDir();
                Assert.IsNotNull(filePath);
                Assert.IsTrue(File.Exists(filePath));
                ApplicationLog.WriteLine(string.Format(null, "NUCmer BVT : Successfully validated the File Path '{0}'.", filePath));

                var parser = new FastAParser();
                refSeqList = parser.Parse(filePath).ToList();

                // Gets the query sequence from the FastA file
                var queryFilePath = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.SearchSequenceFilePathNode).TestDir();
                Assert.IsNotNull(queryFilePath);
                Assert.IsTrue(File.Exists(queryFilePath));
                ApplicationLog.WriteLine(string.Format(null, "NUCmer BVT : Successfully validated the File Path '{0}'.", queryFilePath));

                searchSeqList = parser.Parse(queryFilePath).ToList();
            }
            else
            {
                // Gets the reference & search sequences from the configuration file
                var seqAlphabet = Utility.GetAlphabet(utilityObj.xmlUtil.GetTextValue(nodeName, Constants.AlphabetNameNode));

                var sequences = utilityObj.xmlUtil.GetTextValues(nodeName, Constants.ReferenceSequencesNode);
                refSeqList = sequences.Select((t, i) => new Sequence(seqAlphabet, encodingObj.GetBytes(t)) {ID = "ref " + i}).Cast<ISequence>().ToList();

                sequences = utilityObj.xmlUtil.GetTextValues(nodeName, Constants.SearchSequencesNode);
                searchSeqList = sequences.Select((t, i) => new Sequence(seqAlphabet, encodingObj.GetBytes(t)) { ID = "qry " + i }).Cast<ISequence>().ToList();
            }

            var aligner = new NucmerPairwiseAligner
            {
                MaximumSeparation = 0,
                MinimumScore = 2,
                SeparationFactor = 0.12f,
                BreakLength = 2,
                LengthOfMUM = long.Parse(utilityObj.xmlUtil.GetTextValue(nodeName, Constants.MUMAlignLengthNode), null)
            };

            var align = aligner.Align(refSeqList, searchSeqList);

            var expectedSequences = isFilePath
                    ? utilityObj.xmlUtil.GetFileTextValue(nodeName, Constants.ExpectedSequencesNode)
                    : utilityObj.xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequencesNode);

            var expSeqArray = expectedSequences.Split(',');

            // Gets all the aligned sequences in comma separated format
            foreach (IPairwiseSequenceAlignment seqAlignment in align)
            {
                foreach (var alignedSeq in seqAlignment)
                {
                    var actualStr = alignedSeq.FirstSequence.ConvertToString();
                    Assert.IsTrue(expSeqArray.Contains(actualStr));

                    actualStr = alignedSeq.SecondSequence.ConvertToString();
                    Assert.IsTrue(expSeqArray.Contains(actualStr));
                }
            }
        }

        /// <summary>
        /// Validates the Unique Matches for the input provided.
        /// </summary>
        /// <param name="matches">Max Unique Match list</param>
        /// <param name="nodeName">Node name to be read from xml</param>
        /// <param name="additionalParam">Unique Match/Sub level LIS/LIS</param>
        /// <param name="isFilePath">Nodes to be read from Text file?</param>
        /// <returns>True, if successfully validated</returns>
        bool ValidateUniqueMatches(IList<Match> matches,
            string nodeName, AdditionalParameters additionalParam, bool isFilePath)
        {
            switch (additionalParam)
            {
                case AdditionalParameters.PerformClusterBuilder:
                    // Validates the Cluster builder MUMs
                    var firstSeqOrderExpected =
                        utilityObj.xmlUtil.GetTextValue(nodeName, Constants.ClustFirstSequenceStartNode);
                    var lengthExpected =
                        utilityObj.xmlUtil.GetTextValue(nodeName, Constants.ClustLengthNode);
                    var secondSeqOrderExpected =
                        utilityObj.xmlUtil.GetTextValue(nodeName, Constants.ClustSecondSequenceStartNode);

                    var firstSeqOrderActual = new StringBuilder();
                    var lengthActual = new StringBuilder();
                    var secondSeqOrderActual = new StringBuilder();

                    var cb = new ClusterBuilder { MinimumScore = 0 };

                    var meObj = matches.Select(m => new MatchExtension(m)).ToList();

                    // Order the mum list with query sequence order and
                    // Assign query sequence to the MUM's
                    for (var index = 0; index < meObj.Count(); index++)
                    {
                        meObj.ElementAt(index).ReferenceSequenceMumOrder = index + 1;
                        meObj.ElementAt(index).QuerySequenceMumOrder = index + 1;
                    }

                    var clusts = cb.BuildClusters(meObj);

                    foreach (var clust in clusts)
                    {
                        foreach (var maxMatchExtension in clust.Matches)
                        {
                            firstSeqOrderActual.Append(maxMatchExtension.ReferenceSequenceMumOrder);
                            secondSeqOrderActual.Append(maxMatchExtension.QuerySequenceMumOrder);
                            lengthActual.Append(maxMatchExtension.Length);
                        }
                    }

                    if ((0 != string.Compare(firstSeqOrderExpected.Replace(",", ""),
                        firstSeqOrderActual.ToString(), true, CultureInfo.CurrentCulture))
                        || (0 != string.Compare(lengthExpected.Replace(",", ""),
                        lengthActual.ToString(), true, CultureInfo.CurrentCulture))
                        || (0 != string.Compare(secondSeqOrderExpected.Replace(",", ""),
                        secondSeqOrderActual.ToString(), true, CultureInfo.CurrentCulture)))
                    {
                        ApplicationLog.WriteLine("NUCmer BVT : Unique match not matching");
                        return false;
                    }
                    break;
                case AdditionalParameters.FindUniqueMatches:
                    // Gets all the unique matches properties to be validated as in xml.
                    string[] firstSeqOrder = null;
                    string[] length = null;
                    string[] secondSeqOrder = null;

                    if (isFilePath)
                    {
                        firstSeqOrder = utilityObj.xmlUtil.GetFileTextValue(nodeName,
                            Constants.FirstSequenceMumOrderNode).Split(',');
                        length = utilityObj.xmlUtil.GetFileTextValue(nodeName,
                            Constants.LengthNode).Split(',');
                        secondSeqOrder = utilityObj.xmlUtil.GetFileTextValue(nodeName,
                            Constants.SecondSequenceMumOrderNode).Split(',');
                    }
                    else
                    {
                        firstSeqOrder = utilityObj.xmlUtil.GetTextValue(nodeName,
                            Constants.FirstSequenceMumOrderNode).Split(',');
                        length = utilityObj.xmlUtil.GetTextValue(nodeName,
                            Constants.LengthNode).Split(',');
                        secondSeqOrder = utilityObj.xmlUtil.GetTextValue(nodeName,
                            Constants.SecondSequenceMumOrderNode).Split(',');
                    }

                    var i = 0;

                    IList<MatchExtension> meNewObj = matches.Select(m => new MatchExtension(m)).ToList();

                    // Order the mum list with query sequence order and
                    // Assign query sequence to the MUM's
                    for (var index = 0; index < meNewObj.Count(); index++)
                    {
                        meNewObj.ElementAt(index).ReferenceSequenceMumOrder = index + 1;
                        meNewObj.ElementAt(index).QuerySequenceMumOrder = index + 1;
                    }

                    // Loops through all the matches and validates the same.
                    foreach (var match in meNewObj)
                    {
                        if ((0 != string.Compare(firstSeqOrder[i],
                            match.ReferenceSequenceMumOrder.ToString((IFormatProvider)null), true,
                            CultureInfo.CurrentCulture))
                            || (0 != string.Compare(length[i],
                            match.Length.ToString((IFormatProvider)null), true,
                            CultureInfo.CurrentCulture))
                            || (0 != string.Compare(secondSeqOrder[i],
                            match.QuerySequenceMumOrder.ToString((IFormatProvider)null), true,
                            CultureInfo.CurrentCulture)))
                        {
                            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                                "NUCmer BVT : Unique match not matching at index '{0}'", i.ToString((IFormatProvider)null)));
                            return false;
                        }
                        i++;
                    }
                    break;
                default:
                    break;
            }
            return true;
        }

        /// <summary>
        /// Validates the NUCmer align method for several test cases for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node name to be read from xml</param>
        /// <param name="isAlignList">Is align method to take list?</param>
        void ValidateNUCmerAlignSimpleGeneralTestCases(string nodeName,
            bool isAlignList)
        {
            string[] referenceSequences = null;
            string[] searchSequences = null;
            var refSeqList = new List<ISequence>();
            var searchSeqList = new List<ISequence>();

            // Gets the reference & search sequences from the configurtion file
            referenceSequences = utilityObj.xmlUtil.GetTextValues(nodeName,
                Constants.ReferenceSequencesNode);
            searchSequences = utilityObj.xmlUtil.GetTextValues(nodeName,
              Constants.SearchSequencesNode);

            var seqAlphabet = Utility.GetAlphabet(
                utilityObj.xmlUtil.GetTextValue(nodeName,
                Constants.AlphabetNameNode));

            for (var i = 0; i < referenceSequences.Length; i++)
            {
                ISequence referSeq = new Sequence(seqAlphabet,
                    encodingObj.GetBytes(referenceSequences[i]));
                refSeqList.Add(referSeq);
            }

            for (var i = 0; i < searchSequences.Length; i++)
            {
                ISequence searchSeq = new Sequence(seqAlphabet,
                    encodingObj.GetBytes(searchSequences[i]));
                searchSeqList.Add(searchSeq);
            }

            // Gets the mum length from the xml
            var mumLength = utilityObj.xmlUtil.GetTextValue(nodeName,
                Constants.MUMAlignLengthNode);

            var nucmerObj = new NucmerPairwiseAligner();

            // Update other values for NUCmer object
            nucmerObj.MaximumSeparation = int.Parse
                (utilityObj.xmlUtil.GetTextValue(nodeName, Constants.MUMAlignLengthNode), (IFormatProvider)null);
            nucmerObj.MinimumScore = int.Parse(
                utilityObj.xmlUtil.GetTextValue(nodeName, Constants.MUMAlignLengthNode), (IFormatProvider)null);
            nucmerObj.SeparationFactor = int.Parse(
                utilityObj.xmlUtil.GetTextValue(nodeName, Constants.MUMAlignLengthNode), (IFormatProvider)null);
            nucmerObj.BreakLength = int.Parse(
                utilityObj.xmlUtil.GetTextValue(nodeName, Constants.MUMAlignLengthNode), (IFormatProvider)null);
            nucmerObj.LengthOfMUM = long.Parse(mumLength, null);

            IList<ISequenceAlignment> alignSimple = null;

            if (isAlignList)
            {
                var listOfSeq = new List<ISequence>
                {
                    refSeqList[0],
                    searchSeqList[0]
                };
                alignSimple = nucmerObj.AlignSimple(listOfSeq);
            }

            var expectedSequences = string.Empty;
            expectedSequences = utilityObj.xmlUtil.GetTextValue(nodeName,
                Constants.ExpectedSequencesNode);

            var expSeqArray = expectedSequences.Split(',');

            var j = 0;

            // Gets all the aligned sequences in comma seperated format
            foreach (IPairwiseSequenceAlignment seqAlignment in alignSimple)
            {
                foreach (var alignedSeq in seqAlignment)
                {
                    Assert.AreEqual(expSeqArray[j], new string(alignedSeq.FirstSequence.Select(a => (char)a).ToArray()));
                    ++j;
                    Assert.AreEqual(expSeqArray[j], new string(alignedSeq.SecondSequence.Select(a => (char)a).ToArray()));
                    j++;
                }
            }

            ApplicationLog.WriteLine("NUCmer BVT : Successfully validated all the aligned sequences.");
        }

        #endregion Supported Methods
    }
}
