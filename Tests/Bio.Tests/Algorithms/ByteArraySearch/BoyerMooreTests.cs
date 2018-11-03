using System;
using System.Collections.Generic;
using Bio.Algorithms.ByteArraySearch;
using NUnit.Framework;
using static Bio.Alphabets;

namespace Bio.Tests.Algorithms.ByteArraySearch
{
    /// <summary>
    ///     BoyerMoore Test cases for byte array data.
    /// </summary>
    [TestFixture]
    class BoyerMooreTests
    {
        /// <summary>
        ///     Test of Search method with a single sequence as input.
        /// </summary>
        [Test]
        public void SearchSingleSequenceTest()
        {
            byte[] sequence = new Sequence(DNA, "AGCTAGCTA").GetInternalArray();
            byte[] pattern = new Sequence(DNA, "AGCT").GetInternalArray();
            IBytePatternFinder patternFinder = new BoyerMoore();
            patternFinder.SetPattern(pattern);

            // Search whole of sequence.
            long expected = 0;
            long actual = patternFinder.Search(sequence);
            Assert.IsTrue(actual == expected);

            // Search starting at position 2.
            expected = 4;
            actual = patternFinder.Search(sequence, 1);
            Assert.IsTrue(actual == expected);

            // Search starting at position 6.
            expected = -1;
            actual = patternFinder.Search(sequence, 5);
            Assert.IsTrue(actual == expected);

            // ----------------------------------------------------------
            // Copy the sequence bytes into a larger array. 
            byte[] newSequence = new byte[100];
            Array.Copy(sequence, 0, newSequence, 0, sequence.Length);

            // Search portion of array containing the sequence.
            expected = 0;
            actual = patternFinder.Search(newSequence, 0, sequence.Length);
            Assert.IsTrue(actual == expected);

            // Search portion of array containing the sequence starting at position 2.
            expected = 4;
            actual = patternFinder.Search(newSequence, 1, sequence.Length);
            Assert.IsTrue(actual == expected);

            // Search portion of array containing the sequence starting at position 6.
            expected = -1;
            actual = patternFinder.Search(newSequence, 5, sequence.Length);
            Assert.IsTrue(actual == expected);

            // Search whole array starting at position 1.
            expected = 0;
            actual = patternFinder.Search(newSequence);
            Assert.IsTrue(actual == expected);

            // Search whole array starting at position 6.
            expected = -1;
            actual = patternFinder.Search(newSequence, 5);
            Assert.IsTrue(actual == expected);

        }

        /// <summary>
        ///     Test of SearchForOccurrence method.
        /// </summary>
        [Test]
        public void SearchForOccurrenceTest()
        {
            byte[] sequence = new Sequence(DNA, "AGCTAGCTA").GetInternalArray();
            byte[] pattern = new Sequence(DNA, "AGCT").GetInternalArray();
            IBytePatternFinder patternFinder = new BoyerMoore(pattern);

            long expected = 0;
            long actual = patternFinder.SearchForOccurrence(sequence, 1);
            Assert.IsTrue(actual == expected);

            expected = 4;
            actual = patternFinder.SearchForOccurrence(sequence, 2);
            Assert.IsTrue(actual == expected);

            expected = -1;
            actual = patternFinder.SearchForOccurrence(sequence, 3);
            Assert.IsTrue(actual == expected);
        }

