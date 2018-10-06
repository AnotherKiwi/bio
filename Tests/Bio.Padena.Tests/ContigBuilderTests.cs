using System.Collections.Generic;
using System.Linq;
using Bio;
using Bio.Algorithms.Assembly.Padena;
using NUnit.Framework;

namespace Bio.Padena.Tests
{
    /// <summary>
    /// Test for Step 5 in Parallel De Novo Assembly
    /// This step builds contigs from input graph.
    /// </summary>
    [TestFixture]
    public class ContigBuilderTests : ParallelDeNovoAssembler
    {
        /// <summary>
        /// Test Step 5 - Contig Builder Class
        /// </summary>
        [Test]
        public void TestContigBuilder1()
        {
            const int KmerLength = 11;
            const int DangleThreshold = 3;
            const int RedundantThreshold = 10;

            var readSeqs = TestInputs.GetDanglingReads();
            SequenceReads.Clear();
            SetSequenceReads(readSeqs);
            this.KmerLength = KmerLength;
            DanglingLinksThreshold = DangleThreshold;
            DanglingLinksPurger = new DanglingLinksPurger(DangleThreshold);
            RedundantPathLengthThreshold = RedundantThreshold;
            RedundantPathsPurger = new RedundantPathsPurger(RedundantThreshold);
            ContigBuilder = new SimplePathContigBuilder();

            CreateGraph();
            UnDangleGraph();
            RemoveRedundancy();
            var graphCount = Graph.NodeCount;
            long graphEdges = Graph.GetNodes().Select(n => n.ExtensionsCount).Sum();

            var contigs = BuildContigs();
            var contigsBuiltGraphCount = Graph.NodeCount;
            long contigsBuilt = Graph.GetNodes().Select(n => n.ExtensionsCount).Sum();

            // Compare the two graphs
            Assert.AreEqual(1, contigs.Count());
            var expectedContigs = new HashSet<string>() 
            { 
                "ATCGCTAGCATCGAACGATCATT" 
            };

            foreach (var contig in contigs)
            {
                var s = new string(contig.Select(a => (char)a).ToArray());
                Assert.IsTrue(expectedContigs.Contains(s));
            }

            Assert.AreEqual(graphCount, contigsBuiltGraphCount);
            Assert.AreEqual(graphEdges, contigsBuilt);
        }

        /// <summary>
        /// Test Step 5 - Contig Builder Class
        /// </summary>
        [Test]
        public void TestContigBuilder2()
        {
            const int KmerLength = 6;
            const int RedundantThreshold = 10;

            var readSeqs = TestInputs.GetRedundantPathReads();
            SequenceReads.Clear();
            SetSequenceReads(readSeqs);
            this.KmerLength = KmerLength;
            RedundantPathLengthThreshold = RedundantThreshold;
            RedundantPathsPurger = new RedundantPathsPurger(RedundantThreshold);
            ContigBuilder = new SimplePathContigBuilder();

            CreateGraph();
            RemoveRedundancy();
            var graphCount = Graph.NodeCount;
            long graphEdges = Graph.GetNodes().Select(n => n.ExtensionsCount).Sum();

            var contigs = BuildContigs();
            var contigsBuiltGraphCount = Graph.NodeCount;
            long contigsBuilt = Graph.GetNodes().Select(n => n.ExtensionsCount).Sum();

            // Compare the two graphs
            Assert.AreEqual(1, contigs.Count());
            var s = new string(contigs.ElementAt(0).Select(a => (char)a).ToArray());
            Assert.AreEqual("ATGCCTCCTATCTTAGCGATGCGGTGT", s);
            Assert.AreEqual(graphCount, contigsBuiltGraphCount);
            Assert.AreEqual(graphEdges, contigsBuilt);
        }
    }
}
