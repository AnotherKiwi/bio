using Bio;
using NUnit.Framework;
using System.Collections.Generic;

namespace Bio.Tests
{
    /// <summary>
    /// Test for Qualitative sequence.
    /// </summary>
    [TestFixture]
    public class QualitativeSequenceTest
    {
        /// <summary>
        /// Test constructor by passing byte array as sequence data and quality scores.
        /// </summary>
        [Test]
        public void TestConstructorWithByteArray()
        {
            var sequenceData = new byte[6];
            sequenceData[0] = (byte)'C';
            sequenceData[1] = (byte)'A';
            sequenceData[2] = (byte)'A';
            sequenceData[3] = (byte)'G';
            sequenceData[4] = (byte)'C';
            sequenceData[5] = (byte)'T';

            var qualityScores = new byte[6];
            qualityScores[0] = 65;
            qualityScores[1] = 65;
            qualityScores[2] = 65;
            qualityScores[3] = 65;
            qualityScores[4] = 110;
            qualityScores[5] = 125;

            var expectedSequence = "CAAGCT";
            var qualitativeSequence = new QualitativeSequence(Alphabets.DNA, FastQFormatType.Illumina_v1_3, sequenceData, qualityScores);

            var actual = "";
            foreach (var bt in qualitativeSequence)
            {
                actual += (char)bt;
            }
            Assert.AreEqual(expectedSequence, actual);

            Assert.AreEqual(qualitativeSequence.Alphabet, Alphabets.DNA);
            Assert.AreEqual(qualitativeSequence.Count, 6);
            // 
            // Test for indexer
            Assert.AreEqual(qualitativeSequence[0], (byte)'C');
            Assert.AreEqual(qualitativeSequence[1], (byte)'A');
            Assert.AreEqual(qualitativeSequence[2], (byte)'A');
            Assert.AreEqual(qualitativeSequence[3], (byte)'G');
            Assert.AreEqual(qualitativeSequence[4], (byte)'C');
            Assert.AreEqual(qualitativeSequence[5], (byte)'T');

            var index = 0;
            foreach (var qualityScore in qualitativeSequence.GetEncodedQualityScores())
            {
                Assert.AreEqual(qualityScores[index++], qualityScore);
            }
        }

        /// <summary>
        /// Test constructor by passing string as sequence data and quality scores.
        /// </summary>
        [Test]
        public void TestConstructorWithString()
        {
            var expectedSequence = "CAAGCT";
            var expectedQualityScore = "AAABBC";
            var qualityScores = new byte[6];
            qualityScores[0] = 65;
            qualityScores[1] = 65;
            qualityScores[2] = 65;
            qualityScores[3] = 66;
            qualityScores[4] = 66;
            qualityScores[5] = 67;
            var qualitativeSequence = new QualitativeSequence(Alphabets.DNA, FastQFormatType.Illumina_v1_3, expectedSequence, expectedQualityScore);

            var actual = "";
            foreach (var bt in qualitativeSequence)
            {
                actual += (char)bt;
            }
            Assert.AreEqual(expectedSequence, actual);

            Assert.AreEqual(qualitativeSequence.Alphabet, Alphabets.DNA);
            Assert.AreEqual(qualitativeSequence.Count, 6);
            // 
            // Test for indexer
            Assert.AreEqual(qualitativeSequence[0], (byte)'C');
            Assert.AreEqual(qualitativeSequence[1], (byte)'A');
            Assert.AreEqual(qualitativeSequence[2], (byte)'A');
            Assert.AreEqual(qualitativeSequence[3], (byte)'G');
            Assert.AreEqual(qualitativeSequence[4], (byte)'C');
            Assert.AreEqual(qualitativeSequence[5], (byte)'T');

            var index = 0;
            foreach (var qualityScore in qualitativeSequence.GetEncodedQualityScores())
            {
                Assert.AreEqual(qualityScores[index++], qualityScore);
            }
        }

        /// <summary>
        /// Validate GetMinQuality score.
        /// </summary>
        [Test]
        public void TestGetMinQualScore()
        {
            byte b;
            b = QualitativeSequence.GetMinEncodedQualScore(FastQFormatType.Solexa_Illumina_v1_0);
            Assert.AreEqual((byte)59, b);

            b = QualitativeSequence.GetMinEncodedQualScore(FastQFormatType.Sanger);
            Assert.AreEqual((byte)33, b);

            b = QualitativeSequence.GetMinEncodedQualScore(FastQFormatType.Illumina_v1_3);
            Assert.AreEqual((byte)64, b);
        }

