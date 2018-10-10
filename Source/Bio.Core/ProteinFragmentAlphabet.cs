using System;
using System.Linq;
using static Bio.Properties.Resource;

namespace Bio
{
    /// <inheritdoc cref="IProteinFragmentAlphabet" />
    /// <seealso cref="StrictProteinAlphabet"/>
    /// <seealso cref="IProteinFragmentAlphabet"/>

    public class ProteinFragmentAlphabet : StrictProteinAlphabet, IProteinFragmentAlphabet
    {
        /// <summary>
        ///     Instance of the ProteinFragmentAlphabet class.
        /// </summary>
        public new static readonly ProteinFragmentAlphabet Instance;

        /// <summary>
        ///     Set up the static instance.
        /// </summary>
        static ProteinFragmentAlphabet()
        {
            Instance = new ProteinFragmentAlphabet();
        }

        /// <summary>
        ///     Initializes a new instance of the ProteinFragmentAlphabet class.
        /// </summary>
        protected ProteinFragmentAlphabet()
        {
            Name = ProteinFragmentAlphabetName;
            IsCaseSensitive = true;

            SequenceDelimiter = (byte)'.';

            AddAminoAcid(SequenceDelimiter, "...", "Sequence Delimiter", false);
        }


        /// <inheritdoc />
        public byte SequenceDelimiter { get; }

        /// <inheritdoc />
        /// <remarks>
        ///     Also checks that the sequence contains  no more than two
        ///     <see cref="SequenceDelimiter"/> symbols, and that they occur
        ///     in appropriate positions.
        /// </remarks>
        public override bool ValidateSequence(byte[] symbols)
        {
            return ValidateSequence(symbols, 0, symbols.LongLength);
        }

        /// <inheritdoc />
        /// <remarks>
        ///     Also checks that the sequence contains  no more than two
        ///     <see cref="SequenceDelimiter"/> symbols, and that they occur
        ///     in appropriate positions.
        /// </remarks>
        public override bool ValidateSequence(byte[] symbols, long offset, long length)
        {
            if (!base.ValidateSequence(symbols, offset, length))
            {
                return false;
            }

            int sequenceLength = symbols.Length;
            if (sequenceLength < 3)
            {
                throw new ArgumentOutOfRangeException(nameof(sequenceLength), SequenceLengthLessThanMinimum);
            }

            // Check that the peptide has no more than two SequenceDelimiter symbols, and that they
            // appear in allowed places (in second and/or second to last position in sequence).
            int symbolCount = symbols.Count(s => s == SequenceDelimiter);
            switch (symbolCount)
            {
                case 2:
                    return symbols[1] == SequenceDelimiter && symbols[sequenceLength - 2] == SequenceDelimiter;
                case 1:
                    return symbols[1] == SequenceDelimiter || symbols[sequenceLength - 2] == SequenceDelimiter;
                default:
                    return false;
            }
        }
    }
}
