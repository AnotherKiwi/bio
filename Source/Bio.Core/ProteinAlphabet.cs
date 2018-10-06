using static Bio.Properties.Resource;
using System.Collections.Generic;

namespace Bio
{
    /// <inheritdoc cref="IProteinAlphabet"/>

    public class ProteinAlphabet : BasicProteinAlphabet, IProteinAlphabet
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
            HasGaps = true;
            HasAmbiguity = false;
            HasTerminations = true;
            IsComplementSupported = false;

            Gap = (byte)'-';
            Ter = (byte)'*';

            AddAminoAcid(Gap, "---", "Gap");
            AddAminoAcid(Ter, "***", "Termination");
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
    }
}
