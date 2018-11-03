using NUnit.Framework;
using static System.Text.Encoding;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Bio.Extensions;
using static Bio.Alphabets;
using static Bio.Alphabets.ComparisonResult;

namespace Bio.Tests.NewAlphabets
{
    /// <summary>
    ///     Tests for the new alphabets added to the six defined in Bio version 3.0.0-alpha
    ///     (i.e. AminoAcids, ProteinFragment, PeaksPeptide, and ProteinScapePeptide).
    /// </summary>
    /// <remarks>
    ///     Also tests the new properties added to the existing alphabet types (AlphabetType,
    ///     and IsCaseSensitive).
    /// </remarks>
    [TestFixture]
    class NewAlphabetsTests
    {
        private enum AlphabetType
        {
            AmbiguousDNA,
            AmbiguousRNA,
            AmbiguousProtein,
            AminoAcids,
            DNA,
            ProteinFragment,
            ProteinScapePeptide,
            PeaksPeptide,
            Protein,
            RNA
        }

        #region Helper Methods

        /// <summary>
        /// 	Auto detects the consensus alphabet of the supplied lists of sequences.
        /// </summary>
        /// <param name="sequenceLists">The sequence lists.</param>
        /// <returns>
        ///  	<see cref="IAminoAcidsAlphabet"/>: the consensus alphabet of the supplied lists of sequences.
        /// </returns>
        private IAlphabet GetConsensusAlphabet(params List<byte[]>[] sequenceLists) =>
            GetConsensusAlphabet(null, sequenceLists);

        /// <summary>
        /// 	Auto detects the consensus alphabet of the supplied lists of sequences.
        /// </summary>
        /// <param name="fromAlphabets">The alphabets to use in the search for a consensus alphabet.</param>
        /// <param name="sequenceLists">The sequence lists.</param>
        /// <returns>
        ///  	<see cref="IAminoAcidsAlphabet"/>: the consensus alphabet of the supplied lists of sequences.
        /// </returns>
        private IAlphabet GetConsensusAlphabet(IEnumerable<IAlphabet> fromAlphabets, params List<byte[]>[] sequenceLists)
        {
            var testSequences = new List<byte[]>();
            foreach (var sequenceList in sequenceLists)
            {
                testSequences.AddRange(sequenceList);
            }
            return fromAlphabets == null 
                ? Alphabets.GetConsensusAlphabet(testSequences) 
                : Alphabets.GetConsensusAlphabet(testSequences, fromAlphabets);
        }

        /// <summary>
        /// 	Auto detects the consensus alphabet of the supplied lists of sequences.
        /// </summary>
        /// <param name="sequenceLists">The sequence lists.</param>
        /// <returns>
        ///  	<see cref="IAminoAcidsAlphabet"/>: the consensus alphabet of the supplied lists of sequences.
        /// </returns>
        private IAminoAcidsAlphabet GetConsensusAminoAcidsAlphabet(params List<byte[]>[] sequenceLists)
        {
            var testSequences = new List<byte[]>();
            foreach (var sequenceList in sequenceLists)
            {
                testSequences.AddRange(sequenceList);
            }
            return Alphabets.GetConsensusAminoAcidsAlphabet(testSequences);
        }

        /// <summary>
        /// 	Gets invalid amino acid sequences.
        /// </summary>
        /// <returns>
        ///  	<see cref="T:List{byte[]}"/>: invalid amino acid sequences.
        /// </returns>
        private List<byte[]> GetInvalidAminoAcids() =>
            new List<byte[]>
            {
                ASCII.GetBytes("a"),
                ASCII.GetBytes("ATGc")
            };

        /// <summary>
        /// 	Gets invalid PeaksPeptide sequences.
        /// </summary>
        /// <returns>
        ///  	<see cref="T:List{byte[]}"/>: invalid PeaksPeptide sequences.
        /// </returns>
        private List<byte[]> GetInvalidPeaksPeptides()
        {
            var sequences = new List<byte[]>
            {
                ASCII.GetBytes("A.TgC(+57.06)"),
                ASCII.GetBytes("A.TGC(+-57.06)"),
                ASCII.GetBytes("A.T(+57.)GC"),
                ASCII.GetBytes("A.T(+57)GC"),
                ASCII.GetBytes("A.T(57)GC"),
                ASCII.GetBytes("A.T(+57. 06)GC"),
                ASCII.GetBytes("A.T(+..98).G"),
                ASCII.GetBytes("A.T(su V)G.C"),
                ASCII.GetBytes("A.T(usb V)G.C"),
                ASCII.GetBytes("A.T(suub V)G.C"),
                ASCII.GetBytes("A.T(subV)G.C"),
                ASCII.GetBytes("A.T(sub v)G.C"),
                ASCII.GetBytes("A(+57.06).TGC"),
                ASCII.GetBytes("A.TG.C(-57.08)"),
                ASCII.GetBytes("T(sub V)G"),
                ASCII.GetBytes("T(-.98)G"),
                ASCII.GetBytes("TG(sub V)"),
                ASCII.GetBytes("TG(-.98)")
            };

            // Append sequences used to test the base class alphabet.
            sequences.AddRange(GetInvalidProteinFragments());
            return sequences;
        }

        /// <summary>
        /// 	Gets invalid ProteinFragment sequences.
        /// </summary>
        /// <returns>
        ///  	<see cref="T:List{byte[]}"/>: invalid ProteinFragment sequences.
        /// </returns>
        private List<byte[]> GetInvalidProteinFragments()
        {
            var sequences = new List<byte[]>
            {
                ASCII.GetBytes("ATGC"),
                ASCII.GetBytes(".ATG.C"),
                ASCII.GetBytes("A.TGC."),
                ASCII.GetBytes("A..TGC"),
                ASCII.GetBytes("A.TGC.RFP.S"),
                ASCII.GetBytes("A.TGC.RFP"),
                ASCII.GetBytes("ATGC.RFP"),
                ASCII.GetBytes("A.TG.c")
            };

            // Append sequences used to test the base class alphabet.
            sequences.AddRange(GetInvalidAminoAcids());
            return sequences;
        }

        /// <summary>
        /// 	Gets invalid ProteinScapePeptide sequences.
        /// </summary>
        /// <returns>
        ///  	<see cref="T:List{byte[]}"/>: invalid ProteinFragment sequences.
        /// </returns>
        private List<byte[]> GetInvalidProteinScapePeptides()
        {
            var cTerm = ASCII.GetString(new[] { ProteinScapePeptide.CTerminus });
            var nTerm = ASCII.GetString(new[] { ProteinScapePeptide.NTerminus });
            var sequences = new List<byte[]>
            {
                ASCII.GetBytes("A.TG." + nTerm),
                ASCII.GetBytes("A.TG." + nTerm),
                ASCII.GetBytes(cTerm + "-.T.G"),
                ASCII.GetBytes(cTerm + "-.TG.C"),
                ASCII.GetBytes(cTerm + "-.T." + nTerm),
                ASCII.GetBytes(cTerm + "-.TG." + nTerm)
            };

            // Append sequences used to test the base class alphabet.
            sequences.AddRange(GetInvalidProteinFragments());
            return sequences;
        }

        /// <summary>
        /// 	Gets valid ambiguous DNA sequences.
        /// </summary>
        /// <returns>
        ///  	<see cref="T:List{byte[]}"/>: valid ambiguous DNA sequences.
        /// </returns>
        private List<byte[]> GetValidAmbiguousDNA() =>
            new List<byte[]>
            {
                ASCII.GetBytes("ATGCMRSW"),
                ASCII.GetBytes("ATG--CMRSW"),
            };

        /// <summary>
        /// 	Gets valid ambiguous RNA sequences.
        /// </summary>
        /// <returns>
        ///  	<see cref="T:List{byte[]}"/>: valid ambiguous RNA sequences.
        /// </returns>
        private List<byte[]> GetValidAmbiguousRNA() =>
            new List<byte[]>
            {
                ASCII.GetBytes("AUGCMRSW"),
                ASCII.GetBytes("AUG--CMRSW"),
            };

        /// <summary>
        /// 	Gets valid amino acid sequences (including some that could also be validated as DNA).
        /// </summary>
        /// <returns>
        ///  	<see cref="T:List{byte[]}"/>: valid amino acid sequences.
        /// </returns>
        private List<byte[]> GetValidAminoAcids() =>
            new List<byte[]>
            {
                ASCII.GetBytes("A"),
                ASCII.GetBytes("ATGC"),
                ASCII.GetBytes("ATGCRSM")
            };

        /// <summary>
        /// 	Gets valid DNA sequences.
        /// </summary>
        /// <returns>
        ///  	<see cref="T:List{byte[]}"/>: valid DNA sequences.
        /// </returns>
        private List<byte[]> GetValidDNA() =>
            new List<byte[]>
            {
                ASCII.GetBytes("A"),
                ASCII.GetBytes("ATGC"),
                ASCII.GetBytes("ATG--C"),
            };

