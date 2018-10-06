using System;
using System.Collections.Generic;
using System.Text;

namespace Bio
{
    /// <summary>
    ///     An alphabet that extends <see cref="IProteinAlphabet"/> by adding symbols for ambiguous amino acids.
    /// </summary>
    /// <remarks>
    ///     Specifically, adds B (for Glx), J (for Xle), X (for Xxx) and Z (for Glx).
    /// </remarks>
    public interface IAmbiguousProteinAlphabet : IProteinAlphabet
    {

        /// <summary>
        ///     Gets the symbol for the ambiguous amino acid B (i.e. Asx = Aspartic Acid or Asparagine).
        /// </summary>
        byte B { get; }

        /// <summary>
        ///     Gets the symbol for the ambiguous amino acid J (i.e. Xle = Leucine or Isoleucine).
        /// </summary>
        byte J { get; }

        /// <summary>
        ///     Gets the symbol for the undetermined or atypical amino acid X (i.e. Xxx).
        /// </summary>
        byte X { get; }

        /// <summary>
        ///     Gets the symbol for the ambiguous amino acid Z (i.e. Glx = Glutamic Acid or Glutamine).
        /// </summary>
        byte Z { get; }
    }
}
