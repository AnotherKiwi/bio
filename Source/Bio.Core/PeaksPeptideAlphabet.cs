﻿using static Bio.Properties.Resource;
using System.Collections.Generic;
using System.Linq;
using Bio.Core.Extensions;

namespace Bio
{
    /// <inheritdoc cref="IPeaksPeptideAlphabet" />
    /// <seealso cref="StrictProteinAlphabet"/>
    /// <seealso cref="IPeaksPeptideAlphabet"/>

    public class PeaksPeptideAlphabet : ProteinFragmentAlphabet, IPeaksPeptideAlphabet
    {
        private readonly HashSet<byte> _aminoAcids;

        /// <summary>
        /// 	Set of digits (0-9).
        /// </summary>
        private readonly byte[] _digits;

        /// <summary>
        /// 	Set of numeric symbols (digits and '.').
        /// </summary>
        private readonly byte[] _numerics;

        /// <summary>
        /// 	Set of numeric signs ('+' and '-').
        /// </summary>
        private readonly byte[] _signs;

        /// <summary>
        /// 	Set of numeric signs ('+' and '-') and the period ('.').
        /// </summary>
        private readonly byte[] _signsAndPeriod;

        /// <summary>
        ///     Instance of the PeaksPeptideAlphabet class.
        /// </summary>
        public new static readonly PeaksPeptideAlphabet Instance;

        /// <summary>
        ///     Set up the static instance.
        /// </summary>
        static PeaksPeptideAlphabet()
        {
            Instance = new PeaksPeptideAlphabet();
        }

        /// <summary>
        ///     Initializes a new instance of the PeaksPeptideAlphabet class.
        /// </summary>
        protected PeaksPeptideAlphabet()
        {
            Name = PeaksPeptideAlphabetName;
            IsCaseSensitive = true;

            _aminoAcids = GetUnambiguousAminoAcids();

            ModificationBeginDelimiter = (byte) '(';
            ModificationEndDelimiter = (byte) ')';

            // Symbols that are valid only within the two modification delimiters.
            Digit0 = (byte) '0';
            Digit1 = (byte) '1';
            Digit2 = (byte) '2';
            Digit3 = (byte) '3';
            Digit4 = (byte) '4';
            Digit5 = (byte) '5';
            Digit6 = (byte) '6';
            Digit7 = (byte) '7';
            Digit8 = (byte) '8';
            Digit9 = (byte) '9';
            SmallS = (byte) 's';
            SmallU = (byte) 'u';
            SmallB = (byte) 'b';
            Plus = (byte) '+';
            Minus = (byte) '-';
            Period = (byte) '.';
            Space = (byte) ' ';
            Sub = new[] {SmallS, SmallU, SmallB, Space};

            _digits = new []
                {Digit0, Digit1, Digit2, Digit3, Digit4, Digit5, Digit6, Digit7, Digit8, Digit9};
            _numerics = new []
                {Digit0, Digit1, Digit2, Digit3, Digit4, Digit5, Digit6, Digit7, Digit8, Digit9, Period};
            _signs = new[] {Plus, Minus};
            _signsAndPeriod = new[] {Plus, Minus, Period};

            // Add the modification specification symbols to the alphabet.
            AddAminoAcids();
        }

        private void AddAminoAcids()
        {
            AddAminoAcid(ModificationBeginDelimiter, "(((", "Modification Start Delimiter", false);
            AddAminoAcid(ModificationEndDelimiter, ", false), false), false)", "Modification End Delimiter", false);
            AddAminoAcid(Digit0, "000", "Modification Digit '0'", false);
            AddAminoAcid(Digit1, "111", "Modification Digit '1'", false);
            AddAminoAcid(Digit2, "222", "Modification Digit '2'", false);
            AddAminoAcid(Digit3, "333", "Modification Digit '3'", false);
            AddAminoAcid(Digit4, "444", "Modification Digit '4'", false);
            AddAminoAcid(Digit5, "555", "Modification Digit '5'", false);
            AddAminoAcid(Digit6, "666", "Modification Digit '6'", false);
            AddAminoAcid(Digit7, "777", "Modification Digit '7'", false);
            AddAminoAcid(Digit8, "888", "Modification Digit '8'", false);
            AddAminoAcid(Digit9, "999", "Modification Digit '9'", false);
            AddAminoAcid(SmallS, "sss", "Modification Sub Char 's'", false);
            AddAminoAcid(SmallU, "uuu", "Modification Sub Char 's'", false);
            AddAminoAcid(SmallB, "bbb", "Modification Sub Char 's'", false);
            AddAminoAcid(Plus, "+++", "Modification Char '+'", false);
            AddAminoAcid(Minus, "---", "Modification Char '-'", false);
            AddAminoAcid(Space, "   ", "Modification Char ' '", false);
        }

