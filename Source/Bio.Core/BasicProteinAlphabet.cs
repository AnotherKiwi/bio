using Bio.Util;
using static Bio.Properties.Resource;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Bio
{
    /// <inheritdoc />
    /// <seealso cref="T:Bio.IBasicProteinAlphabet" />,
    /// <seealso cref="T:Bio.IAlphabet" />

    public class BasicProteinAlphabet : IBasicProteinAlphabet
    {
        #region Private members

        /// <summary>
        ///     Symbol to three-letter amino acid abbreviation.
        /// </summary>
        private readonly Dictionary<byte, string> abbreviationMap1to3 = new Dictionary<byte, string>();

        /// <summary>
        ///     Three-letter amino acid abbreviation to symbol.
        /// </summary>
        private readonly Dictionary<string, byte> abbreviationMap3to1 = new Dictionary<string, byte>();

        /// <summary>
        ///     Mapping from ambiguous symbol to set of basic symbols they represent.
        /// </summary>
        private readonly Dictionary<byte, HashSet<byte>> ambiguousSymbolToBasicSymbolsMap
            = new Dictionary<byte, HashSet<byte>>();

        /// <summary>
        ///     Holds the amino acids present in this alphabet.
        /// </summary>
        private readonly List<byte> aminoAcids = new List<byte>();

        /// <summary>
        ///     Amino acids map. <br/>
        ///     Maps A to A and a to A etc. so that the internal dictionary key will contain unique values. <br/>
        ///     This is used in the IsValidSymbol method to address Scenarios like a == A, M == m etc.
        /// </summary>
        private readonly Dictionary<byte, byte> aminoAcidValueMap = new Dictionary<byte, byte>();

        /// <summary>
        ///     Mapping from set of symbols to corresponding ambiguous symbol.
        /// </summary>
        private readonly Dictionary<HashSet<byte>, byte> basicSymbolsToAmbiguousSymbolMap
            = new Dictionary<HashSet<byte>, byte>(new HashSetComparer<byte>());

        /// <summary>
        ///     Symbol to FriendlyName mapping.
        /// </summary>
        private readonly Dictionary<byte, string> friendlyNameMap = new Dictionary<byte, string>();

        #endregion Private members

        /// <summary>
        ///     Instance of the BasicProteinAlphabet class.
        /// </summary>
        public static readonly BasicProteinAlphabet Instance;

        /// <summary>
        ///     Set up the static BasicProteinAlphabet instance.
        /// </summary>
        static BasicProteinAlphabet()
        {
            Instance = new BasicProteinAlphabet();
        }

        /// <summary>
        ///     Initializes a new instance of the BasicProteinAlphabet class.
        /// </summary>
        protected BasicProteinAlphabet()
        {
            Name = BasicProteinAlphabetName;
            HasGaps = false;
            HasAmbiguity = false;
            HasTerminations = false;
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

            AddAminoAcid(A, "Ala", "Alanine", (byte)'a');
            AddAminoAcid(C, "Cys", "Cysteine", (byte)'c');
            AddAminoAcid(D, "Asp", "Aspartic Acid", (byte)'d');
            AddAminoAcid(E, "Glu", "Glutamic Acid", (byte)'e');
            AddAminoAcid(F, "Phe", "Phenylalanine", (byte)'f');
            AddAminoAcid(G, "Gly", "Glycine", (byte)'g');
            AddAminoAcid(H, "His", "Histidine", (byte)'h');
            AddAminoAcid(I, "Ile", "Isoleucine", (byte)'i');
            AddAminoAcid(K, "Lys", "Lysine", (byte)'k');
            AddAminoAcid(L, "Leu", "Leucine", (byte)'l');
            AddAminoAcid(M, "Met", "Methionine", (byte)'m');
            AddAminoAcid(N, "Asn", "Asparagine", (byte)'n');
            AddAminoAcid(O, "Pyl", "Pyrrolysine", (byte)'o');
            AddAminoAcid(P, "Pro", "Proline", (byte)'p');
            AddAminoAcid(Q, "Gln", "Glutamine", (byte)'q');
            AddAminoAcid(R, "Arg", "Arginine", (byte)'r');
            AddAminoAcid(S, "Ser", "Serine", (byte)'s');
            AddAminoAcid(T, "Thr", "Threoine", (byte)'t');
            AddAminoAcid(U, "Sec", "Selenocysteine", (byte)'u');
            AddAminoAcid(V, "Val", "Valine", (byte)'v');
            AddAminoAcid(W, "Trp", "Tryptophan", (byte)'w');
            AddAminoAcid(Y, "Tyr", "Tyrosine", (byte)'y');
        }

        /// <inheritdoc />
        public byte A { get; }

        /// <inheritdoc />
        public byte C { get; }

        /// <inheritdoc />
        public int Count => aminoAcids.Count;

        /// <inheritdoc />
        public byte D { get; }

        /// <inheritdoc />
        public byte E { get; }

        /// <inheritdoc />
        public byte F { get; }

        /// <inheritdoc />
        public byte G { get; }

        /// <inheritdoc />
        public byte H { get; }

        /// <inheritdoc />
        /// <remarks>
        ///     This alphabet does NOT have ambiguity symbols.
        /// </remarks>
        public bool HasAmbiguity { get; protected set; }

        /// <inheritdoc />
        /// <remarks>
        ///     This alphabet does NOT have gap symbols.
        /// </remarks>
        public bool HasGaps { get; protected set; }

        /// <inheritdoc />
        /// <remarks>
        ///     This alphabet does NOT have termination symbols.
        /// </remarks>
        public bool HasTerminations { get; protected set; }

        /// <inheritdoc />
        public byte I { get; }

        /// <inheritdoc />
        /// <remarks>
        ///     This alphabet does NOT support complements.
        /// </remarks>
        public bool IsComplementSupported { get; protected set; }

        /// <inheritdoc />
        /// <remarks>
        ///     This is NOT a DNA alphabet.
        /// </remarks>
        public bool IsDna => false;

        /// <inheritdoc />
        /// <remarks>
        /// This IS a protein alphabet.
        /// </remarks>
        public bool IsProtein => true;

        /// <inheritdoc />
        /// <remarks>
        ///     This is NOT a RNA alphabet.
        /// </remarks>
        public bool IsRna => false;

        /// <inheritdoc />
        public byte K { get; }

        /// <inheritdoc />
        public byte L { get; }

        /// <inheritdoc />
        public byte M { get; }

        /// <inheritdoc />
        public byte N { get; }

        /// <inheritdoc />
        /// <remarks>
        ///     This is always 'BasicProtein'.
        /// </remarks>
        public string Name { get; protected set; }

        /// <inheritdoc />
        public byte O { get; }

        /// <inheritdoc />
        public byte P { get; }

        /// <inheritdoc />
        public byte Q { get; }

        /// <inheritdoc />
        public byte R { get; }

        /// <inheritdoc />
        public byte S { get; }

        /// <inheritdoc />
        public byte T { get; }

        /// <inheritdoc />
        public byte U { get; }

        /// <inheritdoc />
        public byte V { get; }

        /// <inheritdoc />
        public byte W { get; }

        /// <inheritdoc />
        public byte Y { get; }

        /// <inheritdoc />
       public byte this[int index] => aminoAcids[index];

        /// <summary>
        /// Adds a Amino acid to the existing amino acids.
        /// </summary>
        /// <param name="aminoAcidValue">Amino acid to be added.</param>
        /// <param name="threeLetterAbbreviation">Three letter abbreviation for the symbol.</param>
        /// <param name="friendlyName">User friendly name of the symbol.</param>
        /// <param name="otherPossibleValues">Maps capitals and small Letters.</param>
        protected void AddAminoAcid(byte aminoAcidValue, string threeLetterAbbreviation,
            string friendlyName, params byte[] otherPossibleValues)
        {
            // Verify whether the aminoAcidValue or other possible values already exist or not.
            if (aminoAcidValueMap.ContainsKey(aminoAcidValue) ||
                otherPossibleValues.Any(x => aminoAcidValueMap.Keys.Contains(x)))
            {
                throw new ArgumentException(SymbolExistsInAlphabet, nameof(aminoAcidValue));
            }
            if (string.IsNullOrEmpty(friendlyName))
            {
                throw new ArgumentNullException(nameof(friendlyName));
            }

            aminoAcidValueMap.Add(aminoAcidValue, aminoAcidValue);
            foreach (var value in otherPossibleValues)
            {
                aminoAcidValueMap.Add(value, aminoAcidValue);
            }

            aminoAcids.Add(aminoAcidValue);
            abbreviationMap1to3.Add(aminoAcidValue, threeLetterAbbreviation);
            abbreviationMap3to1.Add(threeLetterAbbreviation, aminoAcidValue);
            friendlyNameMap.Add(aminoAcidValue, friendlyName);
        }

        /// <inheritdoc />
        /// <remarks>
        ///     This is always <c>false</c>, since this alphabet has no ambiguous symbols.
        /// </remarks>
        public virtual bool CheckIsAmbiguous(byte item)
        {
            return false;
        }

        /// <inheritdoc />
        /// <remarks>
        ///     This is always <c>false</c>, since this alphabet has no gap symbols.
        /// </remarks>
        public virtual bool CheckIsGap(byte item)
        {
            return false;
        }

        /// <inheritdoc />
        public virtual bool CompareSymbols(byte x, byte y)
        {
            if (aminoAcidValueMap.TryGetValue(x, out var aminoAcidA))
            {
                if (aminoAcidValueMap.TryGetValue(y, out var aminoAcidB))
                {
                    if (ambiguousSymbolToBasicSymbolsMap.ContainsKey(aminoAcidA) ||
                        ambiguousSymbolToBasicSymbolsMap.ContainsKey(aminoAcidB))
                    {
                        return false;
                    }

                    return aminoAcidA == aminoAcidB;
                }
                else
                {
                    throw new ArgumentException(InvalidParameter, nameof(y));
                }
            }
            else
            {
                throw new ArgumentException(InvalidParameter, nameof(x));
            }
        }

        /// <inheritdoc />
        /// <remarks>
        ///     This alphabet does NOT support ambiguity, so a
        ///     <see cref="NotSupportedException"/> is thrown.
        /// </remarks>
        public HashSet<byte> GetAmbiguousSymbols()
        {
            return ambiguousSymbolToBasicSymbolsMap.Any()
                ? new HashSet<byte>(ambiguousSymbolToBasicSymbolsMap.Keys) 
                : throw new NotSupportedException();
        }

        /// <inheritdoc />
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
            return aminoAcids.GetEnumerator();
        }

        /// <inheritdoc />
        public string GetFriendlyName(byte item)
        {
            friendlyNameMap.TryGetValue(aminoAcidValueMap[item], out var fName);
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
                if (!aminoAcidValueMap.ContainsKey(sequence[i]))
                {
                    invalidSymbols.Add(sequence[i]);
                }
            }

            return invalidSymbols.ToArray();
        }

        /// <inheritdoc />
        public byte GetSymbolFromThreeLetterAbbrev(string item)
        {
            abbreviationMap3to1.TryGetValue(item, out var symbol);
            return symbol;
        }

        /// <inheritdoc />
        public byte[] GetSymbolValueMap()
        {
            var symbolMap = new byte[256];

            foreach (var mapping in aminoAcidValueMap)
            {
                symbolMap[mapping.Key] = mapping.Value;
            }

            return symbolMap;
        }
        
        /// <inheritdoc />
        public string GetThreeLetterAbbreviation(byte item)
        {
            abbreviationMap1to3.TryGetValue(aminoAcidValueMap[item], out var threeLetterAbbreviation);
            return threeLetterAbbreviation;
        }

        /// <inheritdoc />
        public HashSet<byte> GetValidSymbols()
        {
            return new HashSet<byte>(aminoAcidValueMap.Keys);
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
            if (!aminoAcidValueMap.TryGetValue(ambiguousAminoAcid, out var ambiguousSymbol) ||
                !aminoAcidsToMap.All(x => aminoAcidValueMap.Keys.Contains(x)))
            {
                throw new ArgumentException(CouldNotRecognizeSymbol, nameof(ambiguousAminoAcid));
            }

            var mappingValues = new byte[aminoAcidsToMap.Length];
            var i = 0;
            foreach (var valueToMap in aminoAcidsToMap)
            {
                mappingValues[i++] = aminoAcidValueMap[valueToMap];
            }

            // ReSharper disable once LocalVariableHidesMember
            var basicSymbols = new HashSet<byte>(mappingValues);
            ambiguousSymbolToBasicSymbolsMap.Add(ambiguousSymbol, basicSymbols);
            basicSymbolsToAmbiguousSymbolMap.Add(basicSymbols, ambiguousSymbol);
        }

        /// <summary>
        ///     Converts the alphabet to a string.
        /// </summary>
        /// <returns>The alphabet as a string.</returns>
        public override string ToString()
        {
            return new string(aminoAcids.Select(x => (char)x).ToArray());
        }

        /// <inheritdoc />
        /// <remarks>
        ///     This alphabet does NOT support ambiguity, so the method always returns <c>false</c>.
        /// </remarks>
        public bool TryGetAmbiguousSymbol(HashSet<byte> symbols, out byte ambiguousSymbol)
        {
            ambiguousSymbol = 0;
            return basicSymbolsToAmbiguousSymbolMap.Any() && 
                   basicSymbolsToAmbiguousSymbolMap.TryGetValue(symbols, out ambiguousSymbol);
        }

        /// <inheritdoc />
        /// <remarks>
        ///     This alphabet does NOT support ambiguity, so the method always returns <c>false</c>.
        /// </remarks>
        public bool TryGetBasicSymbols(byte ambiguousSymbol, out HashSet<byte> basicSymbols)
        {
            basicSymbols = null;
            return ambiguousSymbolToBasicSymbolsMap.Any() && 
                   ambiguousSymbolToBasicSymbolsMap.TryGetValue(ambiguousSymbol, out basicSymbols);
        }

        /// <inheritdoc />
        /// <remarks>
        ///     This alphabet does NOT support complements, so the method always returns <c>false</c>.
        /// </remarks>
        public bool TryGetComplementSymbol(byte symbol, out byte complementSymbol)
        {
            // Complement is not possible.
            complementSymbol = default(byte);
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

        /// <inheritdoc />
        /// <remarks>
        ///     This alphabet does NOT include gap symbols, so the method always returns <c>false</c>.
        /// </remarks>
        public virtual bool TryGetDefaultGapSymbol(out byte defaultGapSymbol)
        {
            defaultGapSymbol = default(byte);
            return false;
        }

        /// <inheritdoc />
        /// <remarks>
        ///     This alphabet does NOT include termination symbols, so the method always returns <c>false</c>.
        /// </remarks>
        public virtual bool TryGetDefaultTerminationSymbol(out byte defaultTerminationSymbol)
        {
            defaultTerminationSymbol = default(byte);
            return false;
        }

        /// <inheritdoc />
        /// <remarks>
        ///     This alphabet does NOT include gap symbols, so the method always returns <c>false</c>.
        /// </remarks>
        public virtual bool TryGetGapSymbols(out HashSet<byte> gapSymbols)
        {
            gapSymbols = null;
            return false;
        }

        /// <inheritdoc />
        /// <remarks>
        ///     This alphabet does NOT include termination symbols, so the method always returns <c>false</c>.
        /// </remarks>
        public virtual bool TryGetTerminationSymbols(out HashSet<byte> terminationSymbols)
        {
            terminationSymbols = null;
            return false;
        }

        // offset and length arguments made optional by Stephen Haines.
        // TODO: Write a test.
        /// <inheritdoc />
        public bool ValidateSequence(byte[] symbols, long offset = 0, long length = -1)
        {
            if (symbols == null)
            {
                throw new ArgumentNullException(nameof(symbols));
            }

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), @"> 0");
            }

            if (length == -1)
            {
                length = symbols.Length;
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), @"> 0");
            }

            if (offset + length > symbols.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(length), LengthPlusOffsetCannotExceedSeqLength);
            }

            for (var i = offset; i < length; i++)
            {
                if (!aminoAcidValueMap.ContainsKey(symbols[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