        /// <summary>
        /// Validate GetMaxScore.
        /// </summary>
        [Test]
        public void TestGetMaxQualScore()
        {
            byte b;
            b = QualitativeSequence.GetMaxEncodedQualScore(FastQFormatType.Solexa_Illumina_v1_0);
            Assert.AreEqual((byte)126, b);

            b = QualitativeSequence.GetMaxEncodedQualScore(FastQFormatType.Sanger);
            Assert.AreEqual((byte)126, b);

            b = QualitativeSequence.GetMaxEncodedQualScore(FastQFormatType.Illumina_v1_3);
            Assert.AreEqual((byte)126, b);
        }

        /// <summary>
        /// Validate GetDefaultQualityScore.
        /// </summary>
        [Test]
        public void TestGetDefaultQualScore()
        {
            byte b;
            b = QualitativeSequence.GetDefaultQualScore(FastQFormatType.Sanger);
            Assert.AreEqual((byte)93, b);

            b = QualitativeSequence.GetDefaultQualScore(FastQFormatType.Solexa_Illumina_v1_0);
            Assert.AreEqual((byte)124, b);

            b = QualitativeSequence.GetDefaultQualScore(FastQFormatType.Illumina_v1_3);
            Assert.AreEqual((byte)124, b);
        }

        /// <summary>
        /// Validate ConvertFromSolexaToIllumina
        /// </summary>
        [Test]
        public void TestConvertFromSolexa_Illumina_v1_0ToIllumina_v1_3()
        {
            byte[] illuminaScores;
            var solexaScores = new byte[2];
            solexaScores[0] = (byte)60;
            solexaScores[1] = (byte)60;
            illuminaScores = QualitativeSequence.ConvertEncodedQualityScore(FastQFormatType.Solexa_Illumina_v1_0, FastQFormatType.Illumina_v1_3, solexaScores);
            Assert.IsNotNull((object)illuminaScores);
            Assert.AreEqual(2, illuminaScores.Length);
            Assert.AreEqual((byte)65, illuminaScores[0]);
            Assert.AreEqual((byte)65, illuminaScores[1]);
        }

        /// <summary>
        /// Validate ConvertFromSolexaToSanger.
        /// </summary>
        [Test]
        public void TestConvertFromSolexa_Illumina_v1_0ToSanger()
        {
            byte[] sangerScores;
            var solexaScores = new byte[1];
            solexaScores[0] = (byte)59;
            sangerScores = QualitativeSequence.ConvertEncodedQualityScore(FastQFormatType.Solexa_Illumina_v1_0, FastQFormatType.Sanger, solexaScores);
            Assert.IsNotNull((object)sangerScores);
            Assert.AreEqual(1, sangerScores.Length);
            Assert.AreEqual((byte)33, sangerScores[0]);
        }

        /// <summary>
        /// Validate ConvertFromSangerToSolexa
        /// </summary>
        [Test]
        public void TestConvertFromSangerToSolexa_Illumina_v1_0()
        {
            byte[] solexaScores;
            var sangerScores = new byte[2];
            sangerScores[0] = (byte)34;
            sangerScores[1] = (byte)34;
            solexaScores = QualitativeSequence.ConvertEncodedQualityScore(FastQFormatType.Sanger, FastQFormatType.Solexa_Illumina_v1_0, sangerScores);
            Assert.IsNotNull((object)solexaScores);
            Assert.AreEqual(2, solexaScores.Length);
            Assert.AreEqual((byte)59, solexaScores[0]);
            Assert.AreEqual((byte)59, solexaScores[1]);
        }

        /// <summary>
        /// Validate ConvertFromSangerToIllumina
        /// </summary>
        [Test]
        public void TestConvertFromSangerToIllumina_v1_3()
        {
            byte[] illuminaScores;
            var sangerScores = new byte[2];
            sangerScores[0] = (byte)33;
            sangerScores[1] = (byte)33;
            illuminaScores = QualitativeSequence.ConvertEncodedQualityScore(FastQFormatType.Sanger, FastQFormatType.Illumina_v1_3, sangerScores);
            Assert.IsNotNull((object)illuminaScores);
            Assert.AreEqual(2, illuminaScores.Length);
            Assert.AreEqual((byte)64, illuminaScores[0]);
            Assert.AreEqual((byte)64, illuminaScores[1]);
        }

