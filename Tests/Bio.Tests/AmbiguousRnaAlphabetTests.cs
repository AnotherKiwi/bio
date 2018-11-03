using NUnit.Framework;

namespace Bio.Tests
{
    /// <summary>
    /// Tests the AmbiguousRnaAlphabet class.
    /// </summary>
    [TestFixture]
    public class AmbiguousAmbiguousRnaAlphabetTests
    {
        /// <summary>
        /// Tests the AmbiguousRNAAlphabet class.
        /// </summary>
        [Test]
        public void TestAmbiguousRnaAlphabetCompareSymbols()
        {
            AmbiguousRnaAlphabet ambiguousRnaAlphabet = AmbiguousRnaAlphabet.Instance;
            Assert.AreEqual(false, ambiguousRnaAlphabet.CompareSymbols((byte)'A', (byte)'M'));
            Assert.AreEqual(true, ambiguousRnaAlphabet.CompareSymbols((byte)'A', (byte)'A'));
        }
    }
}
