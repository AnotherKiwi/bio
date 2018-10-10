using System;
using System.Collections;
using static Bio.Properties.Resource;
using System.Collections.Generic;
using System.Linq;
using Bio.Util;

namespace Bio
{
    /// <inheritdoc cref="IProteinAlphabet"/>

    public class ProteinAlphabet : StrictProteinAlphabet, IProteinAlphabet
    {
/*
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
        ///     Holds the unambiguous amino acids present in this alphabet.
        /// </summary>
        private readonly HashSet<byte> unambiguousAminoAcids = new HashSet<byte>();

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
*/

        /// <summary>
        ///     Instance of the ProteinAlphabet class.
        /// </summary>
        public new static readonly ProteinAlphabet Instance;

        /// <summary>
        ///     Set up the static instance.
        /// </summary>
        static ProteinAlphabet()
        {
            Instance = new ProteinAlphabet();
        }

        /// <summary>
        ///     Initializes a new instance of the ProteinAlphabet class.
        /// </summary>
        protected ProteinAlphabet()
        {
            Name = ProteinAlphabetName;
            HasGaps = true;
            HasAmbiguity = false;
            HasTerminations = true;
            IsCaseSensitive = false;
            IsComplementSupported = false;
/*
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
*/
            Gap = (byte)'-';
            Ter = (byte)'*';

            AddAminoAcids();
        }

        protected  void AddAminoAcids()
        {
            // Need to clear the collections of symbols defined in the base class.
            ResetAminoAcids();

            AddAminoAcid(A, "Ala", "Alanine", true, (byte)'a');
            AddAminoAcid(C, "Cys", "Cysteine", true, (byte)'c');
            AddAminoAcid(D, "Asp", "Aspartic Acid", true, (byte)'d');
            AddAminoAcid(E, "Glu", "Glutamic Acid", true, (byte)'e');
            AddAminoAcid(F, "Phe", "Phenylalanine", true, (byte)'f');
            AddAminoAcid(G, "Gly", "Glycine", true, (byte)'g');
            AddAminoAcid(H, "His", "Histidine", true, (byte)'h');
            AddAminoAcid(I, "Ile", "Isoleucine", true, (byte)'i');
            AddAminoAcid(K, "Lys", "Lysine", true, (byte)'k');
            AddAminoAcid(L, "Leu", "Leucine", true, (byte)'l');
            AddAminoAcid(M, "Met", "Methionine", true, (byte)'m');
            AddAminoAcid(N, "Asn", "Asparagine", true, (byte)'n');
            AddAminoAcid(O, "Pyl", "Pyrrolysine", true, (byte)'o');
            AddAminoAcid(P, "Pro", "Proline", true, (byte)'p');
            AddAminoAcid(Q, "Gln", "Glutamine", true, (byte)'q');
            AddAminoAcid(R, "Arg", "Arginine", true, (byte)'r');
            AddAminoAcid(S, "Ser", "Serine", true, (byte)'s');
            AddAminoAcid(T, "Thr", "Threonine", true, (byte)'t');
            AddAminoAcid(U, "Sec", "Selenocysteine", true, (byte)'u');
            AddAminoAcid(V, "Val", "Valine", true, (byte)'v');
            AddAminoAcid(W, "Trp", "Tryptophan", true, (byte)'w');
            AddAminoAcid(Y, "Tyr", "Tyrosine", true, (byte)'y');

            AddAminoAcid(Gap, "---", "Gap", false);
            AddAminoAcid(Ter, "***", "Termination", false);
        }

        /// <inheritdoc />
        public byte Gap { get; }

        /// <inheritdoc />
        public byte Ter { get; }

        /// <inheritdoc />
        /// <remarks></remarks>
        public override bool CheckIsGap(byte item)
        {
            return item == Gap;
        }

        /// <inheritdoc />
        /// <remarks>
        ///     This alphabet DOES include a default gap symbol, so the method always returns <c>true</c>.
        /// </remarks>
        public override bool TryGetDefaultGapSymbol(out byte defaultGapSymbol)
        {
            defaultGapSymbol = Gap;
            return true;
        }

        /// <inheritdoc />
        /// <remarks>
        ///     This alphabet DOES include a default termination symbol, so the method always returns <c>true</c>.
        /// </remarks>
        public override bool TryGetDefaultTerminationSymbol(out byte defaultTerminationSymbol)
        {
            defaultTerminationSymbol = Ter;
            return true;
        }

        /// <inheritdoc />
        /// <remarks>
        ///     This alphabet DOES include a gap symbol, so the method always returns <c>true</c>.
        /// </remarks>
        public override bool TryGetGapSymbols(out HashSet<byte> gapSymbols)
        {
            gapSymbols = new HashSet<byte> {Gap};
            return true;
        }