        /// <summary>
        ///     Test of Search method with multiple sequences as input.
        /// </summary>
        [Test]
        public void SearchMultipleSequencesTest()
        {
            byte[] pattern = new Sequence(DNA, "AGCT").GetInternalArray();
            IBytePatternFinder patternFinder = new BoyerMoore(pattern);

            byte[] sequence1 = new Sequence(DNA, new String('C', 100000) + "AGCTAGCTA").GetInternalArray();
            byte[] sequence2 = new Sequence(DNA, "GCTAGCTA").GetInternalArray();
            byte[] sequence3 = new Sequence(DNA, "CTAGCTA").GetInternalArray();
            byte[] sequence4 = new Sequence(DNA, "CCCCCCC").GetInternalArray();
            byte[] sequence5 = new Sequence(DNA, "AGCTAGCTA").GetInternalArray();
            byte[] sequence6 = new Sequence(DNA, "GCTAGCTA").GetInternalArray();
            byte[] sequence7 = new Sequence(DNA, "CTAGCTA").GetInternalArray();
            byte[] sequence8 = new Sequence(DNA, "CCCCCCC").GetInternalArray();
            List<byte[]> sequences = new List<byte[]> { sequence1, sequence2, sequence3, sequence4, sequence5, sequence6, sequence7, sequence8};

            var expected = new List<long> { 100000, 3, 2, -1, 0, 3, 2, -1 };
            IReadOnlyList<long> actual = patternFinder.Search(sequences);

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        /// <summary>
        ///     Test of Search method with multiple sequences as input.
        /// </summary>
        [Test]
        public void IsFoundInAnyTest()
        {
            byte[] pattern = new Sequence(DNA, "GGGGGGGGGGG").GetInternalArray();
            IBytePatternFinder patternFinder = new BoyerMoore(pattern);

            byte[] sequence1 = new Sequence(DNA, new String('C', 40000) + "AGCTAGCTA").GetInternalArray();
            byte[] sequence2 = new Sequence(DNA, new String('C', 30000) + "GCTAGCTA").GetInternalArray();
            byte[] sequence3 = new Sequence(DNA, new String('C', 20000) + "CTAGCTA").GetInternalArray();
            byte[] sequence4 = new Sequence(DNA, new String('C', 50000) + "CTAGCTAG").GetInternalArray();
            byte[] sequence5 = new Sequence(DNA, new String('C', 10000) + "AGCTAGCTA").GetInternalArray();
            byte[] sequence6 = new Sequence(DNA, new String('C', 10000) + "GCTAGCTA").GetInternalArray();
            byte[] sequence7 = new Sequence(DNA, new String('C', 20000) + "CTAGCTA").GetInternalArray();
            byte[] sequence8 = new Sequence(DNA, new String('C', 30000) + "GCTAGCTAG").GetInternalArray();
            byte[] sequence9 = new Sequence(DNA, new String('C', 40000) + "GCTAGCTAGC").GetInternalArray();
            byte[] sequence10 = new Sequence(DNA, new String('C', 40000) + "AGCTAGCTA").GetInternalArray();
            byte[] sequence11 = new Sequence(DNA, new String('C', 30000) + "GCTAGCTA").GetInternalArray();
            byte[] sequence12 = new Sequence(DNA, new String('C', 20000) + "CTAGCTA").GetInternalArray();
            byte[] sequence13 = new Sequence(DNA, new String('C', 50000) + "CCCCCCC").GetInternalArray();
            byte[] sequence14 = new Sequence(DNA, new String('C', 10000) + "AGCTAGCTA").GetInternalArray();
            byte[] sequence15 = new Sequence(DNA, new String('C', 10000) + "GCTAGCTA").GetInternalArray();
            byte[] sequence16 = new Sequence(DNA, new String('C', 1) + "GGGGGGGGGGG").GetInternalArray();
            byte[] sequence17 = new Sequence(DNA, new String('C', 100000000) + "GGGGGGGGGGG").GetInternalArray();

            // Find quickly in last task started.
            List<byte[]> sequences = new List<byte[]>
            {
                sequence1, sequence2, sequence3, sequence4, sequence5, sequence6, sequence7, sequence8,
                sequence9, sequence10, sequence11, sequence12, sequence13, sequence14, sequence15, sequence16
            };
            Assert.IsTrue(patternFinder.IsFoundInAny(sequences));

            // Find slowly in last task started.
            sequences = new List<byte[]>
            {
                sequence1, sequence2, sequence3, sequence4, sequence5, sequence6, sequence7, sequence8,
                sequence9, sequence10, sequence11, sequence12, sequence13, sequence14, sequence15, sequence17
            };
            Assert.IsTrue(patternFinder.IsFoundInAny(sequences));

            // Find quickly in first task started.
            sequences = new List<byte[]>
            {
                sequence16, sequence1, sequence2, sequence3, sequence4, sequence5, sequence6, sequence7,
                sequence8, sequence9, sequence10, sequence11, sequence12, sequence13, sequence14, sequence15
            };
            Assert.IsTrue(patternFinder.IsFoundInAny(sequences));

            // Find slowly in first task started.
            sequences = new List<byte[]>
            {
                sequence17, sequence1, sequence2, sequence3, sequence4, sequence5, sequence6, sequence7,
                sequence8, sequence9, sequence10, sequence11, sequence12, sequence13, sequence14, sequence15
            };
            Assert.IsTrue(patternFinder.IsFoundInAny(sequences));

            // Test with no matches.
            sequences = new List<byte[]>
                { sequence1, sequence2, sequence3, sequence4, sequence5, sequence6, sequence7 };
            Assert.IsFalse(patternFinder.IsFoundInAny(sequences));
        }

        /// <summary>
        ///     Test of IsFoundInAll method.
        /// </summary>
        [Test]
        public void IsFoundInAllTest()
        {
            byte[] pattern = new Sequence(DNA, "AGCT").GetInternalArray();
            IBytePatternFinder patternFinder = new BoyerMoore(pattern);

            byte[] sequence1 = new Sequence(DNA, new String('C', 40000) + "AGCTAGCTA").GetInternalArray();
            byte[] sequence2 = new Sequence(DNA, new String('C', 30000) + "GCTAGCTA").GetInternalArray();
            byte[] sequence3 = new Sequence(DNA, new String('C', 20000) + "CTAGCTA").GetInternalArray();
            byte[] sequence4 = new Sequence(DNA, new String('C', 10000000) + "GGGGG").GetInternalArray();
            byte[] sequence5 = new Sequence(DNA, new String('C', 1) + "GGGGG").GetInternalArray();

            // Find in all.
            List<byte[]> sequences = new List<byte[]> { sequence1, sequence2, sequence3 };
            Assert.IsTrue(patternFinder.IsFoundInAny(sequences));

            // Fail to find slowly in last task started.
            sequences = new List<byte[]> { sequence1, sequence2, sequence3, sequence4 };
            Assert.IsFalse(patternFinder.IsFoundInAll(sequences));

            // Fail to find quickly in last task started.
            sequences = new List<byte[]> { sequence1, sequence2, sequence3, sequence5 };
            Assert.IsFalse(patternFinder.IsFoundInAll(sequences));

            // Fail to find slowly in first task started.
            sequences = new List<byte[]> { sequence4, sequence1, sequence2, sequence3 };
            Assert.IsFalse(patternFinder.IsFoundInAll(sequences));

            // Fail to find quickly in first task started.
            sequences = new List<byte[]> { sequence5, sequence1, sequence2, sequence3 };
            Assert.IsFalse(patternFinder.IsFoundInAll(sequences));
        }

        /// <summary>
        ///     Test of IsFoundIn method with a single sequence as input.
        /// </summary>
        [Test]
        public void IsFoundInSingleSequenceTest()
        {
            byte[] sequence = new Sequence(DNA, "AGCTAGCTA").GetInternalArray();
            byte[] pattern = new Sequence(DNA, "AGCT").GetInternalArray();
            IBytePatternFinder patternFinder = new BoyerMoore(pattern);

            // IsFoundIn whole of sequence.
            bool actual = patternFinder.IsFoundIn(sequence);
            Assert.IsTrue(actual);

            // IsFoundIn starting at position 2.
            actual = patternFinder.IsFoundIn(sequence, 1);
            Assert.IsTrue(actual);

            // IsFoundIn starting at position 6.
            actual = patternFinder.IsFoundIn(sequence, 5);
            Assert.IsFalse(actual);

            // ----------------------------------------------------------
            // Copy the sequence bytes into a larger array. 
            byte[] newSequence = new byte[100];
            Array.Copy(sequence, 0, newSequence, 0, sequence.Length);

            // IsFoundIn portion of array containing the sequence.
            actual = patternFinder.IsFoundIn(newSequence, 0, sequence.Length);
            Assert.IsTrue(actual);

            // IsFoundIn portion of array containing the sequence starting at position 2.
            actual = patternFinder.IsFoundIn(newSequence, 1, sequence.Length);
            Assert.IsTrue(actual);

            // IsFoundIn portion of array containing the sequence starting at position 6.
            actual = patternFinder.IsFoundIn(newSequence, 5, sequence.Length);
            Assert.IsFalse(actual);

            // IsFoundIn whole array starting at position 1.
            actual = patternFinder.IsFoundIn(newSequence);
            Assert.IsTrue(actual);

            // IsFoundIn whole array starting at position 6.
            actual = patternFinder.IsFoundIn(newSequence, 5);
            Assert.IsFalse(actual);

        }

        /// <summary>
        ///     Test of IsFoundIn method with multiple sequences as input.
        /// </summary>
        [Test]
        public void IsFoundInMultipleSequencesTest()
        {
            byte[] pattern = new Sequence(DNA, "AGCT").GetInternalArray();
            IBytePatternFinder patternFinder = new BoyerMoore(pattern);

            byte[] sequence1 = new Sequence(DNA, new String('C', 100000) + "AGCTAGCTA").GetInternalArray();
            byte[] sequence2 = new Sequence(DNA, "GCTAGCTA").GetInternalArray();
            byte[] sequence3 = new Sequence(DNA, "CTAGCTA").GetInternalArray();
            byte[] sequence4 = new Sequence(DNA, "CCCCCCC").GetInternalArray();
            byte[] sequence5 = new Sequence(DNA, "AGCTAGCTA").GetInternalArray();
            byte[] sequence6 = new Sequence(DNA, "GCTAGCTA").GetInternalArray();
            byte[] sequence7 = new Sequence(DNA, "CTAGCTA").GetInternalArray();
            byte[] sequence8 = new Sequence(DNA, "CCCCCCC").GetInternalArray();
            List<byte[]> sequences = new List<byte[]> { sequence1, sequence2, sequence3, sequence4, sequence5, sequence6, sequence7, sequence8 };

            var expected = new List<bool> { true, true, true, false, true, true, true, false };
            IReadOnlyList<bool> actual = patternFinder.IsFoundIn(sequences);

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }
    }
}