        /// <summary>
        /// 	Gets valid PeaksPeptide sequences.
        /// </summary>
        /// <returns>
        ///  	<see cref="T:List{byte[]}"/>: valid PeaksPeptide sequences.
        /// </returns>
        /// <remarks>
        ///     Optionally includes symbols from base alphabets.
        /// </remarks>
        private List<byte[]> GetValidPeaksPeptides()
        {
            var sequences = new List<byte[]>
            {
                ASCII.GetBytes("A.T(+.98)G.C"),
                ASCII.GetBytes("A.T(+.98)GC"),
                ASCII.GetBytes("A.TGC(+57.06)"),
                ASCII.GetBytes("A.T(sub V)G.C"),
                ASCII.GetBytes("A.T(sub V)G"),
                ASCII.GetBytes("A.TG(sub V)"),
                ASCII.GetBytes("T(sub V)G.C")
            };

            return sequences;
        }

        /// <summary>
        /// 	Gets valid ProteinFragment sequences.
        /// </summary>
        /// <returns>
        ///  	<see cref="T:List{byte[]}"/>: valid ProteinFragment sequences.
        /// </returns>
        private List<byte[]> GetValidProteinFragments() => 
            new List<byte[]>
            {
                ASCII.GetBytes("A.T.G"),
                ASCII.GetBytes("A.TG.C"),
                ASCII.GetBytes("ATG.C"),
                ASCII.GetBytes("A.TGC")
            };

        /// <summary>
        /// 	Gets valid ProteinScapePeptide sequences.
        /// </summary>
        /// <returns>
        ///  	<see cref="T:List{byte[]}"/>: valid ProteinScapePeptide sequences.
        /// </returns>
        /// <remarks>
        ///     Optionally includes symbols from base alphabets.
        /// </remarks>
        private List<byte[]> GetValidProteinScapePeptides()
        {
            var cTerm = ASCII.GetString(new[] { ProteinScapePeptide.CTerminus });
            var nTerm = ASCII.GetString(new[] { ProteinScapePeptide.NTerminus });
            var sequences = new List<byte[]>
            {
                ASCII.GetBytes("A.T." + cTerm),
                ASCII.GetBytes("A.TG." + cTerm),
                ASCII.GetBytes(nTerm + ".T.G"),
                ASCII.GetBytes(nTerm + ".TG.C"),
                ASCII.GetBytes(nTerm + ".T." + cTerm),
                ASCII.GetBytes(nTerm + ".TG." + cTerm)
            };

            return sequences;
        }

        /// <summary>
        /// 	Gets valid RNA sequences.
        /// </summary>
        /// <returns>
        ///  	<see cref="T:List{byte[]}"/>: valid RNA sequences.
        /// </returns>
        private List<byte[]> GetValidRNA() =>
            new List<byte[]>
            {
                ASCII.GetBytes("A"),
                ASCII.GetBytes("AUGC"),
                ASCII.GetBytes("AUG--C"),
            };

        [SuppressMessage("ReSharper", "RedundantAssignment")]
        private void ValidatePublicProperties(AlphabetType option)
        {
            IAlphabet alphabetInstance = null;
            var alphabetType = AlphabetTypes.None;
            int count = 0;
            var hasAmbiguity = true;
            var hasGaps = true;
            var hasTermination = true;
            var isCaseSensitive = true;
            var isComplementSupported = true;
            var name = "";

            switch (option)
            {
                case AlphabetType.AmbiguousDNA:
                    alphabetInstance = AmbiguousDNA;
                    alphabetType = AlphabetTypes.AmbiguousDNA;
                    count = 16;
                    hasAmbiguity = true;
                    hasGaps = true;
                    hasTermination = false;
                    isCaseSensitive = false;
                    isComplementSupported = true;
                    name = "AmbiguousDna";
                    break;
                case AlphabetType.AmbiguousProtein:
                    alphabetInstance = AmbiguousProtein;
                    alphabetType = AlphabetTypes.AmbiguousProtein;
                    count = 28;
                    hasAmbiguity = true;
                    hasGaps = true;
                    hasTermination = true;
                    isCaseSensitive = false;
                    isComplementSupported = false;
                    name = "AmbiguousProtein";
                    break;
                case AlphabetType.AmbiguousRNA:
                    alphabetInstance = AmbiguousRNA;
                    alphabetType = AlphabetTypes.AmbiguousRNA;
                    count = 16;
                    hasAmbiguity = true;
                    hasGaps = true;
                    hasTermination = false;
                    isCaseSensitive = false;
                    isComplementSupported = true;
                    name = "AmbiguousRna";
                    break;
                case AlphabetType.AminoAcids:
                    alphabetInstance = AminoAcids;
                    alphabetType = AlphabetTypes.AminoAcids;
                    count = 22;
                    hasAmbiguity = false;
                    hasGaps = false;
                    hasTermination = false;
                    isCaseSensitive = true;
                    isComplementSupported = false;
                    name = "AminoAcids";
                    break;
                case AlphabetType.DNA:
                    alphabetInstance = DNA;
                    alphabetType = AlphabetTypes.DNA;
                    count = 5;
                    hasAmbiguity = false;
                    hasGaps = true;
                    hasTermination = false;
                    isCaseSensitive = false;
                    isComplementSupported = true;
                    name = "Dna";
                    break;
                case AlphabetType.PeaksPeptide:
                    alphabetInstance = PeaksPeptide;
                    alphabetType = AlphabetTypes.PeaksPeptide;
                    count = 41;
                    hasAmbiguity = false;
                    hasGaps = false;
                    hasTermination = false;
                    isCaseSensitive = true;
                    isComplementSupported = false;
                    name = "PeaksPeptide";
                    break;
                case AlphabetType.ProteinFragment:
                    alphabetInstance = ProteinFragment;
                    alphabetType = AlphabetTypes.ProteinFragment;
                    count = 23;
                    hasAmbiguity = false;
                    hasGaps = false;
                    hasTermination = false;
                    isCaseSensitive = true;
                    isComplementSupported = false;
                    name = "ProteinFragment";
                    break;
                case AlphabetType.ProteinScapePeptide:
                    alphabetInstance = ProteinScapePeptide;
                    alphabetType = AlphabetTypes.ProteinScapePeptide;
                    count = 25;
                    hasAmbiguity = false;
                    hasGaps = false;
                    hasTermination = false;
                    isCaseSensitive = true;
                    isComplementSupported = false;
                    name = "ProteinScapePeptide";
                    break;
                case AlphabetType.Protein:
                    alphabetInstance = Protein;
                    alphabetType = AlphabetTypes.Protein;
                    count = 24;
                    hasAmbiguity = false;
                    hasGaps = true;
                    hasTermination = true;
                    isCaseSensitive = false;
                    isComplementSupported = false;
                    name = "Protein";
                    break;
                case AlphabetType.RNA:
                    alphabetInstance = RNA;
                    alphabetType = AlphabetTypes.RNA;
                    count = 5;
                    hasAmbiguity = false;
                    hasGaps = true;
                    hasTermination = false;
                    isCaseSensitive = false;
                    isComplementSupported = true;
                    name = "Rna";
                    break;
            }

            Assert.AreEqual(alphabetType, alphabetInstance?.AlphabetType);
            Assert.AreEqual(count, alphabetInstance?.Count);
            Assert.AreEqual(hasGaps, alphabetInstance?.HasGaps);
            Assert.AreEqual(hasAmbiguity, alphabetInstance?.HasAmbiguity);
            Assert.AreEqual(hasTermination, alphabetInstance?.HasTerminations);
            Assert.AreEqual(isCaseSensitive, alphabetInstance?.IsCaseSensitive);
            Assert.AreEqual(isComplementSupported, alphabetInstance?.IsComplementSupported);
            Assert.AreEqual(name, alphabetInstance?.Name);
        }

        private bool IsAlphabetOfType(AlphabetTypes alphabetTypes, AlphabetTypes testValue) =>
            ((alphabetTypes | AlphabetTypes.PlugIn) & testValue) == testValue;

        #endregion Helper Methods

        /// <summary>
        /// 	Tests the values of the <see cref="AlphabetTypes"/> enumeration.
        /// </summary>
        [Test]
        public void AlphabetTypesTest()
        {
            var alphabetType = AlphabetTypes.DNA;
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllAminoAcids, alphabetType));
            Assert.AreEqual(true, IsAlphabetOfType(AlphabetTypes.AllDNA, alphabetType));
            Assert.AreEqual(true, IsAlphabetOfType(AlphabetTypes.AllNucleotides, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllRNA, alphabetType));

