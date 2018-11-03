namespace Bio
{
    /// <summary>
    ///     An alphabet for a Peptide sequence that is a fragment of a Protein, which will contain
    ///     the symbol(s) of the preceding and/or following amino acids in the protein sequence separated
    ///     from the peptide sequence by a delimiter symbol.
    /// </summary>
    /// <seealso cref="IStandardAminoAcidsAlphabet" />

    public interface IProteinFragmentAlphabet : IStandardAminoAcidsAlphabet
    {
        /// <summary>
        /// 	Gets the symbol that denotes the start and the end of the peptide sequence.
        /// </summary>
        byte SequenceDelimiter { get; }
    }
}
