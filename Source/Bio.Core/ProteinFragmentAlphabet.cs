using static Bio.Properties.Resource;
using System.Linq;
using static Bio.Alphabets;

namespace Bio
{
    /// <inheritdoc cref="IProteinFragmentAlphabet" />
    /// <seealso cref="AminoAcidsAlphabet"/>
    /// <seealso cref="IProteinFragmentAlphabet"/>

    public class ProteinFragmentAlphabet : AminoAcidsAlphabet, IProteinFragmentAlphabet
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
            AlphabetType = AlphabetTypes.ProteinFragment;
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
        public override bool ValidateSequence(byte[] symbols) => 
            ValidateSequence(symbols, 0, GetSymbolCount(symbols), checkSequenceDelimiters: true);

        /// <summary>
        /// 	Validates the sequence.
        /// </summary>
        /// <param name="symbols">The symbols to be validated.</param>
        /// <param name="offset">The offset at which to start validation. This MUST be 0.</param>
        /// <param name="length">The number of symbols to validate. This MUST be the TOTAL NUMBER
        /// of symbols in the sequence.</param>
        /// <remarks>
        ///  	Also checks that the sequence contains no more than two
        ///     <see cref="SequenceDelimiter" /> symbols, and that they occur
        ///     in appropriate positions.
        ///     <para>
        ///         The whole Protein Fragment must be validated, so an exception is thrown
        ///         if <paramref name="offset"/> is not zero or <paramref name="length"/>
        ///         does not equal the number of symbols in the sequence.
        ///     </para>
        /// </remarks>
        public override bool ValidateSequence(byte[] symbols, long offset, long length) =>
            ValidateSequence(symbols, offset, length, checkSequenceDelimiters: true);

        /// <summary>
        /// 	Validates the sequence. Also returns <c>true</c> if the sequence is empty.
        /// </summary>
        /// <param name="symbols">The symbols to be validated.</param>
        /// <param name="offset">The offset at which to start validation. This MUST be 0.</param>
        /// <param name="length">
        ///     The number of symbols to validate. This MUST be the TOTAL NUMBER of symbols in the sequence.
        /// </param>
        /// <param name="checkSequenceDelimiters">
        ///     If set to <c>true</c>, the number and positions of sequence delimiter symbols is checked.
        /// </param>
        /// <remarks>
        ///  	Optionally, also checks that the sequence contains no more than two
        ///     <see cref="SequenceDelimiter" /> symbols, and that they occur
        ///     in appropriate positions.
        ///     <para>
        ///         The whole sequence of the ProteinFragment must be validated, so an exception
        ///         is thrown if <paramref name="offset"/> is not zero or <paramref name="length"/>
        ///         does not equal the number of symbols in the sequence.
        ///     </para>
        /// </remarks>
        protected bool ValidateSequence(byte[] symbols, long offset, long length, bool checkSequenceDelimiters)
        {
            if (!base.ValidateSequence(symbols, offset, length))
            {
                return false;
            }

            // An empty array of symbols is OK, as long as offset and length are both 0.
            if ((symbols.LongLength == 0) && (offset == 0) && (length == 0))
            {
                return true;
            }

            if ((offset != 0) || (length < 3))
            {
                return false;
            }

            if (!checkSequenceDelimiters) return true;

            // Check that the peptide has no more than two SequenceDelimiter symbols, and that they
            // appear in allowed places (in second and/or second to last position in sequence).
            long symbolCount = symbols.Count(s => s == SequenceDelimiter);
            switch (symbolCount)
            {
                case 2:
                    return (symbols[1] == SequenceDelimiter) && (symbols[length - 2] == SequenceDelimiter);
                case 1:
                    return (symbols[1] == SequenceDelimiter) || (symbols[length - 2] == SequenceDelimiter);
                default:
                    return false;
            }
        }
    }
}
