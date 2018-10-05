using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Bio.Properties;

namespace Bio
{
    /// <summary>
    /// Adds symbols for ambiguous amino acid to those in the <see cref="ProteinAlphabet"/>.<br/>
    /// Specifically:
    /// B - Asp or Asn,
    /// J - Leu or Ile,
    /// X - any amino acid,
    /// Z - Glu or Gln.
    /// </summary>
    /// <seealso cref="ProteinAlphabet"/>,
    /// <seealso cref="IAmbiguousProteinAlphabet"/>,
    /// <seealso cref="IAlphabet"/>
    public class AmbiguousProteinAlphabet : ProteinAlphabet, IAmbiguousProteinAlphabet
    {
        /// <summary>
        /// Instance of the AmbiguousProteinAlphabet class.
        /// </summary>
        public new static readonly AmbiguousProteinAlphabet Instance;

        /// <summary>
        /// Initializes static members of the AmbiguousProteinAlphabet class.
        /// </summary>
        static AmbiguousProteinAlphabet()
        {
            Instance = new AmbiguousProteinAlphabet();
        }

        /// <summary>
        /// Initializes a new instance of the AmbiguousProteinAlphabet class.
        /// </summary>
        protected AmbiguousProteinAlphabet()
        {
            Name = Resource.AmbiguousProteinAlphabetName;
            HasAmbiguity = true;

            X = (byte)'X';
            Z = (byte)'Z';
            B = (byte)'B';
            J = (byte)'J';

            AddAminoAcid(X, "Xaa", "Undetermined or atypical", (byte)'x');
            AddAminoAcid(Z, "Glx", "Glutamic Acid or Glutamine", (byte)'z');
            AddAminoAcid(B, "Asx", "Aspartic Acid or Asparagine", (byte)'b');
            AddAminoAcid(J, "Xle", "Leucine or Isoleucine", (byte)'j');

            // Map ambiguous symbols.
            MapAmbiguousAminoAcid(B, new byte[] { D, N });
            MapAmbiguousAminoAcid(Z, new byte[] { Q, E });
            MapAmbiguousAminoAcid(J, new byte[] { L, I });
            MapAmbiguousAminoAcid(X, new byte[] { A, C, D, E, F, G, H, I, K, L, M, N, O, P, Q, R, S, T, U, V, W, Y });
        }

        /// <inheritdoc />
        public byte B { get; }

        /// <inheritdoc />
        public byte J { get; }

        /// <inheritdoc />
        public byte X { get; }

        /// <inheritdoc />
        public byte Z { get; }

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

            // Validate that all are valid protein symbols
            var validValues = GetValidSymbols();
            var symbolsInUpperCase = new HashSet<byte>();

            foreach (var symbol in symbols)
            {
                if (!validValues.Contains(symbol))
                {
                    throw new ArgumentException(string.Format(
                        CultureInfo.CurrentCulture, Resource.INVALID_SYMBOL, symbol, Name));
                }

                var upperCaseSymbol = symbol;
                if (symbol >= 97 && symbol <= 122)
                {
                    upperCaseSymbol = (byte)(symbol - 32);
                }

                symbolsInUpperCase.Add(upperCaseSymbol);
            }

            if (symbols.Contains(X))
            {
                return X;
            }

            // Remove all gap symbols
            TryGetGapSymbols(out var gapItems);

            TryGetDefaultGapSymbol(out var defaultGap);

            symbolsInUpperCase.ExceptWith(gapItems);

            switch (symbolsInUpperCase.Count)
            {
                case 0:
                    // All are gap characters, return default 'Gap'
                    return defaultGap;
                case 1:
                    return symbols.First();
                default:
                {
                    var baseSet = new HashSet<byte>();

                    foreach (var n in symbolsInUpperCase)
                    {
                        if (TryGetBasicSymbols(n, out var ambiguousSymbols))
                        {
                            baseSet.UnionWith(ambiguousSymbols);
                        }
                        else
                        {
                            // If not found in ambiguous map, it has to be base / unambiguous character
                            baseSet.Add(n);
                        }
                    }

                    return TryGetAmbiguousSymbol(baseSet, out var returnValue) ? returnValue : X;
                }
            }
        }
    }
}
