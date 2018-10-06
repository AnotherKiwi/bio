using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Bio.Properties;

namespace Bio
{
    /// <inheritdoc cref="IAmbiguousDnaAlphabet" />
    /// <seealso cref="DnaAlphabet"/>, 
    /// <seealso cref="IAmbiguousDnaAlphabet"/>
    public class AmbiguousDnaAlphabet : DnaAlphabet, IAmbiguousDnaAlphabet
    {
        /// <summary>
        ///     New instance of AmbiguousDnaAlphabet class.
        /// </summary>
        public new static readonly AmbiguousDnaAlphabet Instance;

        /// <summary>
        ///     Initializes static members of the AmbiguousDnaAlphabet class.
        /// </summary>
        static AmbiguousDnaAlphabet()
        {
            Instance = new AmbiguousDnaAlphabet();
        }

        /// <summary>
        ///     Initializes a new instance of the AmbiguousDnaAlphabet class.
        /// </summary>
        protected AmbiguousDnaAlphabet()
        {
            Name = Resource.AmbiguousDnaAlphabetName;
            HasAmbiguity = true;

            AC = (byte)'M';
            GA = (byte)'R';
            GC = (byte)'S';
            AT = (byte)'W';
            TC = (byte)'Y';
            GT = (byte)'K';
            GCA = (byte)'V';
            ACT = (byte)'H';
            GAT = (byte)'D';
            GTC = (byte)'B';
            Any = (byte)'N';

            AddNucleotide(AC, "Adenine or Cytosine", (byte)'m');
            AddNucleotide(GA, "Guanine or Adenine", (byte)'r');
            AddNucleotide(GC, "Guanine or Cytosine", (byte)'s');
            AddNucleotide(AT, "Adenine or Thymine", (byte)'w');
            AddNucleotide(TC, "Thymine or Cytosine", (byte)'y');
            AddNucleotide(GT, "Guanine or Thymine", (byte)'k');
            AddNucleotide(GCA, "Guanine or Cytosine or Adenine", (byte)'v');
            AddNucleotide(ACT, "Adenine or Cytosine or Thymine", (byte)'h');
            AddNucleotide(GAT, "Guanine or Adenine or Thymine", (byte)'d');
            AddNucleotide(GTC, "Guanine or Thymine or Cytosine", (byte)'b');
            AddNucleotide(Any, "Any", (byte)'n');

            // map complements.
            MapComplementNucleotide(Any, Any);
            MapComplementNucleotide(AC, GT);
            MapComplementNucleotide(AT, AT);
            MapComplementNucleotide(ACT, GAT);
            MapComplementNucleotide(GA, TC);
            MapComplementNucleotide(GC, GC);
            MapComplementNucleotide(GT, AC);
            MapComplementNucleotide(GAT, ACT);
            MapComplementNucleotide(GCA, GTC);
            MapComplementNucleotide(GTC, GCA);
            MapComplementNucleotide(TC, GA);

            // Map ambiguous symbols.
            MapAmbiguousNucleotide(Any, new byte[] { A, C, G, T });
            MapAmbiguousNucleotide(AC, new byte[] { A, C });
            MapAmbiguousNucleotide(GA, new byte[] { G, A });
            MapAmbiguousNucleotide(GC, new byte[] { G, C });
            MapAmbiguousNucleotide(AT, new byte[] { A, T });
            MapAmbiguousNucleotide(TC, new byte[] { T, C });
            MapAmbiguousNucleotide(GT, new byte[] { G, T });
            MapAmbiguousNucleotide(GCA, new byte[] { G, C, A });
            MapAmbiguousNucleotide(ACT, new byte[] { A, C, T });
            MapAmbiguousNucleotide(GAT, new byte[] { G, A, T });
            MapAmbiguousNucleotide(GTC, new byte[] { G, T, C });
        }

        /// <inheritdoc />
        public byte AC { get; private set; }

        /// <inheritdoc />
        public byte GA { get; private set; }

        /// <inheritdoc />
        public byte GC { get; private set; }

        /// <inheritdoc />
        public byte AT { get; private set; }

        /// <inheritdoc />
        public byte TC { get; private set; }

        /// <inheritdoc />
        public byte GT { get; private set; }

        /// <inheritdoc />
        public byte GCA { get; private set; }

        /// <inheritdoc />
        public byte ACT { get; private set; }

        /// <inheritdoc />
        public byte GAT { get; private set; }

        /// <inheritdoc />
        public byte GTC { get; private set; }

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

            var symbolsInUpperCase = new HashSet<byte>();
            
            // Validate that all are valid DNA symbols
            var validValues = GetValidSymbols();

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
            else if (symbolsInUpperCase.Count == 1)
            {
                return symbolsInUpperCase.First();
            }
            else
            {
                var baseSet = new HashSet<byte>();
                HashSet<byte> ambiguousSymbols;

                foreach (var n in symbolsInUpperCase)
                {
                    ambiguousSymbols = null;
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
}
