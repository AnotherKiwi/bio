using System;
using System.Linq;
using System.Text;

using Bio.TestAutomation.Util;
using Bio.Util.Logging;

using NUnit.Framework;

namespace Bio.Tests
{
    /// <summary>
    /// Test Automation code for Bio Qualitative sequence validations.
    /// </summary>
    [TestFixture]
    public class QualitativeSequenceBvtTestCases
    {
        #region Enums

        /// <summary>
        /// Qualitative Sequence method Parameters which are used for different test cases 
        /// based on which the test cases are executed.
        /// </summary>
        enum QualitativeSequenceParameters
        {
            Score,
            ByteArray,
        };

        #endregion Enums

        readonly Utility utilityObj = new Utility(@"TestUtils\QualitativeTestsConfig.xml");        

        #region Qualitative Sequence Test Cases

        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna Sequence
        /// with Sanger FastQFormat and specified score.
        /// Input Data : Dna Alphabet,Dna Sequence,"Sanger" FastQFormat.
        /// and Score "120" 
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateSangerFormatTypeDnaQualitativeSequenceWithScore()
        {
            GeneralQualitativeSequence(Constants.SimpleDnaSangerNode,
                QualitativeSequenceParameters.Score);
        }

        /// <summary>
        /// Validate Reverse and complement sequence.
        /// Input Data : Dna Alphabet,Dna Sequence,"Sanger" FastQFormat.
        /// Output Data : Validation of Reverse/Complement Sequence.
        /// </summary>
        [Category("Priority0")]
        public void ValidateSangerFormatTypeDnaReverseComplement()
        {
            // Gets the actual sequence and the alphabet from the Xml
            var alphabet = Utility.GetAlphabet(utilityObj.xmlUtil.GetTextValue(
                Constants.SimpleDnaSangerNode, Constants.AlphabetNameNode));
            var expectedFormatType = Utility.GetFastQFormatType(
                utilityObj.xmlUtil.GetTextValue(Constants.SimpleDnaSangerNode, Constants.FastQFormatType));
            var inputSequence = utilityObj.xmlUtil.GetTextValue(
                Constants.SimpleDnaSangerNode, Constants.inputSequenceNode);
            var compSequence = utilityObj.xmlUtil.GetTextValue(
                Constants.SimpleDnaSangerNode, Constants.ComplementQualSeqNode);
            var expectedRevCompSeq = utilityObj.xmlUtil.GetTextValue(
                Constants.SimpleDnaSangerNode, Constants.RevComplement);
            var expectedRevSeq = utilityObj.xmlUtil.GetTextValue(
                Constants.SimpleDnaSangerNode, Constants.ReverseQualSeq);            
            var inputQuality = utilityObj.xmlUtil.GetTextValue(
            Constants.SimpleDnaSangerNode, Constants.InputByteArrayNode);
            var byteArray = Encoding.UTF8.GetBytes(inputQuality);
            
            var createdQualitativeSequence =
                new QualitativeSequence(alphabet, expectedFormatType,
                    inputSequence, inputQuality);

            var revSeq = createdQualitativeSequence.GetReversedSequence();
            var revCompSeq = createdQualitativeSequence.GetReverseComplementedSequence();
            var compSeq = createdQualitativeSequence.GetComplementedSequence();

            Assert.AreEqual(expectedRevSeq, new string(revSeq.Select(a => (char)a).ToArray()));
            Assert.AreEqual(expectedRevCompSeq, new string(revCompSeq.Select(a => (char)a).ToArray()));
            Assert.AreEqual(compSequence, new string(compSeq.Select(a => (char)a).ToArray()));

            ApplicationLog.WriteLine("Qualitative BVT: Successfully validated Reverse, Complement and ReverseComplement sequence");
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna Sequence
        /// with Solexa FastQFormat and specified score.
        /// Input Data : Dna Alphabet,Dna Sequence,"Solexa" FastQFormat.
        /// and Score "104" 
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateSolexaFormatTypeDnaQualitativeSequenceWithScore()
        {
            GeneralQualitativeSequence(Constants.SimpleDnaSolexaNode,
                QualitativeSequenceParameters.Score);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna Sequence
        /// with Illumina FastQFormat and specified score.
        /// Input Data : Dna Alphabet,Dna Sequence,"Illumina" FastQFormat.
        /// and Score "104" 
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateIlluminaFormatTypeDnaQualitativeSequenceWithScore()
        {
            GeneralQualitativeSequence(Constants.SimpleDnaIlluminaNode,
                QualitativeSequenceParameters.Score);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna Sequence
        /// with Solexa FastQFormat and Byte array.
        /// Input Data : Dna Alphabet,Dna Sequence,"Solexa" FastQFormat.
        /// Output Data : Validation of Created Qualitative sequence with score.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateSolexaFormatTypeDnaQualitativeSequenceWithByteArray()
        {
            GeneralQualitativeSequence(Constants.SimpleDnaSolexaNode,
                QualitativeSequenceParameters.ByteArray);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna Sequence
        /// with Illumina FastQFormat and Byte array.
        /// Input Data : Dna Alphabet,Dna Sequence,"Illumina" FastQFormat.
        /// Output Data : Validation of Created Qualitative sequence with score.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateIlluminaFormatTypeDnaQualitativeSequenceWithByteArray()
        {
            GeneralQualitativeSequence(Constants.SimpleDNAIlluminaByteArrayNode,
                QualitativeSequenceParameters.ByteArray);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna Sequence
        /// with Sanger FastQFormat and Byte array.
        /// Input Data : Dna Alphabet,Dna Sequence,"Sanger" FastQFormat.
        /// Output Data : Validation of Created Qualitative sequence with score.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateSangerFormatTypeDnaQualitativeSequenceWithByteArray()
        {
            GeneralQualitativeSequence(Constants.SimpleDnaSangerNode,
                QualitativeSequenceParameters.ByteArray);
        }

        /// <summary>
        /// Validate clear Qualitative Sequence
        /// Input Data : Dna Sequence.
        /// Output Data: Qualitative sequence having sequence data.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateQualititaiveSequenceConstructor()
        {
            // Gets the actual sequence and the alphabet from the Xml
            var alphabet = Utility.GetAlphabet(utilityObj.xmlUtil.GetTextValue(
            Constants.SimpleDnaSolexaNode, Constants.AlphabetNameNode));
            var expectedFormatType = Utility.GetFastQFormatType(
            utilityObj.xmlUtil.GetTextValue(Constants.SimpleDnaSolexaNode,
            Constants.FastQFormatType));
            QualitativeSequence createdQualitativeSequence = null;
            var inputSequence = utilityObj.xmlUtil.GetTextValue(
            Constants.SimpleDnaSolexaNode, Constants.inputSequenceNode);

            var qualScores = new byte[inputSequence.Count()];

            for (var i = 0; i < inputSequence.Count(); i++)
            {
                qualScores[i] = (byte)'{';
            }

            // Create a Qualitative Sequence.
            createdQualitativeSequence = new QualitativeSequence(
                    alphabet, expectedFormatType, Encoding.UTF8.GetBytes(inputSequence), qualScores);

            var qualSequence = new string(createdQualitativeSequence.Select(a => (char)a).ToArray());

            // Validate Qualitative Sequence after addition of Seq Item.
            Assert.IsTrue(!string.IsNullOrEmpty(qualSequence));
            Assert.AreEqual(inputSequence, qualSequence);

            // Logs to the VSTest GUI
            ApplicationLog.WriteLine(string.Format(null, "Qualitative Sequence BVT: Qualitative Sequence {0} is as expected.", qualSequence));
        }

        /// <summary>
        /// Validate convert from Sanger to solexa and Illumina.
        /// Input Data : Sanger quality value, Sanger format sequence.
        /// Output Data :Validate convert from Sanger to Illumina and Solexa.
        /// </summary>
        [Category("Priority0")]
        public void ConvertSangerToSolexaAndIllumina()
        {
            // Gets the actual sequence and the Qual score from the Xml
            var sangerSequence = utilityObj.xmlUtil.GetTextValue(
                Constants.SangerToSolexaAndIlluminaNode, Constants.SangerSequence);
            var expectedSolexaSequence = utilityObj.xmlUtil.GetTextValue(
                Constants.SangerToSolexaAndIlluminaNode, Constants.SolexaSequence);
            var expectedIlluminaSequence = utilityObj.xmlUtil.GetTextValue(
                Constants.SangerToSolexaAndIlluminaNode, Constants.IlluminaSequence);
            var sangerQualScore = utilityObj.xmlUtil.GetTextValue(
                Constants.SangerToSolexaAndIlluminaNode, Constants.SangerQualScore);
            var expectedSolexaQualScore = utilityObj.xmlUtil.GetTextValue(
                Constants.SangerToSolexaAndIlluminaNode, Constants.SolexaQualScore);
            var expectedIlluminaQualScore = utilityObj.xmlUtil.GetTextValue(
                Constants.SangerToSolexaAndIlluminaNode, Constants.IlluminaQualScore);

            string solexaQualScore = null;
            string illuminaQualScore = null;
            var scoreValue = Encoding.UTF8.GetBytes(sangerQualScore);

            // Create a Sanger qualitative sequence.
            var sangerQualSequence = new QualitativeSequence(Alphabets.DNA,
                FastQFormatType.Sanger, sangerSequence, sangerQualScore);           

            // Convert Sanger to Solexa.
            var solexaQualSequence = sangerQualSequence.ConvertTo(
                FastQFormatType.Solexa_Illumina_v1_0);

            var sangerToSolexa = QualitativeSequence.ConvertEncodedQualityScore(FastQFormatType.Sanger, FastQFormatType.Solexa_Illumina_v1_0, scoreValue);
            var scores = solexaQualSequence.GetEncodedQualityScores();
            solexaQualScore = Encoding.UTF8.GetString(scores, 0, scores.Length);

            // Validate converted solexa score.            

            var qualSequence = new string(Encoding.UTF8.GetChars(sangerToSolexa));

            Assert.AreEqual(expectedSolexaQualScore, qualSequence);
            Assert.AreEqual(solexaQualScore, expectedSolexaQualScore);

            var solexaQualString=new string(solexaQualSequence.Select(a => (char)a).ToArray());
            Assert.AreEqual(solexaQualString,expectedSolexaSequence);

            ApplicationLog.WriteLine(string.Format(null,
                "Qualitative Sequence BVT:Qualitative Solexa score type {0} is as expected.",
                solexaQualScore));

            // Convert Sanger to Illumina.
            var illuminaQualSequence = sangerQualSequence.ConvertTo(
                FastQFormatType.Illumina_v1_3);
            scores = illuminaQualSequence.GetEncodedQualityScores();
            illuminaQualScore = Encoding.UTF8.GetString(scores, 0, scores.Length);

            var sangerToIllumina = QualitativeSequence.ConvertEncodedQualityScore(FastQFormatType.Sanger, FastQFormatType.Illumina_v1_3, scoreValue);
            var illuminaQualSeq = new string(illuminaQualSequence.Select(a => (char)a).ToArray());

            //// Validate converted illumina score.
            Assert.AreEqual(illuminaQualScore, expectedIlluminaQualScore);
            Assert.AreEqual(illuminaQualSeq, expectedIlluminaSequence);

            var sangerToIlluminaString = new string(Encoding.UTF8.GetChars(sangerToIllumina));            
            Assert.AreEqual(expectedIlluminaQualScore, sangerToIlluminaString);

            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence BVT:Qualitative Illumina score type {0} is as expected.",
                illuminaQualScore));
        }

        /// <summary>
        /// Validate convert from Solexa to Sanger and Illumina.
        /// Input Data : Solexa quality value, Solexa format sequence.
        /// Output Data : Validate convert from Solexa to Sanger and Illumina.
        /// </summary>
        [Category("Priority0")]
        public void ConvertSolexaToSangerAndIllumina()
        {
            // Gets the actual sequence and the Qual score from the Xml
            var solexaSequence = utilityObj.xmlUtil.GetTextValue(
                Constants.SolexaToSangerAndIlluminaNode, Constants.SolexaSequence);
            var expectedSangerSequence = utilityObj.xmlUtil.GetTextValue(
                Constants.SolexaToSangerAndIlluminaNode, Constants.SangerSequence);
            var expectedIlluminaSequence = utilityObj.xmlUtil.GetTextValue(
                Constants.SolexaToSangerAndIlluminaNode, Constants.IlluminaSequence);
            var solexaQualScore = utilityObj.xmlUtil.GetTextValue(
                Constants.SolexaToSangerAndIlluminaNode, Constants.SolexaQualScore);
            var expectedSangerQualScore = utilityObj.xmlUtil.GetTextValue(
                Constants.SolexaToSangerAndIlluminaNode, Constants.SangerQualScore);
            var expectedIlluminaQualScore = utilityObj.xmlUtil.GetTextValue(
                Constants.SolexaToSangerAndIlluminaNode, Constants.IlluminaQualScore);
            var byteValue = Encoding.UTF8.GetBytes(solexaQualScore);

            string sangerQualScore = null;
            string illuminaQualScore = null;

            var qualScores = new byte[solexaSequence.Count()];

            for (var i = 0; i < solexaSequence.Count(); i++)
            {
                qualScores[i] = (byte)'{';
            }

            var solexaSequenceinBytes = Encoding.UTF8.GetBytes(solexaSequence);

            // Create a Solexa qualitative sequence.
            var solexaQualSequence = new QualitativeSequence(Alphabets.DNA,
                FastQFormatType.Solexa_Illumina_v1_0, solexaSequenceinBytes, byteValue);

            // Convert Solexa to Sanger.
            var sangerQualSequence = solexaQualSequence.ConvertTo(
                FastQFormatType.Sanger);
            var scores = sangerQualSequence.GetEncodedQualityScores();
            sangerQualScore = Encoding.UTF8.GetString(scores, 0, scores.Length);

            Assert.AreEqual(expectedSangerQualScore, sangerQualScore);
            Assert.AreEqual(new string(sangerQualSequence.Select(a => (char)a).ToArray()), expectedSangerSequence);

            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence BVT:Qualitative Sanger score type {0} is as expected.",
                sangerQualScore));

            // Convert Solexa to Illumina.
            var illuminaQualSequence =
               solexaQualSequence.ConvertTo(FastQFormatType.Illumina_v1_3);
            scores = illuminaQualSequence.GetEncodedQualityScores();
            illuminaQualScore = Encoding.UTF8.GetString(scores, 0, scores.Length);
            Assert.AreEqual(expectedIlluminaQualScore, illuminaQualScore);
            
            var illuminaQualseq=new string(illuminaQualSequence.Select(a => (char)a).ToArray());

            // Validate converted illumina score.
            Assert.AreEqual(illuminaQualScore, expectedIlluminaQualScore);
            Assert.AreEqual(illuminaQualseq, expectedIlluminaSequence);
            
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence BVT:Qualitative Illumina score type {0} is as expected.",
                illuminaQualScore));
        }

