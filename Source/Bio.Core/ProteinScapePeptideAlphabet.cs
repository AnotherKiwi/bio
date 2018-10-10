using System;
using System.Linq;
using static Bio.Properties.Resource;

namespace Bio
{
    /// <inheritdoc cref="IProteinScapePeptideAlphabet" />
    /// <seealso cref="StrictProteinAlphabet"/>
    /// <seealso cref="IProteinScapePeptideAlphabet"/>

    public class ProteinScapePeptideAlphabet : ProteinFragmentAlphabet, IProteinScapePeptideAlphabet
    {
        /// <summary>
        ///     Instance of the ProteinScapePeptideAlphabet class.
        /// </summary>
        public new static readonly ProteinScapePeptideAlphabet Instance;

        /// <summary>
        ///     Set up the static instance.
        /// </summary>
        static ProteinScapePeptideAlphabet()
        {
            Instance = new ProteinScapePeptideAlphabet();
        }

        /// <summary>
        ///     Initializes a new instance of the ProteinScapePeptideAlphabet class.
        /// </summary>
        protected ProteinScapePeptideAlphabet()
        {
            Name = ProteinScapePeptideAlphabetName;
            IsCaseSensitive = true;

            CTerminus = (byte)'-';
            NTerminus = (byte)'~';

            AddAminoAcid(CTerminus, "---", "C-terminus", false);
            AddAminoAcid(NTerminus, "~~~", "N-terminus", false);
        }

        /// <inheritdoc />
        public byte CTerminus { get; }

        /// <inheritdoc />
        public byte NTerminus { get; }

        /// <inheritdoc />
        /// <remarks>
        ///     Also checks that the sequence contains two
        ///     <see cref="ProteinFragmentAlphabet.SequenceDelimiter"/>
        ///     symbols, and that they occur in appropriate positions.
        /// </remarks>
        public override bool ValidateSequence(byte[] symbols)
        {
            return ValidateSequence(symbols, 0, symbols.LongLength);
        }

        /// <inheritdoc />
        /// <remarks>
        ///     Also checks that the sequence contains two
        ///     <see cref="ProteinFragmentAlphabet.SequenceDelimiter"/>
        ///     symbols, and that they occur in appropriate positions.
        /// </remarks>
        public override bool ValidateSequence(byte[] symbols, long offset, long length)
        {
            // Make sure the sequence doesn't contain any invalid symbols.
            if (!base.ValidateSequence(symbols, offset, length))
            {
                return false;
            }

            int sequenceLength = symbols.Length;
            if (sequenceLength < 3)
            {
                throw new ArgumentOutOfRangeException(nameof(sequenceLength), SequenceLengthLessThanMinimum);
            }

            // Check that the peptide has exactly two SequenceDelimiter symbols, and that
            // N- and C-terminus symbols (if present) are in appropriate positions.
            if ((symbols.Count(s => s == SequenceDelimiter) != 2) || !CheckTerminiAreValid(symbols, sequenceLength))
            {
                return false;
            }

            return (symbols[1] == SequenceDelimiter) && (symbols[sequenceLength - 2] == SequenceDelimiter);
        }

        /// <summary>
        /// 	Checks that, if present, the C- and/or N-termini symbols are in valid positions.
        /// </summary>
        /// <param name="symbols">The symbols.</param>
        /// <param name="sequenceLength">Length of the sequence.</param>
        /// <returns>
        ///  	<see cref="bool"/>: 
        ///  	<c>true</c> if the C- and/or N-termini symbols either not present or they
        ///     are in valid positions, <c>false</c> otherwise.
        /// </returns>
        private bool CheckTerminiAreValid(byte[] symbols, int sequenceLength)
        {
            return (!symbols.Contains(CTerminus) || (symbols[sequenceLength - 1] == CTerminus)) && 
                   (!symbols.Contains(NTerminus) || symbols[0] == NTerminus);
        }
    }
}
