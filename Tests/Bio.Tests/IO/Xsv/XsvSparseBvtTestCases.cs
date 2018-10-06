using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using Bio.Algorithms.Assembly;
using Bio.IO.Xsv;
using Bio.TestAutomation.Util;
using Bio.Util.Logging;

using NUnit.Framework;

namespace Bio.Tests.IO.Xsv
{
    /// <summary>
    /// XsvSparse Bvt parser Test case implementation.
    /// </summary>
    [TestFixture]
    public class XsvSparseBvtTestCases
    {
        #region Enums

        /// <summary>
        /// Additional Parameters which are used for different test cases 
        /// based on which the test cases are executed.
        /// </summary>
        enum AdditionalParameters
        {
            FormatFilePath,
            ForamtListWithFilePath
        };

        #endregion Enums

        Utility utilityObj = new Utility(@"TestUtils\TestsConfig.xml");

        #region Parser Test cases

        /// <summary>
        /// Parse a valid XsvSparse file (Small size sequence less than 35 kb) and convert the 
        /// same to one sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : XsvSparse File
        /// Validation : Expected sequence, Sequence Alphabet, Sequence ID.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void XsvSparseBvtParserValidateParseFilePath()
        {
            XsvSparseParserGeneralTestCases(Constants.SimpleXsvSparseNodeName);
        }

        /// <summary>
        /// Validate All properties in XsvSparse parser class
        /// Input : One line sequence and update all properties
        /// Validation : Validate the properties
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void XsvSparseBvtParserProperties()
        {
            var xsvParser = new XsvContigParser(Alphabets.DNA, ',', '#');
            Assert.AreEqual(Constants.XsvSparseDescription, xsvParser.Description);
            Assert.AreEqual(Constants.XsvSparseFileTypes, xsvParser.SupportedFileTypes);
            Assert.AreEqual(Constants.XsvSparseName, xsvParser.Name);

            ApplicationLog.WriteLine
                ("Successfully validated all the properties of XsvSparse Parser class.");
        }

        /// <summary>
        /// Validate SparseContigFormatter Format()
        /// Input : Xsv file.
        /// Validation : Validation of Format() method.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void XsvSparseParseContig()
        {
            // Gets the expected file from the Xml
            var filePathObj = utilityObj.xmlUtil.GetTextValue(
                Constants.SimpleXsvSparseNodeName,
                Constants.FilePathNode).TestDir();

            Assert.IsTrue(File.Exists(filePathObj));
            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "XsvSparse Formatter BVT: File Exists in the Path '{0}'.",
                filePathObj));

            var parserObj = new XsvContigParser(Alphabets.DNA, ',', '#');
            parserObj.Parse(filePathObj);

            var contig = parserObj.ParseContig(filePathObj);

            // Validate parsed temp file with original Xsv file.
            Assert.AreEqual(26048682, contig.Length);
            Assert.AreEqual(26048682, contig.Consensus.Count);
            Assert.AreEqual("Chr22+Chr22+Chr22+Chr22", contig.Consensus.ID);
            Assert.AreEqual(56, contig.Sequences.Count);

