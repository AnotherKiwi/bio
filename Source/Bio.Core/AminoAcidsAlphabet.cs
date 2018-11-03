using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static System.Array;

using Bio.Util;
using static Bio.Alphabets;
using static Bio.Properties.Resource;

namespace Bio
{
    /// <inheritdoc cref="IAminoAcidsAlphabet" />
    /// <seealso cref="IAminoAcidsAlphabet" />,
    /// <seealso cref="IAlphabet" />

    public class AminoAcidsAlphabet : IAminoAcidsAlphabet, IStandardAminoAcidsAlphabet
    {
        #region Private members

        /// <summary>
        ///     Symbol to three-letter amino acid abbreviation.
        /// </summary>
        private readonly Dictionary<byte, string> _abbreviationMap1To3 = new Dictionary<byte, string>();

        /// <summary>
        ///     Three-letter amino acid abbreviation to symbol.
        /// </summary>
        private readonly Dictionary<string, byte> _abbreviationMap3To1 = new Dictionary<string, byte>();

        /// <summary>
        ///     Mapping from ambiguous symbol to set of basic symbols they represent.
        /// </summary>
        private readonly Dictionary<byte, HashSet<byte>> _ambiguousSymbolToBasicSymbolsMap
            = new Dictionary<byte, HashSet<byte>>();

        /// <summary>
        ///     Holds the amino acids present in this alphabet.
        /// </summary>
        private readonly List<byte> _aminoAcids = new List<byte>();

        /// <summary>
        ///     Amino acids map. <br/>
        ///     Maps A to A and a to A etc. so that the internal dictionary key will contain unique values. <br/>
        ///     This is used in the IsValidSymbol method to address Scenarios like a == A, M == m etc.
        /// </summary>
        private readonly Dictionary<byte, byte> _aminoAcidValueMap = new Dictionary<byte, byte>();

        /// <summary>
        ///     Mapping from set of symbols to corresponding ambiguous symbol.
        /// </summary>
        private readonly Dictionary<HashSet<byte>, byte> _basicSymbolsToAmbiguousSymbolMap
            = new Dictionary<HashSet<byte>, byte>(new HashSetComparer<byte>());

        /// <summary>
        ///     Symbol to FriendlyName mapping.
        /// </summary>
        private readonly Dictionary<byte, string> _friendlyNameMap = new Dictionary<byte, string>();

        /// <summary>
        ///     Holds the unambiguous amino acids present in this alphabet.
        /// </summary>
        private readonly HashSet<byte> _unambiguousAminoAcids = new HashSet<byte>();

        #endregion Private members

        /// <summary>
        ///     Instance of the StrictProteinAlphabet class.
        /// </summary>
        public static readonly AminoAcidsAlphabet Instance;

        /// <summary>
        ///     Set up the static StrictProteinAlphabet instance.
        /// </summary>
        static AminoAcidsAlphabet()
        {
            Instance = new AminoAcidsAlphabet();
        }

        /// <summary>
        ///     Initializes a new instance of the StrictProteinAlphabet class.
        /// </summary>
        protected AminoAcidsAlphabet()
        {
            Name = AminoAcidsAlphabetName;
            AlphabetType = AlphabetTypes.AminoAcids;
            HasGaps = false;
            HasAmbiguity = false;
            HasTerminations = false;
            IsCaseSensitive = true;
            IsComplementSupported = false;

            A = (byte)'A';
            C = (byte)'C';
            D = (byte)'D';
            E = (byte)'E';
            F = (byte)'F';
            G = (byte)'G';
            H = (byte)'H';
            I = (byte)'I';
            K = (byte)'K';
            L = (byte)'L';
            M = (byte)'M';
            N = (byte)'N';
            O = (byte)'O';
            P = (byte)'P';
            Q = (byte)'Q';
            R = (byte)'R';
            S = (byte)'S';
            T = (byte)'T';
            U = (byte)'U';
            V = (byte)'V';
            W = (byte)'W';
            Y = (byte)'Y';

            AddAminoAcids();
        }

