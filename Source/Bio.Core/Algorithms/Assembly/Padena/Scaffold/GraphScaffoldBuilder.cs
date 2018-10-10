using System;
using System.Collections.Generic;
using System.Linq;
using Bio.Algorithms.Assembly.Padena.Scaffold.ContigOverlapGraph;
using Bio.Properties;

namespace Bio.Algorithms.Assembly.Padena.Scaffold
{
    /// <summary>
    /// Generates Scaffolds using Graph.
    /// Algorithm:
    /// Step1: Generate contig overlap graph. 
    /// Step2: Map Reads to contigs.
    /// Step3: Generate Contig Mate Pair Map.
    /// Step4: Filter Paired Reads.
    /// Step5: Distance Orientation.
    /// Step6: Trace Scaffold Paths.
    /// Step7: Assemble paths.
    /// Step8: Generate sequence of scaffolds.
    /// </summary>
    public class GraphScaffoldBuilder : IGraphScaffoldBuilder
    {
        /// <summary>
        /// Number of paired read required to connect two contigs.
        /// </summary>
        private int redundancyField;

        /// <summary>
        /// Depth for graph traversal.
        /// </summary>
        private int depthField;

        /// <summary>
        /// Kmer length.
        /// </summary>
        private int kmerLength;

        /// <summary>
        /// Mapping reads to mate pairs.
        /// </summary>
        private IMatePairMapper mapPairedReads;

        /// <summary>
        /// Mapping reads to contigs.
        /// </summary>
        private IReadContigMapper readContigMap;

        /// <summary>
        /// Filtering of mate pairs based on orientation of contigs.
        /// </summary>
        private IOrientationBasedMatePairFilter pairedReadFilter;

        /// <summary>
        /// Calculation of distance between contigs using mate pairs.
        /// </summary>
        private IDistanceCalculator distanceCalculator;

        /// <summary>
        /// Traversal of contig overlap graph.
        /// </summary>
        private ITracePath tracePath;

        /// <summary>
        /// Removal of containing paths and removal of overlapping paths.
        /// </summary>
        private IPathPurger pathAssembler;

        /// <summary>
        /// Initializes a new instance of the GraphScaffoldBuilder class. 
        /// </summary>
        public GraphScaffoldBuilder()
        {
            mapPairedReads = new MatePairMapper();
            readContigMap = new ReadContigMapper();
            pairedReadFilter = new OrientationBasedMatePairFilter();
            tracePath = new TracePath();
            pathAssembler = new PathPurger();

            //Hierarchical Scaffolding With Bambus
            //by: Mihai Pop, Daniel S. Kosack, Steven L. Salzberg
            //Genome Research, Vol. 14, No. 1. (January 2004), pp. 149-159.
            redundancyField = 2;

            //Memory and performance optimization.
            depthField = 10;
        }

        /// <summary>
        /// Initializes a new instance of the GraphScaffoldBuilder class.
        /// </summary>
        /// <param name="mapPairedReads">Mapping reads to mate pairs.</param>
        /// <param name="readContigMap"> Mapping reads to contigs.</param>
        /// <param name="pairedReadFilter">Filtering of mate pairs.</param>
        /// <param name="distanceCalculator">Calculation of distance between 
        /// contigs using mate pairs.</param>
        /// <param name="tracePath">Traversal of contig overlap graph.</param>
        /// <param name="pathAssembler">Removal of containing paths and removal of overlapping paths.</param>
        public GraphScaffoldBuilder(
            IMatePairMapper mapPairedReads,
            IReadContigMapper readContigMap,
            IOrientationBasedMatePairFilter pairedReadFilter,
            IDistanceCalculator distanceCalculator,
            ITracePath tracePath,
            IPathPurger pathAssembler)
        {
            this.mapPairedReads = mapPairedReads ?? new MatePairMapper();
            this.readContigMap = readContigMap ?? new ReadContigMapper();
            this.pairedReadFilter = pairedReadFilter ?? new OrientationBasedMatePairFilter();
            this.tracePath = tracePath ?? new TracePath();
            this.pathAssembler = pathAssembler ?? new PathPurger();

            if (null != distanceCalculator)
            {
                this.distanceCalculator = distanceCalculator;
            }

            //Hierarchical Scaffolding With Bambus
            //by: Mihai Pop, Daniel S. Kosack, Steven L. Salzberg
            //Genome Research, Vol. 14, No. 1. (January 2004), pp. 149-159.
            redundancyField = 2;

            //Memory and performance optimization.
            depthField = 10;
        }

