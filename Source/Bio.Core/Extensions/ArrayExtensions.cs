using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bio.Algorithms.ByteArraySearch;

namespace Bio.Core.Extensions
{
    /// <summary>
    /// Extension methods for arrays
    /// </summary>
    public static class ArrayExtensions
    {
        private static bool initialized;
        private static PropertyInfo longLengthProperty;
        private static MethodInfo longCopyMethod;

        /// <summary>
        /// This method provides access to the LongLength property in a 
        /// portable fashion by looking it up for the platform using reflection.
        /// The PropertyInfo is cached off for performance.
        /// </summary>
        /// <param name="data">Array</param>
        /// <returns>64-bit length</returns>
        public static long GetLongLength(this Array data)
        {
            Initialize();

            return (longLengthProperty != null)
                ? (long)longLengthProperty.GetValue(data)
                    : data.Length;
        }

        /// <summary>
        /// This method performs a 64-bit array copy if the platform supports it.
        /// </summary>
        /// <param name="source">Source array</param>
        /// <param name="startSource">Starting position</param>
        /// <param name="dest">Destination array</param>
        /// <param name="startDest">Starting position</param>
        /// <param name="count">Count</param>
        public static void LongCopy(Array source, long startSource,
            Array dest, long startDest, long count)
        {
            Initialize();

            if (longCopyMethod != null)
            {
                longCopyMethod.Invoke(null, new object[] { source, startSource, dest, startDest, count });
            }
            else
            {
                Array.Copy(source, (int)startSource, dest, (int)startDest, (int)count);
            }
        }

        /// <summary>
        /// Runs each item through a conversion and returns the produced array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="input"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static TOutput[] ConvertAll<T, TOutput>(this T[] input, Func<T, TOutput> converter)
        {
            if (converter == null)
                throw new ArgumentNullException("converter cannot be null.");

            TOutput[] output = new TOutput[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                output[i] = converter(input[i]);
            }

            return output;
        }

        /// <summary>
        /// Method to locate our cached reflection data.
        /// </summary>
        private static void Initialize()
        {
            if (!initialized)
            {
                Type t = typeof(Array);
                longLengthProperty = t.GetRuntimeProperties().FirstOrDefault(pi => pi.Name == "LongLength");
                longCopyMethod =
                    t.GetRuntimeMethods().FirstOrDefault(mi =>
                        mi.Name == "Copy" && mi.GetParameters().Length == 5
                        && mi.GetParameters().Last().ParameterType == typeof(long));

                initialized = true;
            }
        }

        /// <summary>
        /// Returns a new array with the specified range of values.
        /// </summary>
        /// <typeparam name="T">Array type.</typeparam>
        /// <param name="data">Source data.</param>
        /// <param name="startIndex">Index to begind sub array at.</param>
        /// <param name="length">Length of sub array.</param>
        /// <returns></returns>
        public static T[] GetRange<T>(this T[] data, int startIndex, int length)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            T[] result = new T[length];

            int index = 0;
            for (int i = startIndex; i < startIndex + length; i++)
            {
                result[index++] = data[i];
            }

            return result;
        }

        /// <summary>
        ///     Returns the index of the search pattern in the sequence, if found. Otherwise returns -1.
        /// </summary>
        /// <param name="sequence">The byte array of symbols to search for the sub-sequence.</param>
        /// <param name="pattern">The pattern to find.</param>
        /// <param name="offset">The start index for the search.</param>
        /// <param name="length">
        ///     The maximum number of bytes of the sequence to be searched. If set to -1, the
        ///     whole of the byte array is searched.
        /// </param>
        /// <returns>
        ///  	<see cref="long" />:
        ///     the zero-based index of the sub-sequence in the searched sequence, if found; -1 otherwise.
        /// </returns>
        /// <remarks>
        /// 	Uses the Boyer-Moore search algorithm. If searching for the same pattern in 
        ///     multiple sequences, the <c>Search</c> method of the <see cref="BoyerMoore"/>
        ///     class will give better performance.
        /// </remarks>
        public static long FindIndexOf(this byte[] sequence, byte[] pattern, long offset = 0, long length = -1)
        {
            return new BoyerMoore(pattern).Search(sequence, offset, length);
        }

        /// <summary>
        /// 	Searches for all matches of the current pattern in the sequence.
        /// </summary>
        /// <param name="sequence">The byte array to search for the pattern.</param>
        /// <param name="pattern">The pattern to find.</param>
        /// <param name="offset">The offset at which to start the search.</param>
        /// <param name="length">
        ///     The maximum number of bytes of the sequence to be searched. If set to -1, the
        ///     whole of the byte array is searched.
        /// </param>
        /// <returns>
        ///  	<see cref="IEnumerable{T}"/>: enumerable collection of the indexes at which the
        ///     search pattern was found, or -1 if no match was found.
        /// </returns>
        /// <remarks>
        ///     Returns {-1} if the search pattern was not found. <br/>
        /// 	Uses the Boyer-Moore search algorithm. If searching for the same pattern in 
        ///     multiple sequences, the <c>Search</c> method of the <see cref="BoyerMoore"/>
        ///     class will give better performance.
        /// </remarks>
        public static IReadOnlyList<long> FindAll(this byte[] sequence, byte[] pattern, long offset = 0, long length = -1)
        {
            return new BoyerMoore(pattern).SearchForAll(sequence, offset, length);
        }

        /// <summary>
        /// 	Determines if the search pattern is matched in the sequence.
        /// </summary>
        /// <param name="sequence">The byte array to search for the pattern.</param>
        /// <param name="pattern">The pattern to find.</param>
        /// <param name="offset">The offset at which to start the search.</param>
        /// <param name="length">
        ///     The maximum number of bytes of the sequence to be searched. If set to -1, the
        ///     whole of the byte array is searched.
        /// </param>
        /// <returns>
        ///  	<see cref="bool"/>: <c>true</c> if the current search pattern was matched in the input
        ///     byte array; <c>false</c> otherwise.
        /// </returns>
        /// <remarks>
        ///     If the input byte array has multiple padding 0 values, the search may be limited
        ///     to the portion of the array containing data by setting the <paramref name="length"/>
        ///     to the appropriate value. <br/>
        /// 	Uses the Boyer-Moore search algorithm. If searching for the same pattern in 
        ///     multiple sequences, the <c>Search</c> method of the <see cref="BoyerMoore"/>
        ///     class will give better performance.
        /// </remarks>
        public static bool Contains(this byte[] sequence, byte[] pattern, long offset = 0, long length = -1)
        {
            return (new BoyerMoore(pattern).Search(sequence, offset, length) >= 0);
        }
    }
}
