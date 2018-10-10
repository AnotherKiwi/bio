﻿using System.Collections.Generic;
using System.Linq;
using Bio.Algorithms.Assembly.Graph;
using Bio.Algorithms.Assembly.Padena;
using NUnit.Framework;

namespace Bio.Padena.Tests
{
    /// <summary>
    /// Test for Step 2 in Parallel De Novo Assembly
    /// This step creates de bruijn graph from kmers.
    /// </summary>
    [TestFixture]
    public class GraphConstructionTests : ParallelDeNovoAssembler
    {
        /// <summary>
        /// Test Step 2 in De Novo algorithm - graph building
        /// </summary>
        [Test]
        public void TestDeBruijnGraphBuilderTiny()
        {
            const int KmerLength = 3;
            List<ISequence> reads = TestInputs.GetTinyReads();
            this.KmerLength = KmerLength;
            SequenceReads.Clear();
            SetSequenceReads(reads);

            CreateGraph();
            DeBruijnGraph graph = Graph;

            Assert.AreEqual(9, graph.NodeCount);
            HashSet<string> nodeStrings = new HashSet<string>(graph.GetNodes().Select(n => 
                new string(graph.GetNodeSequence(n).Select(a => (char)a).ToArray())));
            Assert.IsTrue(nodeStrings.Contains("ATG") || nodeStrings.Contains("CAT"));
            Assert.IsTrue(nodeStrings.Contains("TGC") || nodeStrings.Contains("GCA"));
            Assert.IsTrue(nodeStrings.Contains("GCC") || nodeStrings.Contains("GGC"));
            Assert.IsTrue(nodeStrings.Contains("TCC") || nodeStrings.Contains("GGA"));
            Assert.IsTrue(nodeStrings.Contains("CCT") || nodeStrings.Contains("AGG"));
            Assert.IsTrue(nodeStrings.Contains("CTA") || nodeStrings.Contains("TAG"));
            Assert.IsTrue(nodeStrings.Contains("TAT") || nodeStrings.Contains("ATA"));
            Assert.IsTrue(nodeStrings.Contains("ATC") || nodeStrings.Contains("GAT"));
            Assert.IsTrue(nodeStrings.Contains("CTC") || nodeStrings.Contains("GAG"));
            long totalEdges = graph.GetNodes().Select(n => n.ExtensionsCount).Sum();
            Assert.AreEqual(31, totalEdges);
        }

        /// <summary>
        /// Test Step 2 in De Novo algorithm - graph building
        /// </summary>
        [Test]
        public void TestDeBruijnGraphBuilderSmall()
        {
            const int KmerLength = 6;
            List<ISequence> reads = TestInputs.GetSmallReads();
            this.KmerLength = KmerLength;
            SequenceReads.Clear();
            SetSequenceReads(reads);

            CreateGraph();
            DeBruijnGraph graph = Graph;

            Assert.AreEqual(20, graph.NodeCount);
            HashSet<string> nodeStrings = GetGraphNodesForSmallReads();
            string nodeStr, nodeStrRC;
            foreach (DeBruijnNode node in graph.GetNodes())
            {
                nodeStr = new string(graph.GetNodeSequence(node).Select(a => (char)a).ToArray());
                nodeStrRC = new string(graph.GetNodeSequence(node).GetReverseComplementedSequence().Select(a => (char)a).ToArray());
                Assert.IsTrue(nodeStrings.Contains(nodeStr) || nodeStrings.Contains(nodeStrRC));
            }

            long totalEdges = graph.GetNodes().Select(n => n.ExtensionsCount).Sum();
            Assert.AreEqual(51, totalEdges);
        }

        #region Expected Output
        /// <summary>
        /// Expected graph nodes for sequences in GetSmallReads()
        /// </summary>
        /// <returns>Expected graph nodes</returns>
        private static HashSet<string> GetGraphNodesForSmallReads()
        {
            return new HashSet<string>
            {
                "GATGCC",
                "ATGCCT",
                "TGCCTC",
                "GCCTCC",
                "CCTCCT",
                "CTCCTA",
                "TCCTAT",
                "CCTATC",
                "CTATCG",
                "TATCGA",
                "ATCGAT",
                "TCGATC",
                "CGATCG",
                "GATCGT",
                "ATCGTC",
                "TCGTCG",
                "CGTCGA",
                "GTCGAT",
                "TCGATG",
                "CGATGC"
            };
        }
        #endregion
    }
}
