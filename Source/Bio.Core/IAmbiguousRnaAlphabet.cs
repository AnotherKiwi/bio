// ReSharper disable InconsistentNaming
namespace Bio
{
    /// <summary>
    ///     The basic alphabet that describes symbols used in RNA sequences.
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
    ///         U - Uracil,
    ///         W - A or U,
    ///         Y - U or C,
    ///         H - A or C or U,
    ///         K - G or U,
    ///         D - G or A or U,
    ///         B - G or U or C,
    ///         N - A or G or U or C,
    ///         - - Gap.
    ///     </para>
    /// </summary>

    public interface IAmbiguousRnaAlphabet : IRnaAlphabet
    {

        /// <summary>
        ///     Gets Ambiguous symbols A-Adenine C-Cytosine.
        /// </summary>
        byte AC { get; }

        /// <summary>
        ///     Gets Ambiguous symbols G-Guanine A-Adenine.
        /// </summary>
        byte GA { get; }

        /// <summary>
        ///     Gets Ambiguous symbols G-Guanine C-Cytosine.
        /// </summary>
        byte GC { get; }

        /// <summary>
        ///     Gets Ambiguous symbols A-Adenine U-Uracil.
        /// </summary>
        byte AU { get; }

        /// <summary>
        ///     Gets Ambiguous symbols U-Uracil C-Cytosine.
        /// </summary>
        byte UC { get; }

        /// <summary>
        ///     Gets Ambiguous symbols  G-Guanine U-Uracil.
        /// </summary>
        byte GU { get; }

        /// <summary>
        ///     Gets Ambiguous symbols G-Guanine C-Cytosine A-Adenine.
        /// </summary>
        byte GCA { get; }

        /// <summary>
        ///     Gets Ambiguous symbols A-Adenine C-Cytosine U-Uracil.
        /// </summary>
        byte ACU { get; }

        /// <summary>
        ///     Gets Ambiguous symbols G-Guanine A-Adenine U-Uracil.
        /// </summary>
        byte GAU { get; }

        /// <summary>
        ///     Gets Ambiguous symbols G-Guanine U-Uracil C-Cytosine.
        /// </summary>
        byte GUC { get; }

        /// <summary>
        ///     Gets Ambiguous symbol Any.
        /// </summary>
        byte Any { get; }
    }
}
