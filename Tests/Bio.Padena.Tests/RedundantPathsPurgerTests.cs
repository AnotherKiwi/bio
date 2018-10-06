using System.Collections.Generic;
using System.Linq;
using Bio;
using Bio.Algorithms.Assembly.Padena;
using NUnit.Framework;

namespace Bio.Padena.Tests
{
    /// <summary>
    /// Test for Step 4 in Parallel De Novo Assembly
    /// This step performs error correction on the input graph.
    /// It removes redundant paths in the graph.
    /// </summary>
    [TestFixture]
    public class RedundantPathsPurgerTests : ParallelDeNovoAssembler
    {
        /// <summary>
        /// Test Step 4 - Redundant Paths Purger class
        /// </summary>
        [Test]
        public void TestRedundantPathsPurger()
        {
            const int KmerLength = 5;
            const int RedundantThreshold = 10;

            var readSeqs = TestInputs.GetRedundantPathReads();
            SequenceReads.Clear();
            SetSequenceReads(readSeqs);
            this.KmerLength = KmerLength;
            RedundantPathLengthThreshold = RedundantThreshold;
            RedundantPathsPurger = new RedundantPathsPurger(RedundantThreshold);

            CreateGraph();
            var graphCount = Graph.NodeCount;
            long graphEdges = Graph.GetNodes().Select(n => n.ExtensionsCount).Sum();

            RemoveRedundancy();
            var redundancyRemovedGraphCount = Graph.NodeCount;
            long redundancyRemovedGraphEdge = Graph.GetNodes().Select(n => n.ExtensionsCount).Sum();

            // Compare the two graphs
            Assert.AreEqual(5, graphCount - redundancyRemovedGraphCount);
            Assert.AreEqual(12, graphEdges - redundancyRemovedGraphEdge);
        }
    }
}
