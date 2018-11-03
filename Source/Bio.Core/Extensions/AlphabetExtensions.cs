using System;
using System.Collections.Generic;
using System.Linq;
using static Bio.Alphabets;

namespace Bio.Extensions
{
    /// <summary>
    /// Alphabet extensions used to supplement the IAlphabet interface without
    /// requiring an implementation by the class.
    /// </summary>
    public static class AlphabetExtensions
    {
        /// <summary>
        /// This returns true/false if the given symbol value is considered a termination
        /// value in the alphabet.
        /// </summary>
        /// <param name="alphabet">Alphabet to test</param>
        /// <param name="value">Value to check</param>
        /// <returns>True if value is a termination symbol, false if terminations are not supported or value is not.</returns>
        public static bool CheckIsTermination(this IAlphabet alphabet, byte value)
        {
            if (alphabet == null)
                throw new ArgumentNullException(nameof(alphabet));

            // Not supported?
            if (!alphabet.HasTerminations)
                return false;

            // Get the termination set and return true/false on match.
            HashSet<byte> symbols;
            return (alphabet.TryGetTerminationSymbols(out symbols)
                    && symbols.Contains(value));
        }

        /// <summary>
        /// This returns true/false if the given symbol value is considered a termination
        /// value in the alphabet.
        /// </summary>
        /// <param name="alphabet">Alphabet to test</param>
        /// <param name="value">Value to check</param>
        /// <returns>True if value is a termination symbol, false if terminations are not supported or value is not.</returns>
        public static bool CheckIsTermination(this IAlphabet alphabet, char value)
        {
            return CheckIsTermination(alphabet, (byte) value);
        }

        /// <summary>
        /// Checks if the provided item is a gap character or not
        /// </summary>
        /// <param name="alphabet">Alphabet to test against.</param>
        /// <param name="item">Item to be checked</param>
        /// <returns>True if the specified item is a gap</returns>
        public static bool CheckIsGap(this IAlphabet alphabet, char item)
        {
            if (alphabet == null)
                throw new ArgumentNullException(nameof(alphabet));
            
            return alphabet.CheckIsGap((byte) item);
        }

        /// <summary>
        /// Checks if the provided item is an ambiguous character or not
        /// </summary>
        /// <param name="alphabet">Alphabet to test against.</param>
        /// <param name="item">Item to be checked</param>
        /// <returns>True if the specified item is a ambiguous</returns>
        public static bool CheckIsAmbiguous(this IAlphabet alphabet, char item)
        {
            if (alphabet == null)
                throw new ArgumentNullException(nameof(alphabet));

            return alphabet.CheckIsAmbiguous((byte)item);
        }

        /// <summary>
        /// 	Compares the symbols in the two alphabets and returns the
        ///     appropriate <see cref="Alphabets.ComparisonResult"/> value.
        /// </summary>
        /// <param name="alphabet">The base alphabet.</param>
        /// <param name="comparedAlphabet">The compared alphabet.</param>
        /// <returns>
        ///  	<see cref="Alphabets.ComparisonResult"/>:
        ///     a value indicating the relationship of the compared alphabet's symbols
        ///     to those of the first.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// alphabet
        /// or
        /// comparedAlphabet
        /// </exception>
        public static ComparisonResult CompareTo(this IAlphabet alphabet, IAlphabet comparedAlphabet)
        {
            if (alphabet == null)
            {
                throw new ArgumentNullException(nameof(alphabet));
            }

            if (comparedAlphabet == null)
            {
                throw new ArgumentNullException(nameof(comparedAlphabet));
            }

            HashSet<byte> symbols = alphabet.GetValidSymbols();
            HashSet<byte> comparedSymbols = comparedAlphabet.GetValidSymbols();

            if (symbols.SetEquals(comparedSymbols))
            {
                return ComparisonResult.Identical;
            }

            if (symbols.IsProperSupersetOf(comparedSymbols))
            {
                return ComparisonResult.Superset;
            }

            if (symbols.IsProperSubsetOf(comparedSymbols))
            {
                return ComparisonResult.Subset;
            }

            if (symbols.Overlaps(comparedSymbols))
            {
                return ComparisonResult.Intersects;
            }

            return ComparisonResult.NoOverlap;
        }

        /// <summary>
        /// Gets the friendly name of a given symbol.
        /// </summary>
        /// <param name="alphabet"> </param>
        /// <param name="item">Symbol to find friendly name.</param>
        /// <returns>Friendly name of the given symbol.</returns>
        public static string GetFriendlyName(this IAlphabet alphabet, char item)
        {
            if (alphabet == null)
                throw new ArgumentNullException(nameof(alphabet));

            return alphabet.GetFriendlyName((byte)item);
        }

        /// <summary>
        /// 	Determines whether the alphabet contains symbols for Amino Acids.
        /// </summary>
        /// <param name="alphabet">The alphabet.</param>
        /// <returns>
        ///  	<see cref="bool"/>:
        ///  	<c>true</c> if the alphabet contains symbols for Amino Acids; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasAminoAcidSymbols(this IAlphabet alphabet) =>
            ((AlphabetTypes.AllAminoAcids | AlphabetTypes.PlugIn) & alphabet.AlphabetType) ==
            alphabet.AlphabetType;

        /// <summary>
        /// 	Determines whether the alphabet contains symbols for DNA.
        /// </summary>
        /// <param name="alphabet">The alphabet.</param>
        /// <returns>
        ///  	<see cref="bool"/>:
        ///  	<c>true</c> if the alphabet contains symbols for DNA; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasDnaSymbols(this IAlphabet alphabet) =>
            ((AlphabetTypes.AllDNA | AlphabetTypes.PlugIn) & alphabet.AlphabetType) ==
            alphabet.AlphabetType;

