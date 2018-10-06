using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Bio.Properties;

namespace Bio
{
    /// <inheritdoc cref="IAmbiguousRnaAlphabet" />
    /// <seealso cref="RnaAlphabet"/>,
    /// <seealso cref="IAmbiguousRnaAlphabet"/>

    public class AmbiguousRnaAlphabet : RnaAlphabet, IAmbiguousRnaAlphabet
    {
        /// <summary>
        ///     New instance of the AmbiguousRnaAlphabet class.
        /// </summary>
        public new static readonly AmbiguousRnaAlphabet Instance;

        /// <summary>
        ///     Initializes static members of the AmbiguousRnaAlphabet class.
        /// </summary>
        static AmbiguousRnaAlphabet()
        {
            Instance = new AmbiguousRnaAlphabet();
        }

        /// <summary>
        ///     Initializes a new instance of the AmbiguousRnaAlphabet class.
        /// </summary>
        protected AmbiguousRnaAlphabet()
        {
            Name = Resource.AmbiguousRnaAlphabetName;
            HasAmbiguity = true;

            AC = (byte)'M';
            GA = (byte)'R';
            GC = (byte)'S';
            AU = (byte)'W';
            UC = (byte)'Y';
            GU = (byte)'K';
            GCA = (byte)'V';
            ACU = (byte)'H';
            GAU = (byte)'D';
            GUC = (byte)'B';
            Any = (byte)'N';

            AddNucleotide(Any, "Any", (byte)'n');
            AddNucleotide(AC, "Adenine or Cytosine", (byte)'m');
            AddNucleotide(GA, "Guanine or Adenine", (byte)'r');
            AddNucleotide(GC, "Guanine or Cytosine", (byte)'s');
            AddNucleotide(AU, "Adenine or Uracil", (byte)'w');
            AddNucleotide(UC, "Uracil or Cytosine", (byte)'y');
            AddNucleotide(GU, "Guanine or Uracil", (byte)'k');
            AddNucleotide(GCA, "Guanine or Cytosine or Adenine", (byte)'v');
            AddNucleotide(ACU, "Adenine or Cytosine or Uracil", (byte)'h');
            AddNucleotide(GAU, "Guanine or Adenine or Uracil", (byte)'d');
            AddNucleotide(GUC, "Guanine or Uracil or Cytosine", (byte)'b');

            // map complements.
            MapComplementNucleotide(Any, Any);
            MapComplementNucleotide(AC, GU);
            MapComplementNucleotide(AU, AU);
            MapComplementNucleotide(ACU, GAU);
            MapComplementNucleotide(GA, UC);
            MapComplementNucleotide(GC, GC);
            MapComplementNucleotide(GU, AC);
            MapComplementNucleotide(GAU, ACU);
            MapComplementNucleotide(GCA, GUC);
            MapComplementNucleotide(GUC, GCA);
            MapComplementNucleotide(UC, GA);

            // Map ambiguous symbols.
            MapAmbiguousNucleotide(Any, new byte[] { A, C, G, U });
            MapAmbiguousNucleotide(AC, new byte[] { A, C });
            MapAmbiguousNucleotide(GA, new byte[] { G, A });
            MapAmbiguousNucleotide(GC, new byte[] { G, C });
            MapAmbiguousNucleotide(AU, new byte[] { A, U });
            MapAmbiguousNucleotide(UC, new byte[] { U, C });
            MapAmbiguousNucleotide(GU, new byte[] { G, U });
            MapAmbiguousNucleotide(GCA, new byte[] { G, C, A });
            MapAmbiguousNucleotide(ACU, new byte[] { A, C, U });
            MapAmbiguousNucleotide(GAU, new byte[] { G, A, U });
            MapAmbiguousNucleotide(GUC, new byte[] { G, U, C });
        }

        /// <inheritdoc />
        public byte AC { get; private set; }

        /// <inheritdoc />
        public byte GA { get; private set; }

        /// <inheritdoc />
        public byte GC { get; private set; }

        /// <inheritdoc />
        public byte AU { get; private set; }

        /// <inheritdoc />
        public byte UC { get; private set; }

        /// <inheritdoc />
        public byte GU { get; private set; }

        /// <inheritdoc />
        public byte GCA { get; private set; }

        /// <inheritdoc />
        public byte ACU { get; private set; }

        /// <inheritdoc />
        public byte GAU { get; private set; }

        /// <inheritdoc />
        public byte GUC { get; private set; }

        /// <inheritdoc />
        public byte Any { get; private set; }

        /// <inheritdoc />
        public override byte GetConsensusSymbol(HashSet<byte> symbols)
        {   
            if (symbols == null)
            {
                throw new ArgumentNullException(nameof(symbols));
            }

            if (symbols.Count == 0)
            {
                throw new ArgumentException(Resource.SymbolCountZero);
            }

            // Validate that all are valid DNA symbols
            var validValues = GetValidSymbols();

            var symbolsInUpperCase = new HashSet<byte>();

            foreach (var symbol in symbols)
            {
                if (!validValues.Contains(symbol))
                {
                    throw new ArgumentException(string.Format(
                        CultureInfo.CurrentCulture, Resource.INVALID_SYMBOL, (char)symbol, Name));
                }

                var upperCaseSymbol = symbol;
                if (symbol >= 97 && symbol <= 122)
                {
                    upperCaseSymbol = (byte)(symbol - 32);
                }

                symbolsInUpperCase.Add(upperCaseSymbol);
            }

            // Remove all gap symbols
            HashSet<byte> gapItems;
            TryGetGapSymbols(out gapItems);

            byte defaultGap;
            TryGetDefaultGapSymbol(out defaultGap);

            symbolsInUpperCase.ExceptWith(gapItems);

            if (symbolsInUpperCase.Count == 0)
            {
                // All are gap characters, return default 'Gap'
                return defaultGap;
            }
            if (symbolsInUpperCase.Count == 1)
            {
                return symbols.First();
            }
            
            var baseSet = new HashSet<byte>();
            foreach (var n in symbolsInUpperCase)
            {
                HashSet<byte> ambiguousSymbols;
                if (TryGetBasicSymbols(n, out ambiguousSymbols))
                {
                    baseSet.UnionWith(ambiguousSymbols);
                }
                else
                {
                    // If not found in ambiguous map, it has to be base / unambiguous character
                    baseSet.Add(n);
                }
            }

            byte returnValue;
            TryGetAmbiguousSymbol(baseSet, out returnValue);

            return returnValue;
        }
    }
}