        /// <summary>
        /// Validate ConvertFromIlluminaToSolexa
        /// </summary>
        [Test]
        public void TestConvertFromIllumina_v1_3ToSolexa_Illumina_v1_0()
        {
            byte[] solexaScores;
            var illuminaScores = new byte[2];
            illuminaScores[0] = (byte)65;
            illuminaScores[1] = (byte)65;
            solexaScores = QualitativeSequence.ConvertEncodedQualityScore(FastQFormatType.Illumina_v1_3, FastQFormatType.Solexa_Illumina_v1_0, illuminaScores);
            Assert.IsNotNull((object)solexaScores);
            Assert.AreEqual(2, solexaScores.Length);
            Assert.AreEqual((byte)59, solexaScores[0]);
            Assert.AreEqual((byte)59, solexaScores[1]);
        }

        /// <summary>
        /// Validate - ConvertFromIlluminaToSanger
        /// </summary>
        [Test]
        public void TestConvertFromIllumina_v1_3ToSanger()
        {
            byte[] sangerScores;
            var illuminaScores = new byte[2];
            illuminaScores[0] = (byte)64;
            illuminaScores[1] = (byte)64;
            sangerScores = QualitativeSequence.ConvertEncodedQualityScore(FastQFormatType.Illumina_v1_3, FastQFormatType.Sanger, illuminaScores);
            Assert.IsNotNull((object)sangerScores);
            Assert.AreEqual(2, sangerScores.Length);
            Assert.AreEqual((byte)33, sangerScores[0]);
            Assert.AreEqual((byte)33, sangerScores[1]);
        }

        /// <summary>
        /// Validate - GetPhredQualityScore
        /// </summary>
        [Test]
        public void TestGetPhredQualityScore()
        {
            // Validate using SangerFormat.
            var pharedQualityScores = GetPharedQualityScoresForSanger();
            var encodedSangerQualityScores = GetSangerEncodedQualityScores(pharedQualityScores);
            var symbols = GetSymbols(encodedSangerQualityScores.Length);
            var qualSeq = new QualitativeSequence(Alphabets.DNA, FastQFormatType.Sanger, symbols, encodedSangerQualityScores);

            for (var i = 0; i < qualSeq.Count; i++)
            {
                Assert.AreEqual(pharedQualityScores[i], qualSeq.GetPhredQualityScore(i));
            }

            // Validate using illumina v1.3 .
            pharedQualityScores = GetPharedQualityScoresForIllumina_v1_3();
            var encodedIllumina_v1_3_QualityScores = GetIllumina_v1_3_EncodedQualityScores(pharedQualityScores);
            symbols = GetSymbols(encodedIllumina_v1_3_QualityScores.Length);
            qualSeq = new QualitativeSequence(Alphabets.DNA, FastQFormatType.Illumina_v1_3, symbols, encodedIllumina_v1_3_QualityScores);
            for (var i = 0; i < qualSeq.Count; i++)
            {
                Assert.AreEqual(pharedQualityScores[i], qualSeq.GetPhredQualityScore(i));
            }

            // Validate using illumina v1.5
            pharedQualityScores = GetPharedQualityScoresForIllumina_v1_5();
            var encodedIllumina_v1_5_QualityScores = GetIllumina_v1_5_EncodedQualityScores(pharedQualityScores);
            symbols = GetSymbols(encodedIllumina_v1_5_QualityScores.Length);
            qualSeq = new QualitativeSequence(Alphabets.DNA, FastQFormatType.Illumina_v1_5, symbols, encodedIllumina_v1_5_QualityScores);
            for (var i = 0; i < qualSeq.Count; i++)
            {
                Assert.AreEqual(pharedQualityScores[i], qualSeq.GetPhredQualityScore(i));
            }

            // Validate using illumina v1.8
            pharedQualityScores = GetPharedQualityScoresForIllumina_v1_8();
            var encodedIllumina_v1_8_QualityScores = GetIllumina_v1_8_EncodedQualityScores(pharedQualityScores);
            symbols = GetSymbols(encodedIllumina_v1_8_QualityScores.Length);
            qualSeq = new QualitativeSequence(Alphabets.DNA, FastQFormatType.Illumina_v1_8, symbols, encodedIllumina_v1_8_QualityScores);
            for (var i = 0; i < qualSeq.Count; i++)
            {
                Assert.AreEqual(pharedQualityScores[i], qualSeq.GetPhredQualityScore(i));
            }


            // Validate using illumina v1.0 .
            var solexaQualityScores = GetSolexaQualityScoresForIllumina_v1_0();
            var encodedIllumina_v1_0_QualityScores = GetIllumina_v1_0_EncodedQualityScores(solexaQualityScores);
            symbols = GetSymbols(encodedIllumina_v1_0_QualityScores.Length);
            qualSeq = new QualitativeSequence(Alphabets.DNA, FastQFormatType.Solexa_Illumina_v1_0, symbols, encodedIllumina_v1_0_QualityScores);
            for (var i = 0; i < qualSeq.Count; i++)
            {
                Assert.AreEqual(solexaQualityScores[i], qualSeq.GetSolexaQualityScore(i));
            }

        }

