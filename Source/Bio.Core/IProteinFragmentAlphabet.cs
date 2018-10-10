using System;
using System.Collections.Generic;
using System.Text;

namespace Bio
{
    /// <summary>
    ///     An alphabet for a Peptide sequence that is a fragment of a Protein, which may contain
    ///     the symbols of the preceding and following amino acids in the protein sequence separated
    ///     from the peptide sequence by a delimiter symbol.
    /// </summary>
    /// <seealso cref="IAminoAcidAlphabet" />

    public interface IProteinFragmentAlphabet : IAminoAcidAlphabet
    {
        /// <summary>
        /// 	Gets the symbol that denotes the start and the end of the peptide sequence.
        /// </summary>
        byte SequenceDelimiter { get; }
    }
}
