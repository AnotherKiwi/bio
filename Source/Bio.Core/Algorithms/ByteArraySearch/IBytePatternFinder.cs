using System;
using System.Collections.Generic;

namespace Bio.Algorithms.ByteArraySearch
{
    /// <summary>
    ///     Interface to be exposed by classes that implement a pattern finding algorithm for searching byte arrays.
    /// </summary>
    interface IBytePatternFinder
    {
        /// <summary>
        /// 	Determines if the current search pattern is matched in the given byte array.
        /// </summary>
        /// <param name="searchArray">The byte array to search for the pattern.</param>
        /// <param name="offset">The offset at which to start the search.</param>
        /// <param name="length">
        ///     The maximum number of bytes of the input array to be searched. If set to -1, the
        ///     whole of the byte array is searched.
        /// </param>
        /// <returns>
        ///  	<see cref="bool"/>: <c>true</c> if the current search pattern was matched in the input
        ///     byte array; <c>false</c> otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException">searchArray</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// length
        /// or
        /// offset
        /// </exception>
        /// <remarks>
        ///     If the input byte array has multiple padding 0 values, the search may be limited
        ///     to the portion of the array containing data by setting the <paramref name="length"/>
        ///     to the appropriate value.
        /// </remarks>
        bool IsFoundIn(byte[] searchArray, long offset = 0, long length = -1);

        /// <summary>
        /// 	Determines if the current search pattern is matched in each of the given byte arrays.
        ///     The returned list contains the results of each search in the same order as the input
        ///     collection of byte arrays.
        /// </summary>
        /// <param name="searchArrays">The search arrays.</param>
        /// <returns>
        ///  	<see cref="IReadOnlyList{T}"/>: list of the results in the same order as the input arrays.
        /// </returns>
        /// <remarks>
        ///     The whole of each input byte array is searched.
        /// </remarks>
        IReadOnlyList<bool> IsFoundIn(IEnumerable<byte[]> searchArrays);

        /// <summary>
        /// 	Determines if the current search pattern is matched in all of the given byte arrays.
        /// </summary>
        /// <param name="searchArrays">The search arrays.</param>
        /// <returns>
        ///  	<see cref="bool"/>: <c>true</c> if the search pattern is matched in all of the input
        ///     byte arrays; <c>false</c> otherwise.
        /// </returns>
        /// <remarks>
        ///     The whole of each input byte array is searched.
        /// </remarks>
        bool IsFoundInAll(IEnumerable<byte[]> searchArrays);

        /// <summary>
        /// 	Determines if the current search pattern is matched in any of the given byte arrays.
        /// </summary>
        /// <param name="searchArrays">The search arrays.</param>
        /// <returns>
        ///  	<see cref="bool"/>: <c>true</c> if the search pattern is matched in at least one of the input
        ///     byte arrays; <c>false</c> otherwise.
        /// </returns>
        /// <remarks>
        ///     The whole of each input byte array is searched.
        /// </remarks>
        bool IsFoundInAny(IEnumerable<byte[]> searchArrays);

        /// <summary>
        /// 	Gets the pattern to use in searches of byte arrays.
        /// </summary>
        /// <value>
        ///  	<see cref="byte[]"/>: the pattern to use in searches of byte arrays.
        /// </value>
        byte[] Pattern { get; }

        /// <summary>
        /// 	Searches the specified byte array for the first occurrence of the current search pattern.
        /// </summary>
        /// <param name="searchArray">The byte array to search for the pattern.</param>
        /// <param name="offset">The offset at which to start the search.</param>
        /// <param name="length">
        ///     The maximum number of bytes of the input array to be searched. If set to -1, the
        ///     whole of the byte array is searched.
        /// </param>
        /// <returns>
        ///  	<see cref="long"/>: the index of the first occurrence of the current search pattern, if
        ///     found; -1 otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException">searchArray</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// length
        /// or
        /// offset
        /// </exception>
        /// <remarks>
        ///     Returns -1 if the search pattern was not found. <br/>
        ///     If the input byte array has multiple padding 0 values, the search may be limited
        ///     to the portion of the array containing data by setting the <paramref name="length"/>
        ///     to the appropriate value.
        /// </remarks>
        long Search(byte[] searchArray, long offset = 0, long length = -1);

        /// <summary>
        /// 	Searches the specified byte arrays for the current pattern, and returns a list of the
        ///     results of each search in the same order as the input arrays.
        /// </summary>
        /// <param name="searchArrays">The search arrays.</param>
        /// <returns>
        ///  	<see cref="IReadOnlyList{T}"/>: list of the results in the same order as the input arrays.
        /// </returns>
        /// <remarks>
        ///     The whole of each input byte array is searched, and only the index of the first occurrence of
        ///     the pattern is returned.
        /// </remarks>
        IReadOnlyList<long> Search(IEnumerable<byte[]> searchArrays);

        /// <summary>
        /// 	Searches for all matches of the current search pattern in the given byte array.
        /// </summary>
        /// <param name="searchArray">The byte array to search for the pattern.</param>
        /// <param name="offset">The offset at which to start the search.</param>
        /// <param name="length">
        ///     The maximum number of bytes of the input array to be searched. If set to -1, the
        ///     whole of the byte array is searched.
        /// </param>
        /// <returns>
        ///  	<see cref="IReadOnlyList{T}"/>: collection of the indexes at which the
        ///     search pattern was found, or -1 if no match was found.
        /// </returns>
        /// <remarks>
        ///     Returns {-1} if the search pattern was not found.
        /// </remarks>
        IReadOnlyList<long> SearchForAll(byte[] searchArray, long offset = 0, long length = -1);

        /// <summary>
        /// 	Searches for the specified occurrence of the current search pattern in the given byte array.
        /// </summary>
        /// <param name="searchArray">The byte array to search for the pattern.</param>
        /// <param name="occurrence">The occurrence to find.</param>
        /// <param name="offset">The offset at which to start the search.</param>
        /// <param name="length">
        ///     The maximum number of bytes of the input array to be searched. If set to -1, the
        ///     whole of the byte array is searched.
        /// </param>
        /// <returns>
        ///  	<see cref="long"/>: the index of the specified occurrence of the current search pattern, if
        ///     found; -1 otherwise.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">occurrence</exception>
        /// <remarks>
        ///     Returns -1 if the search pattern was not found.
        /// </remarks>
        long SearchForOccurrence(byte[] searchArray, long occurrence, long offset = 0, long length = -1);

        /// <summary>
        /// 	Sets the search pattern for this instance of the <see cref="BoyerMoore"/> search class.
        /// </summary>
        /// <param name="pattern">The pattern to find in input byte arrays.</param>
        void SetPattern(byte[] pattern);

    }
}
