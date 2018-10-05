using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Bio.Util;
using static Bio.Properties.Resource;

namespace Bio
{
    /// <summary>
    /// The basic alphabet that describes symbols used in sequences of amino
    /// acids that come from codon encodings of RNA. This alphabet allows for
    /// the twenty amino acids as well as a termination and gap symbol.
    /// <para>
    /// The character representations come from the NCBIstdaa standard and
    /// are used in many sequence file formats. The NCBIstdaa standard has all
    /// the same characters as NCBIeaa and IUPACaa, including Pyrrolysine, but
    /// adds Selenocysteine, termination, and gap symbols to the latter.
    /// </para>
    /// <para>
    /// The entries in this alphabet are (Symbol - Extended Symbol - Name):<br/>
    /// A - Ala - Alanine,
    /// C - Cys - Cysteine,
    /// D - Asp - Aspartic Acid,
    /// E - Glu - Glutamic Acid,
    /// F - Phe - Phenylalanine,
    /// G - Gly - Glycine,
    /// H - His - Histidine,
    /// I - Ile - Isoleucine,
    /// K - Lys - Lysine,
    /// L - Leu - Leucine,
    /// M - Met - Methionine,
    /// N - Asn - Asparagine,
    /// O - Pyl - Pyrrolysine,
    /// P - Pro - Proline,
    /// Q - Gln - Glutamine,
    /// R - Arg - Arginine,
    /// S - Ser - Serine,
    /// T - Thr - Threonine,
    /// U - Sel - Selenocysteine,
    /// V - Val - Valine,
    /// W - Trp - Tryptophan,
    /// Y - Tyr - Tyrosine,
    /// * - Ter - Termination,
    /// - - --- - Gap.
    /// </para>
    /// </summary>
    /// <seealso cref="IAlphabet"/>
    public class ProteinAlphabet : IProteinAlphabet
    {
        #region Private members

        /// <summary>
        /// Contains only basic symbols including Gap
        /// </summary>
        private readonly HashSet<byte> basicSymbols = new HashSet<byte>();

        /// <summary>
        /// Amino acids map  -  Maps A to A  and a to A
        /// that is key will contain unique values.
        /// This will be used in the IsValidSymbol method to address Scenarios like a == A, G == g etc.
        /// </summary>
        private readonly Dictionary<byte, byte> aminoAcidValueMap = new Dictionary<byte, byte>();

        /// <summary>
        /// Symbol to three-letter amino acid abbreviation.
        /// </summary>
        private readonly Dictionary<byte, string> abbreviationMap1to3 = new Dictionary<byte, string>();

        /// <summary>
        /// Three-letter amino acid abbreviation to symbol.
        /// </summary>
        private readonly Dictionary<string, byte> abbreviationMap3to1 = new Dictionary<string, byte>();

        /// <summary>
        /// Symbol to FriendlyName mapping.
        /// </summary>
        private readonly Dictionary<byte, string> friendlyNameMap = new Dictionary<byte, string>();

        /// <summary>
        /// Holds the amino acids present in this ProteinAlphabet.
        /// </summary>
        private readonly List<byte> aminoAcids = new List<byte>();

        /// <summary>
        /// Mapping from set of symbols to corresponding ambiguous symbol.
        /// </summary>
        private readonly Dictionary<HashSet<byte>, byte> basicSymbolsToAmbiguousSymbolMap 
            = new Dictionary<HashSet<byte>, byte>(new HashSetComparer<byte>());  

        /// <summary>
        /// Mapping from ambiguous symbol to set of basic symbols they represent.
        /// </summary>
        private readonly Dictionary<byte, HashSet<byte>> ambiguousSymbolToBasicSymbolsMap 
            = new Dictionary<byte, HashSet<byte>>();

        #endregion Private members

        /// <summary>
        /// Initializes static members of the ProteinAlphabet class.
        /// Set up the static instance.
        /// </summary>
        static ProteinAlphabet()
        {
            Instance = new ProteinAlphabet();
        }

