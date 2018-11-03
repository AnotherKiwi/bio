﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using Bio.Algorithms.Assembly.Padena;

namespace Bio.Padena.Tests
{
    [TestFixture]
    public class TestToString
    {
        /// <summary>
        /// Test for PadenaAssembly ToString() Function.
        /// </summary>
        [Test]
        public void TestPadenaAssemblyToString()
        {
            ISequence seq2 = new Sequence(Alphabets.DNA, "ACAAAAGCAAC");
            ISequence seq1 = new Sequence(Alphabets.DNA, "ATGAAGGCAATACTAGTAGT");
            IList<ISequence> contigList = new List<ISequence>
            {
                seq1,
                seq2
            };
            PadenaAssembly denovoAssembly = new PadenaAssembly();
            denovoAssembly.AddContigs(contigList);

            string actualString = denovoAssembly.ToString().Replace(Environment.NewLine, "");
            string expectedString = "ATGAAGGCAATACTAGTAGTACAAAAGCAAC";
            Assert.AreEqual(actualString, expectedString);
        }
    }
}
