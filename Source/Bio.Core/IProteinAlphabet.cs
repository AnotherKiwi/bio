﻿namespace Bio
{
    /// <summary>
    ///     An alphabet that extends <see cref="IStrictProteinAlphabet"/> by adding termination
    ///     and gap symbols, and by removing case sensitivity of symbols.
    /// </summary>
    /// <remarks>
    /// Specifically, adds Gap (i.e. '-'), and Ter (i.e. '*', for terminations).<br/>
    /// Symbols for ambiguous amino acids are NOT included in this alphabet.
    /// </remarks>
    /// <seealso cref="IStrictProteinAlphabet" />,
    /// <seealso cref="IAlphabet"/>

    public interface IProteinAlphabet : IStrictProteinAlphabet
    {

        /// <summary>
        ///     Gets the Gap character.
        /// </summary>
        byte Gap { get; }

        /// <summary>
        ///     Gets the Termination character.
        /// </summary>
        byte Ter { get; }
    }
}
