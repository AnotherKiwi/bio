﻿using System.Collections.Generic;
using Bio;
using Bio.Algorithms.Alignment.MultipleSequenceAlignment;
using NUnit.Framework;

namespace Bio.Pamsam.Tests
{
    /// <summary>
    /// Test for Profile class
    /// </summary>
    [TestFixture]
    public class ProfileTests
    {
        /// <summary>
        /// Test Profile class
        /// </summary>
        [Test]
        public void TestProfile()
        {
            ISequence templateSequence = new Sequence(Alphabets.AmbiguousDNA, "ATGCSWRYKMBVHDN-");
            var itemSet = new Dictionary<byte, int>();
            for (var i = 0; i < templateSequence.Count; ++i)
            {
                itemSet.Add(templateSequence[i], i);

                if (char.IsLetter((char)templateSequence[i]))
                    itemSet.Add((byte)char.ToLower((char)templateSequence[i]), i);
            }
            Profiles.ItemSet = itemSet;

            ISequence seqA = new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT");
            ISequence seqB = new Sequence(Alphabets.DNA, "GGGAATCAAAATCAG");

            var sequences = new List<ISequence>
            {
                seqA,
                seqB
            };

            // Test GenerateProfiles
            var profileA = Profiles.GenerateProfiles(sequences[0]);
            Assert.AreEqual(16, profileA.ColumnSize);
            Assert.AreEqual(sequences[0].Count, profileA.RowSize);

            // Test ProfileMatrix
            Assert.AreEqual(1, profileA.ProfilesMatrix[0][2]);
            Assert.AreEqual(0, profileA.ProfilesMatrix[0][3]);

            // Test ProfileAlignment
            var profileAlignmentA = ProfileAlignment.GenerateProfileAlignment(sequences[0]);
            Assert.AreEqual(1, profileAlignmentA.ProfilesMatrix[0][2]);
            Assert.AreEqual(0, profileAlignmentA.ProfilesMatrix[0][3]);
            Assert.AreEqual(1, profileAlignmentA.NumberOfSequences);

            var profileAlignmentB = ProfileAlignment.GenerateProfileAlignment(sequences);
            Assert.AreEqual(1, profileAlignmentB.ProfilesMatrix[0][2]);
            Assert.AreEqual(0, profileAlignmentB.ProfilesMatrix[0][3]);
            Assert.AreEqual(2, profileAlignmentB.NumberOfSequences);

            Assert.AreEqual(0.5, profileAlignmentB.ProfilesMatrix[5][0]);
            Assert.AreEqual(0.5, profileAlignmentB.ProfilesMatrix[5][1]);
            Assert.AreEqual(0, profileAlignmentB.ProfilesMatrix[5][2]);
        }
    }
}