            // Log to GUI.
            ApplicationLog.WriteLine("Successfully validated the ParseConting() method with Xsv file");
        }

        #endregion Test cases

        #region Formatter Test cases

        /// <summary>
        /// Parse a valid XsvSparse file (Small size sequence less than 35 kb) and convert the 
        /// same to one sequence using Parse(file-name) method and format back using 
        /// Format(isequence, filename)
        /// Input : XsvSparse File
        /// Validation : Format is successful.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void XsvSparseBvtFormatterValidateFilePath()
        {
            XsvSparseFormatterGeneralTestCases(Constants.SimpleXsvSparseNodeName,
                AdditionalParameters.FormatFilePath);
        }

        /// <summary>
        /// Parse a valid XsvSparse file (Small size sequence less than 35 kb) and convert the 
        /// same to one sequence using Parse(file-name) method and format back using 
        /// Format(ListSequence, filename)
        /// Input : XsvSparse File
        /// Validation : Format is successful.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void XsvSparseBvtFormatterValidateFilePathWithSeqList()
        {
            XsvSparseFormatterGeneralTestCases(Constants.SimpleXsvSparseNodeName,
                AdditionalParameters.ForamtListWithFilePath);
        }

        /// <summary>
        /// Validate All properties in XsvSparse formatter class
        /// Input : One line sequence and update all properties
        /// Validation : Validate the properties
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void XsvSparseBvtFormatterProperties()
        {
            var formatterObj = new XsvSparseFormatter(Constants.CharSeperator,
                Constants.SequenceIDPrefix);
            Assert.AreEqual(Constants.XsvSparseFormatterDescription, formatterObj.Description);
            Assert.AreEqual(Constants.XsvSparseFileTypes, formatterObj.SupportedFileTypes);
            Assert.AreEqual(Constants.XsvSparseFormatterNode, formatterObj.Name);
            Assert.AreEqual(Constants.XsvSeperator, formatterObj.Separator);
            Assert.AreEqual(Constants.XsvSeqIdPrefix, formatterObj.SequenceIDPrefix);

            ApplicationLog.WriteLine
                ("Successfully validated all the properties of XsvSparse Formatter class.");
        }

        /// <summary>
        /// Validate XsvContigFormatter Write()
        /// Input : Xsv file.
        /// Validation : Validation of Write() method.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void XsvSparseContigFormatterWrite()
        {
            // Gets the expected sequence from the Xml
            var filePathObj = utilityObj.xmlUtil.GetTextValue(
                Constants.SimpleXsvSparseNodeName,
                Constants.FilePathNode).TestDir();

            Assert.IsTrue(File.Exists(filePathObj));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Xsv Contig Formatter BVT: File Exists in the Path '{0}'.",
                filePathObj));
            Contig expectedContig;

            var parserObj = new XsvContigParser(Alphabets.DNA, ',', '#');
            var contig = parserObj.ParseContig(filePathObj);                
            
            var seqId = contig.Sequences.Aggregate(string.Empty, (current, seq) => current + (seq.Sequence.ID + ","));

            // Write Xsv file.            
            var formatObj = new XsvContigFormatter(',', '#');
            formatObj.Format(contig, Constants.XsvTempFileName);

            var parserObjNew = new XsvContigParser(Alphabets.DNA, ',', '#');
            expectedContig = parserObjNew.ParseContig(Constants.XsvTempFileName);
             
            var expectedseqId = expectedContig.Sequences.Aggregate(string.Empty, (current, seq) => current + (seq.Sequence.ID + ","));

            // Validate parsed temp file with original Xsv file.
            Assert.AreEqual(contig.Length, expectedContig.Length);
            Assert.AreEqual(contig.Consensus.Count, expectedContig.Consensus.Count);            
            Assert.AreEqual(contig.Consensus.ID, expectedContig.Consensus.ID);
            Assert.AreEqual(contig.Sequences.Count, expectedContig.Sequences.Count);
            Assert.AreEqual(seqId.Length, expectedseqId.Length);
            Assert.AreEqual(seqId, expectedseqId);
            File.Delete(Constants.XsvTempFileName);
            ApplicationLog.WriteLine("Successfully validated the Write Xsv file");
        }

        #endregion

        #region Supporting Methods

        /// <summary>
        /// XsvSparse parser generic method called by all the test cases 
        /// to validate the test case based on the parameters passed.
        /// </summary>
        /// <param name="nodename">Xml node Name.</param>
        /// based on which the validation of  test case is done.</param>
        void XsvSparseParserGeneralTestCases(string nodename)
        {
            // Gets the expected file from the Xml
            var filePathObj = utilityObj.xmlUtil.GetTextValue(nodename,
                Constants.FilePathNode).TestDir();

            Assert.IsTrue(File.Exists(filePathObj));
            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "XsvSparse Parser BVT: File Exists in the Path '{0}'.",
                filePathObj));

            IEnumerable<ISequence> seqList = null;
            SparseSequence sparseSeq = null;
            var parserObj = new XsvContigParser(Alphabets.DNA, Constants.CharSeperator, Constants.SequenceIDPrefix);

            var expectedSeqIds = utilityObj.xmlUtil.GetTextValue(nodename,
                Constants.SequenceIdNode);

            seqList = parserObj.Parse(filePathObj);
            sparseSeq = (SparseSequence)seqList.ElementAt(0);

            if (null == sparseSeq)
            {
                var expCount = utilityObj.xmlUtil.GetTextValue(nodename,
                    Constants.SequenceCountNode);

                Assert.IsNotNull(seqList);
                Assert.AreEqual(expCount, seqList.Count());
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "XsvSparse Parser BVT: Number of Sequences found are '{0}'.",
                    expCount));

                var expectedSeqIdArray = expectedSeqIds.Split(',');

                var sparseSeqList = seqList.ToList();
                for (var i = 0; i < expectedSeqIdArray.Length; i++)
                {
                    Assert.AreEqual(expectedSeqIdArray[i], sparseSeqList[i].ID);
                }
            }
            else
            {
                var idArray = expectedSeqIds.Split(',');
                Assert.AreEqual(sparseSeq.ID, idArray[0]);
            }

            ApplicationLog.WriteLine(
                "XsvSparse Parser BVT: The XsvSparse sequence is validated successfully with Parse() method.");

            Assert.IsNotNull(sparseSeq.Alphabet);
            Assert.AreEqual(sparseSeq.Alphabet.Name.ToLower(CultureInfo.InvariantCulture),
                utilityObj.xmlUtil.GetTextValue(nodename,
                Constants.AlphabetNameNode).ToLower(CultureInfo.InvariantCulture));

            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "XsvSparse Parser BVT: The Sequence Alphabet is '{0}' and is as expected.",
                sparseSeq.Alphabet.Name));
        }

        /// <summary>
        /// XsvSparse formatter generic method called by all the test cases 
        /// to validate the test case based on the parameters passed.
        /// </summary>
        /// <param name="nodename">Xml node Name.</param>
        /// <param name="additionalParam">Additional parameter 
        /// based on which the validation of  test case is done.</param>
        void XsvSparseFormatterGeneralTestCases(string nodename,
            AdditionalParameters additionalParam)
        {
            // Gets the expected sequence from the Xml
            var filePathObj = utilityObj.xmlUtil.GetTextValue(nodename,
                Constants.FilePathNode).TestDir();

            Assert.IsTrue(File.Exists(filePathObj));
            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "XsvSparse Formatter BVT: File Exists in the Path '{0}'.",
                filePathObj));

            IEnumerable<ISequence> seqList = null;
            SparseSequence sparseSeq = null;
            var parserObj = new XsvContigParser(Alphabets.DNA, Constants.CharSeperator, Constants.SequenceIDPrefix);

            seqList = parserObj.Parse(filePathObj);
            sparseSeq = (SparseSequence)seqList.ElementAt(0);

            var sparseSeqItems = sparseSeq.GetKnownSequenceItems();

            var tempFile = Path.GetTempFileName();

            var formatterObj = new XsvSparseFormatter(Constants.CharSeperator, Constants.SequenceIDPrefix);
            switch (additionalParam)
            {
                case AdditionalParameters.FormatFilePath:
                    formatterObj.Format(sparseSeq, tempFile);
                    break;
                default:
                    break;
                case AdditionalParameters.ForamtListWithFilePath:
                    formatterObj.Format(seqList.ToList(), tempFile);
                    break;
            }
            var newParserObj = new XsvContigParser(Alphabets.DNA, Constants.CharSeperator, Constants.SequenceIDPrefix);
            // Parse a formatted Xsv file and validate.
            seqList = newParserObj.Parse(filePathObj);
            var expectedSeq = (SparseSequence)seqList.ElementAt(0);

            var expectedSparseSeqItems = expectedSeq.GetKnownSequenceItems();

            for (var i = 0; i < sparseSeqItems.Count; i++)
            {
                var seqItem = sparseSeqItems[i];
                var expectedSeqItem = expectedSparseSeqItems[i];
                Assert.AreEqual(seqItem.Index, expectedSeqItem.Index);
            }

            ApplicationLog.WriteLine("Successfully validated the format Xsv file");
        }

        #endregion Supporting Methods
    }
}