        /// <summary>
        /// Builds scaffolds from list of reads and contigs.
        /// </summary>
        /// <param name="reads">List of reads.</param>
        /// <param name="contigs">List of contigs.</param>
        /// <param name="lengthofKmer">Kmer Length.</param>
        /// <param name="depth">Depth for graph traversal.</param>
        /// <param name="redundancy">Number of mate pairs required to create a link between two contigs.
        ///  Hierarchical Scaffolding With Bambus
        ///  by: Mihai Pop, Daniel S. Kosack, Steven L. Salzberg
        ///  Genome Research, Vol. 14, No. 1. (January 2004), pp. 149-159.</param>
        /// <returns>List of scaffold sequences.</returns>
        public IList<ISequence> BuildScaffold(
            IEnumerable<ISequence> reads,
            IList<ISequence> contigs,
            int lengthofKmer,
            int depth = 10,
            int redundancy = 2)
        {
            if (contigs == null)
            {
                throw new ArgumentNullException(nameof(contigs));
            }

            if (null == reads)
            {
                throw new ArgumentNullException(nameof(reads));
            }

            if (lengthofKmer <= 0)
            {
                throw new ArgumentException("lengthofKmer");
            }

            if (depth <= 0)
            {
                throw new ArgumentException(Resource.Depth);
            }

            if (redundancy < 0)
            {
                throw new ArgumentException(Resource.NegativeRedundancy);
            }

            depthField = depth;
            redundancyField = redundancy;
            kmerLength = lengthofKmer;

            IEnumerable<ISequence> readSeqs = ValidateReads(reads);

            //Step1: Generate contig overlap graph.
            IList<ISequence> contigsList = new List<ISequence>(contigs);
            ContigGraph contigGraph = GenerateContigOverlapGraph(contigsList);
            IEnumerable<Node> nodes = contigGraph.Nodes.Where(t => t.ExtensionsCount == 0);

            foreach (Node node in nodes)
            {
                contigsList.Remove(contigGraph.GetNodeSequence(node));
            }

            // Step2: Map Reads to contigs.
            ReadContigMap readContigMaps = ReadContigMap(contigsList, readSeqs);
            contigsList = null;

            // Step3: Generate Contig Mate Pair Map.
            ContigMatePairs contigMatePairs = MapPairedReadsToContigs(readContigMaps, readSeqs);
            readContigMaps = null;

            // Step4: Filter Paired Reads.
            contigMatePairs = FilterReadsBasedOnOrientation(contigMatePairs);

            // Step5: Distance Calculation.
            CalculateDistanceBetweenContigs(contigMatePairs);

            // Step6: Trace Scaffold Paths.
            IList<ScaffoldPath> paths = TracePath(contigGraph, contigMatePairs);
            contigMatePairs = null;

            // Step7: Assemble paths.
            PathPurger(paths);

            // Step8: Generate sequence of scaffolds.
            return GenerateScaffold(contigGraph, paths);
        }

        /// <summary>
        /// Implements dispose to suppress GC finalize
        /// This is done as this class creates an instance 
        /// of ContigGraph which extends IDisposable.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Generate contig overlap graph.
        /// </summary>
        /// <param name="contigs">List of contig sequences.</param>
        /// <returns>Contig Graph.</returns>
        protected ContigGraph GenerateContigOverlapGraph(IList<ISequence> contigs)
        {
            if (contigs == null)
            {
                throw new ArgumentNullException(nameof(contigs));
            }

            ContigGraph contigGraph = new ContigGraph();
            contigGraph.BuildContigGraph(contigs, kmerLength);
            return contigGraph;
        }

        /// <summary>
        /// Map reads to contigs.
        /// </summary>
        /// <param name="contigs">List of sequences of contigs.</param>
        /// <param name="reads">List of sequences of reads.</param>
        /// <returns>Map of reads and contigs.</returns>
        protected ReadContigMap ReadContigMap(IList<ISequence> contigs, IEnumerable<ISequence> reads)
        {
            return readContigMap.Map(contigs, reads, kmerLength);
        }

        /// <summary>
        /// Map paired reads to contigs using FASTA sequence header.
        /// </summary>
        /// <param name="readContigMaps">Map between reads and contigs.</param>
        /// <param name="reads">Sequences of reads.</param>
        /// <returns>Contig Mate Pair map.</returns>
        protected ContigMatePairs MapPairedReadsToContigs(ReadContigMap readContigMaps, IEnumerable<ISequence> reads)
        {
            return mapPairedReads.MapContigToMatePairs(reads, readContigMaps);
        }

        /// <summary>
        /// Filter reads based on orientation of contigs.
        /// </summary>
        /// <param name="contigMatePairs">Contig Mate Pair map.</param>
        /// <returns>Returns Contig Mate Pair map.</returns>
        protected ContigMatePairs FilterReadsBasedOnOrientation(ContigMatePairs contigMatePairs)
        {
            return pairedReadFilter.FilterPairedReads(contigMatePairs, redundancyField);
        }