        /// <summary>
        /// 	Determines whether the alphabet contains symbols for RNA.
        /// </summary>
        /// <param name="alphabet">The alphabet.</param>
        /// <returns>
        ///  	<see cref="bool"/>:
        ///  	<c>true</c> if the alphabet contains symbols for RNA; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasRnaSymbols(this IAlphabet alphabet) =>
            ((AlphabetTypes.AllRNA | AlphabetTypes.PlugIn) & alphabet.AlphabetType) ==
            alphabet.AlphabetType;

        /// <summary>
        /// 	Checks if a given alphabet is a subset of another.
        /// </summary>
        /// <param name="subsetAlphabet">The subset alphabet.</param>
        /// <param name="supersetAlphabet">The potential superset alphabet.</param>
        /// <returns>
        ///  	<see cref="bool"/>: 
        ///  	<c>true</c> if the first alphabet is a subset of the second, <c>false</c> otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     subsetAlphabet - A subset alphabet must be supplied.
        ///     or
        ///     supersetAlphabet - A potential superset alphabet must be supplied.
        /// </exception>
        /// <remarks>
        ///     Returns <c>false</c> if the supplied <paramref name="supersetAlphabet"/> is <c>null</c>,
        ///     since an alphabet cannot be a subset of an empty alphabet.
        /// </remarks>
        public static bool IsSubsetOf(this IAlphabet subsetAlphabet, IAlphabet supersetAlphabet)
        {
            if (subsetAlphabet == null)
                throw new ArgumentNullException(nameof(subsetAlphabet));

            if (supersetAlphabet == null)
                return false;

            if (!SubsetAlphabetsMap.ContainsKey(supersetAlphabet))
                throw new ArgumentOutOfRangeException(nameof(supersetAlphabet), @"Unsupported alphabet encountered.");

            return SubsetAlphabetsMap[supersetAlphabet].Contains(subsetAlphabet);
        }

        /// <summary>
        /// 	Checks if a given alphabet is a superset of another. 
        /// </summary>
        /// <param name="supersetAlphabet">The superset alphabet.</param>
        /// <param name="subsetAlphabet">The potential subset alphabet.</param>
        /// <returns>
        ///  	<see cref="bool"/>: 
        ///  	<c>true</c> if the first alphabet is a superset of the second, <c>false</c> otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     subsetAlphabet - A superset alphabet must be supplied.
        ///     or
        ///     supersetAlphabet - A potential subset alphabet must be supplied.
        /// </exception>
        /// <remarks>
        ///     Returns <c>true</c> if the supplied <paramref name="subsetAlphabet"/> is <c>null</c>,
        ///     since any alphabet is a superset of an empty alphabet.
        /// </remarks>
        public static bool IsSupersetOf(this IAlphabet supersetAlphabet, IAlphabet subsetAlphabet)
        {
            if (supersetAlphabet == null)
                throw new ArgumentNullException(nameof(supersetAlphabet));

            if (subsetAlphabet == null)
                return true;

            if (!SupersetAlphabetsMap.ContainsKey(subsetAlphabet))
                throw new ArgumentOutOfRangeException(nameof(subsetAlphabet), @"Unsupported alphabet encountered.");

            return SupersetAlphabetsMap[subsetAlphabet].Contains(supersetAlphabet);
        }

        /// <summary>
        /// 	Gets a list of the subset alphabets of the given alphabet.
        /// </summary>
        /// <param name="alphabet">The alphabet for which subsets are required.</param>
        /// <returns>
        ///  	<see cref="IReadOnlyList{IAlphabet}" />:
        ///     Returns a list of the subset alphabets of the given alphabet.
        /// </returns>
        /// <exception cref="ArgumentNullException">alphabet - Alphabet must be supplied.</exception>
        /// <exception cref="ArgumentOutOfRangeException">alphabet - Unsupported alphabet encountered.</exception>
        public static IReadOnlyList<IAlphabet> GetSubsets(this IAlphabet alphabet)
        {
            if (alphabet == null)
                throw new ArgumentNullException(nameof(alphabet));

            if (!SubsetAlphabetsMap.ContainsKey(alphabet))
                throw new ArgumentOutOfRangeException(nameof(alphabet), @"Unsupported alphabet encountered.");

            return SubsetAlphabetsMap[alphabet];
        }

        /// <summary>
        /// 	Gets a list of the superset alphabets of the given alphabet.
        /// </summary>
        /// <param name="alphabet">The alphabet for which superset are required.</param>
        /// <returns>
        ///  	<see cref="IReadOnlyList{IAlphabet}" />:
        ///     Returns a list of the superset alphabets of the given alphabet.
        /// </returns>
        /// <exception cref="ArgumentNullException">alphabet - Alphabet must be supplied.</exception>
        /// <exception cref="ArgumentOutOfRangeException">alphabet - Unsupported alphabet encountered.</exception>
        public static IReadOnlyList<IAlphabet> GetSupersets(this IAlphabet alphabet)
        {
            if (alphabet == null)
                throw new ArgumentNullException(nameof(alphabet));

            if (!SupersetAlphabetsMap.ContainsKey(alphabet))
                throw new ArgumentOutOfRangeException(nameof(alphabet), @"Unsupported alphabet encountered.");

            return SupersetAlphabetsMap[alphabet];
        }
    }
}
