using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using static Bio.Alphabets;
using static Bio.Core.Extensions.ArrayExtensions;

namespace Bio.Tests.Algorithms.ByteArraySearch
{
    [TestFixture]
    public class ArrayExtensionTests
    {
        /// <summary>
        ///     Test of FindIndexOf extension method.
        /// </summary>
        [Test]
        public void FindAllTest()
        {
            byte[] sequence = new Sequence(DNA, "AGCTAGCTA").GetInternalArray();
            byte[] pattern = new Sequence(DNA, "AGCT").GetInternalArray();

            List<long> expected = new List<long> { 0, 4 };
            IReadOnlyList<long> actual = sequence.FindAll(pattern);
            Assert.IsTrue(Compare(expected, actual));

            expected = new List<long> { 4 };
            actual = sequence.FindAll(pattern, 1);
            Assert.IsTrue(Compare(expected, actual));

            expected = new List<long> { -1 };
            actual = sequence.FindAll(pattern, 5);
            Assert.IsTrue(Compare(expected, actual));

            // ----------------------------------------------------------
            // Copy the sequence bytes into a larger array padded with 0s. 
            byte[] newSequence = new byte[100];
            Array.Copy(sequence, 0, newSequence, 0, sequence.Length);

            // Search portion of array containing the sequence.
            expected = new List<long> { 0, 4 };
            actual = newSequence.FindAll(pattern, 0, sequence.Length);
            Assert.IsTrue(Compare(expected, actual));

            // Search portion of array containing the sequence starting at position 2.
            expected = new List<long> { 4 };
            actual = newSequence.FindAll(pattern, 1, sequence.Length);
            Assert.IsTrue(Compare(expected, actual));

            // Search portion of array containing the sequence starting at position 6.
            expected = new List<long> { -1 };
            actual = newSequence.FindAll(pattern, 5, sequence.Length);
            Assert.IsTrue(Compare(expected, actual));

            // Search whole array starting at position 1.
            expected = new List<long> { 0, 4 };
            actual = newSequence.FindAll(pattern);
            Assert.IsTrue(Compare(expected, actual));

            // Search whole array starting at position 6.
            expected = new List<long> { -1 };
            actual = newSequence.FindAll(pattern, 5);
            Assert.IsTrue(Compare(expected, actual));
        }

        /// <summary>
        ///     Test of FindIndexOf extension method.
        /// </summary>
        [Test]
        public void FindIndexOfTest()
        {
            byte[] sequence = new Sequence(DNA, "AGCTAGCTA").GetInternalArray();
            byte[] pattern = new Sequence(DNA, "AGCT").GetInternalArray();

            // Search whole of sequence.
            long expected = 0;
            long actual = sequence.FindIndexOf(pattern);
            Assert.IsTrue(actual == expected);

            // Search starting at position 2.
            expected = 4;
            actual = sequence.FindIndexOf(pattern, 1);
            Assert.IsTrue(actual == expected);

            // Search starting at position 6.
            expected = -1;
            actual = sequence.FindIndexOf(pattern, 5);
            Assert.IsTrue(actual == expected);

            // ----------------------------------------------------------
            // Copy the sequence bytes into a larger array padded with 0s. 
            byte[] newSequence = new byte[100];
            Array.Copy(sequence, 0, newSequence, 0, sequence.Length);

            // Search portion of array containing the sequence.
            expected = 0;
            actual = newSequence.FindIndexOf(pattern, 0, sequence.Length);
            Assert.IsTrue(actual == expected);

            // Search portion of array containing the sequence starting at position 2.
            expected = 4;
            actual = newSequence.FindIndexOf(pattern, 1, sequence.Length);
            Assert.IsTrue(actual == expected);

            // Search portion of array containing the sequence starting at position 6.
            expected = -1;
            actual = newSequence.FindIndexOf(pattern, 5, sequence.Length);
            Assert.IsTrue(actual == expected);

            // Search whole array starting at position 1.
            expected = 0;
            actual = newSequence.FindIndexOf(pattern);
            Assert.IsTrue(actual == expected);

            // Search whole array starting at position 6.
            expected = -1;
            actual = newSequence.FindIndexOf(pattern, 5);
            Assert.IsTrue(actual == expected);
        }


        /// <summary>
        ///     Test of Contains extension method.
        /// </summary>
        [Test]
        public void ContainsTest()
        {
            byte[] sequence = new Sequence(DNA, "AGCTAGCTA").GetInternalArray();
            byte[] pattern = new Sequence(DNA, "AGCT").GetInternalArray();

            // Search whole of sequence.
            bool expected = true;
            bool actual = sequence.Contains(pattern);
            Assert.IsTrue(actual == expected);

            // Search starting at position 2.
            expected = true;
            actual = sequence.Contains(pattern, 1);
            Assert.IsTrue(actual == expected);

            // Search starting at position 6.
            expected = false;
            actual = sequence.Contains(pattern, 5);
            Assert.IsTrue(actual == expected);

            // ----------------------------------------------------------
            // Copy the sequence bytes into a larger array padded with 0s. 
            byte[] newSequence = new byte[100];
            Array.Copy(sequence, 0, newSequence, 0, sequence.Length);

            // Search portion of array containing the sequence.
            expected = true;
            actual = newSequence.Contains(pattern, 0, sequence.Length);
            Assert.IsTrue(actual == expected);

            // Search portion of array containing the sequence starting at position 2.
            expected = true;
            actual = newSequence.Contains(pattern, 1, sequence.Length);
            Assert.IsTrue(actual == expected);

            // Search portion of array containing the sequence starting at position 6.
            expected = false;
            actual = newSequence.Contains(pattern, 5, sequence.Length);
            Assert.IsTrue(actual == expected);

            // Search whole array starting at position 1.
            expected = true;
            actual = newSequence.Contains(pattern);
            Assert.IsTrue(actual == expected);

            // Search whole array starting at position 6.
            expected = false;
            actual = newSequence.Contains(pattern, 5);
            Assert.IsTrue(actual == expected);
        }

        private static bool Compare(IList<long> expected, IReadOnlyList<long> actual)
        {
            if (expected.Count != actual.Count)
                return false;

            for (int i = 0; i < expected.Count; i++)
            {
                if (expected[i] != actual[i])
                    return false;
            }

            return true;
        }
    }
}