        /// <inheritdoc />
        public byte SmallB { get; }

        /// <inheritdoc />
        public byte Digit0 { get; }

        /// <inheritdoc />
        public byte Digit1 { get; }

        /// <inheritdoc />
        public byte Digit2 { get; }

        /// <inheritdoc />
        public byte Digit3 { get; }

        /// <inheritdoc />
        public byte Digit4 { get; }

        /// <inheritdoc />
        public byte Digit5 { get; }

        /// <inheritdoc />
        public byte Digit6 { get; }

        /// <inheritdoc />
        public byte Digit7 { get; }

        /// <inheritdoc />
        public byte Digit8 { get; }

        /// <inheritdoc />
        public byte Digit9 { get; }

        /// <inheritdoc />
        public byte Minus { get; }

        /// <inheritdoc />
        public byte ModificationBeginDelimiter { get; }

        /// <inheritdoc />
        public byte ModificationEndDelimiter { get; }

        /// <inheritdoc />
        public byte Period { get; }

        /// <inheritdoc />
        public byte Plus { get; }

        /// <inheritdoc />
        public byte SmallS { get; }

        /// <inheritdoc />
        public byte Space { get; }

        /// <inheritdoc />
        public byte[] Sub { get; }

        /// <inheritdoc />
        public byte SmallU { get; }

        /// <inheritdoc />
        public override bool ValidateSequence(byte[] symbols)
        {
            return ValidateSequence(symbols, 0, symbols.LongLength);
        }

        /// <inheritdoc />
        public override bool ValidateSequence(byte[] symbols, long offset, long length)
        {
            // Make sure the sequence doesn't contain any invalid symbols, and
            // that it starts with an amino acid symbol.
            if (!(base.ValidateSequence(symbols, offset, length) && _aminoAcids.Contains(symbols[0])))
            {
                return false;
            }

            int sequenceLength = symbols.Length;
            var isModification = false;
            var preceedingWasAA = true;
            byte[] ubSpace = {SmallU, SmallB, Space};

            int i = 1;
            while (i < sequenceLength)
            {
                byte symbol = symbols[i];

                if (symbol == ModificationBeginDelimiter)
                {
                    // '(' must follow an amino acid symbol.
                    if (!preceedingWasAA)
                    {
                        return false;
                    }

                    isModification = true;
                }

                else if (symbol == ModificationEndDelimiter)
                {
                    // A preceding '(' must have occurred.
                    if (!isModification)
                    {
                        return false;
                    }

                    isModification = false;
                }

                else if (symbol == SmallS)
                {
                    // Starting a 'sub' modification, which must obey the pattern '(sub A)'.
                    if (!CheckSubstitutionIsValid(symbols, i, sequenceLength, _aminoAcids))
                    {
                        return false;
                    }

                    // Skip to the symbol before the ')' symbol.
                    i += 4;
                }

                else if (ubSpace.Contains(symbol))
                {
                    // symbol should never be able to be 'u', 'b' or ' '. 
                    return false;
                }

                else if (_signs.Contains(symbol) && !CheckSignIsValid(symbols, i, sequenceLength))
                {
                    return false;
                }

                else if (_digits.Contains(symbol) && !CheckDigitIsValid(symbols, i, sequenceLength))
                {
                    return false;
                }

                else if (symbol == Period && !CheckPeriodIsValid(symbols, i, sequenceLength))
                {
                    return false;
                }

                else
                {
                    preceedingWasAA = _aminoAcids.Contains(symbol);
                    if (!preceedingWasAA)
                    {
                        return false;
                    }
                }

                i++;
            }

            return true;
        }

