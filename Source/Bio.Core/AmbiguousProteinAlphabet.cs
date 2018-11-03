using Bio.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static Bio.Alphabets;

namespace Bio
{
    /// <inheritdoc cref="IAmbiguousProteinAlphabet" />
    /// <seealso cref="ProteinAlphabet"/>,
    /// <seealso cref="IAmbiguousProteinAlphabet"/>

    public class AmbiguousProteinAlphabet : ProteinAlphabet, IAmbiguousProteinAlphabet
    {
        /// <summary>
        ///     Instance of the AmbiguousProteinAlphabet class.
        /// </summary>
        public new static readonly AmbiguousProteinAlphabet Instance;

        /// <summary>
        ///     Contains only ambiguous symbols including Termination.
        /// </summary>
        private readonly HashSet<byte> _ambiguousSymbols = new HashSet<byte>();

        /// <summary>
        ///     Initializes static members of the AmbiguousProteinAlphabet class.
        /// </summary>
        static AmbiguousProteinAlphabet()
        {
            Instance = new AmbiguousProteinAlphabet();
        }

        /// <summary>
        ///     Initializes a new instance of the AmbiguousProteinAlphabet class.
        /// </summary>
        protected AmbiguousProteinAlphabet()
        {
            Name = Resource.AmbiguousProteinAlphabetName;
            AlphabetType = AlphabetTypes.AmbiguousProtein;
            HasAmbiguity = true;

            B = (byte)'B';
            J = (byte)'J';
            X = (byte)'X';
            Z = (byte)'Z';

            // Add to ambiguous symbols
            _ambiguousSymbols.Add(B);
            _ambiguousSymbols.Add((byte)char.ToLower((char)B));
            _ambiguousSymbols.Add(J);
            _ambiguousSymbols.Add((byte)char.ToLower((char)J));
            _ambiguousSymbols.Add(X);
            _ambiguousSymbols.Add((byte)char.ToLower((char)X));
            _ambiguousSymbols.Add(Z);
            _ambiguousSymbols.Add((byte)char.ToLower((char)Z));
            _ambiguousSymbols.Add(Ter);

            AddAminoAcid(X, "Xaa", "Undetermined or atypical", false, (byte)'x');
            AddAminoAcid(Z, "Glx", "Glutamic Acid or Glutamine", false, (byte)'z');
            AddAminoAcid(B, "Asx", "Aspartic Acid or Asparagine", false, (byte)'b');
            AddAminoAcid(J, "Xle", "Leucine or Isoleucine", false, (byte)'j');

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
        /// <remarks></remarks>
        public override bool CheckIsAmbiguous(byte item)
        {
            return _ambiguousSymbols.Contains(item);
        }

        /// <inheritdoc />
        /// <remarks></remarks>
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
            HashSet<byte> validValues = GetValidSymbols();
            var symbolsInUpperCase = new HashSet<byte>();

            foreach (byte symbol in symbols)
            {
                if (!validValues.Contains(symbol))
                {
                    throw new ArgumentException(string.Format(
                        CultureInfo.CurrentCulture, Resource.INVALID_SYMBOL, symbol, Name));
                }

                byte upperCaseSymbol = symbol;
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
            TryGetGapSymbols(out HashSet<byte> gapItems);

            TryGetDefaultGapSymbol(out byte defaultGap);

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

                    foreach (byte n in symbolsInUpperCase)
                    {
                        if (TryGetBasicSymbols(n, out HashSet<byte> ambiguousSymbols))
                        {
                            baseSet.UnionWith(ambiguousSymbols);
                        }
                        else
                        {
                            // If not found in ambiguous map, it has to be base / unambiguous character
                            baseSet.Add(n);
                        }
                    }

                    return TryGetAmbiguousSymbol(baseSet, out byte returnValue) ? returnValue : X;
                }
            }
        }
    }
}
