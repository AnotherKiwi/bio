using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bio;
using Bio.IO.Snp;
using NUnit.Framework;

namespace Bio.Tests.IO.Snp
{
    ///<summary>
    /// Snp parser test cases
    ///</summary>
    [TestFixture]
    public class SnpParserTests
    {
        private readonly string snpFileName = @"TestUtils\SnpFile.tsv".TestDir();

        /// <summary>
        /// Parse a Snp file
        /// Input : Snp File
        /// Validation : Expected sequence, Chromosome position, Sequence Alphabet, Sequence ID.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void SnpParserTest()
        {
            SnpParserGeneralTestCases("ParseFilePath");
        }

        /// <summary>
        /// Parse a  Snp file and ParseAlleleOne property set to false
        /// and validate with the expected sequence.
        /// Input : Snp File
        /// Validation : Expected sequence, Chromosome position, Sequence Alphabet, Sequence ID.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void SnpParsereParseAlleleTwoTest()
        {
            SnpParserGeneralTestCases("ParseAlleleTwo");
        }

        /// <summary>
        /// Validate All properties in Snp parser class
        /// Validation : Validate the properties
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void SnpParserPropertiesTest()
        {
            var snpParser = new SimpleSnpParser(Alphabets.DNA);
            Assert.AreEqual("Basic SNP Parser that uses XSV format", snpParser.Description);
            Assert.AreEqual("Basic SNP", snpParser.Name);
            Assert.IsTrue(snpParser.ParseAlleleOne);
            Assert.AreEqual(".tsv", snpParser.SupportedFileTypes);
        }

        /// <summary>
        /// Validate Equals(snpItem) method with valid values.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void SnpItemValidateEqualsSnpItem()
        {
            var snpItem1 = new SnpItem();
            snpItem1.AlleleOne = 'A';
            snpItem1.AlleleTwo = 'G';

            var snpItem2 = new SnpItem();
            snpItem2.AlleleOne = 'A';
            snpItem2.AlleleTwo = 'G';

            Assert.IsTrue(snpItem2.Equals(snpItem1));
        }

        /// <summary>
        /// Validate Equals(object) method with valid values.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void SnpItemValidateEqualsObject()
        {
            var snpItem1 = new SnpItem();
            snpItem1.AlleleOne = 'A';
            snpItem1.AlleleTwo = 'G';

            var snpItem2 = new SnpItem();
            snpItem2.AlleleOne = 'A';
            snpItem2.AlleleTwo = 'G';

            Assert.IsTrue(snpItem2.Equals((object)snpItem1));
        }

        /// <summary>
        /// Snp parser generic method called by all the test cases 
        /// to validate the test case based on the parameters passed.
        /// </summary>
        /// <param name="switchParam">Additional parameter 
        /// based on which the validation of  test case is done.</param>
        void SnpParserGeneralTestCases(string switchParam)
        {
            Assert.IsTrue(File.Exists(snpFileName));

            IList<ISequence> seqList = null;
            SparseSequence sparseSeq = null;
            var parser = new SimpleSnpParser();

            var expectedPosition = "45162,72434,145160,172534,245162,292534";

            var expectedPositions = expectedPosition.Split(',');
            string[] expectedCharacters = null;

            switch (switchParam)
            {
                case "ParseAlleleTwo":
                    parser.ParseAlleleOne = false;
                    var expectedAlleleTwoSequence = "T,A,T,C,T,A";
                        
                    expectedCharacters = expectedAlleleTwoSequence.Split(',');
                    break;
                default:
                    var expectedSequence = "C,G,A,G,C,G";
                    expectedCharacters = expectedSequence.Split(',');
                    break;
            }

            seqList = parser.Parse(snpFileName).ToList();
                    sparseSeq = (SparseSequence)seqList[0];

            if (null == sparseSeq)
            {
                Assert.IsNotNull(seqList);
                Assert.AreEqual(1, seqList.Count);
            }

            for (var i = 0; i < expectedPositions.Length; i++)
            {
                var item = sparseSeq[int.Parse(expectedPositions[i], null)];
                var s = (char)item;
                Assert.AreEqual(expectedCharacters[i], s.ToString());
            }

            Assert.IsNotNull(sparseSeq.Alphabet);
            Assert.AreEqual(sparseSeq.Alphabet.Name.ToLower(), AmbiguousDnaAlphabet.Instance.Name.ToLower());
            var expSequenceID = "Chr1";
            Assert.AreEqual(expSequenceID, sparseSeq.ID);
        }
    }
}