        /// <summary>
        /// 	Checks the period symbol. Returns <c>true</c> if it is part of a
        ///     valid numeric pattern in a modification.
        /// </summary>
        /// <param name="symbols">The symbols to check.</param>
        /// <param name="index">The current index.</param>
        /// <param name="sequenceLength">Length of the sequence.</param>
        /// <returns>
        ///  	<see cref="bool"/>: 
        ///  	<c>true</c> if the current symbol is part of a valid numeric pattern, <c>false</c> otherwise.
        /// </returns>
        /// <remarks>
        ///     Periods occur in patterns like 'C(+31.99)' and 'N(+.98)', so there must be room for more symbols
        ///     in the sequence, the preceding symbol must be a digit or a sign, and the following symbol must
        ///     be a digit.
        /// </remarks>
        private bool CheckPeriodIsValid(byte[] symbols, int index, int sequenceLength)
        {
            return (_digits.Contains(symbols[index - 1]) || _signsAndPeriod.Contains(symbols[index + 1])) &&
                   (index < sequenceLength - 1) &&
                   _digits.Contains(symbols[index + 1]);
        }

        /// <summary>
        /// 	Checks the sign symbols. Returns <c>true</c> if the current symbol is part of a
        ///     valid numeric pattern.
        /// </summary>
        /// <param name="symbols">The symbols to check.</param>
        /// <param name="index">The current index.</param>
        /// <param name="sequenceLength">Length of the sequence.</param>
        /// <returns>
        ///  	<see cref="bool"/>: 
        ///  	<c>true</c> if the current symbol is part of a valid numeric pattern, <c>false</c> otherwise.
        /// </returns>
        /// <remarks>
        ///     Signs occur in patterns like 'C(+31.99)' and 'N(+.98)', so there must be room for 4 more symbols
        ///     in the sequence and the following symbol must be a digit or '.".
        /// </remarks>
        private bool CheckSignIsValid(in byte[] symbols, int index, int sequenceLength)
        {
            return (symbols[index - 1] == ModificationBeginDelimiter) && 
                   (index < sequenceLength - 4) &&                       
                   _numerics.Contains(symbols[index + 1]);            
        }

        /// <summary>
        /// 	Checks digits. Returns <c>true</c> if the current symbol is part of a
        ///     valid numeric pattern in a modification.
        /// </summary>
        /// <param name="symbols">The symbols to check.</param>
        /// <param name="index">The current index.</param>
        /// <param name="sequenceLength">Length of the sequence.</param>
        /// <returns>
        ///  	<see cref="bool"/>: 
        ///  	<c>true</c> if the current symbol is part of a valid substitution pattern, <c>false</c> otherwise.
        /// </returns>
        /// <remarks>
        ///     Digits occur in patterns like 'C(+31.99)' and 'N(+.98)', so there must be room for 1 more symbol
        ///     in the sequence, the preceding symbol must be a sign or a period, and the following symbol must
        ///     be another digit, a '.' or a ")".
        /// </remarks>
        private bool CheckDigitIsValid(byte[] symbols, int index, int sequenceLength)
        {
            return (index < sequenceLength - 1) &&
                   _signs.Contains(symbols[index - 1]) &&
                   _signsAndPeriod.Contains(symbols[index - 1]) &&
                   (_digits.Contains(symbols[index + 1]) || 
                    (symbols[index + 1] == Period) ||
                    (symbols[index + 1] == ModificationEndDelimiter));
        }

        /// <summary>
        /// 	Returns <c>true</c> if the current symbol is the beginning of a valid substitution modification.
        /// </summary>
        /// <param name="symbols">The symbols.</param>
        /// <param name="index">Index of the current.</param>
        /// <param name="sequenceLength">Length of the sequence.</param>
        /// <param name="aminoAcids">Set of valid amino acid symbols.</param>
        /// <returns>
        ///     <see cref="bool" />: 
        ///     <c>true</c> if the current symbol begins a valid substitution, <c>false</c> otherwise.
        /// </returns>
        /// <remarks>
        ///  	A valid substitution is like '(sub V)', so there must be room for 5 more symbols, the
        ///     5th char after 's' must be ')', 's' must be followed by 'ub ' and a valid amino acid symbol.
        /// </remarks>
        private bool  CheckSubstitutionIsValid(in byte[] symbols, int index, int sequenceLength, 
            in HashSet<byte> aminoAcids)
        {
            return (symbols[index - 1] == ModificationBeginDelimiter) && 
                   (index < sequenceLength - 5) &&                       
                   (symbols[index + 5] == ModificationEndDelimiter) &&   
                   (symbols.GetRange(index, 4) == Sub) &&
                   (aminoAcids.Contains(symbols[index + 4]));

        }
    }
}