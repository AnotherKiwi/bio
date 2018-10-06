using System;

namespace Bio
{
    /// <summary>
    /// An alphabet that extends <see cref="IAlphabet"/> by adding symbols for unambiguous amino acids.
    /// </summary>
    /// <remarks>
    /// Specifically, adds A, C, D, E, F, G, H, I, K, L, M, N, O, P, Q, R, S, T, U, V, W, and Y
    /// (where O is pyrrolysine and U is selenocysteine).<br/>
    /// Symbols for ambiguous amino acids, terminations or gaps are not included in this alphabet.
    /// </remarks>

    public interface IBasicProteinAlphabet : IAlphabet
    {

        /// <summary>
        /// Gets the symbol for the amino acid Alanine. 
        /// </summary>
        byte A { get; }

        /// <summary>
        /// Gets the symbol for the amino acid Cysteine.
        /// </summary>
        byte C { get; }

        /// <summary>
        /// Gets the symbol for the amino acid Aspartic Acid.
        /// </summary>
        byte D { get; }

        /// <summary>
        /// Gets the symbol for the amino acid Glutamic Acid.
        /// </summary>
        byte E { get; }

        /// <summary>
        /// Gets the symbol for the amino acid Phenylalanine. 
        /// </summary>
        byte F { get; }

        /// <summary>
        /// Gets the symbol for the amino acid Glycine.
        /// </summary>
        byte G { get; }

        /// <summary>
        /// Gets the symbol for the amino acid Histidine.
        /// </summary>
        byte H { get; }

        /// <summary>
        /// Gets the symbol for the amino acid Isoleucine.
        /// </summary>
        byte I { get; }

        /// <summary>
        /// Gets the symbol for the amino acid Lysine.
        /// </summary>
        byte K { get; }

        /// <summary>
        /// Gets the symbol for the amino acid Leucine.
        /// </summary>
        byte L { get; }

        /// <summary>
        /// Gets the symbol for the amino acid Methionine.
        /// </summary>
        byte M { get; }

        /// <summary>
        /// Gets the symbol for the amino acid Asparagine.
        /// </summary>
        byte N { get; }

        /// <summary>
        /// Gets the symbol for the amino acid Pyrrolysine.
        /// </summary>
        byte O { get; }

        /// <summary>
        /// Gets the symbol for the amino acid Proline.
        /// </summary>
        byte P { get; }

        /// <summary>
        /// Gets the symbol for the amino acid Glutamine.
        /// </summary>
        byte Q { get; }

        /// <summary>
        /// Gets the symbol for the amino acid Arginine.
        /// </summary>
        byte R { get; }

        /// <summary>
        /// Gets the symbol for the amino acid Serine.
        /// </summary>
        byte S { get; }

        /// <summary>
        /// Gets the symbol for the amino acid Threoine.
        /// </summary>
        byte T { get; }

        /// <summary>
        /// Gets the symbol for the amino acid Selenocysteine.
        /// </summary>
        byte U { get; }

        /// <summary>
        /// Gets the symbol for the amino acid Valine.
        /// </summary>
        byte V { get; }

        /// <summary>
        /// Gets the symbol for the amino acid Tryptophan.
        /// </summary>
        byte W { get; }

        /// <summary>
        /// Gets the symbol for the amino acid Tyrosine.
        /// </summary>
        byte Y { get; }

        /// <summary>
        /// 	Gets the invalid symbols in the specified sequence, in ascending order.
        /// </summary>
        /// <param name="sequence">The sequence to check for invalid symbols.</param>
        /// <returns>
        ///  	<see cref="T:byte[]"/>: the invalid symbols in the specified sequence.
        /// </returns>
        /// <exception cref="ArgumentNullException">sequence</exception>
        byte[] GetInvalidSymbols(ISequence sequence);

        /// <summary>
        /// Gets the symbol from a three-letter abbreviation.
        /// </summary>
        /// <param name="item">Three-letter abbreviation to find symbol.</param>
        /// <returns>Symbol corresponding to three-letter abbreviation.</returns>
        byte GetSymbolFromThreeLetterAbbrev(string item);

        /// <summary>
        ///     Gets the three-letter abbreviation of a given symbol.
        /// </summary>
        /// <param name="item">Symbol to find three-letter abbreviation.</param>
        /// <returns>Three-letter abbreviation of the given symbol.</returns>
        string GetThreeLetterAbbreviation(byte item);
    }
}