        /// <inheritdoc cref="IStandardAminoAcidsAlphabet" />
        public byte A { get; }

        /// <inheritdoc />
        public AlphabetTypes AlphabetType { get; protected set; }

        /// <inheritdoc cref="IStandardAminoAcidsAlphabet" />
        public byte C { get; }

        /// <inheritdoc cref="IStandardAminoAcidsAlphabet" />
        public int Count => _aminoAcids.Count;

        /// <inheritdoc cref="IStandardAminoAcidsAlphabet" />
        public byte D { get; }

        /// <inheritdoc cref="IStandardAminoAcidsAlphabet" />
        public byte E { get; }

        /// <inheritdoc cref="IStandardAminoAcidsAlphabet" />
        public byte F { get; }

        /// <inheritdoc cref="IStandardAminoAcidsAlphabet" />
        public byte G { get; }

        /// <inheritdoc cref="IStandardAminoAcidsAlphabet" />
        public byte H { get; }

        /// <inheritdoc cref="IAlphabet.HasAmbiguity" />
        /// <remarks>
        ///     This alphabet does NOT have ambiguity symbols.
        /// </remarks>
        public bool HasAmbiguity { get; protected set; }

        /// <inheritdoc cref="IAlphabet.HasGaps" />
        /// <remarks>
        ///     This alphabet does NOT have gap symbols.
        /// </remarks>
        public bool HasGaps { get; protected set; }

        /// <inheritdoc cref="IAlphabet.HasTerminations" />
        /// <remarks>
        ///     This alphabet does NOT have termination symbols.
        /// </remarks>
        public bool HasTerminations { get; protected set; }

        /// <inheritdoc cref="IStandardAminoAcidsAlphabet" />
        public byte I { get; }

        /// <inheritdoc />
        /// <remarks>
        ///     This alphabet IS case sensitive.
        /// </remarks>
        public bool IsCaseSensitive { get; protected set; }

        /// <inheritdoc />
        /// <remarks>
        ///     This alphabet does NOT support complements.
        /// </remarks>
        public bool IsComplementSupported { get; protected set; }

        /// <inheritdoc cref="IStandardAminoAcidsAlphabet" />
        public byte K { get; }

        /// <inheritdoc cref="IStandardAminoAcidsAlphabet" />
        public byte L { get; }

        /// <inheritdoc cref="IStandardAminoAcidsAlphabet" />
        public byte M { get; }

        /// <inheritdoc cref="IStandardAminoAcidsAlphabet" />
        public byte N { get; }

        /// <inheritdoc cref="IAlphabet.Name" />
        /// <remarks>
        ///     This is always 'StrictProtein'.
        /// </remarks>
        public string Name { get; protected set; }

        /// <inheritdoc cref="IStandardAminoAcidsAlphabet" />
        public byte O { get; }

        /// <inheritdoc cref="IStandardAminoAcidsAlphabet" />
        public byte P { get; }

        /// <inheritdoc cref="IStandardAminoAcidsAlphabet" />
        public byte Q { get; }

        /// <inheritdoc cref="IStandardAminoAcidsAlphabet" />
        public byte R { get; }

        /// <inheritdoc cref="IStandardAminoAcidsAlphabet" />
        public byte S { get; }

        /// <inheritdoc cref="IStandardAminoAcidsAlphabet" />
        public byte T { get; }

        /// <inheritdoc cref="IStandardAminoAcidsAlphabet" />
        public byte U { get; }

        /// <inheritdoc cref="IStandardAminoAcidsAlphabet" />
        public byte V { get; }

        /// <inheritdoc cref="IStandardAminoAcidsAlphabet" />
        public byte W { get; }

        /// <inheritdoc cref="IStandardAminoAcidsAlphabet" />
        public byte Y { get; }

        /// <inheritdoc cref="IAlphabet.this" />
        public byte this[int index] => _aminoAcids[index];