        /// <summary>
        /// Initializes a new instance of the ProteinAlphabet class.
        /// </summary>
        protected ProteinAlphabet()
        {
            Name = ProteinAlphabetName;
            HasGaps = true;
            HasAmbiguity = false;
            HasTerminations = true;
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

            Gap = (byte)'-';
            Ter = (byte)'*';

            // Add to basic symbols
            basicSymbols.Add(A); basicSymbols.Add((byte)char.ToLower((char)A));
            basicSymbols.Add(C); basicSymbols.Add((byte)char.ToLower((char)C));
            basicSymbols.Add(D); basicSymbols.Add((byte)char.ToLower((char)D));
            basicSymbols.Add(E); basicSymbols.Add((byte)char.ToLower((char)E));
            basicSymbols.Add(F); basicSymbols.Add((byte)char.ToLower((char)F));
            basicSymbols.Add(G); basicSymbols.Add((byte)char.ToLower((char)G));
            basicSymbols.Add(H); basicSymbols.Add((byte)char.ToLower((char)H));
            basicSymbols.Add(I); basicSymbols.Add((byte)char.ToLower((char)I));
            basicSymbols.Add(K); basicSymbols.Add((byte)char.ToLower((char)K));
            basicSymbols.Add(L); basicSymbols.Add((byte)char.ToLower((char)L));
            basicSymbols.Add(M); basicSymbols.Add((byte)char.ToLower((char)M));
            basicSymbols.Add(N); basicSymbols.Add((byte)char.ToLower((char)N));
            basicSymbols.Add(O); basicSymbols.Add((byte)char.ToLower((char)O));
            basicSymbols.Add(P); basicSymbols.Add((byte)char.ToLower((char)P));
            basicSymbols.Add(Q); basicSymbols.Add((byte)char.ToLower((char)Q));
            basicSymbols.Add(R); basicSymbols.Add((byte)char.ToLower((char)R));
            basicSymbols.Add(S); basicSymbols.Add((byte)char.ToLower((char)S));
            basicSymbols.Add(T); basicSymbols.Add((byte)char.ToLower((char)T));
            basicSymbols.Add(U); basicSymbols.Add((byte)char.ToLower((char)U));
            basicSymbols.Add(V); basicSymbols.Add((byte)char.ToLower((char)V));
            basicSymbols.Add(W); basicSymbols.Add((byte)char.ToLower((char)W));
            basicSymbols.Add(Y); basicSymbols.Add((byte)char.ToLower((char)Y));
            basicSymbols.Add(Gap);

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

            AddAminoAcid(Gap, "---", "Gap");
            AddAminoAcid(Ter, "***", "Termination");
        }

        /// <inheritdoc />
        public byte A { get; }

        /// <inheritdoc />
        public byte C { get; }

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
        public byte I { get; }

        /// <inheritdoc />
        public byte K { get; }

        /// <inheritdoc />
        public byte L { get; }

        /// <inheritdoc />
        public byte M { get; }

        /// <inheritdoc />
        public byte N { get; }

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
        public byte Gap { get; }

        /// <inheritdoc />
        public byte Ter { get; }

