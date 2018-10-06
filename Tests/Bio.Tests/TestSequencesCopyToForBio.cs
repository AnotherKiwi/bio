using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Bio.Tests
{
    /// <summary>
    /// Summary description for TestSequencesCopyToForBio
    /// </summary>
    [TestFixture]
    public class TestSequencesCopyToForBio
    {
        /// <summary>
        /// Test for Sequence CopyTo method.
        /// </summary>
        [Test]
        public void TestSequenceCopyTo()
        {
            var seq = new Sequence(Alphabets.DNA, "ATCG");
            var array = new byte[2];
            seq.CopyTo(array, 1, 2);
            var expectedValue = "TC";
            var b = new StringBuilder();
            b.Append((char)array[0]);
            b.Append((char)array[1]);
            var actualValue = b.ToString();
            Assert.AreEqual(expectedValue, actualValue);
        }

        /// <summary>
        /// Test for DeriveSequence CopyTo method.
        /// </summary>
        [Test]
        public void TestDeriveSequenceCopyTo()
        {
            ISequence seq = new Sequence(Alphabets.DNA, "ATCG");
            var derSeq = new DerivedSequence(seq, false, true);
            var array = new byte[2];
            derSeq.CopyTo(array, 1, 2);
            var expectedValue = "AG";
            var b = new StringBuilder();
            b.Append((char)array[0]);
            b.Append((char)array[1]);
            var actualValue = b.ToString();
            Assert.AreEqual(expectedValue, actualValue);
        }

        /// <summary>
        /// Test for DeriveSequence CopyTo method.
        /// </summary>
        [Test]
        public void TestSparseSequenceCopyTo()
        {
            IEnumerable<byte> seqItems = new List<Byte>() { 65, 65, 67, 67 };
            var sparseSeq = new SparseSequence(Alphabets.DNA, 0, seqItems);
            var array = new byte[2];
            sparseSeq.CopyTo(array, 1, 2);
            var expectedValue = "AC";
            var b = new StringBuilder();
            b.Append((char)array[0]);
            b.Append((char)array[1]);
            var actualValue = b.ToString();
            Assert.AreEqual(expectedValue, actualValue);
        }

        /// <summary>
        /// Test for Sequence ConstructorWithISequenceArgument method.
        /// </summary>
        [Test]
        public void TestSequenceConstructorWithISequenceArgument()
        {
            ISequence seq = new Sequence(Alphabets.DNA, "ATCG");
            ISequence newSeq = new Sequence(seq);
            var expectedSequence = "ATCG";
            var actualSequence = new string(newSeq.Select(x => (char)x).ToArray());
            Assert.AreEqual(expectedSequence, actualSequence);
        }

        /// <summary>
        /// Test for DeriveSequence ConstructorWithSequenceArgument method.
        /// </summary>
        [Test]
        public void TestSparseSequenceConstructorWithSequenceArgument()
        {
            IEnumerable<byte> seqItems = new List<Byte>() { 65, 65, 67, 67 };
            var sparseSeq = new SparseSequence(Alphabets.DNA, 0, seqItems);
            var seq = sparseSeq.GetSubSequence(0, sparseSeq.Count);
            var newSparseSequence = new SparseSequence(seq);
            var newSeq = newSparseSequence.GetSubSequence(0, newSparseSequence.Count);

            var expectedValue = "AACC";
            var actualValue = new string(newSeq.Select(x => (char)x).ToArray());
            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}
