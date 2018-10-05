using System;
using System.Collections.Generic;
using System.Text;

namespace Bio
{
    /// <summary>
    /// An alphabet that extends <see cref="IBasicProteinAlphabet"/> by adding termination and gap symbols.
    /// </summary>
    /// <remarks>
    /// Specifically, adds Gap (i.e. '-'), and Ter (i.e. '*', for terminations).<br/>
    /// Symbols for ambiguous amino acids are not included in this alphabet.
    /// </remarks>
    /// <seealso cref="IBasicProteinAlphabet" />,
    /// <seealso cref="IAlphabet"/>
    public interface IProteinAlphabet : IBasicProteinAlphabet
    {

        /// <summary>
        /// Gets the Gap character.
        /// </summary>
        byte Gap { get; }

        /// <summary>
        /// Gets the Termination character.
        /// </summary>
        byte Ter { get; }
    }
}
