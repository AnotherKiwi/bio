using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Bio.Extensions;
using Bio.IO.FastA;
using Bio.TestAutomation.Util;
using Bio.Util.Logging;

using NUnit.Framework;

namespace Bio.Tests
{
    /// <summary>
    /// Test Automation code for Bio Sequences and BVT level validations.
    /// </summary>
    [TestFixture]
    public class SequenceBvtTestCases
    {
        #region Enum

        /// <summary>
        /// Sequence method types to validate different test cases
        /// </summary>
        private enum SequenceMethods
        {
            Complement,
            Reverse,
            ReverseComplement
        }

        #endregion

        readonly Utility utilityObj = new Utility(@"TestUtils\TestsConfig.xml");

        #region Sequence Bvt TestCases

        /// <summary>
        /// Validate a creation of DNA Sequence by passing valid Single Character sequence.
        /// Input Data : Valid DNA Sequence with single character - "A".
        /// Output Data : Validation of created DNA Sequence.
        /// </summary>
        [Category("Priority0")]
        public void ValidateSingleCharDnaSequence()
        {
            // Gets the actual sequence and the alphabet from the Xml
            var alphabetName = utilityObj.xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            var actualSequence = utilityObj.xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.ExpectedSingleChar);

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence is as expected."));

            var createSequence = new Sequence(
                Utility.GetAlphabet(alphabetName), actualSequence);

            Assert.IsNotNull(createSequence);

            // Validate the createdSequence            
            var seqNew = new string(createSequence.Select(a => (char)a).ToArray());
            Assert.AreEqual(seqNew, actualSequence);

            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence is as expected."));

            Assert.AreEqual(Utility.GetAlphabet(alphabetName), createSequence.Alphabet);
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence Alphabet is as expected."));

            ApplicationLog.WriteLine(
                "Sequence BVT: The DNA with single character Sequence is completed successfully.");
        }

        /// <summary>
        /// Validate a creation of DNA Sequence by passing valid string.
        /// Input Data: Valid DNA sequence "ACGA".
        /// Output Data : Validation of created DNA Sequence.
        /// </summary>
        [Category("Priority0")]
        public void ValidateDnaSequence()
        {

            // Gets the actual sequence and the alphabet from the Xml
            var alphabetName = utilityObj.xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            var actualSequence = utilityObj.xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.ExpectedNormalString);

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence ", actualSequence, " and Alphabet ", alphabetName));

            ISequence createSequence = new Sequence(Utility.GetAlphabet(alphabetName),actualSequence);
            Assert.IsNotNull(createSequence);

            var seqNew = createSequence.ConvertToString();

            // Validate the createdSequence
            Assert.AreEqual(seqNew, actualSequence);
            ApplicationLog.WriteLine("Sequence BVT: Sequence is as expected.");

            Assert.AreEqual(Utility.GetAlphabet(alphabetName), createSequence.Alphabet);
            ApplicationLog.WriteLine("Sequence BVT: Sequence Alphabet is as expected.");

