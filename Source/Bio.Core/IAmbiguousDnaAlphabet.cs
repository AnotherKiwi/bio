
// ReSharper disable InconsistentNaming

namespace Bio
{
    /// <summary>
    ///     The basic alphabet that describes symbols used in DNA sequences.
    ///     This alphabet allows not only for the four base nucleotide symbols,
    ///     but also for various ambiguities, and gap symbol.
    ///     <para>
    ///         The character representations come from the NCBI4na standard and
    ///         are used in many sequence file formats. The NCBI4na standard is the
    ///         same as the IUPACna standard with only the addition of the gap
    ///         character.
    ///     </para>
    ///     <para>
    ///         The entries in this dictionary are (Symbol - Name):
    ///         A - Adenine,
    ///         C - Cytosine,
    ///         M - A or C,
    ///         G - Guanine,
    ///         R - G or A,
    ///         S - G or C,
    ///         V - G or V or A,
    ///         T - Thymine,
    ///         W - A or T,
    ///         Y - T or C,
    ///         H - A or C or T,
    ///         K - G or T,
    ///         D - G or A or T,
    ///         B - G or T or C,
    ///         N - A or G or T or C,
    ///         - - Gap.
    ///     </para>
    /// </summary>

    public interface IAmbiguousDnaAlphabet : IDnaAlphabet
    {
        /// <summary>
        ///     Gets Ambiguous symbol A-Adenine C-Cytosine.
        /// </summary>
        byte AC { get; }

        /// <summary>
        ///     Gets Ambiguous symbol G-Guanine A-Adenine.
        /// </summary>
        byte GA { get; }

        /// <summary>
        ///     Gets Ambiguous symbol G-Guanine C-Cytosine.
        /// </summary>
        byte GC { get; }

        /// <summary>
        ///     Gets Ambiguous symbol A-Adenine T-Thymine.
        /// </summary>
        byte AT { get; }

        /// <summary>
        ///     Gets Ambiguous symbol T-Thymine C-Cytosine.
        /// </summary>
        byte TC { get; }

        /// <summary>
        ///     Gets Ambiguous symbol  G-Guanine T-Thymine.
        /// </summary>
        byte GT { get; }

        /// <summary>
        ///     Gets Ambiguous symbol G-Guanine C-Cytosine A-Adenine.
        /// </summary>
        byte GCA { get; }

        /// <summary>
        ///     Gets Ambiguous symbol A-Adenine C-Cytosine T-Thymine.
        /// </summary>
        byte ACT { get; }

        /// <summary>
        ///     Gets Ambiguous symbol G-Guanine A-Adenine T-Thymine.
        /// </summary>
        byte GAT { get; }

        /// <summary>
        ///     Gets Ambiguous symbol G-Guanine T-Thymine C-Cytosine.
        /// </summary>
        byte GTC { get; }

        /// <summary>
        ///     Gets Ambiguous symbol Any.
        /// </summary>
        byte Any { get; }
    }
}
