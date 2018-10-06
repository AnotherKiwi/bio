namespace Bio
{
    /// <summary>
    ///     The basic alphabet that describes symbols used in DNA sequences.
    ///     This alphabet allows only the four base nucleotide symbols, which
    ///     come from the NCBI2na standard, with the addition of a gap symbol.
    ///     <para>
    ///         The entries in this dictionary are (Symbol - Name):
    ///         A - Adenine,
    ///         C - Cytosine,
    ///         G - Guanine,
    ///         T - Thymine,
    ///         - - Gap.
    ///     </para>
    /// </summary>

    public interface IDnaAlphabet : IAlphabet
    {
        /// <summary>
        ///     Gets A - Adenine.
        /// </summary>
        byte A { get; }

        /// <summary>
        ///     Gets T - Thymine.
        /// </summary>
        byte T { get; }

        /// <summary>
        ///     Gets G - Guanine.
        /// </summary>
        byte G { get; }

        /// <summary>
        ///     Gets C - Cytosine.
        /// </summary>
        byte C { get; }

        /// <summary>
        ///     Gets Default Gap symbol.
        /// </summary>
        byte Gap { get; }
    }
}
