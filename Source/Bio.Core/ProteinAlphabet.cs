using System.Collections.Generic;

using static Bio.Properties.Resource;

namespace Bio
{
    /// <inheritdoc cref="IProteinAlphabet"/>

    public class ProteinAlphabet : AminoAcidsAlphabet, IProteinAlphabet
    {
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
            AlphabetType = Alphabets.AlphabetTypes.Protein;
            HasGaps = true;
            HasAmbiguity = false;
            HasTerminations = true;
            IsCaseSensitive = false;
            IsComplementSupported = false;

            Gap = (byte)'-';
            Ter = (byte)'*';

            AddAminoAcids();
        }

        /// <inheritdoc />
        public byte Gap { get; }

        /// <inheritdoc />
        public byte Ter { get; }

        /// <summary>
        /// 	Adds the amino acids to the alphabet.
        /// </summary>
        protected void AddAminoAcids()
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
    }
}