        /// <summary>
        /// Gets or sets the name of this alphabet - this is always 'Protein'.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this alphabet has a gap character.
        /// This alphabet does have a gap character.
        /// </summary>
        public bool HasGaps { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this alphabet has ambiguous characters.
        /// This alphabet does have ambiguous characters.
        /// </summary>
        public bool HasAmbiguity { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this alphabet has termination characters.
        /// This alphabet does have termination characters.
        /// </summary>
        public bool HasTerminations { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether complement is supported or not.
        /// </summary>
        public bool IsComplementSupported { get; protected set; }

        /// <inheritdoc />
        /// <remarks>This is not a DNA alphabet.</remarks>
        public bool IsDna => false;

        /// <inheritdoc />
        /// <remarks>This is a protein alphabet.</remarks>
        public bool IsProtein => true;

        /// <inheritdoc />
        /// <remarks>This is not a RNA alphabet.</remarks>
        public bool IsRna => false;

        /// <summary>
        /// Instance of the ProteinAlphabet class.
        /// </summary>
        public static readonly ProteinAlphabet Instance;

        /// <summary>
        /// Gets count of amino acids in the sequence.
        /// </summary>
        public int Count => aminoAcids.Count;

        /// <summary>
        /// Gets the byte value of item at the given index.
        /// </summary>
        /// <param name="index">Index of the item to retrieve.</param>
        /// <returns>Byte value at the given index.</returns>
        public byte this[int index] => aminoAcids[index];

        /// <summary>
        /// Gets the friendly name of a given symbol.
        /// </summary>
        /// <param name="item">Symbol to find friendly name.</param>
        /// <returns>Friendly name of the given symbol.</returns>
        public string GetFriendlyName(byte item)
        {
            friendlyNameMap.TryGetValue(aminoAcidValueMap[item], out var fName);
            return fName;
        }

        /// <summary>
        /// Gets the three-letter abbreviation of a given symbol.
        /// </summary>
        /// <param name="item">Symbol to find three-letter abbreviation.</param>
        /// <returns>Three-letter abbreviation of the given symbol.</returns>
        public string GetThreeLetterAbbreviation(byte item)
        {
            abbreviationMap1to3.TryGetValue(aminoAcidValueMap[item], out var threeLetterAbbreviation);
            return threeLetterAbbreviation;
        }

        /// <summary>
        /// Gets the symbol from a three-letter abbreviation.
        /// </summary>
        /// <param name="item">Three-letter abbreviation to find symbol.</param>
        /// <returns>Symbol corresponding to three-letter abbreviation.</returns>
        public byte GetSymbolFromThreeLetterAbbrev(string item)
        {
            abbreviationMap3to1.TryGetValue(item, out var symbol);
            return symbol;
        }

        /// <summary>
        /// Gets the complement of the symbol.
        /// </summary>
        /// <param name="symbol">The protein symbol.</param>
        /// <param name="complementSymbol">The complement symbol.</param>
        /// <returns>Returns true if it gets the complements symbol.</returns>
        public bool TryGetComplementSymbol(byte symbol, out byte complementSymbol)
        {
            // Complement is not possible.
            complementSymbol = default(byte);
            return false;
        }

        /// <summary>
        /// This method tries to get the complements for specified symbols.
        /// </summary>
        /// <param name="symbols">Symbols to look up.</param>
        /// <param name="complementSymbols">Complement  symbols (output).</param>
        /// <returns>Returns true if found else false.</returns>
        public bool TryGetComplementSymbol(byte[] symbols, out byte[] complementSymbols)
        {
            complementSymbols = null;
            return false;
        }
        /// <summary>
        /// Gets the default Gap symbol.
        /// </summary>
        /// <param name="defaultGapSymbol">The default symbol.</param>
        /// <returns>Returns true if it gets the Default Gap Symbol.</returns>
        public virtual bool TryGetDefaultGapSymbol(out byte defaultGapSymbol)
        {
            defaultGapSymbol = Gap;
            return true;
        }

        /// <summary>
        /// Gets the default Termination symbol.
        /// </summary>
        /// <param name="defaultTerminationSymbol">The default Termination symbol.</param>
        /// <returns>Returns true if it gets the default Termination symbol.</returns>
        public virtual bool TryGetDefaultTerminationSymbol(out byte defaultTerminationSymbol)
        {
            defaultTerminationSymbol = Ter;
            return true;
        }

        /// <summary>
        /// Gets the Gap symbol.
        /// </summary>
        /// <param name="gapSymbols">The Gap Symbol.</param>
        /// <returns>Returns true if it gets the  Gap symbol.</returns>
        public virtual bool TryGetGapSymbols(out HashSet<byte> gapSymbols)
        {
            gapSymbols = new HashSet<byte> {Gap};
            return true;
        }

        /// <summary>
        /// Gets the Termination symbol.
        /// </summary>
        /// <param name="terminationSymbols">The Termination symbol.</param>
        /// <returns>Returns true if it gets the Termination symbol.</returns>
        public virtual bool TryGetTerminationSymbols(out HashSet<byte> terminationSymbols)
        {
            terminationSymbols = new HashSet<byte> {Ter};
            return true;
        }

        /// <summary>
        /// Gets the set of valid symbols.
        /// </summary>
        /// <returns>Returns HashSet of valid symbols.</returns>
        public HashSet<byte> GetValidSymbols()
        {
            return new HashSet<byte>(aminoAcidValueMap.Keys);
        }

        /// <summary>
        /// Gets the ambiguous characters present in alphabet.
        /// </summary>
        public HashSet<byte> GetAmbiguousSymbols()
        {
            return new HashSet<byte>(ambiguousSymbolToBasicSymbolsMap.Keys);
        }

        /// <summary>
        /// Maps A to A and a to A etc., that is key will contain unique values.<br/>
        /// This will be used in the IsValidSymbol method to address Scenarios like a == A, G == g etc.
        /// </summary>
        public byte[] GetSymbolValueMap()
        {
            var symbolMap = new byte[256];

            foreach (var mapping in aminoAcidValueMap)
            {
                symbolMap[mapping.Key] = mapping.Value;
            }

            return symbolMap;
        }

        /// <summary>
        /// Gets the Ambiguous symbol that corresponds to the specified set of symbols.
        /// </summary>
        /// <param name="symbols">The set of symbols.</param>
        /// <param name="ambiguousSymbol">The Ambiguous symbol.</param>
        /// <returns>Returns true if it gets the Ambiguous symbol.</returns>
        public bool TryGetAmbiguousSymbol(HashSet<byte> symbols, out byte ambiguousSymbol)
        {
            return basicSymbolsToAmbiguousSymbolMap.TryGetValue(symbols, out ambiguousSymbol);
        }

        /// <summary>
        /// Gets the Basic symbol that corresponds to the specified ambiguous symbol.
        /// </summary>
        /// <param name="ambiguousSymbol">The Ambiguous symbol.</param>
        /// <param name="basicSymbols">The Basic symbol.</param>
        /// <returns>Returns true if it gets the Basic symbol.</returns>
        // ReSharper disable once ParameterHidesMember
        public bool TryGetBasicSymbols(byte ambiguousSymbol, out HashSet<byte> basicSymbols)
        {
            return ambiguousSymbolToBasicSymbolsMap.TryGetValue(ambiguousSymbol, out basicSymbols);
        }

        /// <summary>
        /// Compares two symbols.
        /// </summary>
        /// <param name="x">The first symbol to compare.</param>
        /// <param name="y">The second symbol to compare.</param>
        /// <returns>Returns true if x equals y else false.</returns>
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

        /// <summary>
        /// Returns the consensus amino acid for a set of amino acids.
        /// </summary>
        /// <param name="symbols">Set of sequence items.</param>
        /// <returns>Consensus amino acid.</returns>
        public virtual byte GetConsensusSymbol(HashSet<byte> symbols)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 	Gets the invalid symbols in the specified sequence, in ascending order.
        /// </summary>
        /// <param name="sequence">The sequence to check for invalid symbols.</param>
        /// <returns>
        ///  	<see cref="T:byte[]"/>: the invalid symbols in the specified sequence.
        /// </returns>
        /// <exception cref="ArgumentNullException">sequence</exception>
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

        // offset and length arguments made optional by Stephen Haines.
        // TODO: Write a test.
        /// <summary>
        /// Validates if all the symbols provided are recognised symbols in this alphabet or not.
        /// </summary>
        /// <param name="symbols">Symbols to be validated.</param>
        /// <param name="offset">Offset from where validation should start.</param>
        /// <param name="length">Number of symbols to validate from the specified offset.</param>
        /// <returns>True if the validation succeeds, else false.</returns>
        /// <remarks>
        /// If <paramref name="length"/> is <c>-1</c> (the default), the symbols from <paramref name="offset"/>
        /// are validated to the end of the sequence.
        /// </remarks>
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

        /// <summary>
        /// Checks if the provided item is a gap character or not
        /// </summary>
        /// <param name="item">Item to be checked</param>
        /// <returns>True if the specified item is a gap</returns>
        public virtual bool CheckIsGap(byte item)
        {
            return item == Gap;
        }

        /// <summary>
        /// Checks if the provided item is an ambiguous character or not
        /// </summary>
        /// <param name="item">Item to be checked</param>
        /// <returns>True if the specified item is a ambiguous</returns>
        public virtual bool CheckIsAmbiguous(byte item)
        {
            return !basicSymbols.Contains(item);
        }

        /// <summary>
        /// Byte array of amino acids.
        /// </summary>
        /// <returns>Returns the Enumerator for amino acids list.</returns>
        public IEnumerator<byte> GetEnumerator()
        {
            return aminoAcids.GetEnumerator();
        }

        /// <summary>
        /// Converts the ProteinAlphabet to a string.
        /// </summary>
        /// <returns>The ProteinAlphabet as a string.</returns>
        public override string ToString()
        {
            return new string(aminoAcids.Select(x => (char)x).ToArray());
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

        /// <summary>
        /// Maps the ambiguous amino acids to the amino acids it represents. 
        /// For example ambiguous amino acids M represents the basic amino acids A or C.
        /// </summary>
        /// <param name="ambiguousAminoAcid">Ambiguous amino acids.</param>
        /// <param name="aminoAcidsToMap">Nucleotide represented by ambiguous amino acids.</param>
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
    }
}