        /// <summary>
        /// Validate convert from Illumina to Sanger and Solexa.
        /// Input Data : Illumina quality value, Illumina format sequence.
        /// Output Data : Validate convert from Illumina to Sanger and Solexa.
        /// </summary>
        [Category("Priority0")]
        public void ConvertIlluminaToSangerAndSolexa()
        {
            // Gets the actual sequence and the Qual score from the Xml
            var illuminaSequence = utilityObj.xmlUtil.GetTextValue(
                Constants.IlluminaToSangerAndSolexaNode, Constants.IlluminaSequence);          
            var illuminaQualScore = utilityObj.xmlUtil.GetTextValue(
                Constants.IlluminaToSangerAndSolexaNode, Constants.IlluminaQualScore);
            var expectedSangerQualScore = utilityObj.xmlUtil.GetTextValue(
                Constants.IlluminaToSangerAndSolexaNode, Constants.SangerQualScore);
            var expectedSolexaQualScore = utilityObj.xmlUtil.GetTextValue(
                Constants.IlluminaToSangerAndSolexaNode, Constants.SolexaQualScore);

            var illuminaSequenceinBytes = Encoding.UTF8.GetBytes(illuminaSequence);
            var illuminaQualScoreinBytes = Encoding.UTF8.GetBytes(illuminaQualScore);
            var sangerQualScore = QualitativeSequence.ConvertEncodedQualityScore(FastQFormatType.Illumina_v1_3,FastQFormatType.Sanger, illuminaQualScoreinBytes);

            var qualSequenceSanger = new string(Encoding.UTF8.GetChars(sangerQualScore));

            // Validate converted sanger score.
            Assert.AreEqual(expectedSangerQualScore, qualSequenceSanger);

            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence BVT:Qualitative Sanger score type {0} is as expected.",
                sangerQualScore));
            var solexaQualScore = QualitativeSequence.ConvertEncodedQualityScore(FastQFormatType.Illumina_v1_3, FastQFormatType.Solexa_Illumina_v1_0, illuminaQualScoreinBytes);

            var qualSequenceSolexa = new string(Encoding.UTF8.GetChars(solexaQualScore));

            // Validate converted illumina score.
            Assert.AreEqual(expectedSolexaQualScore, qualSequenceSolexa);
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence BVT:Qualitative Solexa format type {0} is as expected.",
                illuminaQualScore));
        }

        #endregion QualitativeSequence Bvt TestCases

        #region Supporting Methods

        /// <summary>
        /// General method to validate creation of Qualitative sequence.
        /// <param name="nodeName">xml node name.</param>
        /// <param name="parameters">Different Qualitative Sequence parameters.</param>
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "inputScore"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "expectedOuptutScore"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "expectedMaxScore"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "byteArray")]
        void GeneralQualitativeSequence(
            string nodeName, QualitativeSequenceParameters parameters)
        {
            //// Gets the actual sequence and the alphabet from the Xml
            var alphabet = Utility.GetAlphabet(utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode));
            var expectedFormatType = Utility.GetFastQFormatType(
                utilityObj.xmlUtil.GetTextValue(nodeName, Constants.FastQFormatType));
            var expectedScore = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedScore);
            QualitativeSequence createdQualitativeSequence = null;
            var inputSequence = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.inputSequenceNode);
            var expectedSequence = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequenceNode);
            var expectedSequenceCount = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.QSequenceCount);           
            var inputQuality = utilityObj.xmlUtil.GetTextValue(
            nodeName, Constants.InputByteArrayNode);            
            var inputScoreArray = Encoding.UTF8.GetBytes(inputQuality);

            // Create and validate Qualitative Sequence.
            switch (parameters)
            {

                case QualitativeSequenceParameters.Score:
                    createdQualitativeSequence = new QualitativeSequence(alphabet, expectedFormatType,
                    inputSequence, inputQuality);
                    var count = 0;
                    // Validate score
                    foreach (var qualScore in createdQualitativeSequence.GetEncodedQualityScores())
                    {
                        Assert.AreEqual(qualScore, inputScoreArray[count]);
                        count++;
                    }
                    break;
                case QualitativeSequenceParameters.ByteArray:
                    var scoreValue = Encoding.UTF8.GetBytes(inputSequence);          
                    var index = 0;
                    createdQualitativeSequence = new QualitativeSequence(alphabet, expectedFormatType,
                    scoreValue, inputScoreArray);

                    // Validate score
                    foreach (var qualScore in createdQualitativeSequence.GetEncodedQualityScores())
                    {
                        Assert.AreEqual(qualScore, inputScoreArray[index]);
                        index++;
                    }
                    break;
                default:
                    break;
            }

            var qualitativeSequence = new string(createdQualitativeSequence.Select(a => (char)a).ToArray());

            // Validate createdSequence qualitative sequence.
            Assert.IsNotNull(createdQualitativeSequence);
            Assert.AreEqual(alphabet, createdQualitativeSequence.Alphabet);

            Assert.AreEqual(expectedSequence, qualitativeSequence);
            Assert.AreEqual(expectedSequenceCount, createdQualitativeSequence.Count.ToString((IFormatProvider)null));
            ApplicationLog.WriteLine(string.Format(null, "Qualitative Sequence BVT:Qualitative Sequence {0} is as expected.", qualitativeSequence));

            Assert.AreEqual(expectedScore, createdQualitativeSequence.GetEncodedQualityScores().Count().ToString((IFormatProvider)null));
            ApplicationLog.WriteLine(string.Format(null, "Qualitative Sequence BVT:Qualitative Sequence Score {0} is as expected.", createdQualitativeSequence.Count().ToString((IFormatProvider)null)));

            Assert.AreEqual(expectedFormatType, createdQualitativeSequence.FormatType);
            ApplicationLog.WriteLine(string.Format(null, "Qualitative Sequence BVT:Qualitative format type {0} is as expected.", createdQualitativeSequence.FormatType));
        }

        #endregion Supporting Methods
    }
}
