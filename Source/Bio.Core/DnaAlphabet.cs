using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Bio.Core.Extensions;
using Bio.Properties;
using Bio.Util;

namespace Bio
{
    /// <inheritdoc />
    /// <seealso cref="IDnaAlphabet"/>
    public class DnaAlphabet : IDnaAlphabet
    {
        #region Private members

        /// <summary>
        ///     Contains only basic symbols including Gap
        /// </summary>
        private readonly HashSet<byte> basicSymbols = new HashSet<byte>();

        /// <summary>
        ///     Nucleotide map  -  Maps A to A  and a to A
        ///     that is key will contain unique values.
        ///     This will be used in the IsValidSymbol method to address Scenarios like a == A, G == g etc.
        /// </summary>
        private readonly Dictionary<byte, byte> nucleotideValueMap = new Dictionary<byte, byte>();

        /// <summary>
        ///     Symbol to Friendly name mapping.
        /// </summary>
        private readonly Dictionary<byte, string> friendlyNameMap = new Dictionary<byte, string>();

        /// <summary>
        ///     Holds the nucleotides present in this DnaAlphabet.
        /// </summary>
        private readonly List<byte> nucleotides = new List<byte>();

        /// <summary>
        /// Mapping from set of symbols to corresponding ambiguous symbol.
        /// </summary>
        private Dictionary<HashSet<byte>, byte> basicSymbolsToAmbiguousSymbolMap = new Dictionary<HashSet<byte>, byte>(new HashSetComparer<byte>());

        /// <summary>
        ///     Mapping from ambiguous symbol to set of basic symbols they represent.
        /// </summary>
        private readonly Dictionary<byte, HashSet<byte>> ambiguousSyToBasicSymbolsMap =
            new Dictionary<byte, HashSet<byte>>();

        /// <summary>
        ///     Holds complements.
        /// </summary>
        private readonly Dictionary<byte, byte> symbolToComplementSymbolMap = new Dictionary<byte, byte>();

        #endregion Private members

        /// <summary>
        ///     Initializes static members of the DnaAlphabet class.
        /// </summary>
        static DnaAlphabet()
        {
            Instance = new DnaAlphabet();
        }

        /// <summary>
        ///     Initializes a new instance of the DnaAlphabet class.
        /// </summary>
        protected DnaAlphabet()
        {
            Name = Resource.DnaAlphabetName;
            HasGaps = true;
            HasAmbiguity = false;
            HasTerminations = false;
            IsComplementSupported = true;
            A = (byte)'A';
            C = (byte)'C';
            G = (byte)'G';
            T = (byte)'T';
            Gap = (byte)'-';

            // Add to basic symbols
            basicSymbols.Add(A);
            basicSymbols.Add((byte)char.ToLower((char)A));
            basicSymbols.Add(C);
            basicSymbols.Add((byte)char.ToLower((char)C));
            basicSymbols.Add(G);
            basicSymbols.Add((byte)char.ToLower((char)G));
            basicSymbols.Add(T);
            basicSymbols.Add((byte)char.ToLower((char)T));
            basicSymbols.Add(Gap);

            // Add nucleotides
            AddNucleotide(A, "Adenine", (byte)'a');
            AddNucleotide(C, "Cytosine", (byte)'c');
            AddNucleotide(G, "Guanine", (byte)'g');
            AddNucleotide(T, "Thymine", (byte)'t');
            AddNucleotide(Gap, "Gap");

            // Populate compliment data
            MapComplementNucleotide(A, T);
            MapComplementNucleotide(T, A);
            MapComplementNucleotide(C, G);
            MapComplementNucleotide(G, C);
            MapComplementNucleotide(Gap, Gap);
        }

        /// <inheritdoc />
        public byte A { get; private set; }

        /// <inheritdoc />
        public byte T { get; private set; }

        /// <inheritdoc />
        public byte G { get; private set; }

        /// <inheritdoc />
        public byte C { get; private set; }

        /// <inheritdoc />
        public byte Gap { get; private set; }

        /// <inheritdoc />
        public string Name { get; protected set; }

        /// <inheritdoc />
        public bool HasGaps { get; protected set; }

        /// <inheritdoc />
        /// <remarks>
        ///     This alphabet does NOT have symbols for ambiguity.
        /// </remarks>
        public bool HasAmbiguity { get; protected set; }

        /// <inheritdoc />
        /// <remarks>
        ///     This alphabet does NOT have termination symbols.
        /// </remarks>
        public bool HasTerminations { get; protected set; }

        /// <inheritdoc />
        /// <remarks>
        ///     This alphabet HAS support for complement.
        /// </remarks>
        public bool IsComplementSupported { get; protected set; }

        /// <inheritdoc />
        /// <remarks>
        ///     This IS a DNA alphabet.
        /// </remarks>
        public bool IsDna => true;

        /// <inheritdoc />
        /// <remarks>
        ///     This is NOT a protein alphabet.
        /// </remarks>
        public bool IsProtein => false;

        /// <inheritdoc />
        /// <remarks>
        ///     This is NOT a RNA alphabet.
        /// </remarks>
        public bool IsRna => false;

