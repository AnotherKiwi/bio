namespace Bio
{
    public interface IPeaksPeptideAlphabet : IProteinFragmentAlphabet
    {
        /// <summary>
        /// 	Gets the 'b' symbol which can only appear within a substitution modification, e.g. '(sub L)'.
        /// </summary>
        byte SmallB { get; }

        /// <summary>
        /// 	Gets the '0' symbol which can only appear within a modification, e.g. '(+28.03)'.
        /// </summary>
        byte Digit0 { get; }

        /// <summary>
        /// 	Gets the '1' symbol which can only appear within a modification, e.g. '(+15.99)'.
        /// </summary>
        byte Digit1 { get; }

        /// <summary>
        /// 	Gets the '2' symbol which can only appear within a modification, e.g. '(+28.03)'.
        /// </summary>
        byte Digit2 { get; }

        /// <summary>
        /// 	Gets the '3' symbol which can only appear within a modification, e.g. '(+31.99)'.
        /// </summary>
        byte Digit3 { get; }

        /// <summary>
        /// 	Gets the '4' symbol which can only appear within a modification, e.g. '(+541.06)'.
        /// </summary>
        byte Digit4 { get; }

        /// <summary>
        /// 	Gets the '5' symbol which can only appear within a modification, e.g. '(+15.99)'.
        /// </summary>
        byte Digit5 { get; }

        /// <summary>
        /// 	Gets the '6' symbol which can only appear within a modification, e.g. '(+541.06)'.
        /// </summary>
        byte Digit6 { get; }

        /// <summary>
        /// 	Gets the '7' symbol which can only appear within a modification, e.g. '(+79.97)'.
        /// </summary>
        byte Digit7 { get; }

        /// <summary>
        /// 	Gets the '8' symbol which can only appear within a modification, e.g. '(+.98)'.
        /// </summary>
        byte Digit8 { get; }

        /// <summary>
        /// 	Gets the '9' symbol which can only appear within a modification, e.g. '(+15.99)'.
        /// </summary>
        byte Digit9 { get; }

        /// <summary>
        /// 	Gets the '-' symbol which can only appear within a modification, e.g. '(-.98)'.
        /// </summary>
        byte Minus { get; }

        /// <summary>
        /// 	Gets the symbol that PEAKS uses to denote the start of a modification.
        /// </summary>
        byte ModificationBeginDelimiter { get; }

        /// <summary>
        /// 	Gets the symbol that PEAKS uses to denote the end of a modification.
        /// </summary>
        byte ModificationEndDelimiter { get; }

        /// <summary>
        /// 	Gets the '.' symbol which can only appear within a modification, e.g. '(+.98)'.
        /// </summary>
        byte Period { get; }

        /// <summary>
        /// 	Gets the '+' symbol which can only appear within a modification, e.g. '(+.98)'.
        /// </summary>
        byte Plus { get; }

        /// <summary>
        /// 	Gets the 's' symbol which can only appear within a substitution modification, e.g. '(sub L)'.
        /// </summary>
        byte SmallS { get; }

        /// <summary>
        /// 	Gets the Space symbol (' ') which can only appear within a substitution modification, e.g. '(sub L)'.
        /// </summary>
        byte Space { get; }

        /// <summary>
        /// 	Gets the Sub collection of symbols ('sub ') which can only appear within a
        ///     substitution modification, e.g. '(sub L)'.
        /// </summary>
        byte[] Sub { get; }

        /// <summary>
        /// 	Gets the 'u' symbol which can only appear within a substitution modification, e.g. '(sub L)'.
        /// </summary>
        byte SmallU { get; }

    }
}