        /// <inheritdoc />
        /// <remarks>
        ///     This alphabet DOES include a termination symbol, so the method always returns <c>true</c>.
        /// </remarks>
        public override bool TryGetTerminationSymbols(out HashSet<byte> terminationSymbols)
        {
            terminationSymbols = new HashSet<byte> {Ter};
            return true;
        }

/*
        /// <inheritdoc />
        public byte A { get; }

        /// <inheritdoc />
        public byte C { get; }

        /// <inheritdoc cref="IAlphabet.Count" />
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

        /// <inheritdoc />
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

        /// <inheritdoc cref="IAlphabet.Name" />
        /// <remarks>
        ///     This is always 'StrictProtein'.
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

        /// <inheritdoc cref="IAlphabet.this" />
        public byte this[int index] => aminoAcids[index];

        /// <summary>
        ///     Adds a Amino acid to the existing amino acids.
        /// </summary>
        /// <param name="aminoAcidValue">Amino acid to be added.</param>
        /// <param name="threeLetterAbbreviation">Three letter abbreviation for the symbol.</param>
        /// <param name="friendlyName">User friendly name of the symbol.</param>
        /// <param name="otherPossibleValues">Maps capitals and lower case letters.</param>
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
            foreach (byte value in otherPossibleValues)
            {
                aminoAcidValueMap.Add(value, aminoAcidValue);
            }

            aminoAcids.Add(aminoAcidValue);
            abbreviationMap1to3.Add(aminoAcidValue, threeLetterAbbreviation);
            abbreviationMap3to1.Add(threeLetterAbbreviation, aminoAcidValue);
            friendlyNameMap.Add(aminoAcidValue, friendlyName);
        }

        /// <inheritdoc cref="IAlphabet.CheckIsAmbiguous" />
        /// <remarks>
        ///     This is always <c>false</c>, since this alphabet has no ambiguous symbols.
        /// </remarks>
        public virtual bool CheckIsAmbiguous(byte item)
        {
            return false;
        }

        /// <inheritdoc />
        public bool CheckIsUnambiguousAminoAcid(byte symbol)
        {
            return unambiguousAminoAcids.Contains(symbol);
        }

        /// <inheritdoc cref="IAlphabet.CompareSymbols" />
        public virtual bool CompareSymbols(byte x, byte y)
        {
            if (aminoAcidValueMap.TryGetValue(x, out byte aminoAcidA))
            {
                if (aminoAcidValueMap.TryGetValue(y, out byte aminoAcidB))
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

        /// <inheritdoc cref="IAlphabet.GetAmbiguousSymbols" />
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
            return aminoAcids.GetEnumerator();
        }

        /// <inheritdoc cref="IAlphabet.GetFriendlyName" />
        public string GetFriendlyName(byte item)
        {
            friendlyNameMap.TryGetValue(aminoAcidValueMap[item], out string fName);
            return fName;
        }

        /// <inheritdoc />
        public byte[] GetInvalidSymbols(ISequence sequence)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(nameof(sequence));
            }

            SortedSet<byte> invalidSymbols = new SortedSet<byte>();
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
            abbreviationMap3to1.TryGetValue(item, out byte symbol);
            return symbol;
        }

        /// <inheritdoc cref="IAlphabet.GetSymbolValueMap" />
        public byte[] GetSymbolValueMap()
        {
            byte[] symbolMap = new byte[256];

            foreach (KeyValuePair<byte, byte> mapping in aminoAcidValueMap)
            {
                symbolMap[mapping.Key] = mapping.Value;
            }

            return symbolMap;
        }

        /// <inheritdoc />
        public string GetThreeLetterAbbreviation(byte item)
        {
            abbreviationMap1to3.TryGetValue(aminoAcidValueMap[item], out string threeLetterAbbreviation);
            return threeLetterAbbreviation;
        }

        /// <summary>
        /// 	Gets a copy of the set of unambiguous amino acids.
        /// </summary>
        /// <returns>
        ///  	<see cref="HashSet{T}"/>: the set of unambiguous amino acids.
        /// </returns>
        public HashSet<byte> GetUnambiguousAminoAcids()
        {
            return new HashSet<byte>(unambiguousAminoAcids);
        }

        /// <inheritdoc cref="IAlphabet.GetValidSymbols" />
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
            if (!aminoAcidValueMap.TryGetValue(ambiguousAminoAcid, out byte ambiguousSymbol) ||
                !aminoAcidsToMap.All(x => aminoAcidValueMap.Keys.Contains(x)))
            {
                throw new ArgumentException(CouldNotRecognizeSymbol, nameof(ambiguousAminoAcid));
            }

            byte[] mappingValues = new byte[aminoAcidsToMap.Length];
            int i = 0;
            foreach (byte valueToMap in aminoAcidsToMap)
            {
                mappingValues[i++] = aminoAcidValueMap[valueToMap];
            }

            // ReSharper disable once LocalVariableHidesMember
            HashSet<byte> basicSymbols = new HashSet<byte>(mappingValues);
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

        /// <inheritdoc cref="IAlphabet.TryGetAmbiguousSymbol" />
        /// <remarks>
        ///     This alphabet does NOT support ambiguity, so the method always returns <c>false</c>.
        /// </remarks>
        public bool TryGetAmbiguousSymbol(HashSet<byte> symbols, out byte ambiguousSymbol)
        {
            ambiguousSymbol = 0;
            return basicSymbolsToAmbiguousSymbolMap.Any() &&
                   basicSymbolsToAmbiguousSymbolMap.TryGetValue(symbols, out ambiguousSymbol);
        }

        /// <inheritdoc cref="IAlphabet.TryGetBasicSymbols" />
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

        // offset and length arguments made optional by Stephen Haines.
        // TODO: Write a test.
        /// <inheritdoc cref="IAlphabet.ValidateSequence" />
        public virtual bool ValidateSequence(byte[] symbols, long offset = 0, long length = -1)
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

            for (long i = offset; i < length; i++)
            {
                if (!aminoAcidValueMap.ContainsKey(symbols[i]))
                {
                    return false;
                }
            }

            return true;
        }
*/
    }
}
