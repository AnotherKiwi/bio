using System;
using System.Collections.Generic;

namespace Bio
{
    /// <summary>
    ///     A case sensitive alphabet that is the base alphabet for all Protein and Peptide alphabets.
    ///     It extends <see cref="IAlphabet"/> by adding symbols for standard (unambiguous) amino acids. 
    /// </summary>
    /// <remarks>
    ///     Specifically, it adds: A, C, D, E, F, G, H, I, K, L, M, N, O, P, Q, R, S, T, U, V, W, and Y
    ///     (where O is pyrrolysine and U is selenocysteine).<br/>
    ///     Symbols for ambiguous amino acids, terminations or gaps are not included in this alphabet.
    /// </remarks>

    public interface IAminoAcidsAlphabet : IAlphabet
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
        /// 	Checks if the given symbol is an unambiguous amino acid.
        /// </summary>
        /// <param name="symbol">The symbol to check.</param>
        /// <returns>
        ///  	<see cref="bool"/>: <c>true</c> if the given symbol is an unambiguous amino acid,
        ///     <c>false</c> otherwise.
        /// </returns>
        bool CheckIsUnambiguousAminoAcid(byte symbol);

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
        ///     Gets the amino acid's symbol from its three-letter abbreviation.
        /// </summary>
        /// <param name="item">Three-letter abbreviation of the amino acid.</param>
        /// <returns>
        ///     Amino acid symbol corresponding to its three-letter abbreviation.
        /// </returns>
        /// <remarks>
        ///     Returns 0 if the supplied string is not a recognised three-letter
        ///     abbreviation for an amino acid.
        /// </remarks>
        byte GetSymbolFromThreeLetterAbbrev(string item);

        /// <summary>
        ///     Gets the three-letter abbreviation of the amino acid represented
        ///     by the given symbol.
        /// </summary>
        /// <param name="item">Symbol of the amino acid.</param>
        /// <returns>
        ///     Amino acid three-letter abbreviation corresponding to the given symbol.
        /// </returns>
        /// <remarks>
        ///     Returns an empty string if the supplied symbol is not a recognised
        ///     amino acid symbol.
        /// </remarks>
        string GetThreeLetterAbbreviation(byte item);

        /// <summary>
        /// 	Gets the set of unambiguous amino acids.
        /// </summary>
        /// <returns>
        ///  	<see cref="HashSet{T}"/>: the set of unambiguous amino acids.
        /// </returns>
        HashSet<byte> GetUnambiguousAminoAcids();
    }
}
