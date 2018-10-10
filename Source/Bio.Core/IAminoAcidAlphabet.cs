using System.Collections.Generic;

namespace Bio
{
    /// <summary>
    ///     An alphabet that exposes symbols for all the unambiguous amino acids, plus members
    ///     of <see cref="IAlphabet"/> that are relevant to amino acid sequences. <br/>
    ///     Specifically, contains A, C, D, E, F, G, H, I, K, L, M, N, O, P, Q, R, S, T, U, V,
    ///     W, and Y (where O is pyrrolysine and U is selenocysteine).
    /// </summary>
    /// <remarks>
    ///     Symbols for ambiguous amino acids, terminations or gaps are not included in this alphabet.
    /// </remarks>
    public interface IAminoAcidAlphabet : IEnumerable<byte>
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
        /// Gets the symbol for the amino acid Threonine.
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
        ///     Gets the count of symbols present in this alphabet.
        /// </summary>
        /// <remarks>
        ///     This includes basic symbols and any gaps, terminations and
        ///     ambiguous symbols present in the alphabet.
        /// </remarks>
        int Count { get; }

        /// <summary>
        ///     Gets a human readable name for the alphabet. 
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Provides array like access to the symbols in this alphabet.
        /// </summary>
        /// <param name="index">Index of symbol present in this alphabet.</param>
        /// <returns>A byte which represents the symbol.</returns>
        byte this[int index] { get; }

        /// <summary>
        ///     Compares two items and specifies whether they are same or not.
        /// </summary>
        /// <param name="x">First symbol to compare.</param>
        /// <param name="y">Second symbol to compare.</param>
        /// <returns>Returns <c>true</c> if x equals y.</returns>
        /// <remarks>
        ///     If the any of the bytes (Nucleotides/Amino Acids) passed does not belong to
        ///     this alphabet then this method throws an exception.
        ///     <para>
        ///         Intended to address scenarios like, N != N, M != A etc.
        ///         For scenarios like A == a, g == G use the IsValidSymbol method.
        ///     </para>
        /// </remarks>
        bool CompareSymbols(byte x, byte y);

        /// <summary>
        ///     Gets the friendly name of a given symbol.
        /// </summary>
        /// <param name="item">Symbol to find friendly name.</param>
        /// <returns>Friendly name of the given symbol.</returns>
        string GetFriendlyName(byte item);

        /// <summary>
        ///     Maps A to A and a to A, etc., so that the internal dictionary key will contain unique values.
        /// </summary>
        /// <remarks>
        ///     This is used in the IsValidSymbol method to address Scenarios like a == A, G == g etc.
        /// </remarks>
        byte[] GetSymbolValueMap();

        /// <summary>
        ///     Gets the symbols that are valid for this alphabet.
        /// </summary>
        /// <remarks>
        ///     This method can be used for better performance where lot of  validation
        ///     happens like in case of Parsers.
        /// </remarks>
        HashSet<byte> GetValidSymbols();

        /// <summary>
        ///     Returns <c>true</c> if all symbols are contained in the alphabet.
        /// </summary>
        /// <param name="symbols">The symbols to be validated.</param>
        /// <returns>Returns <c>true</c> if the validation succeeds, else <c>false</c>.</returns>
        bool ValidateSequence(byte[] symbols);

        /// <summary>
        ///     Returns <c>true</c> if all symbols are contained in the alphabet.
        /// </summary>
        /// <param name="symbols">The symbols to be validated.</param>
        /// <param name="offset">The offset at which to start validation.</param>
        /// <param name="length">The number of symbols to validate from the specified offset.</param>
        /// <returns>
        ///  	<see cref="bool"/>: 
        ///  	<c>true</c> if all symbols are contained in the alphabet, <c>false</c> otherwise.
        /// </returns>
        bool ValidateSequence(byte[] symbols, long offset, long length);
    }
}
