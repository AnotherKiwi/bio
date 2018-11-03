using NUnit.Framework;

namespace Bio.Tests.NewAlphabets
{
    /// <summary>
    ///     Tests the FriendlyName properties of the alphabets that were added to the six implemented
    ///     in the version 3.0.0-alpha release.
    /// </summary>
    class NewAlphabetsFriendlyNameTest
    {
        /// <summary>
        ///     Tests the FriendlyName properties of the alphabets that were added to the six implemented
        ///     in the version 3.0.0-alpha release.
        /// </summary>
        [Test]
        public void TestFriendlyNames()
        {
            // AminoAcids
            Assert.AreEqual(AminoAcidsAlphabet.Instance.GetFriendlyName(AminoAcidsAlphabet.Instance.A), "Alanine");
            Assert.AreEqual(AminoAcidsAlphabet.Instance.GetFriendlyName(AminoAcidsAlphabet.Instance.C), "Cysteine");
            Assert.AreEqual(AminoAcidsAlphabet.Instance.GetFriendlyName(AminoAcidsAlphabet.Instance.D), "Aspartic Acid");
            Assert.AreEqual(AminoAcidsAlphabet.Instance.GetFriendlyName(AminoAcidsAlphabet.Instance.E), "Glutamic Acid");
            Assert.AreEqual(AminoAcidsAlphabet.Instance.GetFriendlyName(AminoAcidsAlphabet.Instance.F), "Phenylalanine");
            Assert.AreEqual(AminoAcidsAlphabet.Instance.GetFriendlyName(AminoAcidsAlphabet.Instance.G), "Glycine");
            Assert.AreEqual(AminoAcidsAlphabet.Instance.GetFriendlyName(AminoAcidsAlphabet.Instance.H), "Histidine");
            Assert.AreEqual(AminoAcidsAlphabet.Instance.GetFriendlyName(AminoAcidsAlphabet.Instance.I), "Isoleucine");
            Assert.AreEqual(AminoAcidsAlphabet.Instance.GetFriendlyName(AminoAcidsAlphabet.Instance.K), "Lysine");
            Assert.AreEqual(AminoAcidsAlphabet.Instance.GetFriendlyName(AminoAcidsAlphabet.Instance.L), "Leucine");
            Assert.AreEqual(AminoAcidsAlphabet.Instance.GetFriendlyName(AminoAcidsAlphabet.Instance.M), "Methionine");
            Assert.AreEqual(AminoAcidsAlphabet.Instance.GetFriendlyName(AminoAcidsAlphabet.Instance.N), "Asparagine");
            Assert.AreEqual(AminoAcidsAlphabet.Instance.GetFriendlyName(AminoAcidsAlphabet.Instance.O), "Pyrrolysine");
            Assert.AreEqual(AminoAcidsAlphabet.Instance.GetFriendlyName(AminoAcidsAlphabet.Instance.P), "Proline");
            Assert.AreEqual(AminoAcidsAlphabet.Instance.GetFriendlyName(AminoAcidsAlphabet.Instance.Q), "Glutamine");
            Assert.AreEqual(AminoAcidsAlphabet.Instance.GetFriendlyName(AminoAcidsAlphabet.Instance.R), "Arginine");
            Assert.AreEqual(AminoAcidsAlphabet.Instance.GetFriendlyName(AminoAcidsAlphabet.Instance.S), "Serine");
            Assert.AreEqual(AminoAcidsAlphabet.Instance.GetFriendlyName(AminoAcidsAlphabet.Instance.T), "Threonine");
            Assert.AreEqual(AminoAcidsAlphabet.Instance.GetFriendlyName(AminoAcidsAlphabet.Instance.U), "Selenocysteine");
            Assert.AreEqual(AminoAcidsAlphabet.Instance.GetFriendlyName(AminoAcidsAlphabet.Instance.V), "Valine");
            Assert.AreEqual(AminoAcidsAlphabet.Instance.GetFriendlyName(AminoAcidsAlphabet.Instance.W), "Tryptophan");
            Assert.AreEqual(AminoAcidsAlphabet.Instance.GetFriendlyName(AminoAcidsAlphabet.Instance.Y), "Tyrosine");

            // ProteinFragment
            Assert.AreEqual(ProteinFragmentAlphabet.Instance.GetFriendlyName(ProteinFragmentAlphabet.Instance.SequenceDelimiter), "Sequence Delimiter");

            // ProteinScapePeptide
            Assert.AreEqual(ProteinScapePeptideAlphabet.Instance.GetFriendlyName(ProteinScapePeptideAlphabet.Instance.CTerminus), "C-terminus");
            Assert.AreEqual(ProteinScapePeptideAlphabet.Instance.GetFriendlyName(ProteinScapePeptideAlphabet.Instance.NTerminus), "N-terminus");
            Assert.AreEqual(ProteinScapePeptideAlphabet.Instance.GetFriendlyName(ProteinScapePeptideAlphabet.Instance.SequenceDelimiter), "Sequence Delimiter");

            // PeaksPeptide
            Assert.AreEqual(PeaksPeptideAlphabet.Instance.GetFriendlyName(PeaksPeptideAlphabet.Instance.Digit0), "Modification Digit '0'");
            Assert.AreEqual(PeaksPeptideAlphabet.Instance.GetFriendlyName(PeaksPeptideAlphabet.Instance.Digit1), "Modification Digit '1'");
            Assert.AreEqual(PeaksPeptideAlphabet.Instance.GetFriendlyName(PeaksPeptideAlphabet.Instance.Digit2), "Modification Digit '2'");
            Assert.AreEqual(PeaksPeptideAlphabet.Instance.GetFriendlyName(PeaksPeptideAlphabet.Instance.Digit3), "Modification Digit '3'");
            Assert.AreEqual(PeaksPeptideAlphabet.Instance.GetFriendlyName(PeaksPeptideAlphabet.Instance.Digit4), "Modification Digit '4'");
            Assert.AreEqual(PeaksPeptideAlphabet.Instance.GetFriendlyName(PeaksPeptideAlphabet.Instance.Digit5), "Modification Digit '5'");
            Assert.AreEqual(PeaksPeptideAlphabet.Instance.GetFriendlyName(PeaksPeptideAlphabet.Instance.Digit6), "Modification Digit '6'");
            Assert.AreEqual(PeaksPeptideAlphabet.Instance.GetFriendlyName(PeaksPeptideAlphabet.Instance.Digit7), "Modification Digit '7'");
            Assert.AreEqual(PeaksPeptideAlphabet.Instance.GetFriendlyName(PeaksPeptideAlphabet.Instance.Digit8), "Modification Digit '8'");
            Assert.AreEqual(PeaksPeptideAlphabet.Instance.GetFriendlyName(PeaksPeptideAlphabet.Instance.Digit9), "Modification Digit '9'");
            Assert.AreEqual(PeaksPeptideAlphabet.Instance.GetFriendlyName(PeaksPeptideAlphabet.Instance.Minus), "Modification Char '-'");
            Assert.AreEqual(PeaksPeptideAlphabet.Instance.GetFriendlyName(PeaksPeptideAlphabet.Instance.ModificationBeginDelimiter), "Modification Start Delimiter");
            Assert.AreEqual(PeaksPeptideAlphabet.Instance.GetFriendlyName(PeaksPeptideAlphabet.Instance.ModificationEndDelimiter), "Modification End Delimiter");
            Assert.AreEqual(PeaksPeptideAlphabet.Instance.GetFriendlyName(PeaksPeptideAlphabet.Instance.Plus), "Modification Char '+'");
            Assert.AreEqual(PeaksPeptideAlphabet.Instance.GetFriendlyName(PeaksPeptideAlphabet.Instance.SmallB), "Modification Sub Char 'b'");
            Assert.AreEqual(PeaksPeptideAlphabet.Instance.GetFriendlyName(PeaksPeptideAlphabet.Instance.SmallS), "Modification Sub Char 's'");
            Assert.AreEqual(PeaksPeptideAlphabet.Instance.GetFriendlyName(PeaksPeptideAlphabet.Instance.SmallU), "Modification Sub Char 'u'");
            Assert.AreEqual(PeaksPeptideAlphabet.Instance.GetFriendlyName(PeaksPeptideAlphabet.Instance.Space), "Modification Char ' '");
            Assert.AreEqual(PeaksPeptideAlphabet.Instance.GetFriendlyName(PeaksPeptideAlphabet.Instance.SequenceDelimiter), "Sequence Delimiter");
        }
    }
}
