namespace Bio
{
    public interface IProteinScapePeptideAlphabet : IProteinFragmentAlphabet
    {

        /// <summary>
        /// 	Gets the symbol used by ProteinScape to denote that the peptide has no following
        ///     amino acid residue, as it is at the C-terminus of the containing protein.
        /// </summary>
        byte CTerminus { get; }

        /// <summary>
        /// 	Gets the symbol used by ProteinScape to denote that the peptide has no preceding
        ///     amino acid residue, as it is at the N-terminus of the containing protein.
        /// </summary>
        byte NTerminus { get; }
    }
}
