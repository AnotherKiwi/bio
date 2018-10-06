using System.Collections.Generic;
using Bio.Algorithms.Assembly.Padena;
using Bio.Tests.Framework;
using Bio.Util.Logging;
using NUnit.Framework;

namespace Bio.Padena.Tests
{
    /// <summary>
    /// Builds scaffold sequence. 
    /// </summary>
    [TestFixture]
    public class ParallelDeNovoAssemblerWithScaffoldBuilders
    {
        static ParallelDeNovoAssemblerWithScaffoldBuilders()
        {
            Trace.Set(Trace.SeqWarnings);
        }

        /// <summary>
        /// Test Class Scaffold Builder using 9 contigs.
        /// </summary>
        [Test]
        public void AssemblerTestWithScaffoldBuilder()
        {
            const int kmerLength = 6;
            const int dangleThreshold = 3;
            const int redundantThreshold = 7;

            using (var assembler = new ParallelDeNovoAssembler())
            {
                assembler.KmerLength = kmerLength;
                assembler.DanglingLinksThreshold = dangleThreshold;
                assembler.RedundantPathLengthThreshold = redundantThreshold;

                assembler.ScaffoldRedundancy = 0;
                assembler.Depth = 3;
                CloneLibrary.Instance.AddLibrary("abc", 5, 20);

                var result = (PadenaAssembly)assembler.Assemble(TestInputs.GetReadsForScaffolds(), true);

                var expectedContigs = new HashSet<string>
                {
                    "TTTTTT","CGCGCG","TTAGCGCG","CGCGCCGCGC","GCGCGC","TTTTTA","TTTTAA","TTTAAA","TTTTAGC","ATGCCTCCTATCTTAGC"
                };

                AlignmentHelpers.CompareSequenceLists(expectedContigs, result.ContigSequences);

                var expectedScaffolds = new HashSet<string>
                {
                    "ATGCCTCCTATCTTAGCGCGC","TTTAAA","TTTTTT","TTTTAGC","TTTTAA","CGCGCCGCGC","TTTTTA","CGCGCG"
                };

                AlignmentHelpers.CompareSequenceLists(expectedScaffolds, result.Scaffolds);
            }
        }
    }
}