        /// <summary>
        ///     Static instance of this class.
        /// </summary>
        public static readonly DnaAlphabet Instance;

        /// <inheritdoc />
        public int Count => nucleotides.Count;

        /// <inheritdoc />
        public byte this[int index] => nucleotides[index];

        /// <inheritdoc />
        public string GetFriendlyName(byte item)
        {
            string fName;
            friendlyNameMap.TryGetValue(nucleotideValueMap[item], out fName);
            return fName;
        }

        /// <inheritdoc />
        /// <remarks>Complements are supported in this alphabet.</remarks>
        public bool TryGetComplementSymbol(byte symbol, out byte complementSymbol)
        {
            // verify whether the nucleotides exist or not.
            byte nucleotide;
            if (nucleotideValueMap.TryGetValue(symbol, out nucleotide))
            {
                return symbolToComplementSymbolMap.TryGetValue(nucleotide, out complementSymbol);
            }
            complementSymbol = default(byte);
            return false;
        }

        /// <inheritdoc />
        /// <remarks>Complements are supported in this alphabet.</remarks>
        public bool TryGetComplementSymbol(byte[] symbols, out byte[] complementSymbols)
        {
            if (symbols == null)
            {
                complementSymbols = null;
                return false;
            }

            var length = symbols.GetLongLength();
            complementSymbols = new byte[length];
            for (long index = 0; index < length; index++)
            {
                byte nucleotide;
                byte complementSymbol;
                if (nucleotideValueMap.TryGetValue(symbols[index], out nucleotide)
                    && symbolToComplementSymbolMap.TryGetValue(nucleotide, out complementSymbol))
                {
                    complementSymbols[index] = complementSymbol;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        /// <inheritdoc />
        /// <remarks>
        ///     This alphabet has a gap symbol, so the method returns <c>true</c>.
        /// </remarks>
        public virtual bool TryGetDefaultGapSymbol(out byte defaultGapSymbol)
        {
            defaultGapSymbol = Gap;
            return true;
        }

        /// <inheritdoc />
        /// <remarks>
        ///     This alphabet has a termination symbol, so the method returns <c>true</c>.
        /// </remarks>
        public virtual bool TryGetDefaultTerminationSymbol(out byte defaultTerminationSymbol)
        {
            defaultTerminationSymbol = default(byte);
            return false;
        }

        /// <inheritdoc />
        /// <remarks>
        ///     This alphabet has a gap symbol, so the method returns <c>true</c>.
        /// </remarks>
        public virtual bool TryGetGapSymbols(out HashSet<byte> gapSymbols)
        {
            gapSymbols = new HashSet<byte> { Gap };
            return true;
        }

        /// <inheritdoc />
        /// <remarks>
        ///     This alphabet has a termination symbol, so the method returns <c>true</c>.
        /// </remarks>
        public virtual bool TryGetTerminationSymbols(out HashSet<byte> terminationSymbols)
        {
            terminationSymbols = null;
            return false;
        }

        /// <inheritdoc />
        public HashSet<byte> GetValidSymbols()
        {
            return new HashSet<byte>(nucleotideValueMap.Keys);
        }

        /// <inheritdoc />
        public bool TryGetAmbiguousSymbol(HashSet<byte> symbols, out byte ambiguousSymbol)
        {
            return basicSymbolsToAmbiguousSymbolMap.TryGetValue(symbols, out ambiguousSymbol);
        }

        /// <inheritdoc />
        public bool TryGetBasicSymbols(byte ambiguousSymbol, out HashSet<byte> basicSymbols)
        {
            return ambiguousSyToBasicSymbolsMap.TryGetValue(ambiguousSymbol, out basicSymbols);
        }

        /// <inheritdoc />
        public virtual bool CompareSymbols(byte x, byte y)
        {
            byte nucleotideA;

            if (nucleotideValueMap.TryGetValue(x, out nucleotideA))
            {
                byte nucleotideB;
                if (nucleotideValueMap.TryGetValue(y, out nucleotideB))
                {
                    if (ambiguousSyToBasicSymbolsMap.ContainsKey(nucleotideA)
                        || ambiguousSyToBasicSymbolsMap.ContainsKey(nucleotideB))
                    {
                        return false;
                    }

                    return nucleotideA == nucleotideB;
                }
                throw new ArgumentException(Resource.InvalidParameter, nameof(y));
            }
            throw new ArgumentException(Resource.InvalidParameter, nameof(x));
        }

        /// <summary>
        ///     Find the consensus nucleotide for a set of nucleotides.
        /// </summary>
        /// <param name="symbols">Set of sequence items.</param>
        /// <returns>Consensus nucleotide.</returns>
        public virtual byte GetConsensusSymbol(HashSet<byte> symbols)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        /// <summary>
        ///     Validates if all symbols provided are DNA symbols or not.
        /// </summary>
        public bool ValidateSequence(byte[] symbols, long offset, long length)
        {
            if (symbols == null)
            {
                throw new ArgumentNullException(nameof(symbols));
            }

            if (symbols.Length < offset + length) {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            for (var i = offset; i < (length + offset); i++)
            {
                if (!nucleotideValueMap.ContainsKey(symbols[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <inheritdoc />
        public virtual bool CheckIsGap(byte item)
        {
            return item == Gap;
        }

        /// <inheritdoc />
        public virtual bool CheckIsAmbiguous(byte item)
        {
            return !basicSymbols.Contains(item);
        }

        /// <inheritdoc />
        public HashSet<byte> GetAmbiguousSymbols()
        {
            return new HashSet<byte>(ambiguousSyToBasicSymbolsMap.Keys);
        }

        /// <inheritdoc />
        public byte[] GetSymbolValueMap()
        {
            var symbolMap = new byte[256];

            foreach (var mapping in nucleotideValueMap)
            {
                symbolMap[mapping.Key] = mapping.Value;
            }

            return symbolMap;
        }

        /// <summary>
        ///     Converts the DNA alphabet to a string.
        /// </summary>
        /// <returns>The DNA alphabet as a string.</returns>
        public override string ToString()
        {
            return new string(nucleotides.Select(x => (char)x).ToArray());
        }

        /// <summary>
        ///     Byte array of nucleotides.
        /// </summary>
        /// <returns>Returns the Enumerator for nucleotides list.</returns>
        public IEnumerator<byte> GetEnumerator()
        {
            return nucleotides.GetEnumerator();
        }

        /// <summary>
        ///     Creates an IEnumerator of the nucleotides.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Add the given nucleotide symbols to this alphabet type.
        /// </summary>
        /// <param name="nucleotideValue">The nucleotide Value.</param>
        /// <param name="friendlyName">User friendly name of the symbol.</param>
        /// <param name="otherPossibleValues">The other Possible Values.</param>
        protected void AddNucleotide(byte nucleotideValue, string friendlyName, params byte[] otherPossibleValues)
        {
            // Verify whether the nucleotide value or other possible values already exist or not.
            if (nucleotideValueMap.ContainsKey(nucleotideValue)
                || otherPossibleValues.Any(x => nucleotideValueMap.Keys.Contains(x)))
            {
                throw new ArgumentException(Resource.SymbolExistsInAlphabet, nameof(nucleotideValue));
            }
            if (string.IsNullOrEmpty(friendlyName))
            {
                throw new ArgumentNullException(nameof(friendlyName));
            }

            nucleotideValueMap.Add(nucleotideValue, nucleotideValue);
            foreach (var value in otherPossibleValues)
            {
                nucleotideValueMap.Add(value, nucleotideValue);
            }

            nucleotides.Add(nucleotideValue);
            friendlyNameMap.Add(nucleotideValue, friendlyName);
        }

        /// <summary>
        ///     Maps the ambiguous nucleotide to the nucleotides it represents.
        ///     For example ambiguous nucleotide M represents the basic nucleotides A or C.
        /// </summary>
        /// <param name="ambiguousNucleotide">Ambiguous nucleotide.</param>
        /// <param name="nucleotidesToMap">Nucleotide represented by ambiguous nucleotide.</param>
        protected void MapAmbiguousNucleotide(byte ambiguousNucleotide, params byte[] nucleotidesToMap)
        {
            byte ambiguousSymbol;

            // Verify whether the nucleotides to map are valid nucleotides.
            if (!nucleotideValueMap.TryGetValue(ambiguousNucleotide, out ambiguousSymbol))
            {
                throw new ArgumentException(Resource.CouldNotRecognizeSymbol, nameof(ambiguousNucleotide));
            }

            var mappingValues = new byte[nucleotidesToMap.Length];
            var i = 0;
            byte validatedValueToMap;

            foreach (var valueToMap in nucleotidesToMap)
            {
                if (!nucleotideValueMap.TryGetValue(valueToMap, out validatedValueToMap))
                {
                    throw new ArgumentException(Resource.CouldNotRecognizeSymbol, nameof(nucleotidesToMap));
                }

                mappingValues[i++] = validatedValueToMap;
            }

            var basicSymbols = new HashSet<byte>(mappingValues);
            ambiguousSyToBasicSymbolsMap.Add(ambiguousSymbol, basicSymbols);
            basicSymbolsToAmbiguousSymbolMap.Add(basicSymbols, ambiguousSymbol);
        }

        /// <summary>
        ///     Verify whether the nucleotides exist or not.
        /// </summary>
        /// <param name="nucleotide">The Nucleotide.</param>
        /// <param name="complementNucleotide">Complement Nucleotide.</param>
        protected void MapComplementNucleotide(byte nucleotide, byte complementNucleotide)
        {
            // verify whether the nucleotides exist or not.
            byte symbol; // validated nucleotides
            if (nucleotideValueMap.TryGetValue(nucleotide, out symbol))
            {
                byte complementSymbol; // validated nucleotides
                if (nucleotideValueMap.TryGetValue(complementNucleotide, out complementSymbol))
                {
                    symbolToComplementSymbolMap.Add(symbol, complementSymbol);
                    return;
                }
            }

            throw new ArgumentException(Resource.CouldNotRecognizeSymbol, nameof(nucleotide));
        }
    }
}