            alphabetType = AlphabetTypes.AmbiguousDNA;
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllAminoAcids, alphabetType));
            Assert.AreEqual(true, IsAlphabetOfType(AlphabetTypes.AllDNA, alphabetType));
            Assert.AreEqual(true, IsAlphabetOfType(AlphabetTypes.AllNucleotides, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllRNA, alphabetType));

            alphabetType = AlphabetTypes.RNA;
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllAminoAcids, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllDNA, alphabetType));
            Assert.AreEqual(true, IsAlphabetOfType(AlphabetTypes.AllNucleotides, alphabetType));
            Assert.AreEqual(true, IsAlphabetOfType(AlphabetTypes.AllRNA, alphabetType));

            alphabetType = AlphabetTypes.AmbiguousRNA;
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllAminoAcids, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllDNA, alphabetType));
            Assert.AreEqual(true, IsAlphabetOfType(AlphabetTypes.AllNucleotides, alphabetType));
            Assert.AreEqual(true, IsAlphabetOfType(AlphabetTypes.AllRNA, alphabetType));

            alphabetType = AlphabetTypes.AminoAcids;
            Assert.AreEqual(true, IsAlphabetOfType(AlphabetTypes.AllAminoAcids, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllDNA, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllNucleotides, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllRNA, alphabetType));

            alphabetType = AlphabetTypes.Protein;
            Assert.AreEqual(true, IsAlphabetOfType(AlphabetTypes.AllAminoAcids, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllDNA, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllNucleotides, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllRNA, alphabetType));

            alphabetType = AlphabetTypes.AmbiguousProtein;
            Assert.AreEqual(true, IsAlphabetOfType(AlphabetTypes.AllAminoAcids, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllDNA, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllNucleotides, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllRNA, alphabetType));

            alphabetType = AlphabetTypes.ProteinFragment;
            Assert.AreEqual(true, IsAlphabetOfType(AlphabetTypes.AllAminoAcids, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllDNA, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllNucleotides, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllRNA, alphabetType));

            alphabetType = AlphabetTypes.ProteinScapePeptide;
            Assert.AreEqual(true, IsAlphabetOfType(AlphabetTypes.AllAminoAcids, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllDNA, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllNucleotides, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllRNA, alphabetType));

            alphabetType = AlphabetTypes.PeaksPeptide;
            Assert.AreEqual(true, IsAlphabetOfType(AlphabetTypes.AllAminoAcids, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllDNA, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllNucleotides, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllRNA, alphabetType));

            // Now test with the Plug-In bit turned on for each alphabet type.

            alphabetType = AlphabetTypes.DNA | AlphabetTypes.PlugIn;
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllAminoAcids, alphabetType));
            Assert.AreEqual(true, IsAlphabetOfType(AlphabetTypes.AllDNA, alphabetType));
            Assert.AreEqual(true, IsAlphabetOfType(AlphabetTypes.AllNucleotides, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllRNA, alphabetType));

            alphabetType = AlphabetTypes.AmbiguousDNA | AlphabetTypes.PlugIn;
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllAminoAcids, alphabetType));
            Assert.AreEqual(true, IsAlphabetOfType(AlphabetTypes.AllDNA, alphabetType));
            Assert.AreEqual(true, IsAlphabetOfType(AlphabetTypes.AllNucleotides, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllRNA, alphabetType));

            alphabetType = AlphabetTypes.RNA | AlphabetTypes.PlugIn;
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllAminoAcids, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllDNA, alphabetType));
            Assert.AreEqual(true, IsAlphabetOfType(AlphabetTypes.AllNucleotides, alphabetType));
            Assert.AreEqual(true, IsAlphabetOfType(AlphabetTypes.AllRNA, alphabetType));

            alphabetType = AlphabetTypes.AmbiguousRNA | AlphabetTypes.PlugIn;
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllAminoAcids, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllDNA, alphabetType));
            Assert.AreEqual(true, IsAlphabetOfType(AlphabetTypes.AllNucleotides, alphabetType));
            Assert.AreEqual(true, IsAlphabetOfType(AlphabetTypes.AllRNA, alphabetType));

            alphabetType = AlphabetTypes.AminoAcids | AlphabetTypes.PlugIn;
            Assert.AreEqual(true, IsAlphabetOfType(AlphabetTypes.AllAminoAcids, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllDNA, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllNucleotides, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllRNA, alphabetType));

            alphabetType = AlphabetTypes.Protein | AlphabetTypes.PlugIn;
            Assert.AreEqual(true, IsAlphabetOfType(AlphabetTypes.AllAminoAcids, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllDNA, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllNucleotides, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllRNA, alphabetType));

            alphabetType = AlphabetTypes.AmbiguousProtein | AlphabetTypes.PlugIn;
            Assert.AreEqual(true, IsAlphabetOfType(AlphabetTypes.AllAminoAcids, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllDNA, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllNucleotides, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllRNA, alphabetType));

            alphabetType = AlphabetTypes.ProteinFragment | AlphabetTypes.PlugIn;
            Assert.AreEqual(true, IsAlphabetOfType(AlphabetTypes.AllAminoAcids, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllDNA, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllNucleotides, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllRNA, alphabetType));

            alphabetType = AlphabetTypes.ProteinScapePeptide | AlphabetTypes.PlugIn;
            Assert.AreEqual(true, IsAlphabetOfType(AlphabetTypes.AllAminoAcids, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllDNA, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllNucleotides, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllRNA, alphabetType));

            alphabetType = AlphabetTypes.PeaksPeptide | AlphabetTypes.PlugIn;
            Assert.AreEqual(true, IsAlphabetOfType(AlphabetTypes.AllAminoAcids, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllDNA, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllNucleotides, alphabetType));
            Assert.AreEqual(false, IsAlphabetOfType(AlphabetTypes.AllRNA, alphabetType));
        }

        /// <summary>
        /// 	Tests the AminoAcidsAlphabet ValidateSequence method with
        ///     valid and with invalid sequences.
        /// </summary>
        [Test]
        public void AminoAcidsAlphabetValidateSequenceTest()
        {
            // Test with valid symbols.
            foreach (byte[] sequence in GetValidAminoAcids())
            {
                Assert.AreEqual(true, AminoAcids.ValidateSequence(sequence));
            }

            // Test with invalid symbols
            foreach (byte[] sequence in GetInvalidAminoAcids())
            {
                Assert.AreEqual(false, AminoAcids.ValidateSequence(sequence));
            }
        }

        [Test]
        public void CompareToTest()
        {
            var dummyAlphabet = DummyAlphabet.Instance;

            Assert.AreEqual(Identical, DNA.CompareTo(DNA));
            Assert.AreEqual(Subset, DNA.CompareTo(AmbiguousDNA));
            Assert.AreEqual(Intersects, DNA.CompareTo(RNA));
            Assert.AreEqual(Intersects, DNA.CompareTo(AmbiguousRNA));
            Assert.AreEqual(Subset, DNA.CompareTo(Protein));
            Assert.AreEqual(Subset, DNA.CompareTo(AmbiguousProtein));
            Assert.AreEqual(Intersects, DNA.CompareTo(AminoAcids));
            Assert.AreEqual(Intersects, DNA.CompareTo(ProteinFragment));
            Assert.AreEqual(Intersects, DNA.CompareTo(ProteinScapePeptide));
            Assert.AreEqual(Intersects, DNA.CompareTo(PeaksPeptide));
            Assert.AreEqual(NoOverlap, DNA.CompareTo(dummyAlphabet));

            Assert.AreEqual(Superset, AmbiguousDNA.CompareTo(DNA));
            Assert.AreEqual(Identical, AmbiguousDNA.CompareTo(AmbiguousDNA));
            Assert.AreEqual(Intersects, AmbiguousDNA.CompareTo(RNA));
            Assert.AreEqual(Intersects, AmbiguousDNA.CompareTo(AmbiguousRNA));
            Assert.AreEqual(Intersects, AmbiguousDNA.CompareTo(Protein));
            Assert.AreEqual(Subset, AmbiguousDNA.CompareTo(AmbiguousProtein));
            Assert.AreEqual(Intersects, AmbiguousDNA.CompareTo(AminoAcids));
            Assert.AreEqual(Intersects, AmbiguousDNA.CompareTo(ProteinFragment));
            Assert.AreEqual(Intersects, AmbiguousDNA.CompareTo(ProteinScapePeptide));
            Assert.AreEqual(Intersects, AmbiguousDNA.CompareTo(PeaksPeptide));
            Assert.AreEqual(NoOverlap, AmbiguousDNA.CompareTo(dummyAlphabet));

            Assert.AreEqual(Intersects, RNA.CompareTo(DNA));
            Assert.AreEqual(Intersects, RNA.CompareTo(AmbiguousDNA));
            Assert.AreEqual(Identical, RNA.CompareTo(RNA));
            Assert.AreEqual(Subset, RNA.CompareTo(AmbiguousRNA));
            Assert.AreEqual(Subset, RNA.CompareTo(Protein));
            Assert.AreEqual(Subset, RNA.CompareTo(AmbiguousProtein));
            Assert.AreEqual(Intersects, RNA.CompareTo(AminoAcids));
            Assert.AreEqual(Intersects, RNA.CompareTo(ProteinFragment));
            Assert.AreEqual(Intersects, RNA.CompareTo(ProteinScapePeptide));
            Assert.AreEqual(Intersects, RNA.CompareTo(PeaksPeptide));
            Assert.AreEqual(NoOverlap, RNA.CompareTo(dummyAlphabet));

            Assert.AreEqual(Intersects, AmbiguousRNA.CompareTo(DNA));
            Assert.AreEqual(Intersects, AmbiguousRNA.CompareTo(AmbiguousDNA));
            Assert.AreEqual(Superset, AmbiguousRNA.CompareTo(RNA));
            Assert.AreEqual(Identical, AmbiguousRNA.CompareTo(AmbiguousRNA));
            Assert.AreEqual(Intersects, AmbiguousRNA.CompareTo(Protein));
            Assert.AreEqual(Subset, AmbiguousRNA.CompareTo(AmbiguousProtein));
            Assert.AreEqual(Intersects, AmbiguousRNA.CompareTo(AminoAcids));
            Assert.AreEqual(Intersects, AmbiguousRNA.CompareTo(ProteinFragment));
            Assert.AreEqual(Intersects, AmbiguousRNA.CompareTo(ProteinScapePeptide));
            Assert.AreEqual(Intersects, AmbiguousRNA.CompareTo(PeaksPeptide));
            Assert.AreEqual(NoOverlap, AmbiguousRNA.CompareTo(dummyAlphabet));

            Assert.AreEqual(Superset, Protein.CompareTo(DNA));
            Assert.AreEqual(Intersects, Protein.CompareTo(AmbiguousDNA));
            Assert.AreEqual(Superset, Protein.CompareTo(RNA));
            Assert.AreEqual(Intersects, Protein.CompareTo(AmbiguousRNA));
            Assert.AreEqual(Identical, Protein.CompareTo(Protein));
            Assert.AreEqual(Subset, Protein.CompareTo(AmbiguousProtein));
            Assert.AreEqual(Superset, Protein.CompareTo(AminoAcids));
            Assert.AreEqual(Intersects, Protein.CompareTo(ProteinFragment));
            Assert.AreEqual(Intersects, Protein.CompareTo(ProteinScapePeptide));
            Assert.AreEqual(Intersects, Protein.CompareTo(PeaksPeptide));
            Assert.AreEqual(NoOverlap, Protein.CompareTo(dummyAlphabet));

            Assert.AreEqual(Superset, AmbiguousProtein.CompareTo(DNA));
            Assert.AreEqual(Superset, AmbiguousProtein.CompareTo(AmbiguousDNA));
            Assert.AreEqual(Superset, AmbiguousProtein.CompareTo(RNA));
            Assert.AreEqual(Superset, AmbiguousProtein.CompareTo(AmbiguousRNA));
            Assert.AreEqual(Superset, AmbiguousProtein.CompareTo(Protein));
            Assert.AreEqual(Identical, AmbiguousProtein.CompareTo(AmbiguousProtein));
            Assert.AreEqual(Superset, AmbiguousProtein.CompareTo(AminoAcids));
            Assert.AreEqual(Intersects, AmbiguousProtein.CompareTo(ProteinFragment));
            Assert.AreEqual(Intersects, AmbiguousProtein.CompareTo(ProteinScapePeptide));
            Assert.AreEqual(Intersects, AmbiguousProtein.CompareTo(PeaksPeptide));
            Assert.AreEqual(NoOverlap, AmbiguousProtein.CompareTo(dummyAlphabet));

            Assert.AreEqual(Intersects, AminoAcids.CompareTo(DNA));
            Assert.AreEqual(Intersects, AminoAcids.CompareTo(AmbiguousDNA));
            Assert.AreEqual(Intersects, AminoAcids.CompareTo(RNA));
            Assert.AreEqual(Intersects, AminoAcids.CompareTo(AmbiguousRNA));
            Assert.AreEqual(Subset, AminoAcids.CompareTo(Protein));
            Assert.AreEqual(Subset, AminoAcids.CompareTo(AmbiguousProtein));
            Assert.AreEqual(Identical, AminoAcids.CompareTo(AminoAcids));
            Assert.AreEqual(Subset, AminoAcids.CompareTo(ProteinFragment));
            Assert.AreEqual(Subset, AminoAcids.CompareTo(ProteinScapePeptide));
            Assert.AreEqual(Subset, AminoAcids.CompareTo(PeaksPeptide));
            Assert.AreEqual(NoOverlap, AminoAcids.CompareTo(dummyAlphabet));

            Assert.AreEqual(Intersects, ProteinFragment.CompareTo(DNA));
            Assert.AreEqual(Intersects, ProteinFragment.CompareTo(AmbiguousDNA));
            Assert.AreEqual(Intersects, ProteinFragment.CompareTo(RNA));
            Assert.AreEqual(Intersects, ProteinFragment.CompareTo(AmbiguousRNA));
            Assert.AreEqual(Intersects, ProteinFragment.CompareTo(Protein));
            Assert.AreEqual(Intersects, ProteinFragment.CompareTo(AmbiguousProtein));
            Assert.AreEqual(Superset, ProteinFragment.CompareTo(AminoAcids));
            Assert.AreEqual(Identical, ProteinFragment.CompareTo(ProteinFragment));
            Assert.AreEqual(Subset, ProteinFragment.CompareTo(ProteinScapePeptide));
            Assert.AreEqual(Subset, ProteinFragment.CompareTo(PeaksPeptide));
            Assert.AreEqual(NoOverlap, ProteinFragment.CompareTo(dummyAlphabet));

            Assert.AreEqual(Intersects, ProteinScapePeptide.CompareTo(DNA));
            Assert.AreEqual(Intersects, ProteinScapePeptide.CompareTo(AmbiguousDNA));
            Assert.AreEqual(Intersects, ProteinScapePeptide.CompareTo(RNA));
            Assert.AreEqual(Intersects, ProteinScapePeptide.CompareTo(AmbiguousRNA));
            Assert.AreEqual(Intersects, ProteinScapePeptide.CompareTo(Protein));
            Assert.AreEqual(Intersects, ProteinScapePeptide.CompareTo(AmbiguousProtein));
            Assert.AreEqual(Superset, ProteinScapePeptide.CompareTo(AminoAcids));
            Assert.AreEqual(Superset, ProteinScapePeptide.CompareTo(ProteinFragment));
            Assert.AreEqual(Identical, ProteinScapePeptide.CompareTo(ProteinScapePeptide));
            Assert.AreEqual(Intersects, ProteinScapePeptide.CompareTo(PeaksPeptide));
            Assert.AreEqual(NoOverlap, ProteinScapePeptide.CompareTo(dummyAlphabet));

            Assert.AreEqual(Intersects, PeaksPeptide.CompareTo(DNA));
            Assert.AreEqual(Intersects, PeaksPeptide.CompareTo(AmbiguousDNA));
            Assert.AreEqual(Intersects, PeaksPeptide.CompareTo(RNA));
            Assert.AreEqual(Intersects, PeaksPeptide.CompareTo(AmbiguousRNA));
            Assert.AreEqual(Intersects, PeaksPeptide.CompareTo(Protein));
            Assert.AreEqual(Intersects, PeaksPeptide.CompareTo(AmbiguousProtein));
            Assert.AreEqual(Superset, PeaksPeptide.CompareTo(AminoAcids));
            Assert.AreEqual(Superset, PeaksPeptide.CompareTo(ProteinFragment));
            Assert.AreEqual(Intersects, PeaksPeptide.CompareTo(ProteinScapePeptide));
            Assert.AreEqual(Identical, PeaksPeptide.CompareTo(PeaksPeptide));
            Assert.AreEqual(NoOverlap, PeaksPeptide.CompareTo(dummyAlphabet));
        }

        /// <summary>
        /// 	Tests the HasDnaSymbols() extension method.
        /// </summary>
        [Test]
        public void HasAminoAcidSymbolsTest()
        {
            Assert.AreEqual(true, AminoAcids.HasAminoAcidSymbols());
            Assert.AreEqual(true, ProteinFragment.HasAminoAcidSymbols());
            Assert.AreEqual(true, ProteinScapePeptide.HasAminoAcidSymbols());
            Assert.AreEqual(true, PeaksPeptide.HasAminoAcidSymbols());
            Assert.AreEqual(true, Protein.HasAminoAcidSymbols());
            Assert.AreEqual(true, AmbiguousProtein.HasAminoAcidSymbols());
            Assert.AreEqual(false, DNA.HasAminoAcidSymbols());
            Assert.AreEqual(false, AmbiguousDNA.HasAminoAcidSymbols());
            Assert.AreEqual(false, RNA.HasAminoAcidSymbols());
            Assert.AreEqual(false, AmbiguousRNA.HasAminoAcidSymbols());
        }

        /// <summary>
        /// 	Tests the HasDnaSymbols() extension method.
        /// </summary>
        [Test]
        public void HasDnaSymbolsTest()
        {
            Assert.AreEqual(false, AminoAcids.HasDnaSymbols());
            Assert.AreEqual(false, ProteinFragment.HasDnaSymbols());
            Assert.AreEqual(false, ProteinScapePeptide.HasDnaSymbols());
            Assert.AreEqual(false, PeaksPeptide.HasDnaSymbols());
            Assert.AreEqual(false, Protein.HasDnaSymbols());
            Assert.AreEqual(false, AmbiguousProtein.HasDnaSymbols());
            Assert.AreEqual(true, DNA.HasDnaSymbols());
            Assert.AreEqual(true, AmbiguousDNA.HasDnaSymbols());
            Assert.AreEqual(false, RNA.HasDnaSymbols());
            Assert.AreEqual(false, AmbiguousRNA.HasDnaSymbols());
        }

        /// <summary>
        /// 	Tests the HasDnaSymbols() extension method.
        /// </summary>
        [Test]
        public void HasRnaSymbolsTest()
        {
            Assert.AreEqual(false, AminoAcids.HasRnaSymbols());
            Assert.AreEqual(false, ProteinFragment.HasRnaSymbols());
            Assert.AreEqual(false, ProteinScapePeptide.HasRnaSymbols());
            Assert.AreEqual(false, PeaksPeptide.HasRnaSymbols());
            Assert.AreEqual(false, Protein.HasRnaSymbols());
            Assert.AreEqual(false, AmbiguousProtein.HasRnaSymbols());
            Assert.AreEqual(false, DNA.HasRnaSymbols());
            Assert.AreEqual(false, AmbiguousDNA.HasRnaSymbols());
            Assert.AreEqual(true, RNA.HasRnaSymbols());
            Assert.AreEqual(true, AmbiguousRNA.HasRnaSymbols());
        }

        /// <summary>
        /// 	Tests the IsSubsetOf() extension method.
        /// </summary>
        [Test]
        public void IsSubsetOfTest()
        {
            // Test AminoAcidsAlphabet
            Assert.AreEqual(false, AminoAcids.IsSubsetOf(AminoAcids));
            Assert.AreEqual(true, AminoAcids.IsSubsetOf(ProteinFragment));
            Assert.AreEqual(true, AminoAcids.IsSubsetOf(ProteinScapePeptide));
            Assert.AreEqual(true, AminoAcids.IsSubsetOf(PeaksPeptide));
            Assert.AreEqual(true, AminoAcids.IsSubsetOf(Protein));
            Assert.AreEqual(true, AminoAcids.IsSubsetOf(AmbiguousProtein));
            Assert.AreEqual(false, AminoAcids.IsSubsetOf(DNA));
            Assert.AreEqual(false, AminoAcids.IsSubsetOf(RNA));
            Assert.AreEqual(false, AminoAcids.IsSubsetOf(AmbiguousDNA));
            Assert.AreEqual(false, AminoAcids.IsSubsetOf(AmbiguousRNA));

            // Test ProteinFragmentAlphabet
            Assert.AreEqual(false, ProteinFragment.IsSubsetOf(AminoAcids));
            Assert.AreEqual(false, ProteinFragment.IsSubsetOf(ProteinFragment));
            Assert.AreEqual(true, ProteinFragment.IsSubsetOf(ProteinScapePeptide));
            Assert.AreEqual(true, ProteinFragment.IsSubsetOf(PeaksPeptide));
            Assert.AreEqual(false, ProteinFragment.IsSubsetOf(Protein));
            Assert.AreEqual(false, ProteinFragment.IsSubsetOf(AmbiguousProtein));
            Assert.AreEqual(false, ProteinFragment.IsSubsetOf(DNA));
            Assert.AreEqual(false, ProteinFragment.IsSubsetOf(RNA));
            Assert.AreEqual(false, ProteinFragment.IsSubsetOf(AmbiguousDNA));
            Assert.AreEqual(false, ProteinFragment.IsSubsetOf(AmbiguousRNA));


            // Test ProteinScapePeptideAlphabet
            Assert.AreEqual(false, ProteinScapePeptide.IsSubsetOf(AminoAcids));
            Assert.AreEqual(false, ProteinScapePeptide.IsSubsetOf(ProteinFragment));
            Assert.AreEqual(false, ProteinScapePeptide.IsSubsetOf(ProteinScapePeptide));
            Assert.AreEqual(false, ProteinScapePeptide.IsSubsetOf(PeaksPeptide));
            Assert.AreEqual(false, ProteinScapePeptide.IsSubsetOf(Protein));
            Assert.AreEqual(false, ProteinScapePeptide.IsSubsetOf(AmbiguousProtein));
            Assert.AreEqual(false, ProteinScapePeptide.IsSubsetOf(DNA));
            Assert.AreEqual(false, ProteinScapePeptide.IsSubsetOf(RNA));
            Assert.AreEqual(false, ProteinScapePeptide.IsSubsetOf(AmbiguousDNA));
            Assert.AreEqual(false, ProteinScapePeptide.IsSubsetOf(AmbiguousRNA));

            // Test PeaksPeptideAlphabet
            Assert.AreEqual(false, PeaksPeptide.IsSubsetOf(AminoAcids));
            Assert.AreEqual(false, PeaksPeptide.IsSubsetOf(ProteinFragment));
            Assert.AreEqual(false, PeaksPeptide.IsSubsetOf(ProteinScapePeptide));
            Assert.AreEqual(false, PeaksPeptide.IsSubsetOf(PeaksPeptide));
            Assert.AreEqual(false, PeaksPeptide.IsSubsetOf(Protein));
            Assert.AreEqual(false, PeaksPeptide.IsSubsetOf(AmbiguousProtein));
            Assert.AreEqual(false, PeaksPeptide.IsSubsetOf(DNA));
            Assert.AreEqual(false, PeaksPeptide.IsSubsetOf(RNA));
            Assert.AreEqual(false, PeaksPeptide.IsSubsetOf(AmbiguousDNA));
            Assert.AreEqual(false, PeaksPeptide.IsSubsetOf(AmbiguousRNA));

            // Test ProteinAlphabet
            Assert.AreEqual(false, Protein.IsSubsetOf(AminoAcids));
            Assert.AreEqual(false, Protein.IsSubsetOf(ProteinFragment));
            Assert.AreEqual(false, Protein.IsSubsetOf(ProteinScapePeptide));
            Assert.AreEqual(false, Protein.IsSubsetOf(PeaksPeptide));
            Assert.AreEqual(false, Protein.IsSubsetOf(Protein));
            Assert.AreEqual(true, Protein.IsSubsetOf(AmbiguousProtein));
            Assert.AreEqual(false, Protein.IsSubsetOf(DNA));
            Assert.AreEqual(false, Protein.IsSubsetOf(RNA));
            Assert.AreEqual(false, Protein.IsSubsetOf(AmbiguousDNA));
            Assert.AreEqual(false, Protein.IsSubsetOf(AmbiguousRNA));

            // Test AmbiguousProteinAlphabet
            Assert.AreEqual(false, AmbiguousProtein.IsSubsetOf(AminoAcids));
            Assert.AreEqual(false, AmbiguousProtein.IsSubsetOf(ProteinFragment));
            Assert.AreEqual(false, AmbiguousProtein.IsSubsetOf(ProteinScapePeptide));
            Assert.AreEqual(false, AmbiguousProtein.IsSubsetOf(PeaksPeptide));
            Assert.AreEqual(false, AmbiguousProtein.IsSubsetOf(Protein));
            Assert.AreEqual(false, AmbiguousProtein.IsSubsetOf(AmbiguousProtein));
            Assert.AreEqual(false, AmbiguousProtein.IsSubsetOf(DNA));
            Assert.AreEqual(false, AmbiguousProtein.IsSubsetOf(RNA));
            Assert.AreEqual(false, AmbiguousProtein.IsSubsetOf(AmbiguousDNA));
            Assert.AreEqual(false, AmbiguousProtein.IsSubsetOf(AmbiguousRNA));

            // Test DNAAlphabet
            Assert.AreEqual(false, DNA.IsSubsetOf(AminoAcids));
            Assert.AreEqual(false, DNA.IsSubsetOf(ProteinFragment));
            Assert.AreEqual(false, DNA.IsSubsetOf(ProteinScapePeptide));
            Assert.AreEqual(false, DNA.IsSubsetOf(PeaksPeptide));
            Assert.AreEqual(true, DNA.IsSubsetOf(Protein));
            Assert.AreEqual(true, DNA.IsSubsetOf(AmbiguousProtein));
            Assert.AreEqual(false, DNA.IsSubsetOf(DNA));
            Assert.AreEqual(false, DNA.IsSubsetOf(RNA));
            Assert.AreEqual(true, DNA.IsSubsetOf(AmbiguousDNA));
            Assert.AreEqual(false, DNA.IsSubsetOf(AmbiguousRNA));

            // Test RNAAlphabet
            Assert.AreEqual(false, RNA.IsSubsetOf(AminoAcids));
            Assert.AreEqual(false, RNA.IsSubsetOf(ProteinFragment));
            Assert.AreEqual(false, RNA.IsSubsetOf(ProteinScapePeptide));
            Assert.AreEqual(false, RNA.IsSubsetOf(PeaksPeptide));
            Assert.AreEqual(true, RNA.IsSubsetOf(Protein));
            Assert.AreEqual(true, RNA.IsSubsetOf(AmbiguousProtein));
            Assert.AreEqual(false, RNA.IsSubsetOf(DNA));
            Assert.AreEqual(false, RNA.IsSubsetOf(RNA));
            Assert.AreEqual(false, RNA.IsSubsetOf(AmbiguousDNA));
            Assert.AreEqual(true, RNA.IsSubsetOf(AmbiguousRNA));

            // Test AmbiguousDNAAlphabet
            Assert.AreEqual(false, AmbiguousDNA.IsSubsetOf(AminoAcids));
            Assert.AreEqual(false, AmbiguousDNA.IsSubsetOf(ProteinFragment));
            Assert.AreEqual(false, AmbiguousDNA.IsSubsetOf(ProteinScapePeptide));
            Assert.AreEqual(false, AmbiguousDNA.IsSubsetOf(PeaksPeptide));
            Assert.AreEqual(false, AmbiguousDNA.IsSubsetOf(Protein));
            Assert.AreEqual(true, AmbiguousDNA.IsSubsetOf(AmbiguousProtein));
            Assert.AreEqual(false, AmbiguousDNA.IsSubsetOf(DNA));
            Assert.AreEqual(false, AmbiguousDNA.IsSubsetOf(RNA));
            Assert.AreEqual(false, AmbiguousDNA.IsSubsetOf(AmbiguousDNA));
            Assert.AreEqual(false, AmbiguousDNA.IsSubsetOf(AmbiguousRNA));

            // Test AmbiguousRNAAlphabet
            Assert.AreEqual(false, AmbiguousRNA.IsSubsetOf(AminoAcids));
            Assert.AreEqual(false, AmbiguousRNA.IsSubsetOf(ProteinFragment));
            Assert.AreEqual(false, AmbiguousRNA.IsSubsetOf(ProteinScapePeptide));
            Assert.AreEqual(false, AmbiguousRNA.IsSubsetOf(PeaksPeptide));
            Assert.AreEqual(false, AmbiguousRNA.IsSubsetOf(Protein));
            Assert.AreEqual(true, AmbiguousRNA.IsSubsetOf(AmbiguousProtein));
            Assert.AreEqual(false, AmbiguousRNA.IsSubsetOf(DNA));
            Assert.AreEqual(false, AmbiguousRNA.IsSubsetOf(RNA));
            Assert.AreEqual(false, AmbiguousRNA.IsSubsetOf(AmbiguousDNA));
            Assert.AreEqual(false, AmbiguousRNA.IsSubsetOf(AmbiguousRNA));
        }

        /// <summary>
        /// 	Tests the IsSupersetOf() extension method.
        /// </summary>
        [Test]
        public void IsSupersetOfTest()
        {
            // Test AminoAcids
            Assert.AreEqual(false, AminoAcids.IsSupersetOf(AminoAcids));
            Assert.AreEqual(false, AminoAcids.IsSupersetOf(ProteinFragment));
            Assert.AreEqual(false, AminoAcids.IsSupersetOf(ProteinScapePeptide));
            Assert.AreEqual(false, AminoAcids.IsSupersetOf(PeaksPeptide));
            Assert.AreEqual(false, AminoAcids.IsSupersetOf(Protein));
            Assert.AreEqual(false, AminoAcids.IsSupersetOf(AmbiguousProtein));
            Assert.AreEqual(false, AminoAcids.IsSupersetOf(DNA));
            Assert.AreEqual(false, AminoAcids.IsSupersetOf(RNA));
            Assert.AreEqual(false, AminoAcids.IsSupersetOf(AmbiguousDNA));
            Assert.AreEqual(false, AminoAcids.IsSupersetOf(AmbiguousRNA));

            // Test ProteinFragment
            Assert.AreEqual(true, ProteinFragment.IsSupersetOf(AminoAcids));
            Assert.AreEqual(false, ProteinFragment.IsSupersetOf(ProteinFragment));
            Assert.AreEqual(false, ProteinFragment.IsSupersetOf(ProteinScapePeptide));
            Assert.AreEqual(false, ProteinFragment.IsSupersetOf(PeaksPeptide));
            Assert.AreEqual(false, ProteinFragment.IsSupersetOf(Protein));
            Assert.AreEqual(false, ProteinFragment.IsSupersetOf(AmbiguousProtein));
            Assert.AreEqual(false, ProteinFragment.IsSupersetOf(DNA));
            Assert.AreEqual(false, ProteinFragment.IsSupersetOf(RNA));
            Assert.AreEqual(false, ProteinFragment.IsSupersetOf(AmbiguousDNA));
            Assert.AreEqual(false, ProteinFragment.IsSupersetOf(AmbiguousRNA));

            // Test ProteinScapePeptide
            Assert.AreEqual(true, ProteinScapePeptide.IsSupersetOf(AminoAcids));
            Assert.AreEqual(true, ProteinScapePeptide.IsSupersetOf(ProteinFragment));
            Assert.AreEqual(false, ProteinScapePeptide.IsSupersetOf(ProteinScapePeptide));
            Assert.AreEqual(false, ProteinScapePeptide.IsSupersetOf(PeaksPeptide));
            Assert.AreEqual(false, ProteinScapePeptide.IsSupersetOf(Protein));
            Assert.AreEqual(false, ProteinScapePeptide.IsSupersetOf(AmbiguousProtein));
            Assert.AreEqual(false, ProteinScapePeptide.IsSupersetOf(DNA));
            Assert.AreEqual(false, ProteinScapePeptide.IsSupersetOf(RNA));
            Assert.AreEqual(false, ProteinScapePeptide.IsSupersetOf(AmbiguousDNA));
            Assert.AreEqual(false, ProteinScapePeptide.IsSupersetOf(AmbiguousRNA));

            // Test PeaksPeptide
            Assert.AreEqual(true, PeaksPeptide.IsSupersetOf(AminoAcids));
            Assert.AreEqual(true, PeaksPeptide.IsSupersetOf(ProteinFragment));
            Assert.AreEqual(false, PeaksPeptide.IsSupersetOf(ProteinScapePeptide));
            Assert.AreEqual(false, PeaksPeptide.IsSupersetOf(PeaksPeptide));
            Assert.AreEqual(false, PeaksPeptide.IsSupersetOf(Protein));
            Assert.AreEqual(false, PeaksPeptide.IsSupersetOf(AmbiguousProtein));
            Assert.AreEqual(false, PeaksPeptide.IsSupersetOf(DNA));
            Assert.AreEqual(false, PeaksPeptide.IsSupersetOf(RNA));
            Assert.AreEqual(false, PeaksPeptide.IsSupersetOf(AmbiguousDNA));
            Assert.AreEqual(false, PeaksPeptide.IsSupersetOf(AmbiguousRNA));

            // Test Protein
            Assert.AreEqual(true, Protein.IsSupersetOf(AminoAcids));
            Assert.AreEqual(false, Protein.IsSupersetOf(ProteinFragment));
            Assert.AreEqual(false, Protein.IsSupersetOf(ProteinScapePeptide));
            Assert.AreEqual(false, Protein.IsSupersetOf(PeaksPeptide));
            Assert.AreEqual(false, Protein.IsSupersetOf(Protein));
            Assert.AreEqual(false, Protein.IsSupersetOf(AmbiguousProtein));
            Assert.AreEqual(true, Protein.IsSupersetOf(DNA));
            Assert.AreEqual(true, Protein.IsSupersetOf(RNA));
            Assert.AreEqual(false, Protein.IsSupersetOf(AmbiguousDNA));
            Assert.AreEqual(false, Protein.IsSupersetOf(AmbiguousRNA));

            // Test AmbiguousProtein
            Assert.AreEqual(true, AmbiguousProtein.IsSupersetOf(AminoAcids));
            Assert.AreEqual(false, AmbiguousProtein.IsSupersetOf(ProteinFragment));
            Assert.AreEqual(false, AmbiguousProtein.IsSupersetOf(ProteinScapePeptide));
            Assert.AreEqual(false, AmbiguousProtein.IsSupersetOf(PeaksPeptide));
            Assert.AreEqual(true, AmbiguousProtein.IsSupersetOf(Protein));
            Assert.AreEqual(false, AmbiguousProtein.IsSupersetOf(AmbiguousProtein));
            Assert.AreEqual(true, AmbiguousProtein.IsSupersetOf(DNA));
            Assert.AreEqual(true, AmbiguousProtein.IsSupersetOf(RNA));
            Assert.AreEqual(true, AmbiguousProtein.IsSupersetOf(AmbiguousDNA));
            Assert.AreEqual(true, AmbiguousProtein.IsSupersetOf(AmbiguousRNA));

            // Test DNA
            Assert.AreEqual(false, DNA.IsSupersetOf(AminoAcids));
            Assert.AreEqual(false, DNA.IsSupersetOf(ProteinFragment));
            Assert.AreEqual(false, DNA.IsSupersetOf(ProteinScapePeptide));
            Assert.AreEqual(false, DNA.IsSupersetOf(PeaksPeptide));
            Assert.AreEqual(false, DNA.IsSupersetOf(Protein));
            Assert.AreEqual(false, DNA.IsSupersetOf(AmbiguousProtein));
            Assert.AreEqual(false, DNA.IsSupersetOf(DNA));
            Assert.AreEqual(false, DNA.IsSupersetOf(RNA));
            Assert.AreEqual(false, DNA.IsSupersetOf(AmbiguousDNA));
            Assert.AreEqual(false, DNA.IsSupersetOf(AmbiguousRNA));

            // Test RNA
            Assert.AreEqual(false, RNA.IsSupersetOf(AminoAcids));
            Assert.AreEqual(false, RNA.IsSupersetOf(ProteinFragment));
            Assert.AreEqual(false, RNA.IsSupersetOf(ProteinScapePeptide));
            Assert.AreEqual(false, RNA.IsSupersetOf(PeaksPeptide));
            Assert.AreEqual(false, RNA.IsSupersetOf(Protein));
            Assert.AreEqual(false, RNA.IsSupersetOf(AmbiguousProtein));
            Assert.AreEqual(false, RNA.IsSupersetOf(DNA));
            Assert.AreEqual(false, RNA.IsSupersetOf(RNA));
            Assert.AreEqual(false, RNA.IsSupersetOf(AmbiguousDNA));
            Assert.AreEqual(false, RNA.IsSupersetOf(AmbiguousRNA));

            // Test AmbiguousDNA
            Assert.AreEqual(false, AmbiguousDNA.IsSupersetOf(AminoAcids));
            Assert.AreEqual(false, AmbiguousDNA.IsSupersetOf(ProteinFragment));
            Assert.AreEqual(false, AmbiguousDNA.IsSupersetOf(ProteinScapePeptide));
            Assert.AreEqual(false, AmbiguousDNA.IsSupersetOf(PeaksPeptide));
            Assert.AreEqual(false, AmbiguousDNA.IsSupersetOf(Protein));
            Assert.AreEqual(false, AmbiguousDNA.IsSupersetOf(AmbiguousProtein));
            Assert.AreEqual(true, AmbiguousDNA.IsSupersetOf(DNA));
            Assert.AreEqual(false, AmbiguousDNA.IsSupersetOf(RNA));
            Assert.AreEqual(false, AmbiguousDNA.IsSupersetOf(AmbiguousDNA));
            Assert.AreEqual(false, AmbiguousDNA.IsSupersetOf(AmbiguousRNA));

            // Test AmbiguousRNA
            Assert.AreEqual(false, AmbiguousRNA.IsSupersetOf(AminoAcids));
            Assert.AreEqual(false, AmbiguousRNA.IsSupersetOf(ProteinFragment));
            Assert.AreEqual(false, AmbiguousRNA.IsSupersetOf(ProteinScapePeptide));
            Assert.AreEqual(false, AmbiguousRNA.IsSupersetOf(PeaksPeptide));
            Assert.AreEqual(false, AmbiguousRNA.IsSupersetOf(Protein));
            Assert.AreEqual(false, AmbiguousRNA.IsSupersetOf(AmbiguousProtein));
            Assert.AreEqual(false, AmbiguousRNA.IsSupersetOf(DNA));
            Assert.AreEqual(true, AmbiguousRNA.IsSupersetOf(RNA));
            Assert.AreEqual(false, AmbiguousRNA.IsSupersetOf(AmbiguousDNA));
            Assert.AreEqual(false, AmbiguousRNA.IsSupersetOf(AmbiguousRNA));
        }

        /// <summary>
        /// 	Tests the PeaksPeptideAlphabet ValidateSequence method with
        ///     valid and with invalid sequences.
        /// </summary>
        [Test]
        public void PeaksPeptideValidateSequenceTest()
        {
            // Test with valid symbols
            foreach (byte[] sequence in GetValidPeaksPeptides())
            {
                Assert.AreEqual(true, PeaksPeptide.ValidateSequence(sequence));
            }

            // Test with invalid symbols
            foreach (byte[] sequence in GetInvalidPeaksPeptides())
            {
                Debug.Print($"Invalid PEAKS sequence: {ASCII.GetString(sequence)}\n");
                Assert.AreEqual(false, PeaksPeptide.ValidateSequence(sequence));
            }
        }

        /// <summary>
        /// 	Tests the ProteinFragmentAlphabet ValidateSequence method with
        ///     valid and with invalid sequences.
        /// </summary>
        [Test]
        public void ProteinFragmentValidateSequenceTest()
        {
            // Test with valid symbols
            foreach (byte[] sequence in GetValidProteinFragments())
            {
                Assert.AreEqual(true, ProteinFragment.ValidateSequence(sequence));
            }

            // Test with invalid symbols
            foreach (byte[] sequence in GetInvalidProteinFragments())
            {
                Assert.AreEqual(false, ProteinFragment.ValidateSequence(sequence));
            }
        }

        /// <summary>
        /// 	Tests the ProteinFragmentAlphabet ValidateSequence method with
        ///     valid and with invalid sequences.
        /// </summary>
        [Test]
        public void ProteinScapePeptideValidateSequenceTest()
        {
            // Test with valid ProteinScapePeptide symbols. 
            foreach (byte[] sequence in GetValidProteinScapePeptides())
            {
                Assert.AreEqual(true, ProteinScapePeptide.ValidateSequence(sequence));
            }

            // Test with invalid symbols
            foreach (byte[] sequence in GetInvalidProteinScapePeptides())
            {
                Assert.AreEqual(false, ProteinScapePeptide.ValidateSequence(sequence));
            }
        }

        /// <summary>
        /// 	Validates each overload of AutoDetectAminoAcidsAlphabet().
        /// </summary>
        [Test]
        public void ValidateAutoDetectAminoAcidsAlphabet()
        {
            IAminoAcidsAlphabet alphabet;

            // Auto-detect simple amino acid sequences.
            foreach (byte[] symbols in GetValidAminoAcids())
            {
                Debug.Print($"Valid amino acid sequence: {ASCII.GetString(symbols)}\n");
                alphabet = AutoDetectAminoAcidsAlphabet(symbols);
                Assert.AreEqual(AminoAcids, alphabet);
                alphabet = AutoDetectAminoAcidsAlphabet(symbols, AminoAcids);
                Assert.AreEqual(AminoAcids, alphabet);
                alphabet = AutoDetectAminoAcidsAlphabet(symbols, 0, GetSymbolCount(symbols), AminoAcids);
                Assert.AreEqual(AminoAcids, alphabet);
            }

            //  Auto-detect ProteinFragment sequences.
            foreach (byte[] symbols in GetValidProteinFragments())
            {
                Debug.Print($"Valid ProteinFragment sequence: {ASCII.GetString(symbols)}\n");
                alphabet = AutoDetectAminoAcidsAlphabet(symbols);
                Assert.AreEqual(ProteinFragment, alphabet);
                alphabet = AutoDetectAminoAcidsAlphabet(symbols, ProteinFragment);
                Assert.AreEqual(ProteinFragment, alphabet);
                alphabet = AutoDetectAminoAcidsAlphabet(symbols, 0, GetSymbolCount(symbols), ProteinFragment);
                Assert.AreEqual(ProteinFragment, alphabet);
            }

            // Auto-detect ProteinScapePeptide sequences.
            foreach (byte[] symbols in GetValidProteinScapePeptides())
            {
                Debug.Print($"Valid ProteinScapePeptide sequence: {ASCII.GetString(symbols)}\n");
                alphabet = AutoDetectAminoAcidsAlphabet(symbols);
                Assert.AreEqual(ProteinScapePeptide, alphabet);
                alphabet = AutoDetectAminoAcidsAlphabet(symbols, ProteinScapePeptide);
                Assert.AreEqual(ProteinScapePeptide, alphabet);
                alphabet = AutoDetectAminoAcidsAlphabet(symbols, 0, GetSymbolCount(symbols), ProteinScapePeptide);
                Assert.AreEqual(ProteinScapePeptide, alphabet);
            }

            // Auto-detect PeaksPeptide sequences.
            foreach (byte[] symbols in GetValidPeaksPeptides())
            {
                Debug.Print($"Valid PeaksPeptide sequence: {ASCII.GetString(symbols)}\n");
                alphabet = AutoDetectAminoAcidsAlphabet(symbols);
                Assert.AreEqual(PeaksPeptide, alphabet);
                alphabet = AutoDetectAminoAcidsAlphabet(symbols, PeaksPeptide);
                Assert.AreEqual(PeaksPeptide, alphabet);
                alphabet = AutoDetectAminoAcidsAlphabet(symbols, 0, GetSymbolCount(symbols), PeaksPeptide);
                Assert.AreEqual(PeaksPeptide, alphabet);
            }
        }

        /// <summary>
        /// 	Validates GetConsensusAlphabet().
        /// </summary>
        [Test]
        public void ValidateGetConsensusAlphabet()
        {
            IAlphabet alphabet;

            // Test overloads with DNA sequences.
            alphabet = GetConsensusAlphabet(GetValidDNA());
            Assert.AreEqual(true, (alphabet == DNA));
            alphabet = GetConsensusAlphabet(AllDNA, GetValidDNA());
            Assert.AreEqual(true, (alphabet == DNA));
            alphabet = GetConsensusAlphabet(AllRNA, GetValidDNA());
            Assert.AreEqual(true, (alphabet == null));

            // Test overloads with DNA and ambiguous DNA sequences.
            alphabet = GetConsensusAlphabet(GetValidDNA(), GetValidAmbiguousDNA());
            Assert.AreEqual(true, (alphabet == AmbiguousDNA));
            alphabet = GetConsensusAlphabet(AllDNA, GetValidDNA(), GetValidAmbiguousDNA());
            Assert.AreEqual(true, (alphabet == AmbiguousDNA));
            alphabet = GetConsensusAlphabet(AllRNA, GetValidDNA(), GetValidAmbiguousDNA());
            Assert.AreEqual(true, (alphabet == null));

            // Test with DNA and ambiguous DNA sequences, in reversed order.
            alphabet = GetConsensusAlphabet(GetValidAmbiguousDNA(), GetValidDNA());
            Assert.AreEqual(true, (alphabet == AmbiguousDNA));

            // Test overloads with RNA sequences.
            alphabet = GetConsensusAlphabet(GetValidRNA());
            Assert.AreEqual(true, (alphabet == RNA));
            alphabet = GetConsensusAlphabet(AllRNA, GetValidRNA());
            Assert.AreEqual(true, (alphabet == RNA));
            alphabet = GetConsensusAlphabet(AllDNA, GetValidRNA());
            Assert.AreEqual(true, (alphabet == null));

            // Test overloads with RNA and ambiguous RNA sequences.
            alphabet = GetConsensusAlphabet(GetValidRNA(), GetValidAmbiguousRNA());
            Assert.AreEqual(true, (alphabet == AmbiguousRNA));
            alphabet = GetConsensusAlphabet(AllRNA, GetValidRNA(), GetValidAmbiguousRNA());
            Assert.AreEqual(true, (alphabet == AmbiguousRNA));
            alphabet = GetConsensusAlphabet(AllDNA, GetValidRNA(), GetValidAmbiguousRNA());
            Assert.AreEqual(true, (alphabet == null));

            // Test with RNA and ambiguous RNA sequences, in reversed order.
            alphabet = GetConsensusAlphabet(GetValidAmbiguousRNA(), GetValidRNA());
            Assert.AreEqual(true, (alphabet == AmbiguousRNA));

            // Test with DNA and RNA sequences.
            // The Protein alphabet contains all DNA and RNA symbols, so gets detected if all alphabets are searched.
            alphabet = GetConsensusAlphabet(GetValidDNA(), GetValidRNA());
            Assert.AreEqual(true, (alphabet == Protein));
            alphabet = GetConsensusAlphabet(All, GetValidDNA(), GetValidRNA());
            Assert.AreEqual(true, (alphabet == Protein));
            alphabet = GetConsensusAlphabet(AllDNA, GetValidDNA(), GetValidRNA());
            Assert.AreEqual(true, (alphabet == null));
            alphabet = GetConsensusAlphabet(AllRNA, GetValidDNA(), GetValidRNA());
            Assert.AreEqual(true, (alphabet == null));

            // Test with ambiguous DNA and RNA sequences.
            // The Protein alphabet contains all ambiguous DNA and RNA symbols.
            alphabet = GetConsensusAlphabet(GetValidAmbiguousDNA(), GetValidAmbiguousRNA());
            Assert.AreEqual(true, (alphabet == Protein));

            // Test with AminoAcids sequences.
            // The ambiguous DNA alphabet contains all the symbols in the test amino acid sequences.
            alphabet = GetConsensusAlphabet(GetValidAminoAcids());
            Assert.AreEqual(true, (alphabet == AmbiguousDNA));

            // Test with combination of AminoAcids and ProteinFragment sequences.
            alphabet = GetConsensusAlphabet(GetValidAminoAcids(), GetValidProteinFragments());
            Assert.AreEqual(true, (alphabet == ProteinFragment));

            // Test with combination of AminoAcids and ProteinFragment sequences, in reverse order.
            alphabet = GetConsensusAlphabet(GetValidProteinFragments(), GetValidAminoAcids());
            Assert.AreEqual(true, (alphabet == ProteinFragment));

            // Test with combination of AminoAcids, ProteinFragment and ProteinScapePeptide sequences.
            alphabet = GetConsensusAlphabet(GetValidAminoAcids(), GetValidProteinFragments(),
                GetValidProteinScapePeptides());
            Assert.AreEqual(true, (alphabet == ProteinScapePeptide));

            // Test with combination of AminoAcids, ProteinFragment and ProteinScapePeptide sequences, in reverse order.
            alphabet = GetConsensusAlphabet(GetValidProteinScapePeptides(), GetValidProteinFragments(),
                GetValidAminoAcids());
            Assert.AreEqual(true, (alphabet == ProteinScapePeptide));

            // Test with combination of AminoAcids, ProteinFragment and PeaksPeptide sequences.
            alphabet = GetConsensusAlphabet(GetValidAminoAcids(), GetValidProteinFragments(),
                GetValidPeaksPeptides());
            Assert.AreEqual(true, (alphabet == PeaksPeptide));

            // Test with combination of AminoAcids, ProteinFragment and PeaksPeptide sequences, in reverse order.
            alphabet = GetConsensusAlphabet(GetValidPeaksPeptides(), GetValidProteinFragments(),
                GetValidAminoAcids());
            Assert.AreEqual(true, (alphabet == PeaksPeptide));

            // Test with combination of AminoAcids, ProteinFragment, ProteinScapePeptide and PeaksPeptide sequences.
            // There is no possible consensus alphabet.
            alphabet = GetConsensusAlphabet(GetValidAminoAcids(), GetValidProteinFragments(),
                GetValidProteinScapePeptides(), GetValidPeaksPeptides());
            Assert.AreEqual(true, (alphabet == null));
        }

        /// <summary>
        /// 	Validates GetConcensusAminoAcidsAlphabet().
        /// </summary>
        /// <remarks>
        ///     Verifies that it returns the correct consensus alphabet type for the input sequences.
        /// </remarks>
        [Test]
        public void ValidateGetConsensusAminoAcidsAlphabet()
        {
            // Test with AminoAcids sequences.
            var alphabet = GetConsensusAminoAcidsAlphabet(GetValidAminoAcids());
            Assert.AreEqual(true, (alphabet == AminoAcids));

            // Test with combination of AminoAcids and ProteinFragment sequences.
            alphabet = GetConsensusAminoAcidsAlphabet(GetValidAminoAcids(), GetValidProteinFragments());
            Assert.AreEqual(true, (alphabet == ProteinFragment));

            // Test with combination of AminoAcids and ProteinFragment sequences, in reverse order.
            alphabet = GetConsensusAminoAcidsAlphabet(GetValidProteinFragments(), GetValidAminoAcids());
            Assert.AreEqual(true, (alphabet == ProteinFragment));

            // Test with combination of AminoAcids, ProteinFragment and ProteinScapePeptide sequences.
            alphabet = GetConsensusAminoAcidsAlphabet(GetValidAminoAcids(), GetValidProteinFragments(),
                GetValidProteinScapePeptides());
            Assert.AreEqual(true, (alphabet == ProteinScapePeptide));

            // Test with combination of AminoAcids, ProteinFragment and ProteinScapePeptide sequences, in reverse order.
            alphabet = GetConsensusAminoAcidsAlphabet(GetValidProteinScapePeptides(), GetValidProteinFragments(),
                GetValidAminoAcids());
            Assert.AreEqual(true, (alphabet == ProteinScapePeptide));

            // Test with combination of AminoAcids, ProteinFragment and PeaksPeptide sequences.
            alphabet = GetConsensusAminoAcidsAlphabet(GetValidAminoAcids(), GetValidProteinFragments(),
                GetValidPeaksPeptides());
            Assert.AreEqual(true, (alphabet == PeaksPeptide));

            // Test with combination of AminoAcids, ProteinFragment and PeaksPeptide sequences, in reverse order.
            alphabet = GetConsensusAminoAcidsAlphabet(GetValidPeaksPeptides(), GetValidProteinFragments(),
                GetValidAminoAcids());
            Assert.AreEqual(true, (alphabet == PeaksPeptide));

            // Test with combination of AminoAcids, ProteinFragment, ProteinScapePeptide and PeaksPeptide sequences.
            // There is no possible consensus alphabet.
            alphabet = GetConsensusAminoAcidsAlphabet(GetValidAminoAcids(), GetValidProteinFragments(),
                GetValidProteinScapePeptides(), GetValidPeaksPeptides());
            Assert.AreEqual(true, (alphabet == null));
        }

        /// <summary>
        ///     Validate AminoAcids's public properties.
        /// </summary>
        [Test]
        public void ValidatePublicPropertiesForAminoAcids()
        {
            ValidatePublicProperties(AlphabetType.AminoAcids);
        }

        /// <summary>
        ///     Validate AmbiguousDNA's public properties.
        /// </summary>
        [Test]
        public void ValidatePublicPropertiesForAmbiguousDNA()
        {
            ValidatePublicProperties(AlphabetType.AmbiguousDNA);
        }

        /// <summary>
        ///     Validate AmbiguousProtein's public properties.
        /// </summary>
        [Test]
        public void ValidatePublicPropertiesForAmbiguousProtein()
        {
            ValidatePublicProperties(AlphabetType.AmbiguousProtein);
        }

        /// <summary>
        ///     Validate AmbiguousRNA's public properties.
        /// </summary>
        [Test]
        public void ValidatePublicPropertiesForAmbiguousRNA()
        {
            ValidatePublicProperties(AlphabetType.AmbiguousRNA);
        }

        /// <summary>
        ///     Validate DNA's public properties.
        /// </summary>
        [Test]
        public void ValidatePublicPropertiesForDNA()
        {
            ValidatePublicProperties(AlphabetType.DNA);
        }

        /// <summary>
        ///     Validate PeaksPeptide's public properties.
        /// </summary>
        [Test]
        public void ValidatePublicPropertiesForPeaksPeptide()
        {
            ValidatePublicProperties(AlphabetType.PeaksPeptide);
        }

        /// <summary>
        ///     Validate Protein's public properties.
        /// </summary>
        [Test]
        public void ValidatePublicPropertiesForProtein()
        {
            ValidatePublicProperties(AlphabetType.Protein);
        }

        /// <summary>
        ///     Validate ProteinFragment's public properties.
        /// </summary>
        [Test]
        public void ValidatePublicPropertiesForProteinFragment()
        {
            ValidatePublicProperties(AlphabetType.ProteinFragment);
        }

        /// <summary>
        ///     Validate ProteinScapePeptide's public properties.
        /// </summary>
        [Test]
        public void ValidatePublicPropertiesForProteinScapePeptide()
        {
            ValidatePublicProperties(AlphabetType.ProteinScapePeptide);
        }

        /// <summary>
        ///     Validate RNA's public properties.
        /// </summary>
        [Test]
        public void ValidatePublicPropertiesForRNA()
        {
            ValidatePublicProperties(AlphabetType.RNA);
        }
    }
}