        /// <summary>
        ///     Adds a Amino acid to the existing amino acids.
        /// </summary>
        /// <param name="aminoAcidValue">Amino acid to be added.</param>
        /// <param name="threeLetterAbbreviation">Three letter abbreviation for the symbol.</param>
        /// <param name="friendlyName">User friendly name of the symbol.</param>
        /// <param name="isUnambiguousAminoAcid">If set to <c>true</c>, the symbol is an unambiguous amino acid.</param>
        /// <param name="otherPossibleValues">Maps capitals and lower case letters.</param>
        protected void AddAminoAcid(byte aminoAcidValue, string threeLetterAbbreviation,
            string friendlyName, bool isUnambiguousAminoAcid, params byte[] otherPossibleValues)
        {
            // Verify whether the aminoAcidValue or other possible values already exist or not.
            if (_aminoAcidValueMap.ContainsKey(aminoAcidValue) ||
                otherPossibleValues.Any(x => _aminoAcidValueMap.Keys.Contains(x)))
            {
                throw new ArgumentException(SymbolExistsInAlphabet, nameof(aminoAcidValue));
            }

            if (string.IsNullOrEmpty(friendlyName))
            {
                throw new ArgumentNullException(nameof(friendlyName));
            }

            _aminoAcidValueMap.Add(aminoAcidValue, aminoAcidValue);
            foreach (byte value in otherPossibleValues)
            {
                _aminoAcidValueMap.Add(value, aminoAcidValue);
            }

            _aminoAcids.Add(aminoAcidValue);
            _abbreviationMap1To3.Add(aminoAcidValue, threeLetterAbbreviation);
            _abbreviationMap3To1.Add(threeLetterAbbreviation, aminoAcidValue);
            _friendlyNameMap.Add(aminoAcidValue, friendlyName);

            if (isUnambiguousAminoAcid)
            {
                _unambiguousAminoAcids.Add(aminoAcidValue);
            }
        }

        /// <summary>
        /// 	Adds the amino acids to the alphabet.
        /// </summary>
        private void AddAminoAcids()
        {
            // Add just the upper case symbols for the amino acids.
            AddAminoAcid(A, "Ala", "Alanine", true);
            AddAminoAcid(C, "Cys", "Cysteine", true);
            AddAminoAcid(D, "Asp", "Aspartic Acid", true);
            AddAminoAcid(E, "Glu", "Glutamic Acid", true);
            AddAminoAcid(F, "Phe", "Phenylalanine", true);
            AddAminoAcid(G, "Gly", "Glycine", true);
            AddAminoAcid(H, "His", "Histidine", true);
            AddAminoAcid(I, "Ile", "Isoleucine", true);
            AddAminoAcid(K, "Lys", "Lysine", true);
            AddAminoAcid(L, "Leu", "Leucine", true);
            AddAminoAcid(M, "Met", "Methionine", true);
            AddAminoAcid(N, "Asn", "Asparagine", true);
            AddAminoAcid(O, "Pyl", "Pyrrolysine", true);
            AddAminoAcid(P, "Pro", "Proline", true);
            AddAminoAcid(Q, "Gln", "Glutamine", true);
            AddAminoAcid(R, "Arg", "Arginine", true);
            AddAminoAcid(S, "Ser", "Serine", true);
            AddAminoAcid(T, "Thr", "Threonine", true);
            AddAminoAcid(U, "Sec", "Selenocysteine", true);
            AddAminoAcid(V, "Val", "Valine", true);
            AddAminoAcid(W, "Trp", "Tryptophan", true);
            AddAminoAcid(Y, "Tyr", "Tyrosine", true);
        }

        /// <inheritdoc cref="IAlphabet.CheckIsAmbiguous" />
        /// <remarks>
        ///     This is always <c>false</c>, since this alphabet has no ambiguous symbols.
        /// </remarks>
        public virtual bool CheckIsAmbiguous(byte item)
        {
            return false;
        }