        private static byte[] GetIllumina_v1_8_EncodedQualityScores(List<int> pharedQualityScores)
        {
            var encodedQualityValues = new byte[pharedQualityScores.Count];
            for (var i = 0; i < pharedQualityScores.Count; i++)
            {
                encodedQualityValues[i] = (byte)(pharedQualityScores[i] + 33);
            }

            return encodedQualityValues;
        }


        private static byte[] GetIllumina_v1_5_EncodedQualityScores(List<int> pharedQualityScores)
        {
            var encodedQualityValues = new byte[pharedQualityScores.Count];
            for (var i = 0; i < pharedQualityScores.Count; i++)
            {
                encodedQualityValues[i] = (byte)(pharedQualityScores[i] + 64);
            }

            return encodedQualityValues;
        }

        private static byte[] GetIllumina_v1_0_EncodedQualityScores(List<int> solexaQualityScores)
        {
            var encodedQualityValues = new byte[solexaQualityScores.Count];
            for (var i = 0; i < solexaQualityScores.Count; i++)
            {
                encodedQualityValues[i] = (byte)(solexaQualityScores[i] + 64);
            }

            return encodedQualityValues;
        }

        private static byte[] GetIllumina_v1_3_EncodedQualityScores(List<int> pharedQualityScores)
        {
            var encodedQualityValues = new byte[pharedQualityScores.Count];
            for (var i = 0; i < pharedQualityScores.Count; i++)
            {
                encodedQualityValues[i] = (byte)(pharedQualityScores[i] + 64);
            }

            return encodedQualityValues;
        }

        private static byte[] GetSangerEncodedQualityScores(List<int> pharedQualityScores)
        {
            var encodedQualityValues = new byte[pharedQualityScores.Count];
            for (var i = 0; i < pharedQualityScores.Count; i++)
            {
                encodedQualityValues[i] = (byte)(pharedQualityScores[i] + 33);
            }

            return encodedQualityValues;
        }

        private static List<int> GetPharedQualityScoresForSanger()
        {
            return GetIntegers(0, 93);
        }

        private static List<int> GetPharedQualityScoresForIllumina_v1_8()
        {
            return GetIntegers(0, 93);
        }

        private static List<int> GetPharedQualityScoresForIllumina_v1_5()
        {
            return GetIntegers(2, 62);
        }

        private static List<int> GetPharedQualityScoresForIllumina_v1_3()
        {
            return GetIntegers(0, 62);
        }

        private static List<int> GetSolexaQualityScoresForIllumina_v1_0()
        {
            return GetIntegers(-5, 62);
        }

        /// <summary>
        /// Gets the list of integer values as specified by from and to.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private static List<int> GetIntegers(int from, int to)
        {
            var count = to - from + 1;
            var values = new List<int>(count);
            for (var i = from; i <= to; i++)
            {
                values.Add(i);
            }

            return values;
        }

        /// <summary>
        /// Gets an array of byte containing symbol 'A'
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private static byte[] GetSymbols(int count)
        {
            var symbols = new byte[count];
            for (var i = 0; i < count; i++)
            {
                symbols[i] = (byte)'A';
            }

            return symbols;
        }
    }
}