            ApplicationLog.WriteLine("Sequence BVT: The DNA Sequence with string is created successfully.");
        }

        /// <summary>
        /// Validate a creation of RNA Sequence by passing valid string sequence.
        /// Input Data : Valid RNA Sequence "GAUUCAAGGGCU".
        /// Output Data : Validation of created RNA sequence.
        /// </summary>
        [Category("Priority0")]
        public void ValidateRnaSequence()
        {
            // Gets the actual sequence and the alphabet from the Xml
            var alphabetName = utilityObj.xmlUtil.GetTextValue(
                Constants.SimpleRnaAlphabetNode, Constants.AlphabetNameNode);
            var actualSequence = utilityObj.xmlUtil.GetTextValue(
                Constants.SimpleRnaAlphabetNode, Constants.ExpectedNormalString);

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence ", actualSequence, " and Alphabet ", alphabetName));

            var createSequence =new Sequence(Utility.GetAlphabet(alphabetName),
                    actualSequence);
            Assert.IsNotNull(createSequence);

            // Validate the createdSequence
            var seqNew = new string(createSequence.Select(a => (char)a).ToArray());
            Assert.AreEqual(seqNew, actualSequence);
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence is as expected."));

            Assert.AreEqual(Utility.GetAlphabet(alphabetName), createSequence.Alphabet);
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence Alphabet is as expected."));
            ApplicationLog.WriteLine(
                "Sequence BVT: The RNA Sequence is created successfully.");
        }

        /// <summary>
        /// Validate a creation of Protein Sequence by passing valid string sequence.
        /// Input Data : Valid Protein sequece "AGTN".
        /// Output Data : Validation of created Protein sequence.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateProteinSequence()
        {
            // Gets the actual sequence and the alphabet from the Xml
            var alphabetName = utilityObj.xmlUtil.GetTextValue(
                Constants.SimpleProteinAlphabetNode, Constants.AlphabetNameNode);
            var actualSequence = utilityObj.xmlUtil.GetTextValue(
                Constants.SimpleProteinAlphabetNode, Constants.ExpectedNormalString);

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence ", actualSequence, " and Alphabet ", alphabetName));

            var createSequence = new Sequence(
                Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsNotNull(createSequence);

            // Validate the createdSequence
            var seqNew = new string(createSequence.Select(a => (char)a).ToArray());
            Assert.AreEqual(seqNew, actualSequence);

            Assert.AreEqual(Utility.GetAlphabet(alphabetName), createSequence.Alphabet);

            ApplicationLog.WriteLine("Sequence BVT: The Protein Sequence is created successfully.");
        }

        /// <summary>
        /// Validate a Sequence creation for a given FastaA file.
        /// Input Data : Valid FastaA file sequence.
        /// Output Data : Validation of FastaA file sequence.
        /// </summary>
        [Category("Priority0")]
        public void ValidateFastaAFileSequence()
        {
            // Gets the expected sequence from the Xml
            var expectedSequence = utilityObj.xmlUtil.GetTextValue(
                Constants.SimpleFastaNodeName, Constants.ExpectedSequenceNode);
            var fastAFilePath = utilityObj.xmlUtil.GetTextValue(
                Constants.SimpleFastaNodeName, Constants.FilePathNode);
            var alphabet = utilityObj.xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.AlphabetNameNode);
            Assert.IsTrue(File.Exists(fastAFilePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: The File exist in the Path ", fastAFilePath));

            IEnumerable<ISequence> sequence = null;
            var parser = new FastAParser();
            {
                // Parse a FastA file Using Parse method and convert the same to sequence.
                parser.Alphabet = Utility.GetAlphabet(alphabet);
                sequence = parser.Parse(fastAFilePath);

                Assert.IsNotNull(sequence);
                var fastASequence = (Sequence)sequence.ElementAt(0);
                Assert.IsNotNull(fastASequence);

                var seqString = sequence.ElementAt(0).Select(a => (char)a).ToArray();
                var newSequence = new string(seqString);

                Assert.AreEqual(expectedSequence, newSequence);
                ApplicationLog.WriteLine(string.Concat(
                    "Sequence BVT: The Sequence is as expected."));

                var tmpEncodedSeq = new byte[fastASequence.Count];
                (fastASequence as IEnumerable<byte>).ToArray().CopyTo(tmpEncodedSeq, 0);

                Assert.AreEqual(expectedSequence.Length, tmpEncodedSeq.Length);
                ApplicationLog.WriteLine(string.Concat(
                    "Sequence BVT: Sequence Length is as expected."));

                Assert.AreEqual(utilityObj.xmlUtil.GetTextValue(
                    Constants.SimpleProteinAlphabetNode, Constants.SequenceIdNode), fastASequence.ID);
                ApplicationLog.WriteLine(string.Concat(
                    "Sequence BVT: SequenceID is as expected."));


                Assert.AreEqual(fastASequence.Alphabet.Name,
                    utilityObj.xmlUtil.GetTextValue(Constants.SimpleFastaNodeName, Constants.AlphabetNameNode));
                ApplicationLog.WriteLine(string.Concat(
                    "Sequence BVT: Sequence Alphabet is as expected."));
            }
        }

        /// <summary>
        /// Validates GetReversedSequence method for a given Dna Sequence.
        /// Input Data: AGTACAGCTCCAGACGT
        /// Output Data : Reverse of Input Sequence.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateGetReversedSequence()
        {
            ValidateSequences(Constants.DnaDerivedSequenceNode, SequenceMethods.Reverse);
        }

        /// <summary>
        /// Validates GetReverseComplementedSequence method for a given Dna Sequence.
        /// Input Data: AGTACAGCTCCAGACGT
        /// Output Data : Reverse complement of Input Sequence. 
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateGetReverseComplementedSequence()
        {
            ValidateSequences(Constants.DnaDerivedSequenceNode, SequenceMethods.ReverseComplement);
        }

        /// <summary>
        /// Validates GetComplementedSequence method for a given Dna Sequence.
        /// Input Data: AGTACAGCTCCAGACGT
        /// Output Data : Complement of Input Dna sequence.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateGetComplementSequence()
        {
            ValidateSequences(Constants.DnaDerivedSequenceNode, SequenceMethods.Complement);
        }

        /// <summary>
        /// Validates Sequence Constructor for a given Dna Sequence.
        /// Input Data: AGTACAGCTCCAGACGT
        /// Output Data : Validation of created Sequence.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateSequenceConstructor()
        {
            // Get input and expected values from xml
            var alphabetName = utilityObj.xmlUtil.GetTextValue(
                Constants.DnaDerivedSequenceNode, Constants.AlphabetNameNode);
            var expectedSequence = utilityObj.xmlUtil.GetTextValue(
                Constants.DnaDerivedSequenceNode, Constants.ExpectedDerivedSequence);
            var alphabet = Utility.GetAlphabet(alphabetName);

            var byteArray = new List<byte[]>
            {
                 Encoding.UTF8.GetBytes(expectedSequence),                 
            };

            // Validating Constructor.
            var constructorSequence = new Sequence(alphabet, byteArray[0]);
            Assert.AreEqual(expectedSequence,
                        new string(constructorSequence.Select(a => (char)a).ToArray()));
            ApplicationLog.WriteLine(string.Concat(
                    "Sequence BVT: Validation of Sequence Constructor completed successfully."));
        }

        /// <summary>
        /// Validates Enumeration in a Sequence.
        /// Input Data: AGTACAGCTCCAGACGT
        /// Output Data : Validation of enumeration.
        [Test]
        [Category("Priority0")]
        public void ValidateEnumerator()
        {
            var alphabetName = utilityObj.xmlUtil.GetTextValue(
                                 Constants.DnaDerivedSequenceNode, Constants.AlphabetNameNode);
            var alphabet = Utility.GetAlphabet(alphabetName);
            var expectedSequence = (utilityObj.xmlUtil.GetTextValue(
                            Constants.DnaDerivedSequenceNode, Constants.ExpectedDerivedSequence));
            var sequence = new Sequence(alphabet, expectedSequence);
            //Validate Count
            Assert.AreEqual(Encoding.UTF8.GetByteCount(expectedSequence), sequence.Count);
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Validation of Count operation completed successfully."));

            //Validate Enumerator.
            var sequenceString = "";
            var enumFromSequence = sequence.GetEnumerator();
            while (enumFromSequence.MoveNext())
            {
                sequenceString += ((char)enumFromSequence.Current);
            }

            Assert.AreEqual(sequenceString, expectedSequence);
            ApplicationLog.WriteLine(string.Concat(
              "Sequence BVT: Validation of Enumerator operation completed successfully."));
        }

        /// <summary>
        /// Validate Sequence LastIndexOfNonGap().
        /// Input data : Sequence.
        /// Output Data : Validation of LastIndexOfNonGap() method.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateSequenceLastIndexOfNonGap()
        {
            // Get input and expected values from xml
            var expectedSequence = utilityObj.xmlUtil.GetTextValue(Constants.RnaDerivedSequenceNode,
                Constants.ExpectedSequence);
            var alphabetName = utilityObj.xmlUtil.GetTextValue(Constants.RnaDerivedSequenceNode,
                Constants.AlphabetNameNode);

            var alphabet = Utility.GetAlphabet(alphabetName);

            // Create a Sequence object.
            var seqObj =
                new Sequence(alphabet, expectedSequence);

            var index = seqObj.LastIndexOfNonGap();

            Assert.AreEqual(expectedSequence.Length - 1, index);
        }

        /// <summary>
        /// Validates CopyTo
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateCopyTo()
        {
            // Get input and expected values from xml
            var expectedSequence = utilityObj.xmlUtil.GetTextValue(Constants.RnaDerivedSequenceNode,
                Constants.ExpectedSequence);
            var alphabetName = utilityObj.xmlUtil.GetTextValue(Constants.RnaDerivedSequenceNode,
                Constants.AlphabetNameNode);
            var alphabet = Utility.GetAlphabet(alphabetName);

            // Create a Sequence object.
            ISequence iseqObj =
                new Sequence(alphabet, expectedSequence);
            var seqObj = new Sequence(iseqObj);
            var array = new byte[expectedSequence.Length];
            seqObj.CopyTo(array, 0, expectedSequence.Length);
            var builder = new StringBuilder();
            for (var i = 0; i < expectedSequence.Length; i++)
            {
                builder.Append((char)array[i]);
            }
            var actualValue = builder.ToString();
            Assert.AreEqual(expectedSequence, actualValue);

            //check with a part of the expected seq only
            seqObj.CopyTo(array, 0, 5);
            builder = new StringBuilder();
            for (var i = 0; i < 5; i++)
            {
                builder.Append((char)array[i]);
            }
            actualValue = builder.ToString();
            Assert.AreEqual(expectedSequence.Substring(0, 5), actualValue);
        }

        #endregion Sequence Bvt TestCases
        
        #region Supporting Methods

        /// <summary>
        /// Supporting method for validating Sequence operations.
        /// Input Data: Parent node,child node and Enum. 
        /// Output Data : Validation of public methods in Sequence class.
        /// </summary>
        void ValidateSequences(string parentNode, SequenceMethods option)
        {
            var alphabetName = utilityObj.xmlUtil.GetTextValue(
                                 parentNode, Constants.AlphabetNameNode);
            var alphabet = Utility.GetAlphabet(alphabetName);
            ISequence seq = null;
            var expectedValue = "";
            ISequence sequence = new Sequence(alphabet, Encoding.UTF8.GetBytes(
                                utilityObj.xmlUtil.GetTextValue(parentNode, 
                                Constants.ExpectedDerivedSequence)));
            switch (option)
            {
                case SequenceMethods.Reverse:
                    seq = sequence.GetReversedSequence();
                    expectedValue = utilityObj.xmlUtil.GetTextValue(
                    parentNode, Constants.Reverse);
                    break;
                case SequenceMethods.ReverseComplement:
                    seq = sequence.GetReverseComplementedSequence();
                    expectedValue = utilityObj.xmlUtil.GetTextValue(
                    parentNode, Constants.ReverseComplement);
                    break;
                case SequenceMethods.Complement:
                    seq = sequence.GetComplementedSequence();
                    expectedValue = utilityObj.xmlUtil.GetTextValue(
                    parentNode, Constants.Complement);
                    break;
            }

            Assert.AreEqual(expectedValue, seq.ConvertToString());
            ApplicationLog.WriteLine(string.Concat(
                    "Sequence BVT: Validation of Sequence operation ", option, " completed successfully."));

        }

        #endregion Supporting Methods
    }
}