        /// <inheritdoc cref="IAlphabet.CheckIsGap" />
        /// <remarks>
        ///     This is always <c>false</c>, since this alphabet has no gap symbols.
        /// </remarks>
        public virtual bool CheckIsGap(byte item)
        {
            return false;
        }

        /// <inheritdoc />
        public bool CheckIsUnambiguousAminoAcid(byte symbol)
        {
            return _unambiguousAminoAcids.Contains(symbol);
        }

        /// <inheritdoc cref="IAlphabet.CompareSymbols" />
        public virtual bool CompareSymbols(byte x, byte y)
        {
            if (_aminoAcidValueMap.TryGetValue(x, out byte aminoAcidA))
            {
                if (_aminoAcidValueMap.TryGetValue(y, out byte aminoAcidB))
                {
                    if (_ambiguousSymbolToBasicSymbolsMap.ContainsKey(aminoAcidA) ||
                        _ambiguousSymbolToBasicSymbolsMap.ContainsKey(aminoAcidB))
                    {
                        return false;
                    }

                    return aminoAcidA == aminoAcidB;
                }

                throw new ArgumentException(InvalidParameter, nameof(y));
            }

            throw new ArgumentException(InvalidParameter, nameof(x));
        }

        /// <inheritdoc cref="IAlphabet.GetAmbiguousSymbols" />
        /// <remarks>
        ///     This alphabet does NOT support ambiguity, so a
        ///     <see cref="NotSupportedException"/> is thrown.
        /// </remarks>
        public HashSet<byte> GetAmbiguousSymbols()
        {
            return _ambiguousSymbolToBasicSymbolsMap.Any()
                ? new HashSet<byte>(_ambiguousSymbolToBasicSymbolsMap.Keys) 
                : throw new NotSupportedException();
        }

        /// <inheritdoc cref="IAlphabet.GetConsensusSymbol" />
        /// <remarks>
        ///     Consensus symbols are not supported by unambiguous protein alphabets, so a
        ///     <see cref="NotSupportedException"/> is thrown.
        /// </remarks>
        public virtual byte GetConsensusSymbol(HashSet<byte> symbols)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Byte array of amino acids.
        /// </summary>
        /// <returns>Returns the Enumerator for amino acids list.</returns>
        public IEnumerator<byte> GetEnumerator()
        {
            return _aminoAcids.GetEnumerator();
        }

        /// <inheritdoc cref="IAlphabet.GetFriendlyName" />
        public string GetFriendlyName(byte item)
        {
            _friendlyNameMap.TryGetValue(_aminoAcidValueMap[item], out string fName);
            return fName;
        }

        /// <inheritdoc />
        public byte[] GetInvalidSymbols(ISequence sequence)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(nameof(sequence));
            }

            var invalidSymbols = new SortedSet<byte>();
            for (long i = 0; i < sequence.Count; i++)
            {
                if (!_aminoAcidValueMap.ContainsKey(sequence[i]))
                {
                    invalidSymbols.Add(sequence[i]);
                }
            }

