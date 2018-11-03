using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bio.Algorithms.ByteArraySearch
{
    /// <summary>
    ///     Implements the IBytePatternFinder interface, using the Boyer-Moore algorithm to search
    ///     for a pattern in byte arrays.
    /// </summary>
    /// <seealso cref="IBytePatternFinder" />
    public sealed class BoyerMoore : IBytePatternFinder
    {
        #region Private Fields

        private static long _patternLength;
        private static long _patternLengthMinusOne;
        private long[] _charTable;
        private long[] _offsetTable;

        #endregion  Private Fields

        /// <summary>
        /// 	Initializes a new instance of the <see cref="BoyerMoore"/> class.
        /// </summary>
        /// <remarks>
        ///     The pattern to be searched for must be set using the <see cref="SetPattern"/> method.
        /// </remarks>
        public BoyerMoore()
        {
        }

        /// <summary>
        /// 	Initializes a new instance of the <see cref="BoyerMoore"/> class with the pattern
        ///     to be found in input byte arrays.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        public BoyerMoore(byte[] pattern)
        {
            SetPattern(pattern);
        }

        /// <inheritdoc />
        public byte[] Pattern { get; private set; }

        /// <inheritdoc />
        public void SetPattern(byte[] pattern)
        {
            Pattern = pattern;
            _patternLength = Pattern.Length;
            _patternLengthMinusOne = _patternLength - 1;
            _charTable = MakeByteTable(pattern);
            _offsetTable = MakeOffsetTable(pattern);
        }

        /// <inheritdoc />
        public bool IsFoundIn(byte[] searchArray, long offset = 0, long length = -1)
            => (Search(searchArray, offset, length) >= 0);

        /// <inheritdoc />
        public IReadOnlyList<bool> IsFoundIn(IEnumerable<byte[]> searchArrays)
        {
            // Create tasks
            IList<Task<bool>> tasks = searchArrays.Select(
                searchArray => Task<bool>.Factory.StartNew(t => IsFoundIn(searchArray),
                    TaskCreationOptions.None)).ToList();

            // Wait for all the tasks
            Task.WaitAll(tasks.ToArray());

            var results = new List<bool>();
            foreach (Task<bool> task in tasks)
            {
                results.Add(task.Result);
            }

            return results;
        }

        /// <inheritdoc />
        public bool IsFoundInAll(IEnumerable<byte[]> searchArrays)
        {
            // Create cancellation token.
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            // Create tasks using cancellation token.
            List<Task<bool>> tasks = searchArrays.Select(
                searchArray => Task<bool>.Factory.StartNew(t => IsFoundIn(searchArray),
                    cancellationToken)).ToList();

            int completed = 0;
            int taskCount = tasks.Count;

            // Wait for the tasks to finish in turn.
            while (completed < taskCount)
            {
                int taskIndex = Task.WaitAny(tasks.ToArray(), cancellationToken);
                completed++;
                Debug.Print($"Task {taskIndex} completed: {completed} searches finished\n");

                // If the search didn't find the pattern, cancel the rest of the tasks.
                if (!tasks[taskIndex].Result)
                {
                    Debug.Print($"Found in task {taskIndex}\n");
                    cancellationTokenSource.Cancel();
                    return false;
                }

                tasks.RemoveAt(taskIndex);
            }

            return true;
        }

        /// <inheritdoc />
        public bool IsFoundInAny(IEnumerable<byte[]> searchArrays)
        {
            // Create cancellation token.
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            // Create tasks using cancellation token.
            List<Task<bool>> tasks = searchArrays.Select(
                searchArray => Task<bool>.Factory.StartNew(t => IsFoundIn(searchArray),
                    cancellationToken)).ToList();

            int completed = 0;
            int taskCount = tasks.Count;

            // Wait for the tasks to finish in turn.
            while (completed < taskCount)
            {
                int taskIndex = Task.WaitAny(tasks.ToArray(), cancellationToken);
                completed++;
                Debug.Print($"Task {taskIndex} completed: {completed} searches finished\n");

                // If the search found the pattern, cancel the rest of the tasks.
                if (tasks[taskIndex].Result)
                {
                    Debug.Print($"Found in task {taskIndex}\n");
                    cancellationTokenSource.Cancel();
                    return true;
                }

                tasks.RemoveAt(taskIndex);
            }

            return false;
        }

        /// <inheritdoc />
        public long Search(byte[] searchArray, long offset = 0, long length = -1)
        {
            if (_patternLength == 0)
                throw new Exception("A valid pattern is required");

            if (searchArray == null)
                throw new ArgumentNullException(nameof(searchArray));

            if (length == -1 || length > searchArray.LongLength)
                length = searchArray.LongLength;

            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));

            if ((offset < 0) || (offset >= length))
                throw new ArgumentOutOfRangeException(nameof(offset));

            for (long i = offset + _patternLengthMinusOne; i < length;)
            {
                long j;

                for (j = _patternLengthMinusOne; Pattern[j] == searchArray[i]; --i, --j)
                {
                    if (j != 0)
                        continue;

                    return i;
                }

                i += Math.Max(_offsetTable[_patternLengthMinusOne - j], _charTable[searchArray[i]]);
            }

            return -1;
        }

        /// <inheritdoc />
        public IReadOnlyList<long> Search(IEnumerable<byte[]> searchArrays)
        {
            // Create tasks
            IList<Task<long>> tasks = searchArrays.Select(
                searchArray => Task<long>.Factory.StartNew(t => Search(searchArray),
                    TaskCreationOptions.None)).ToList();

            // Wait for all the task
            Task.WaitAll(tasks.ToArray());

            var results = new List<long>();
            foreach (Task<long> task in tasks)
            {
                results.Add(task.Result);
            }

            return results;
        }

        /// <inheritdoc />
        public IReadOnlyList<long> SearchForAll(byte[] searchArray, long offset = 0, long length = -1)
        {
            if (_patternLength == 0)
                throw new Exception("A valid pattern is required");

            if (searchArray == null)
                throw new ArgumentNullException(nameof(searchArray));

            if (length == -1 || length > searchArray.LongLength)
                length = searchArray.LongLength;

            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));

            if ((offset < 0) || (offset >= length))
                throw new ArgumentOutOfRangeException(nameof(offset));

            var results = new List<long>();

            for (long i = offset + _patternLengthMinusOne; i < length;)
            {
                long j;

                for (j = _patternLengthMinusOne; Pattern[j] == searchArray[i]; --i, --j)
                {
                    if (j != 0)
                        continue;

                    results.Add(i);
                    break;
                }

                i += Math.Max(_offsetTable[_patternLengthMinusOne - j], _charTable[searchArray[i]]);
            }

            if (results.Count == 0)
                results.Add(-1);

            return results;
        }

        /// <inheritdoc />
        public long SearchForOccurrence(byte[] searchArray, long occurrence, long offset = 0, long length = -1)
        {
            if (occurrence < 1)
                throw new ArgumentOutOfRangeException(nameof(occurrence));

            long index = offset;
            long count = 0;

            do
            {
                index = Search(searchArray, index, length);

                if (index == -1)
                    return -1;

                count++;
                index++;
            } while (count < occurrence);

            return index - 1;
        }

        static long[] MakeByteTable(byte[] pattern)
        {
            const int alphabetSize = 256;
            long[] table = new long[alphabetSize];

            for (long i = 0; i < alphabetSize; ++i)
                table[i] = _patternLength;

            for (long i = 0; i < _patternLengthMinusOne; ++i)
                table[pattern[i]] = _patternLengthMinusOne - i;

            return table;
        }

        static long[] MakeOffsetTable(byte[] pattern)
        {
            long[] table = new long[_patternLength];
            long lastPrefixPosition = _patternLength;

            for (long i = _patternLengthMinusOne; i >= 0; --i)
            {
                if (IsPrefix(pattern, i + 1))
                    lastPrefixPosition = i + 1;

                table[_patternLengthMinusOne - i] = lastPrefixPosition - i + _patternLengthMinusOne;
            }

            for (long i = 0; i < _patternLengthMinusOne; ++i)
            {
                long suffixLength = SuffixLength(pattern, i);
                table[suffixLength] = _patternLengthMinusOne - i + suffixLength;
            }

            return table;
        }

        static bool IsPrefix(byte[] pattern, long p)
        {
            for (long i = p, j = 0; i < _patternLength; ++i, ++j)
                if (pattern[i] != pattern[j])
                    return false;

            return true;
        }

        static long SuffixLength(byte[] pattern, long p)
        {
            long len = 0;

            for (long i = p, j = _patternLengthMinusOne; i >= 0 && pattern[i] == pattern[j]; --i, --j)
                ++len;

            return len;
        }
    }
}