        /// <summary>
        /// Calculate distance between contigs using paired reads.
        /// </summary>
        /// <param name="contigMatePairs">Contig Mate Pair map.</param>
        /// <returns>Number of contig-read pairs.</returns>
        protected int CalculateDistanceBetweenContigs(ContigMatePairs contigMatePairs)
        {
            if (contigMatePairs == null)
            {
                throw new ArgumentNullException(nameof(contigMatePairs));
            }

            if (distanceCalculator == null)
            {
                distanceCalculator = new DistanceCalculator(contigMatePairs);
                contigMatePairs = distanceCalculator.CalculateDistance();
            }
            else
            {
                contigMatePairs = distanceCalculator.CalculateDistance();
            }

            // this dictionary is updated in this step.
            return contigMatePairs.Count;
        }

        /// <summary>
        /// Performs Breadth First Search in contig overlap graph.
        /// </summary>
        /// <param name="contigGraph">Contig Graph.</param>
        /// <param name="contigMatePairs">Contig Mate Pair map.</param>
        /// <returns>List of Scaffold Paths.</returns>
        protected IList<ScaffoldPath> TracePath(ContigGraph contigGraph, ContigMatePairs contigMatePairs)
        {
            return tracePath.FindPaths(contigGraph, contigMatePairs, kmerLength, depthField);
        }

        /// <summary>
        /// Remove containing and overlapping paths.
        /// </summary>
        /// <param name="paths">List of Scaffold Paths.</param>
        /// <returns>Number of final paths.</returns>
        protected int PathPurger(IList<ScaffoldPath> paths)
        {
            if (paths == null)
            {
                throw new ArgumentNullException(nameof(paths));
            }

            pathAssembler.PurgePath(paths);
            return paths.Count;
        }

        /// <summary>
        /// Generate sequences from list of contig nodes.
        /// </summary>
        /// <param name="contigGraph">Contig Overlap Graph.</param>
        /// <param name="paths">Scaffold paths.</param>
        /// <returns>List of sequences of scaffolds.</returns>
        protected IList<ISequence> GenerateScaffold(
            ContigGraph contigGraph,
            IList<ScaffoldPath> paths)
        {
            if (contigGraph == null)
            {
                throw new ArgumentNullException(nameof(contigGraph));
            }

            if (paths == null)
            {
                throw new ArgumentNullException(nameof(paths));
            }

            List<ISequence> scaffolds = paths.AsParallel().Select(t => t.BuildSequenceFromPath(contigGraph, kmerLength)).ToList();
            IEnumerable<Node> visitedNodes = contigGraph.Nodes.AsParallel().Where(t => !t.IsMarked());
            scaffolds.AddRange(visitedNodes.AsParallel().Select(t => contigGraph.GetNodeSequence(t)));
            contigGraph.Dispose();
            return scaffolds;
        }

        /// <summary>
        /// Dispose field instances.
        /// </summary>
        /// <param name="disposeManaged">If disposeManaged equals true, clean all resources.</param>
        protected virtual void Dispose(bool disposeManaged)
        {
            if (disposeManaged)
            {
                distanceCalculator = null;
                mapPairedReads = null;
                pairedReadFilter = null;
                pathAssembler = null;
                readContigMap = null;
                tracePath = null;
            }
        }

        /// <summary>
        /// Validate input sequences
        /// </summary>
        /// <param name="reads">The Reads</param>
        /// <returns>Valid reads.</returns>
        private IEnumerable<ISequence> ValidateReads(IEnumerable<ISequence> reads)
        {
            IAlphabet readAlphabet = Alphabets.GetAmbiguousAlphabet(reads.First().Alphabet);
            HashSet<byte> ambiguousSymbols = readAlphabet.GetAmbiguousSymbols();
            HashSet<byte> gapSymbols;
            readAlphabet.TryGetGapSymbols(out gapSymbols);

            foreach (ISequence read in reads)
            {
                string originalSequenceId;
                string pairedReadType;
                bool forward;
                string libraryName;
                if (Util.Helper.ValidatePairedSequenceId(read.ID, out originalSequenceId, out forward, out pairedReadType, out libraryName))
                {
                    if (!read.Alphabet.HasAmbiguity)
                    {
                        bool gapSymbolFound = false;
                        for (long index = 0; index < read.Count; index++)
                        {
                            if (gapSymbols.Contains(read[index]))
                            {
                                gapSymbolFound = true;
                            }
                        }

                        if (!gapSymbolFound)
                        {
                            // Exclude the otherinfo if any.
                            read.ID = Util.Helper.GetReadIdExcludingOtherInfo(read.ID);
                            yield return read;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
    }
}