            return invalidSymbols.ToArray();
        }

        /// <summary>
        /// 	Gets the number of symbols in the byte array.
        /// </summary>
        /// <param name="symbols">The byte array of symbols.</param>
        /// <returns>
        ///  	<see cref="long"/>: the number of symbols in the byte array.
        /// </returns>
        /// <remarks>
        ///     Ignores any zero bytes that occur after the symbols.
        /// </remarks>
        protected long GetSymbolCount(byte[] symbols)
        {
            long symbolCount = FindIndex(symbols, s => s == 0);
            if (symbolCount < 0)
            {
                symbolCount = symbols.LongLength;
            }

            return symbolCount;
        }

        /// <inheritdoc />
        public byte GetSymbolFromThreeLetterAbbrev(string item)
        {
            _abbreviationMap3To1.TryGetValue(item, out byte symbol);
            return symbol;
        }

        /// <inheritdoc cref="IAlphabet.GetSymbolValueMap" />
        public byte[] GetSymbolValueMap()
        {
            var symbolMap = new byte[256];

            foreach (KeyValuePair<byte, byte> mapping in _aminoAcidValueMap)
            {
                symbolMap[mapping.Key] = mapping.Value;
            }

            return symbolMap;
        }

        
        /// <inheritdoc />
        public string GetThreeLetterAbbreviation(byte item)
        {
            _abbreviationMap1To3.TryGetValue(_aminoAcidValueMap[item], out string threeLetterAbbreviation);
            return threeLetterAbbreviation ?? string.Empty;
        }

        /// <summary>
        /// 	Gets a copy of the set of unambiguous amino acids.
        /// </summary>
        /// <returns>
        ///  	<see cref="HashSet{T}"/>: the set of unambiguous amino acids.
        /// </returns>
        public HashSet<byte> GetUnambiguousAminoAcids()
        {
            return new HashSet<byte>(_unambiguousAminoAcids);
        }

        /// <inheritdoc cref="IAlphabet.GetValidSymbols" />
        public HashSet<byte> GetValidSymbols()
        {
            return new HashSet<byte>(_aminoAcidValueMap.Keys);
        }

        /// <summary>
        /// Creates an IEnumerator of the amino acids.
        /// </summary>
        /// <returns>Returns Enumerator over alphabet values.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Maps the specified ambiguous amino acid to the amino acids it represents. 
        ///     For example ambiguous amino acid B represents the Glu or Gln.
        /// </summary>
        /// <param name="ambiguousAminoAcid">Ambiguous amino acid.</param>
        /// <param name="aminoAcidsToMap">Amino acids represented by the ambiguous amino acid.</param>
        protected void MapAmbiguousAminoAcid(byte ambiguousAminoAcid, params byte[] aminoAcidsToMap)
        {
            // Verify whether the amino acids to map are valid amino acids.
            if (!_aminoAcidValueMap.TryGetValue(ambiguousAminoAcid, out byte ambiguousSymbol) ||
                !aminoAcidsToMap.All(x => _aminoAcidValueMap.Keys.Contains(x)))
            {
                throw new ArgumentException(CouldNotRecognizeSymbol, nameof(ambiguousAminoAcid));
            }

            var mappingValues = new byte[aminoAcidsToMap.Length];
            int i = 0;
            foreach (byte valueToMap in aminoAcidsToMap)
            {
                mappingValues[i++] = _aminoAcidValueMap[valueToMap];
            }

            var basicSymbols = new HashSet<byte>(mappingValues);
            _ambiguousSymbolToBasicSymbolsMap.Add(ambiguousSymbol, basicSymbols);
            _basicSymbolsToAmbiguousSymbolMap.Add(basicSymbols, ambiguousSymbol);
        }

        /// <summary>
        /// 	Resets the lists that hold amino acid symbols.
        /// </summary>
        /// <remarks>
        ///     Needed to enable the ProteinAlphabet constructor to redefine the amino
        ///     acids, and thus keep all the symbols in the lists in their original order
        ///     (e.g. 'AaCcDd...WwYy-*' for aminoAcids). 
        /// </remarks>
        protected void ResetAminoAcids()
        {
            _aminoAcids.Clear();
            _aminoAcidValueMap.Clear();
            _abbreviationMap1To3.Clear();
            _abbreviationMap3To1.Clear();
            _friendlyNameMap.Clear();
            _unambiguousAminoAcids.Clear();
        }

        /// <summary>
        ///     Converts the alphabet to a string.
        /// </summary>
        /// <returns>The alphabet as a string.</returns>
        public override string ToString()
        {
            return new string(_aminoAcids.Select(x => (char)x).ToArray());
        }

        /// <inheritdoc cref="IAlphabet.TryGetAmbiguousSymbol" />
        /// <remarks>
        ///     This alphabet does NOT support ambiguity, so the method always returns <c>false</c>.
        /// </remarks>
        public bool TryGetAmbiguousSymbol(HashSet<byte> symbols, out byte ambiguousSymbol)
        {
            ambiguousSymbol = 0;
            return _basicSymbolsToAmbiguousSymbolMap.Any() && 
                   _basicSymbolsToAmbiguousSymbolMap.TryGetValue(symbols, out ambiguousSymbol);
        }

        /// <inheritdoc cref="IAlphabet.TryGetBasicSymbols" />
        /// <remarks>
        ///     This alphabet does NOT support ambiguity, so the method always returns <c>false</c>.
        /// </remarks>
        public bool TryGetBasicSymbols(byte ambiguousSymbol, out HashSet<byte> basicSymbols)
        {
            basicSymbols = null;
            return _ambiguousSymbolToBasicSymbolsMap.Any() && 
                   _ambiguousSymbolToBasicSymbolsMap.TryGetValue(ambiguousSymbol, out basicSymbols);
        }

        /// <inheritdoc />
        /// <remarks>
        ///     This alphabet does NOT support complements, so the method always returns <c>false</c>.
        /// </remarks>
        public bool TryGetComplementSymbol(byte symbol, out byte complementSymbol)
        {
            // Complement is not possible.
            complementSymbol = default;
            return false;
        }

        /// <inheritdoc />
        /// <remarks>
        ///     This alphabet does NOT support complements, so the method always returns <c>false</c>.
        /// </remarks>
        public bool TryGetComplementSymbol(byte[] symbols, out byte[] complementSymbols)
        {
            complementSymbols = null;
            return false;
        }

        /// <inheritdoc cref="IAlphabet.TryGetDefaultGapSymbol" />
        /// <remarks>
        ///     This alphabet does NOT include gap symbols, so the method always returns <c>false</c>.
        /// </remarks>
        public virtual bool TryGetDefaultGapSymbol(out byte defaultGapSymbol)
        {
            defaultGapSymbol = default;
            return false;
        }

        /// <inheritdoc cref="IAlphabet.TryGetDefaultTerminationSymbol" />
        /// <remarks>
        ///     This alphabet does NOT include termination symbols, so the method always returns <c>false</c>.
        /// </remarks>
        public virtual bool TryGetDefaultTerminationSymbol(out byte defaultTerminationSymbol)
        {
            defaultTerminationSymbol = default;
            return false;
        }

        /// <inheritdoc cref="IAlphabet.TryGetGapSymbols" />
        /// <remarks>
        ///     This alphabet does NOT include gap symbols, so the method always returns <c>false</c>.
        /// </remarks>
        public virtual bool TryGetGapSymbols(out HashSet<byte> gapSymbols)
        {
            gapSymbols = null;
            return false;
        }

        /// <inheritdoc cref="IAlphabet.TryGetTerminationSymbols" />
        /// <remarks>
        ///     This alphabet does NOT include termination symbols, so the method always returns <c>false</c>.
        /// </remarks>
        public virtual bool TryGetTerminationSymbols(out HashSet<byte> terminationSymbols)
        {
            terminationSymbols = null;
            return false;
        }

        /// <inheritdoc />
        public virtual bool ValidateSequence(byte[] symbols)
        {
            return ValidateSequence(symbols, 0, GetSymbolCount(symbols));
        }

        /// <inheritdoc cref="IAlphabet.ValidateSequence" />
        public virtual bool ValidateSequence(byte[] symbols, long offset, long length)
        {
            if (symbols == null)
            {
                throw new ArgumentNullException(nameof(symbols));
            }

            // An empty array of symbols is OK, as long as offset and length are both 0.
            if ((symbols.LongLength == 0) && (offset == 0) && (length == 0))
            {
                return true;
            }

            if ((offset < 0) || (offset >= symbols.LongLength))
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            if ((length < 0) || (symbols.LongLength < offset + length))
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            for (long i = offset; i < length; i++)
            {
                if (!_aminoAcidValueMap.ContainsKey(symbols[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
