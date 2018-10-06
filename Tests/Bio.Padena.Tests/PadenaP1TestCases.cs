﻿/****************************************************************************
 * PadenaP1TestCases.cs
 * 
 *  This file contains the Padena P1 test cases.
 * 
***************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bio.Algorithms.Assembly.Padena.Scaffold.ContigOverlapGraph;
using Bio.Algorithms.Assembly;
using Bio.Algorithms.Assembly.Graph;
using Bio.Algorithms.Assembly.Padena;
using Bio.Algorithms.Assembly.Padena.Scaffold;
using Bio.Algorithms.Kmer;
using Bio.Extensions;
using Bio.IO.FastA;
using Bio.TestAutomation.Util;
using Bio.Util.Logging;

using NUnit.Framework;
using System.Globalization;
using Bio.Tests.Framework;
using Bio.Tests;

namespace Bio.Padena.Tests
{
    /// <summary>
    /// The class contains P1 test cases to confirm Padena.
    /// 
    /// Note: Due to all the optimizations in PADENA, step 6 no longer works. 
    /// Skipping those tests pending a fix.
    /// </summary>
    [TestFixture]
    public class PadenaP1TestCases
    {

        #region Constructor


        #endregion

        #region PadenaStep1TestCases

        /// <summary>
        /// Validate ParallelDeNovothis is building valid kmers 
        /// using virul genome input reads in a fasta file and kmerLength 28
        /// Input : virul genome input reads and kmerLength 28
        /// Output : kmers sequence
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep1BuildKmersForViralGenomeReads()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidatePadenaBuildKmers(Constants.ViralGenomeReadsNode, true);
            }
        }

        /// <summary>
        /// Validate ParallelDeNovothis is building valid kmers 
        /// using input reads which contains sequence and reverse complement
        /// Input : input reads with reverse complement and kmerLength 20
        /// Output : kmers sequence
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep1BuildKmersWithRCReads()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidatePadenaBuildKmers(Constants.OneLineReadsWithRCNode, false);
            }
        }

        /// <summary>
        /// Validate ParallelDeNovothis is building valid kmers 
        /// using input reads which will generate clusters in step2 graph
        /// Input : input reads which will generate clusters and kmerLength 7
        /// Output : kmers sequence
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep1BuildKmersWithClusters()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidatePadenaBuildKmers(Constants.OneLineReadsWithClustersNode, false);
            }
        }

        /// <summary>
        /// Validate KmersOfSequence ctor (sequence, length) by passing
        /// one line sequence and kmer length 4
        /// Input : Build kmeres from one line input reads of small size 
        /// chromosome sequence and kmerLength 4
        /// Output : kmers of sequence object with build kmers
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep1KmersOfSequenceCtorWithBuildKmers()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateKmersOfSequenceCtorWithBuildKmers(Constants.OneLineReadsNode);
            }
        }

        /// <summary>
        /// Validate KmersOfSequence ctor (sequence, length, set of kmers) 
        /// by passing small size chromsome sequence and kmer length 28
        /// after building kmers
        /// Input : Build kmeres from one line input reads of small size 
        /// chromosome sequence and kmerLength 28
        /// Output : kmers of sequence object with build kmers
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep1KmersOfSequenceCtorWithBuildKmersForSmallSizeSequences()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateKmersOfSequenceCtorWithBuildKmers(Constants.OneLineReadsNode);
            }
        }

        /// <summary>
        /// Validate KmersOfSequence properties
        /// Input : Build kmeres from 4000 input reads of small size 
        /// chromosome sequence and kmerLength 4 
        /// Output : kmers sequence
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidateKmersOfSequenceCtrproperties()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateKmersOfSequenceCtorProperties(Constants.OneLineReadsNode);
            }
        }

        /// <summary>
        /// Validate KmersOfSequence ToSequences() method using small size reads
        /// Input : Build kmeres from 4000 input reads of small size 
        /// chromosome sequence and kmerLength 28 
        /// Output : kmers sequence
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep1KmersOfSequenceToSequencesUsingSmallSizeReads()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateKmersOfSequenceToSequences(Constants.OneLineReadsNode);
            }
        }



        #endregion

        #region PadenaStep2TestCases

        /// <summary>
        /// Validate Graph after building it using build kmers 
        /// with virul genome reads and kmerLength 28
        /// Input: kmers
        /// Output: Graph
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep2BuildGraphForVirulGenome()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidatePadenaBuildGraph(Constants.ViralGenomeReadsNode, true);
            }
        }


        /// <summary>
        /// Validate Graph after building it using build kmers 
        /// with input reads contains reverse complement and kmerLength 20
        /// Input: kmers
        /// Output: Graph
        /// </summary>
        [Test]
        [Ignore("Broken test in Travis - never investigated reason")]
        [Category("Padena")]
        public void ValidatePadenaStep2BuildGraphWithRCReads()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidatePadenaBuildGraph(Constants.OneLineWithRCStep2Node, false);
            }
        }

        /// <summary>
        /// Validate Graph after building it using build kmers 
        /// with input reads which will generate clusters in step2 graph
        /// Input: kmers
        /// Output: Graph
        /// </summary>
        [Test]
        [Ignore("Broken test in Travis - never investigated reason")]
        [Category("Padena")]
        public void ValidatePadenaStep2BuildGraphWithClusters()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidatePadenaBuildGraph(Constants.OneLineReadsWithClustersNode, false);
            }
        }

        /// <summary>
        /// Validate Graph after building it using build kmers 
        /// with input reads which will generate clusters in step2 graph
        /// Input: kmers
        /// Output: Graph
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep2BuildGraphWithSmallSizeReads()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidatePadenaBuildGraph(Constants.SmallChromosomeReadsNode, true);
            }
        }

        ///<summary>
        /// Validate Validate DeBruijinGraph properties
        /// Input: kmers
        /// Output: Graph
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep2DeBruijinGraphProperties()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateDeBruijinGraphproperties(Constants.OneLineStep2GraphNode);
            }
        }

        ///<summary>
        /// Validate Validate DeBruijinGraph properties for small size sequence reads.
        /// Input: kmers
        /// Output: Graph
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep2DeBruijinGraphPropertiesForSmallSizeRC()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateDeBruijinGraphproperties(Constants.OneLineReadsWithRCNode);
            }
        }

        /// <summary>
        /// Validate DeBruijinNode ctor by passing dna 
        /// kmersof sequence and graph object of chromosome
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep2DeBruijinCtrByPassingOneLineRC()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateDeBruijnNodeCtor(Constants.OneLineStep2GraphNode);
            }
        }

        /// <summary>
        /// Validate AddLeftExtension() method by 
        /// passing node object and orinetation 
        /// with chromosome read
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep2DeBruijnNodeAddLeftExtensionWithReads()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateDeBruijnNodeAddLeftExtension(Constants.OneLineWithRCStep2Node);
            }
        }

        /// <summary>
        /// Create dbruijn node by passing kmer and create another node.
        /// Add new node as leftendextension of first node. Validate the 
        /// AddRightEndExtension() method.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep2DeBruijnNodeAddRightExtensionWithRCReads()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateDeBruijnNodeAddRightExtension(Constants.OneLineWithRCStep2Node);
            }
        }

        /// <summary>
        /// Validate RemoveExtension() method by passing node 
        /// object and orientation with one line read
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep2DeBruijnNodeRemoveExtensionWithOneLineReads()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateDeBruijnNodeRemoveExtension(Constants.OneLineWithRCStep2Node);
            }
        }

        #endregion

        #region PadenaStep3TestCases

        /// <summary>
        /// Validate the Padena step3 
        /// which removes dangling links from the graph using reads with rc kmers
        /// Input: Graph with dangling links
        /// Output: Graph without any dangling links
        /// </summary>
        [Test]
        [Ignore("Broken in Travis CI - Never investigated reason")]
        [Category("Padena")]
        public void ValidatePadenaStep3UndangleGraphWithRCReads()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidatePadenaUnDangleGraph(Constants.OneLineWithRCStep2Node, true, false);
            }
        }

        /// <summary>
        /// Validate the Padena step3 
        /// which removes dangling links from the graph using virul genome kmers
        /// Input: Graph with dangling links
        /// Output: Graph without any dangling links
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep3UndangleGraphForViralGenomeReads()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidatePadenaUnDangleGraph(Constants.ViralGenomeReadsNode, true, true);
            }
        }

        /// <summary>
        /// Validate the Padena step3 using input reads which will generate clusters in step2 graph
        /// Input: Graph with dangling links
        /// Output: Graph without any dangling links
        /// </summary>
        [Test]
        [Ignore("Broken in Travis CI - Never investigated reason")]
        [Category("Padena")]
        public void ValidatePadenaStep3UndangleGraphWithClusters()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidatePadenaUnDangleGraph(Constants.OneLineReadsWithClustersAfterDangling, false, false);
            }
        }

        /// <summary>
        /// Validate removal of dangling links by passing input reads with 3 dangling links
        /// Input: Graph with 3 dangling links
        /// Output: Graph without any dangling links
        /// </summary>
        [Test]
        [Ignore("Fails in Travis CI - Haven't investigated why")]        
        [Category("Padena")]
        public void ValidatePadenaStep3UndangleGraphWithDanglingLinks()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidatePadenaUnDangleGraph(Constants.ReadsWithDanglingLinksNode, false, false);
            }
        }

        /// <summary>
        /// Validate removal of dangling links by passing input reads with 3 dangling links
        /// Input: Graph with 3 dangling links
        /// Output: Graph without any dangling links
        /// </summary>
        [Test]
        [Ignore("Broken in Travis CI - Never investigated reason")]
        [Category("Padena")]
        public void ValidatePadenaStep3UndangleGraphWithMultipleDanglingLinks()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidatePadenaUnDangleGraph(Constants.ReadsWithMultipleDanglingLinksNode, false, false);
            }
        }


        /// <summary>
        /// Validate the DanglingLinksPurger is removing the dangling link nodes
        /// from the graph
        /// Input: Graph and dangling node
        /// Output: Graph without any dangling nodes
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep3RemoveErrorNodesForSmallSizeReads()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidatePadenaRemoveErrorNodes(Constants.ViralGenomeReadsNode);
            }
        }

        #endregion

        #region PadenaStep4TestCases

        /// <summary>
        /// Validate Padena step4 ParallelDeNovothis.RemoveRedundancy() by passing graph 
        /// using virul genome reads such that it will create bubbles in the graph
        /// Input: Graph with bubbles
        /// Output: Graph without bubbles
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep4RemoveRedundancyForViralGenomeReads()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidatePadenaRemoveRedundancy(Constants.ViralGenomeReadsNode, true, true);
            }
        }

        /// <summary>
        /// Validate Padena step4 ParallelDeNovothis.RemoveRedundancy() by passing graph 
        /// using input reads with rc such that it will create bubbles in the graph
        /// Input: Graph with bubbles
        /// Output: Graph without bubbles
        /// </summary>
        [Test]
        [Ignore("Broken in Travis CI - Never investigated reason")]
        [Category("Padena")]
        public void ValidatePadenaStep4RemoveRedundancyWithRCReads()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidatePadenaRemoveRedundancy(Constants.OneLineWithRCStep2Node, true, false);
            }
        }

        /// <summary>
        /// Validate Padena step4 ParallelDeNovothis.RemoveRedundancy() by passing graph 
        /// using input reads which will generate clusters in step2 graph
        /// Input: Graph with bubbles
        /// Output: Graph without bubbles
        /// </summary>
        [Test]
        [Ignore("Broken in Travis CI - Never investigated reason")]
        [Category("Padena")]
        public void ValidatePadenaStep4RemoveRedundancyWithClusters()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidatePadenaRemoveRedundancy(Constants.OneLineReadsWithClustersNode, true, false);
            }
        }

        /// <summary>
        /// Validate Padena step4 ParallelDeNovothis.RemoveRedundancy() by passing graph 
        /// using input reads which will generate clusters in step2 graph
        /// Input: Graph with bubbles
        /// Output: Graph without bubbles
        /// </summary>
        [Test]
        [Ignore("Broken in Travis CI - Never investigated reason")]
        [Category("Padena")]
        public void ValidatePadenaStep4RemoveRedundancyWithBubbles()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidatePadenaRemoveRedundancy(Constants.ReadsWithBubblesNode, false, false);
            }
        }

        /// <summary>
        /// Validate Padena step4 ParallelDeNovothis.RemoveRedundancy() by passing graph 
        /// using input reads which will generate clusters in step2 graph
        /// Input: Graph with bubbles
        /// Output: Graph without bubbles
        /// </summary>
        [Test]
        [Ignore("Broken in Travis CI - Never investigated reason")]
        [Category("Padena")]
        public void ValidatePadenaStep4RemoveRedundancyWithMultipleBubbles()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidatePadenaRemoveRedundancy(Constants.ReadsWithMultipleBubblesNode, false, false);
            }
        }

        /// <summary>
        /// Validate Padena step4 ParallelDeNovothis.RemoveRedundancy() by passing graph 
        /// using input reads which will generate clusters in step2 graph using Small size reads
        /// Input: Graph with bubbles
        /// Output: Graph without bubbles
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep4RemoveRedundancyWithSmallSizeReads()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidatePadenaRemoveRedundancy(Constants.Step4ReadsWithSmallSize, false, true);
            }
        }

        /// <summary>
        /// Validate Padena step4 RedundantPathPurgerCtor() by passing graph 
        /// using one line reads 
        /// Input: One line graph
        /// Output: Graph without bubbles
        /// </summary>
        [Test]
        [Ignore("Broken in Travis CI - Never investigated reason")]
        [Category("Padena")]
        public void ValidatePadenaStep4RedundantPathPurgerCtorWithOneLineReads()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateRedundantPathPurgerCtor(Constants.Step4RedundantPathReadsNode, false);
            }
        }

        /// <summary>
        /// Validate DetectErrorNodes() by passing graph object with
        /// one line reads such that it has bubbles
        /// Input: One line graph
        /// Output: Graph without bubbles
        /// </summary>
        [Test]
        [Ignore("Broken in Travis CI - Never investigated reason")]
        [Category("Padena")]
        public void ValidatePadenaStep4DetectErrorNodesForOneLineReads()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateRedundantPathPurgerCtor(Constants.Step4RedundantPathReadsNode, false);
            }
        }

        /// <summary>
        /// Validate Padena RemoveErrorNodes() by passing redundant nodes list and graph
        /// Input : graph and redundant nodes list
        /// Output: Graph without bubbles
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep4RemoveErrorNodes()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateRedundantPathPurgerCtor(Constants.OneLineStep4ReadsNode, false);
            }
        }

        #endregion

        #region PadenaStep5TestCases

        /// <summary>
        /// Validate Padena step5 by passing graph and validating the contigs
        /// Input : graph
        /// Output: Contigs
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep5BuildContigsForViralGenomeReads()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateDe2thisBuildContigs(Constants.ViralGenomeReadsNode, true);
            }
        }

        /// <summary>
        /// Validate Padena step5 by passing graph and validating the contigs
        /// Input : graph
        /// Output: Contigs
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep5BuildContigsWithRCReads()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateDe2thisBuildContigs(Constants.OneLineReadsWithRCNode, false);
            }
        }

        /// <summary>
        /// Validate Padena step5 by passing graph and validating the contigs
        /// Input : graph
        /// Output: Contigs
        /// </summary>
        [Test]
        [Ignore("Broken in Travis CI - Never investigated reason")]
        [Category("Padena")]
        public void ValidatePadenaStep5BuildContigsWithClusters()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateDe2thisBuildContigs(Constants.OneLineReadsWithClustersNode, false);
            }
        }

        /// <summary>
        /// Validate Padena step5 by passing graph and validating the
        /// contigs for small size chromosomes
        /// Input : graph
        /// Output: Contigs
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep5BuildContigsForSmallSizeChromosomes()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateDe2thisBuildContigs(Constants.SmallChromosomeReadsNode, true);
            }
        }

        /// <summary>
        /// Validate Padena step5 SimpleContigBuilder.BuildContigs() 
        /// by passing graph for small size chromosome.
        /// Input : graph
        /// Output: Contigs
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep5SimpleContigBuilderBuildContigsForSmallSizeRC()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateSimpleContigBuilderBuild(Constants.ChromosomeReads, true);
            }
        }

        #endregion

        #region PadenaStep6:Step1:TestCases

        /// <summary>
        /// Validate paired reads for Seq Id Starts with ".."
        /// Input : X1,Y1 format map reads with sequence ID contains "..".
        /// Output : Validate forward and backward reads.
        /// </summary>
        [Test]

        [Category("Padena")]
        public void ValidatePadenaStep6PairedReadsForSeqIDWithDots()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidatePairedReads(Constants.ReadsWithDotsNode);
            }
        }

        /// <summary>
        /// Validate paired reads for Seq Id Starts with ".." between
        /// Input : X1,Y1 format map reads with sequence ID contains ".." between.
        /// Output : Validate forward and backward reads.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep6PairedReadsForSeqIDWithDotsBetween()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidatePairedReads(Constants.ReadsWithDotsBetweenSeqIdNode);
            }
        }

        /// <summary>
        /// Validate paired reads for Seq Id Chr1.X1:abc.X1:50K
        /// Input : X1,Y1 format map reads with sequence ID contains "X1 and Y1 letters" between.
        /// Output : Validate forward and backward reads.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep6PairedReadsForSeqIDContainsX1Y1()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidatePairedReads(Constants.OneLineReadsForPairedReadsNode);
            }
        }

        /// <summary>
        /// Validate paired reads for Seq Id with special characters
        /// Input : X1,Y1 format map reads with sequence ID contains "X1 and Y1 letters" between.
        /// Output : Validate forward and backward reads.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep6PairedReadsForSpecialCharsSeqId()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidatePairedReads(Constants.ReadsWithSpecialCharsNode);
            }
        }

        /// <summary>
        /// Validate paired reads for Mixed reads.
        /// Input : X1,Y1,F,R,1,2 format map reads
        /// Output : Validate forward and backward reads.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep6PairedReadsForMixedFormatReads()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidatePairedReads(Constants.ReadsWithDotsNode);
            }
        }

        /// <summary>
        /// Validate paired reads for 2K and 0.5K library.
        /// Input : X1,Y1,F,R,1,2 format map reads
        /// Output : Validate forward and backward reads.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep6PairedReadsForDifferentLibrary()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidatePairedReads(Constants.ReadsWith2KlibraryNode);
            }
        }

        /// <summary>
        /// Validate paired reads for 10K,50K and 100K library.
        /// Input : X1,Y1,F,R,1,2 format map reads
        /// Output : Validate forward and backward reads.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep6PairedReadsFor100kLibrary()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidatePairedReads(Constants.ReadsWith10KAnd50KAnd100KlibraryNode);
            }
        }

        /// <summary>
        /// Validate paired reads for Reads without any Seq Name
        /// Input : X1,Y1,F,R,1,2 format map reads
        /// Output : Validate forward and backward reads.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep6PairedReadsForSeqsWithoutAnyID()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidatePairedReads(Constants.ReadsWithoutAnySeqIdNode);
            }
        }

        /// <summary>
        /// Validate paired reads for Reads with numeric library name.
        /// Input : X1,Y1,F,R,1,2 format map reads
        /// Output : Validate forward and backward reads.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep6PairedReadsForNumericLibraryName()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidatePairedReads(Constants.ReadsWith10KAnd50KAnd100KlibraryNode);
            }
        }

        /// <summary>
        /// Validate Adding new library information to library list.
        /// Input : Library name,Standard deviation and mean length.
        /// Output : Validate forward and backward reads.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep6Libraryinformation()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.AddLibraryInformation(Constants.AddX1AndY1FormatPairedReadsNode, true);
            }
        }

        /// <summary>
        /// Validate library information for 1 and 2 format paired reads.
        /// Input : 1 and 2 format paired reads.
        /// Output : Validate library information.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep6GetLibraryinformation()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.GetLibraryInformation(Constants.GetLibraryInformationNode);
            }
        }

        #endregion PadenaStep6:Step1:TestCases

        #region PadenaStep6:Step2:TestCases

        /// <summary>
        /// Validate ReadContigMapper.Map() using multiple clustalW 
        /// contigs.
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate MapReads to contigs.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        [Ignore("Broken padena test")]
        public void ValidatePadenaStep6MapReadsToContigForClustalW()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateMapReadsToContig(Constants.MapPairedReadsToContigForClustalWContigsNode, true);
            }
        }

        /// <summary>
        /// Validate ReadContigMapper.Map() using Reverse complement contig.
        /// contigs.
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate MapReads to contigs.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        [Ignore("Broken PADENA Test")]
        public void ValidatePadenaStep6MapReadsToContigForUsingReverseComplementContig()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateMapReadsToContig(Constants.MapPairedReadsToContigForReverseComplementContigsNode,
                    true);
            }
        }

        /// <summary>
        /// Validate ReadContigMapper.Map() using left side contig generator.
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate MapReads to contigs.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep6MapReadsToContigForLeftSideContigGenerator()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateMapReadsToContig(Constants.MapPairedReadsToContigForLeftSideContigGeneratorNode,
                    true);
            }
        }

        /// <summary>
        /// Validate ReadContigMapper.Map() using Contigs generated by passing input 
        /// reads from sequence such that one read is sequence and another 
        /// read is reverse complement
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate MapReads to contigs.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep6MapReadsToContigForOneSeqReadAndOtherRevComp()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateMapReadsToContig(Constants.MapPairedReadsToContigForSeqAndRevCompNode,
                 false);
            }
        }

        /// <summary>
        /// Validate ReadContigMapper.Map() using Right side contig generator.
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate MapReads to contigs.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep6MapReadsToContigForRightSideGenerator()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateMapReadsToContig(Constants.MapPairedReadsToContigForRightSideContigGeneratorNode,
                    false);
            }
        }

        #endregion PadenaStep6:Step2:TestCases

        #region PadenaStep6:Step4:TestCases

        /// <summary>
        /// Validate filter Contig Pairs formed in Forward direction with one 
        /// paired read does not support orientation.
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate filter contig pairs.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep6FilterPairedReadsWithFWReadsNotSupportOrientation()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateFilterPaired(Constants.FilterPairedReadContigsForFWOrnNode, true);
            }
        }

        /// <summary>
        /// Validate filter Contig Pairs formed in Reverse direction with one paired
        /// read does not support orientation.
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate filter contig pairs.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep6FilterPairedReadsWithRevReadsNotSupportOrientation()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateFilterPaired(Constants.FilterPairedReadContigsForRevOrientationNode, true);
            }
        }

        /// <summary>
        /// Validate filter Contig Pairs formed in Forward direction and reverse 
        /// complement of Contig
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate filter contig pairs.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep6FilterPairedsForContigRevComplement()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateFilterPaired(
                  Constants.FilterPairedReadContigsForFWDirectionWithRevCompContigNode, false);
            }
        }

        /// <summary>
        /// Validate filter Contig Pairs formed in Backward direction
        /// and reverse complement of Contig
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate filter contig pairs.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep6FilterPairedsForReverseReadAndRevComplement()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateFilterPaired(
                  Constants.FilterPairedReadContigsForBackwardDirectionWithRevCompContig, true);
            }
        }


        #endregion PaDeNaStep6:Step4:TestCases

        #region PadenaStep6:Step5:TestCases

        /// <summary>
        /// Calculate distance for Contig Pairs formed in Forward 
        /// direction with one paired read does not support orientation.
        /// Input : 3-4 Line sequence reads.
        /// Output : Filtered contigs.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep6CalculateDistanceForForwardPairedReads()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateContigDistance(Constants.FilterPairedReadContigsForFWOrnNode);
            }
        }

        /// <summary>
        /// Calculate distance for Contig Pairs formed in Forward 
        /// direction with one paired read does not support orientation.
        /// Input : 3-4 Line sequence reads.
        /// Output : Filtered contigs.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        [Ignore("Broken PADENA Test")]
        public void ValidatePadenaStep6CalculateDistanceForReversePairedReads()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateContigDistance(Constants.FilterPairedReadContigsForRevOrientationNode);
            }
        }

        /// <summary>
        /// Calculate distance for Contig Pairs formed in Forward direction
        /// and reverse complement of Contig
        /// Input : 3-4 Line sequence reads.
        /// Output : Filtered contigs.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        [Ignore("Broken PADENA Test")]
        public void ValidatePadenaStep6CalculateDistanceForForwardPairedReadsWithRevCompl()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateContigDistance(
                  Constants.FilterPairedReadContigsForFWDirectionWithRevCompContigNode);
            }
        }

        /// <summary>
        /// Calculate distance for Contig Pairs formed in Reverse direction
        /// and reverse complement of Contig
        /// Input : 3-4 Line sequence reads.
        /// Output : Standard deviation and distance between contigs.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep6CalculateDistanceForReversePairedReadsWithRevCompl()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateContigDistance(
                  Constants.FilterPairedReadContigsForBackwardDirectionWithRevCompContig);
            }
        }


        #endregion PadenaStep6:Step5:TestCases

        #region PadenaStep6:Step6:TestCases

        /// <summary>
        /// Validate scaffold path for Contig Pairs formed in Forward
        /// direction with all paired reads support orientation using
        /// FindPath(grpah;ContigPairedReads;KmerLength;Depth)
        /// Input : 3-4 Line sequence reads.
        /// Output : Validation of scaffold paths.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        [Ignore("Broken PADENA Test")]
        public void ValidatePadenaStep6ScaffoldPathsForForwardOrientation()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateScaffoldPath(
                  Constants.ScaffoldPathWithForwardOrientationNode);
            }
        }

        /// <summary>
        /// Validate scaffold path for Contig Pairs formed in Reverse
        /// direction with all paired reads support orientation using
        /// FindPath(grpah;ContigPairedReads;KmerLength;Depth).
        /// Input : 3-4 Line sequence reads.
        /// Output : Validation of scaffold paths.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep6ScaffoldPathsForReverseOrientation()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateScaffoldPath(
                  Constants.ScaffoldPathWithReverseOrientationNode);
            }
        }

        /// <summary>
        /// Validate trace path for Contig Pairs formed in
        /// Forward direction and reverse complement of
        /// Contig
        /// Input : Forward read orientation and rev complement.
        /// Output : Validation of scaffold paths.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep6ScaffoldPathsForForwardDirectionAndRevComp()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateScaffoldPath(
                  Constants.ScaffoldPathWithForwardDirectionAndRevComp);
            }
        }

        /// <summary>
        /// Validate trace path for Contig Pairs formed in
        /// Reverse direction and reverse complement of
        /// Contig
        /// Input : Reverse read orientation and rev complement.
        /// Output : Validation of scaffold paths.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep6ScaffoldPathsForReverseDirectionAndRevComp()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateScaffoldPath(
                  Constants.ScaffoldPathWithReverseDirectionAndRevComp);
            }
        }

        /// <summary>
        /// Validate trace path for Contig Pairs formed in
        /// Forward direction and palindrome of
        /// Contig
        /// Input : Forward read orientation and palindrome of contig.
        /// Output : Validation of scaffold paths.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep6ScaffoldPathsForForwardDirectionAndPalContig()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateScaffoldPath(
                  Constants.ScaffoldPathWithForwardDirectionAndPalContig);
            }
        }

        /// <summary>
        /// Validate trace path for Contig Pairs formed in
        /// Reverse direction and palindrome of
        /// Contig
        /// Input : Reverse read orientation and palindrome of contig.
        /// Output : Validation of scaffold paths.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep6ScaffoldPathsForReverseDirectionAndPalContig()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateScaffoldPath(
                  Constants.ScaffoldPathWithReverseDirectionAndPalContig);
            }
        }


        #endregion PadenaStep6:Step6:TestCases

        #region PadenaStep6:Step7/8:TestCases

        /// <summary>
        /// Validate assembled path by passing scaffold paths for
        /// Contig Pairs formed in Forward direction and reverse 
        /// complement of Contig
        /// Input : 3-4 Line sequence reads.
        /// Output : Assembled paths 
        /// </summary>
        [Test]
        [Ignore("TODO: investigate collection argument being empty.")]
        [Category("Padena")]
        public void ValidatePadenaStep6AssembledPathForForwardAndRevComplContig()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateAssembledPath(
                  Constants.AssembledPathForForwardWithReverseCompl);
            }
        }

        /// <summary>
        /// Validate assembled path by passing scaffold paths for
        /// Contig Pairs formed in Reverse direction and reverse 
        /// complement of Contig
        /// Input : 3-4 Line sequence reads.
        /// Output : Assembled paths 
        /// </summary>
        [Test]
        
        [Category("Padena")]
        [Ignore("Broken PADENA Test")]
        public void ValidatePadenaStep6AssembledPathForReverseAndRevComplContig()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateAssembledPath(
                  Constants.AssembledPathForReverseWithReverseCompl);
            }
        }

        /// <summary>
        /// Validate assembled path by passing scaffold for Contig Pairs 
        /// formed in Forward direction and palindrome of Contig
        /// Input : Sequence reads with Palindrome contigs.
        /// Output : Assembled paths 
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep6AssembledPathForForwardAndPalContig()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateAssembledPath(
                  Constants.AssembledPathForForwardAndPalContig);
            }
        }

        /// <summary>
        /// Validate assembled path by passing scaffold for Contig Pairs 
        /// formed in Reverse direction and palindrome of Contig
        /// Input : Sequence reads with Palindrome contigs.
        /// Output : Assembled paths 
        /// </summary>
        [Test]
        [Ignore("Fails in Travis CI - haven't investigated why")]
        [Category("Padena")]
        public void ValidatePadenaStep6AssembledPathForReverseAndPalContig()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateAssembledPath(
                  Constants.AssembledPathForReverseAndPalContig);
            }
        }

        /// <summary>
        /// Validate Scaffold sequence for small size sequence reads.
        /// Input : small size sequence reads.
        /// Output : Validation of Scaffold sequence.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep6ScaffoldSequenceForSmallReads()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidateScaffoldSequence(Constants.ScaffoldSequenceNode);
            }
        }

        /// <summary>
        ///  Validate Assembled sequences with Euler Test data reads,
        ///  Input : Euler testObj data seq reads.
        ///  output : Aligned sequences.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep6AssembledSequenceWithEulerData()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidatePadenaAssembledSeqs(
                    Constants.AssembledSequencesForEulerDataNode);
            }
        }

        /// <summary>
        ///  Validate Assembled sequences for reads formed 
        ///  scaffold paths containing overlapping paths.
        ///  Input : Viral Genome reads.
        ///  output : Aligned sequences.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep6AssembledSequenceForOverlappingScaffoldPaths()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidatePadenaAssembledSeqs(
                    Constants.AssembledSequencesForViralGenomeReadsNode);
            }
        }

        /// <summary>
        ///  Validate Assembled sequences for reads formed 
        ///  contigs in forward and reverse complement contig.
        ///  Input : Sequence reads.
        ///  output : Aligned sequences.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep6AssembledSequenceForForwardAndRevCompl()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidatePadenaAssembledSeqs(
                    Constants.AssembledSequencesForForwardAndRevComplContigNode);
            }
        }

        /// <summary>
        ///  Validate Assembled sequences for reads formed 
        ///  contigs in forward and palindrome contig.
        ///  Input : Sequence reads.
        ///  output : Aligned sequences.
        /// </summary>
        [Test]
        
        [Category("Padena")]
        public void ValidatePadenaStep6AssembledSequenceForForwardAndPalContig()
        {
            using (var testObj = new PadenaP1Test())
            {
                testObj.ValidatePadenaAssembledSeqs(
                    Constants.AssembledSequencesForForwardAndPalContigNode);
            }
        }

        #endregion PadenaStep6:Step7/8:TestCases
    }

    /// <summary>
    /// This class contains helper methods for Padena.
    /// </summary>
    internal class PadenaP1Test : ParallelDeNovoAssembler
    {
        #region Global Variables

        readonly Utility utilityObj = new Utility(@"TestUtils\PaDeNATestData\PaDeNATestsConfig.xml");

        #endregion Global Variables

        #region Helper Methods

        /// <summary>
        /// Validate ParallelDeNovothis step1 Build kmers 
        /// </summary>
        /// <param name="nodeName">xml node for test data</param>
        /// <param name="isSmallSize">Is file small size?</param>
        internal void ValidatePadenaBuildKmers(string nodeName, bool isSmallSize)
        {
            // Read all the input sequences from xml config file
            var filePath = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.FilePathNode).TestDir();
            var kmerLength = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.KmerLengthNode);
            var expectedKmersCount = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.ExpectedKmersCount);

            // Set kmerLength
            KmerLength = int.Parse(kmerLength, (IFormatProvider)null);
            IEnumerable<ISequence> sequenceReads = null;
            var parser = new FastAParser();
            sequenceReads = parser.Parse(filePath).ToList();
            // Set all the input reads and execute build kmers
            SequenceReads.Clear();

            SetSequenceReads(sequenceReads.ToList());
            var lstKmers =
                (new SequenceToKmerBuilder()).Build(SequenceReads, KmerLength);

            if (isSmallSize)
            {
                Assert.AreEqual(expectedKmersCount, lstKmers.Count().ToString((IFormatProvider)null));
            }
            else
            {
                ValidateKmersList(new List<KmersOfSequence>(lstKmers), sequenceReads.ToList(), nodeName);
            }

            ApplicationLog.WriteLine(@"Padena P1 : Validation of Build with all input reads using ParallelDeNovothis sequence completed successfully");
        }

        /// <summary>
        /// Validate the generated kmers using expected output kmer file
        /// </summary>
        /// <param name="lstKmers">generated kmers</param>
        /// <param name="inputReads">input base sequence reads</param>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateKmersList(IList<KmersOfSequence> lstKmers,
            IList<ISequence> inputReads, string nodeName)
        {
            var kmerOutputFile = utilityObj.xmlUtil.GetTextValue(nodeName,
                Constants.KmersOutputFileNode).TestDir();

            Assert.AreEqual(inputReads.Count, lstKmers.Count);

            // Get the array of kmer sequence using kmer positions
            for (var kmerIndex = 0; kmerIndex < lstKmers.Count; kmerIndex++)
            {
                Assert.AreEqual(
                    new string(inputReads[kmerIndex].Select(a => (char)a).ToArray()),
                    new string(lstKmers[kmerIndex].BaseSequence.Select(a => (char)a).ToArray()));
            }

            // Validate all the generated kmer sequence with the expected kmer sequence
            using (var kmerFile = new StreamReader(kmerOutputFile))
            {
                var line = string.Empty;
                var fileContent = new List<string[]>();
                while (null != (line = kmerFile.ReadLine()))
                {
                    fileContent.Add(line.Split(','));
                }

                for (var kmerIndex = 0; kmerIndex < lstKmers.Count; kmerIndex++)
                {
                    var count = 0;
                    var kmers =
                        lstKmers[kmerIndex].Kmers;

                    foreach (var kmer in kmers)
                    {
                        var sequence =
                            lstKmers[kmerIndex].KmerToSequence(kmer);
                        var aab = new string(sequence.Select(a => (char)a).ToArray());
                        Assert.AreEqual(fileContent[kmerIndex][count], aab);
                        count++;
                    }

                }
            }
        }

        /// <summary>
        /// Validate graph generated using ParallelDeNovothis.CreateGraph() with kmers
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        /// <param name="isLargeSizeReads">Is large size reads?</param>
        internal void ValidatePadenaBuildGraph(string nodeName, bool isLargeSizeReads)
        {
            var filePath = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.FilePathNode).TestDir();
            var kmerLength = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.KmerLengthNode);
            var expectedGraphsNodeCount = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.GraphNodesCountNode);

            // Get the input reads and build kmers
            var parser = new FastAParser();
            IEnumerable<ISequence> sequenceReads = parser.Parse(filePath).ToList();

            KmerLength = int.Parse(kmerLength, null);
            SequenceReads.Clear();

            SetSequenceReads(sequenceReads.ToList());
            CreateGraph();
            var graph = Graph;

            ApplicationLog.WriteLine("Padena P1 : Step1,2 Completed Successfully");

            if (isLargeSizeReads)
                Assert.AreEqual(Int32.Parse(expectedGraphsNodeCount, null), graph.GetNodes().Count());
            else
                ValidateGraph(graph, nodeName);
            
            ApplicationLog.WriteLine(@"Padena P1 : ParallelDeNovothis CreateGraph() validation for Padena step2 completed successfully");
        }

        /// <summary>
        /// Validate the graph nodes sequence, left edges and right edges
        /// </summary>
        /// <param name="graph">graph object</param>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateGraph(DeBruijnGraph graph, string nodeName)
        {
            var nodesSequence = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.NodesSequenceNode).TestDir();
            var nodesLeftEdges = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.NodesLeftEdgesCountNode).TestDir();
            var nodesRightEdges = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.NodeRightEdgesCountNode).TestDir();

            var leftEdgesCount = ReadStringFromFile(nodesLeftEdges).Replace("\r\n", "").Split(',');
            var rightEdgesCount = ReadStringFromFile(nodesRightEdges).Replace("\r\n", "").Split(',');
            var nodesSequences = ReadStringFromFile(nodesSequence).Replace("\r\n", "").Split(',');

            // Validate the nodes 
            for (var iseq = 0; iseq < nodesSequences.Length; iseq++)
            {
                var dbnodes = graph.GetNodes().First(n => graph.GetNodeSequence(n).ConvertToString() == nodesSequences[iseq]
                                                                || graph.GetNodeSequence(n).GetReverseComplementedSequence().ConvertToString() == nodesSequences[iseq]);

                //Due to parallelization the left edges and right edges count
                //can be swapped while processing. if actual left edges count 
                //is either equal to expected left edges count or right edges count and vice versa.
                Assert.IsTrue(
                  dbnodes.LeftExtensionNodesCount.ToString((IFormatProvider)null) == leftEdgesCount[iseq] ||
                  dbnodes.LeftExtensionNodesCount.ToString((IFormatProvider)null) == rightEdgesCount[iseq]);
                Assert.IsTrue(
                  dbnodes.RightExtensionNodesCount.ToString((IFormatProvider)null) == leftEdgesCount[iseq] ||
                  dbnodes.RightExtensionNodesCount.ToString((IFormatProvider)null) == rightEdgesCount[iseq]);
            }
        }

        /// <summary>
        /// Get the input string from the file.
        /// </summary>
        /// <param name="filename">input filename</param>
        /// <returns>Reads the file and returns input string</returns>
        static string ReadStringFromFile(string filename)
        {
            string readString = null;
            using (var reader = new StreamReader(filename))
            {
                readString = reader.ReadToEnd();
            }
            return readString;
        }

        /// <summary>
        /// Validate ParallelDeNovothis.RemoveRedundancy() which removes bubbles formed in the graph
        /// and validate the graph
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        /// <param name="defaultThreshold">Is Default Threshold?</param>
        /// <param name="isMicroorganism">Is micro organsm?</param>
        internal void ValidatePadenaRemoveRedundancy(string nodeName, bool defaultThreshold, bool isMicroorganism)
        {
            var filePath = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode).TestDir();
            var kmerLength = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);
            var expectedNodesCount = utilityObj.xmlUtil.GetTextValue(
              nodeName, Constants.ExpectedNodesCountRemoveRedundancy);

            string danglingThreshold = null;
            string pathlengthThreshold = null;
            if (!defaultThreshold)
            {
                danglingThreshold = utilityObj.xmlUtil.GetTextValue(nodeName,
                  Constants.DanglingLinkThresholdNode);
                pathlengthThreshold = utilityObj.xmlUtil.GetTextValue(nodeName,
                  Constants.PathLengthThresholdNode);
            }

            // Get the input reads and build kmers
            IEnumerable<ISequence> sequenceReads = null;
            var parser = new FastAParser();
            sequenceReads = parser.Parse(filePath).ToList();

            // Build kmers from step1,graph in step2 
            // Remove the dangling links from graph in step3
            // Remove bubbles from graph in step4
            // Validate the graph
            if (!defaultThreshold)
            {
                DanglingLinksThreshold = int.Parse(danglingThreshold, (IFormatProvider)null);
                DanglingLinksPurger =
                    new DanglingLinksPurger(DanglingLinksThreshold);
                RedundantPathLengthThreshold = int.Parse(pathlengthThreshold, (IFormatProvider)null);
                RedundantPathsPurger =
                    new RedundantPathsPurger(RedundantPathLengthThreshold);
            }
            else
            {
                DanglingLinksPurger =
                    new DanglingLinksPurger(int.Parse(kmerLength, (IFormatProvider)null));
                RedundantPathsPurger =
                    new RedundantPathsPurger(int.Parse(kmerLength, (IFormatProvider)null) + 1);
            }
            KmerLength = int.Parse(kmerLength, (IFormatProvider)null);
            SequenceReads.Clear();

            SetSequenceReads(sequenceReads.ToList());
            CreateGraph();
            var graph = Graph;

            ApplicationLog.WriteLine("Padena P1 : Step1,2 Completed Successfully");
            UnDangleGraph();

            ApplicationLog.WriteLine("Padena P1 : Step3 Completed Successfully");
            RemoveRedundancy();

            ApplicationLog.WriteLine("Padena P1 : Step4 Completed Successfully");
            if (isMicroorganism)
            {
                Assert.AreEqual(expectedNodesCount, graph.GetNodes().Count().ToString((IFormatProvider)null));
            }
            else
            {
                ValidateGraph(graph, nodeName);
            }

            ApplicationLog.WriteLine(@"Padena P1 :ParallelDeNovothis.RemoveRedundancy() validation for Padena step4 completed successfully");
        }

        /// <summary>
        /// Validate the ParallelDeNovothis unDangleGraph() method which removes the dangling link
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        /// <param name="defaultThreshold">Default Threshold</param>
        /// <param name="smallSizeChromosome">Small size chromosome</param>
        internal void ValidatePadenaUnDangleGraph(string nodeName, bool defaultThreshold, bool smallSizeChromosome)
        {
            var filePath = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.FilePathNode).TestDir();
            var kmerLength = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.KmerLengthNode);
            var expectedNodesCount = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.NodesCountAfterDanglingGraphNode);
            string danglingThreshold = null;
            
            if (!defaultThreshold)
                danglingThreshold = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.DanglingLinkThresholdNode);

            // Get the input reads and build kmers
            IEnumerable<ISequence> sequenceReads = null;
            var parser = new FastAParser();
            sequenceReads = parser.Parse(filePath).ToList();

            // Build kmers from step1,graph in step2 
            KmerLength = int.Parse(kmerLength, null);
            if (!defaultThreshold)
            {
                DanglingLinksThreshold = int.Parse(danglingThreshold, null);
            }
            else
            {
                DanglingLinksThreshold = int.Parse(kmerLength, null) + 1;
            }
            SequenceReads.Clear();

            SetSequenceReads(sequenceReads.ToList());
            CreateGraph();
            var graph = Graph;

            ApplicationLog.WriteLine("Padena P1 : Step1,2 Completed Successfully");
            DanglingLinksPurger = new DanglingLinksPurger(DanglingLinksThreshold);
            UnDangleGraph();

            ApplicationLog.WriteLine("Padena P1 : Step3 Completed Successfully");
            if (smallSizeChromosome)
            {
                Assert.AreEqual(expectedNodesCount, graph.GetNodes().Count().ToString((IFormatProvider)null));
            }
            else
            {
                ValidateGraph(graph, nodeName);
            }

            ApplicationLog.WriteLine(@"Padena P1 :ParallelDeNovothis.UndangleGraph() validation for Padena step3 completed successfully");
        }

        /// <summary>
        /// Validate KmersOfSequence ctor by passing base sequence reads, kmer length and
        /// built kmers
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        internal void ValidateKmersOfSequenceCtorWithBuildKmers(string nodeName)
        {
            var filePath = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.FilePathNode).TestDir();
            var kmerLength = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.KmerLengthNode);

            // Get the input reads
            IEnumerable<ISequence> sequenceReads = null;
            var parser = new FastAParser();
            sequenceReads = parser.Parse(filePath).ToList();

            var builder = new SequenceToKmerBuilder();
            IList<KmersOfSequence> lstKmers = new List<KmersOfSequence>();

            // Validate KmersOfSequence ctor using build kmers
            foreach (var sequence in sequenceReads)
            {
                var kmer = builder.Build(sequence, int.Parse(kmerLength, (IFormatProvider)null));
                var kmerSequence = new KmersOfSequence(sequence,
                    int.Parse(kmerLength, (IFormatProvider)null), kmer.Kmers);
                lstKmers.Add(kmerSequence);
            }

            ValidateKmersList(lstKmers, sequenceReads.ToList(), nodeName);

            ApplicationLog.WriteLine(@"Padena P1 : KmersOfSequence ctor with build kmers method validation completed successfully");
        }

        /// <summary>
        /// Validate KmersOfSequence ctor by passing base sequence reads, kmer length and
        /// built kmers and validate its properties.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        internal void ValidateKmersOfSequenceCtorProperties(string nodeName)
        {
            var filePath = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode).TestDir();
            var kmerLength = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);
            var expectedSeq = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.BaseSequenceNode);
            var expectedKmers = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.KmersCountNode);

            // Get the input reads
            IEnumerable<ISequence> sequenceReads = null;
            var parser = new FastAParser();
            sequenceReads = parser.Parse(filePath).ToList();
            var builder = new SequenceToKmerBuilder();

            var kmer = builder.Build(sequenceReads.ToList()[0],
                int.Parse(kmerLength, (IFormatProvider)null));
            var kmerSequence = new KmersOfSequence(sequenceReads.ToList()[0],
                int.Parse(kmerLength, (IFormatProvider)null), kmer.Kmers);

            // Validate KmerOfSequence properties.
            Assert.AreEqual(expectedSeq, new string(kmerSequence.BaseSequence.Select(a => (char)a).ToArray()));
            Assert.AreEqual(expectedKmers, kmerSequence.Kmers.Count.ToString((IFormatProvider)null));
            ApplicationLog.WriteLine(@"Padena P1 : KmersOfSequence ctor with build kmers method validation completed successfully");
        }

        /// <summary>
        /// Validate KmersOfSequence ToSequences() method which returns kmers sequence
        /// using its positions
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateKmersOfSequenceToSequences(string nodeName)
        {
            var filePath = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.FilePathNode).TestDir();
            var kmerLength = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.KmerLengthNode);
            var kmerOutputFile = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.KmersOutputFileNode).TestDir();

            // Get the input reads and build kmers
            IEnumerable<ISequence> sequenceReads = null;
            var parser = new FastAParser();
            sequenceReads = parser.Parse(filePath).ToList();
            KmerLength = int.Parse(kmerLength, (IFormatProvider)null);
            SequenceReads.Clear();

            SetSequenceReads(sequenceReads.ToList());
            IList<KmersOfSequence> lstKmers = new List<KmersOfSequence>(
                (new SequenceToKmerBuilder()).Build(SequenceReads, KmerLength));

            // Get the array of kmer sequence using ToSequence()
            var index = 0;

            // Validate the generated kmer sequence with the expected output
            using (var kmerFile = new StreamReader(kmerOutputFile))
            {
                var line = string.Empty;
                var fileContent = new List<string[]>();
                while (null != (line = kmerFile.ReadLine()))
                {
                    fileContent.Add(line.Split(','));
                }

                foreach (var sequenceRead in sequenceReads)
                {
                    var count = 0;
                    var kmerSequence = new KmersOfSequence(sequenceRead,
                        int.Parse(kmerLength, (IFormatProvider)null), lstKmers[index].Kmers);
                    var sequences = kmerSequence.KmersToSequences();
                    foreach (var sequence in sequences)
                    {
                        var aab = new string(sequence.Select(a => (char)a).ToArray());
                        Assert.AreEqual(fileContent[index][count], aab);
                        count++;
                    }
                    index++;
                }
            }

            ApplicationLog.WriteLine(@"Padena P1 : KmersOfSequence ToSequences() method validation completed successfully");
        }

        /// <summary>
        /// Validate Validate DeBruijinGraph properties
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateDeBruijinGraphproperties(string nodeName)
        {
            var filePath = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode).TestDir();
            var kmerLength = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);
            var ExpectedNodesCount = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.GraphNodesCountNode);

            // Get the input reads and build kmers
            IEnumerable<ISequence> sequenceReads = null;
            var parser = new FastAParser();
            sequenceReads = parser.Parse(filePath).ToList();

            KmerLength = int.Parse(kmerLength, (IFormatProvider)null);
            SequenceReads.Clear();

            SetSequenceReads(sequenceReads.ToList());
            CreateGraph();
            var graph = Graph;

            ApplicationLog.WriteLine("Padena P1 : Step1,2 Completed Successfully");

            // Validate DeBruijnGraph Properties.
            Assert.AreEqual(ExpectedNodesCount, graph.GetNodes().Count().ToString((IFormatProvider)null));

            ApplicationLog.WriteLine(@"Padena P1 : ParallelDeNovothis CreateGraph() validation for Padena step2 completed successfully");
        }

        /// <summary>
        /// Validate AddLeftEndExtension() method of DeBruijnNode 
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateDeBruijnNodeAddLeftExtension(string nodeName)
        {
            var filePath = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.FilePathNode).TestDir();
            var kmerLength = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.KmerLengthNode);

            // Get the input reads and build kmers
            IEnumerable<ISequence> sequenceReads = null;
            var parser = new FastAParser();
            sequenceReads = parser.Parse(filePath).ToList();

            // Build kmers from step1
            KmerLength = int.Parse(kmerLength, null);
            SequenceReads.Clear();

            SetSequenceReads(sequenceReads.ToList());
            IList<KmersOfSequence> lstKmers = new List<KmersOfSequence>((new SequenceToKmerBuilder()).Build(SequenceReads, KmerLength));

            // Validate the node creation
            // Create node and add left node.
            var seq = SequenceReads.First();
            var kmerData = new KmerData32();
            kmerData.SetKmerData(seq, lstKmers[0].Kmers.First().Positions[0], KmerLength);

            var node = new DeBruijnNode(kmerData, 1);
            kmerData = new KmerData32();
            kmerData.SetKmerData(seq, lstKmers[1].Kmers.First().Positions[0], KmerLength);

            var leftnode = new DeBruijnNode(kmerData, 1);
            node.SetExtensionNode(false, true, leftnode);
            Assert.AreEqual(lstKmers[1].Kmers.First().Count, node.LeftExtensionNodesCount);
            
            ApplicationLog.WriteLine(@"Padena P1 :DeBruijnNode AddLeftExtension() validation for Padena step2 completed successfully");
        }

        /// <summary>
        /// Validate AddRightEndExtension() method of DeBruijnNode 
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateDeBruijnNodeAddRightExtension(string nodeName)
        {
            var filePath = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode).TestDir();
            var kmerLength = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);

            // Get the input reads and build kmers
            IEnumerable<ISequence> sequenceReads = null;
            var parser = new FastAParser();
            sequenceReads = parser.Parse(filePath).ToList();
            // Build kmers from step1
            KmerLength = int.Parse(kmerLength, (IFormatProvider)null);
            SequenceReads.Clear();

            SetSequenceReads(sequenceReads.ToList());
            IList<KmersOfSequence> lstKmers = new List<KmersOfSequence>(
                (new SequenceToKmerBuilder()).Build(SequenceReads, KmerLength));

            // Validate the node creation
            // Create node and add left node.
            var seq = SequenceReads.First();
            var kmerData = new KmerData32();
            kmerData.SetKmerData(seq, lstKmers[0].Kmers.First().Positions[0], KmerLength);

            var node = new DeBruijnNode(kmerData, 1);
            kmerData = new KmerData32();
            kmerData.SetKmerData(seq, lstKmers[1].Kmers.First().Positions[0], KmerLength);

            var rightNode = new DeBruijnNode(kmerData, 1);

            node.SetExtensionNode(true, true, rightNode);

            Assert.AreEqual(lstKmers[1].Kmers.First().Count,
                node.RightExtensionNodesCount);
            
            ApplicationLog.WriteLine(@"Padena P1 :DeBruijnNode AddRightExtension() validation for Padena step2 completed successfully");

        }
        /// <summary>
        /// Validate RemoveExtension() method of DeBruijnNode 
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateDeBruijnNodeRemoveExtension(string nodeName)
        {
            var filePath = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode).TestDir();
            var kmerLength = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);

            // Get the input reads and build kmers
            IEnumerable<ISequence> sequenceReads = null;
            var parser = new FastAParser();
            sequenceReads = parser.Parse(filePath).ToList();

            // Build kmers from step1
            KmerLength = int.Parse(kmerLength, (IFormatProvider)null);
            SequenceReads.Clear();

            SetSequenceReads(sequenceReads.ToList());
            IList<KmersOfSequence> lstKmers = new List<KmersOfSequence>(
                (new SequenceToKmerBuilder()).Build(SequenceReads, KmerLength));

            // Validate the node creation
            // Create node and add left node.
            var seq = SequenceReads.First();
            var kmerData = new KmerData32();
            kmerData.SetKmerData(seq, lstKmers[0].Kmers.First().Positions[0], KmerLength);

            var node = new DeBruijnNode(kmerData, 1);
            kmerData = new KmerData32();
            kmerData.SetKmerData(seq, lstKmers[1].Kmers.First().Positions[0], KmerLength);

            var leftnode = new DeBruijnNode(kmerData, 1);
            var rightnode = new DeBruijnNode(kmerData, 1);

            node.SetExtensionNode(false, true, leftnode);
            node.SetExtensionNode(true, true, rightnode);

            // Validates count before removing right and left extension nodes.
            Assert.AreEqual(lstKmers[1].Kmers.First().Count,
                node.RightExtensionNodesCount);
            Assert.AreEqual(1, node.RightExtensionNodesCount);
            Assert.AreEqual(1, node.LeftExtensionNodesCount);

            // Remove right and left extension nodes.
            node.RemoveExtensionThreadSafe(rightnode);
            node.RemoveExtensionThreadSafe(leftnode);

            // Validate node after removing right and left extensions.
            Assert.AreEqual(0, node.RightExtensionNodesCount);
            Assert.AreEqual(0, node.LeftExtensionNodesCount);

            ApplicationLog.WriteLine(@"Padena P1 :DeBruijnNode AddRightExtension() validation for Padena step2 completed successfully");
        }

        /// <summary>
        /// Validate the DeBruijnNode ctor by passing the kmer and validating 
        /// the node object.
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateDeBruijnNodeCtor(string nodeName)
        {
            var filePath = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode).TestDir();
            var kmerLength = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);
            var nodeExtensionsCount = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.NodeExtensionsCountNode);
            var kmersCount = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.KmersCountNode);
            var leftNodeExtensionCount = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.LeftNodeExtensionsCountNode);
            var rightNodeExtensionCount = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.RightNodeExtensionsCountNode);

            // Get the input reads and build kmers
            IEnumerable<ISequence> sequenceReads = null;
            var parser = new FastAParser();
            sequenceReads = parser.Parse(filePath).ToList();
            // Build the kmers using this
            KmerLength = int.Parse(kmerLength, (IFormatProvider)null);
            SequenceReads.Clear();
            SetSequenceReads(sequenceReads.ToList());
            IList<KmersOfSequence> lstKmers = new List<KmersOfSequence>(
                (new SequenceToKmerBuilder()).Build(SequenceReads, KmerLength));

            // Validate the node creation
            // Create node and add left node.
            var seq = SequenceReads.First();
            var kmerData = new KmerData32();
            kmerData.SetKmerData(seq, lstKmers[0].Kmers.First().Positions[0], KmerLength);

            var node = new DeBruijnNode(kmerData, 1);
            kmerData = new KmerData32();
            kmerData.SetKmerData(seq, lstKmers[1].Kmers.First().Positions[0], KmerLength);

            var leftnode = new DeBruijnNode(kmerData, 1);
            var rightnode = new DeBruijnNode(kmerData, 1);

            node.SetExtensionNode(false, true, leftnode);
            node.SetExtensionNode(true, true, rightnode);

            // Validate DeBruijnNode class properties.
            Assert.AreEqual(nodeExtensionsCount, node.ExtensionsCount.ToString((IFormatProvider)null));
            Assert.AreEqual(kmersCount, node.KmerCount.ToString((IFormatProvider)null));
            Assert.AreEqual(leftNodeExtensionCount, node.LeftExtensionNodesCount.ToString((IFormatProvider)null));
            Assert.AreEqual(rightNodeExtensionCount, node.RightExtensionNodesCount.ToString((IFormatProvider)null));
            Assert.AreEqual(leftNodeExtensionCount, node.LeftExtensionNodesCount.ToString((IFormatProvider)null));
            Assert.AreEqual(rightNodeExtensionCount, node.RightExtensionNodesCount.ToString((IFormatProvider)null));

            ApplicationLog.WriteLine("Padena P1 : DeBruijnNode ctor() validation for Padena step2 completed successfully");
        }

        /// <summary>
        /// Validate RemoveErrorNodes() method is removing dangling nodes from the graph
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidatePadenaRemoveErrorNodes(string nodeName)
        {
            var filePath = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode).TestDir();
            var kmerLength = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);

            // Get the input reads and build kmers
            IEnumerable<ISequence> sequenceReads = null;
            var parser = new FastAParser();
            sequenceReads = parser.Parse(filePath).ToList();

            // Build kmers from step1,graph in step2 
            // and remove the dangling links from graph in step3
            // Validate the graph
            KmerLength = int.Parse(kmerLength, (IFormatProvider)null);
            SequenceReads.Clear();
            SetSequenceReads(sequenceReads.ToList());
            CreateGraph();
            var graph = Graph;

            // Find the dangling nodes and remove the dangling node
            var danglingLinksPurger =
                new DanglingLinksPurger(int.Parse(kmerLength, (IFormatProvider)null) + 1);
            var danglingnodes = danglingLinksPurger.DetectErroneousNodes(graph);
            danglingLinksPurger.RemoveErroneousNodes(graph, danglingnodes);
            Assert.IsFalse(graph.GetNodes().Contains(danglingnodes.Paths[0].PathNodes[0]));

            ApplicationLog.WriteLine(@"Padena P1 :DeBruijnGraph.RemoveErrorNodes() validation for Padena step3 completed successfully");
        }

        /// <summary>
        /// Creates RedundantPathPurger instance by passing pathlength and count. Detect 
        /// redundant error nodes and remove these nodes from the graph. Validate the graph.
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        /// <param name="isMicroOrganism">Is micro organism</param>    
        internal void ValidateRedundantPathPurgerCtor(string nodeName, bool isMicroOrganism)
        {
            var filePath = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.FilePathNode).TestDir();
            var kmerLength = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.KmerLengthNode);
            var expectedNodesCount = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.ExpectedNodesCountAfterDangling);

            // Get the input reads and build kmers
            IEnumerable<ISequence> sequenceReads = null;
            var parser = new FastAParser();
            sequenceReads = parser.Parse(filePath).ToList();

            // Build kmers from step1,graph in step2 
            // Remove the dangling links from graph in step3
            // Validate the graph
            KmerLength = int.Parse(kmerLength, (IFormatProvider)null);
            SequenceReads.Clear();
            SetSequenceReads(sequenceReads.ToList());
            CreateGraph();
            var graph = Graph;
            DanglingLinksPurger = new DanglingLinksPurger(KmerLength);
            UnDangleGraph();

            // Create RedundantPathPurger instance, detect redundant nodes and remove error nodes
            var redundantPathPurger =
                new RedundantPathsPurger(int.Parse(kmerLength, (IFormatProvider)null) + 1);
            var redundantnodelist = redundantPathPurger.DetectErroneousNodes(graph);
            redundantPathPurger.RemoveErroneousNodes(graph, redundantnodelist);

            if (isMicroOrganism)
                Assert.AreEqual(expectedNodesCount, graph.GetNodes().Count());
            else
                ValidateGraph(graph, nodeName);
            
            ApplicationLog.WriteLine(@"Padena P1 :RedundantPathsPurger ctor and methods validation for Padena step4 completed successfully");
        }

        /// <summary>
        /// Validate ParallelDeNovothis.BuildContigs() by passing graph object
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        /// <param name="isChromosomeRC">Is chromosome RC?</param>
        internal void ValidateDe2thisBuildContigs(string nodeName, bool isChromosomeRC)
        {
            var filePath = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode).TestDir();
            var kmerLength = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);
            var expectedContigsString = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.ContigsNode);
            string[] expectedContigs;
            if (!expectedContigsString.ToUpper(CultureInfo.InstalledUICulture).Contains("PADENATESTDATA"))
                expectedContigs = expectedContigsString.Split(',');
            else
                expectedContigs = ReadStringFromFile(expectedContigsString.TestDir()).Replace("\r\n", "").Split(',');

            var expectedContigsCount = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.ContigsCount);

            // Get the input reads and build kmers
            IEnumerable<ISequence> sequenceReads = null;
            var parser = new FastAParser();
            sequenceReads = parser.Parse(filePath).ToList();

            // Build kmers from step1,graph in step2 
            // Remove the dangling links from graph in step3
            // Remove bubbles form the graph in step4 
            // Pass the graph and build contigs
            // Validate the contigs
            KmerLength = int.Parse(kmerLength, (IFormatProvider)null);
            SequenceReads.Clear();
            SetSequenceReads(sequenceReads.ToList());
            CreateGraph();
            DanglingLinksPurger = new DanglingLinksPurger(KmerLength);
            UnDangleGraph();
            RedundantPathsPurger = new RedundantPathsPurger(KmerLength + 1);
            RemoveRedundancy();
            ContigBuilder = new SimplePathContigBuilder();
            IList<ISequence> contigs = BuildContigs().ToList();

            // Validate contigs count only for Chromosome files. 
            if (isChromosomeRC)
            {
                Assert.AreEqual(expectedContigsCount, contigs.Count.ToString((IFormatProvider)null));
            }
            // validate all contigs of a sequence.
            else
            {
                for (var index = 0; index < contigs.Count(); index++)
                {
                    Assert.IsTrue(expectedContigs.Contains(new string(contigs[index].Select(a => (char)a).ToArray())) ||
                        expectedContigs.Contains(new string(contigs[index].GetReverseComplementedSequence().Select(a => (char)a).ToArray())));
                }
            }

            ApplicationLog.WriteLine(@"Padena P1 :ParallelDeNovothis.BuildContigs() validation for Padena step5 completed successfully");
        }

        /// <summary>
        /// Validate the SimpleContigBuilder Build() method using step 4 graph
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        /// <param name="isChromosomeRC">Is Chromosome RC?</param>
        internal void ValidateSimpleContigBuilderBuild(string nodeName, bool isChromosomeRC)
        {
            var filePath = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode).TestDir();
            var kmerLength = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);
            var expectedContigsString = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.ContigsNode);
            var expectedContigs = expectedContigsString.Split(',');
            var expectedContigsCount = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.ContigsCount);

            // Get the input reads and build kmers
            IEnumerable<ISequence> sequenceReads = null;
            var parser = new FastAParser();
            sequenceReads = parser.Parse(filePath).ToList();

            // Build kmers from step1,graph in step2 
            // Remove the dangling links from graph in step3
            // Remove bubbles from graph in step4
            KmerLength = int.Parse(kmerLength, (IFormatProvider)null);
            SequenceReads.Clear();
            SetSequenceReads(sequenceReads.ToList());
            CreateGraph();
            var graph = Graph;
            UnDangleGraph();
            RemoveRedundancy();

            // Validate the SimpleContigBuilder.Build() by passing graph
            var builder = new SimplePathContigBuilder();
            IList<ISequence> contigs = builder.Build(graph).ToList();

            if (isChromosomeRC)
            {
                Assert.AreEqual(expectedContigsCount,
                    contigs.Count.ToString((IFormatProvider)null));
            }
            else
            {
                // Validate the contigs
                for (var index = 0; index < contigs.Count; index++)
                {
                    Assert.IsTrue(expectedContigs.Contains(new string(contigs[index].Select(a => (char)a).ToArray())));
                }
            }

            ApplicationLog.WriteLine(@"Padena P1 :SimpleContigBuilder.BuildContigs() validation for Padena step5 completed successfully");
        }

        /// <summary>
        /// Validate Map paired reads for a sequence reads.
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidatePairedReads(string nodeName)
        {
            var filePath = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode).TestDir();
            var expectedPairedReadsCount = utilityObj.xmlUtil.GetTextValue(
              nodeName, Constants.PairedReadsCountNode);
            var backwardReadsNode = utilityObj.xmlUtil.GetTextValues(nodeName,
              Constants.BackwardReadsNode);
            var forwardReadsNode = utilityObj.xmlUtil.GetTextValues(nodeName,
              Constants.ForwardReadsNode);
            var expectedLibrary = utilityObj.xmlUtil.GetTextValues(nodeName,
              Constants.LibraryNode);
            var expectedMean = utilityObj.xmlUtil.GetTextValues(nodeName,
              Constants.MeanLengthNode);
            var deviationNode = utilityObj.xmlUtil.GetTextValues(nodeName,
              Constants.DeviationNode);

            IList<ISequence> sequenceReads = new List<ISequence>();
            IList<MatePair> pairedreads = new List<MatePair>();

            // Get the input reads 
            var parser = new FastAParser();
            sequenceReads.AddRange(parser.Parse(filePath));

            // Convert reads to map paired reads.
            var pair = new MatePairMapper();
            pairedreads = pair.Map(sequenceReads);

            // Validate Map paired reads.
            Assert.AreEqual(expectedPairedReadsCount,
                pairedreads.Count.ToString((IFormatProvider)null));

            for (var index = 0; index < pairedreads.Count; index++)
            {
                Assert.IsTrue(forwardReadsNode.Contains(new string(pairedreads[index].GetForwardRead(sequenceReads).Select(a => (char)a).ToArray())));
                Assert.IsTrue(backwardReadsNode.Contains(new string(pairedreads[index].GetReverseRead(sequenceReads).Select(a => (char)a).ToArray())));
                Assert.IsTrue(deviationNode.Contains(pairedreads[index].StandardDeviationOfLibrary.ToString((IFormatProvider)null)));
                Assert.IsTrue(expectedMean.Contains(pairedreads[index].MeanLengthOfLibrary.ToString((IFormatProvider)null)));
                Assert.IsTrue(expectedLibrary.Contains(pairedreads[index].Library.ToString((IFormatProvider)null)));
            }

            ApplicationLog.WriteLine(@"Padena P1 : Map paired reads has been verified successfully");
        }

        /// <summary>
        /// Validate library information
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void GetLibraryInformation(string nodeName)
        {
            var filePath = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode).TestDir();
            var expectedPairedReadsCount = utilityObj.xmlUtil.GetTextValue(
              nodeName, Constants.PairedReadsCountNode);
            var expectedLibraray = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.LibraryName);
            var expectedStdDeviation = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.StdDeviation);
            var mean = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.Mean);

            IList<MatePair> pairedreads = new List<MatePair>();

            // Get the input reads 
            IEnumerable<ISequence> sequences = null;
            var parser = new FastAParser();
            sequences = parser.Parse(filePath).ToList();

            // Convert reads to map paired reads.
            var pair = new MatePairMapper();
            pairedreads = pair.Map(new List<ISequence>(sequences));

            // Validate Map paired reads.
            Assert.AreEqual(expectedPairedReadsCount,
                pairedreads.Count.ToString((IFormatProvider)null));

            // Get library infomration and validate
            var libraryInfo =
                CloneLibrary.Instance.GetLibraryInformation
                (pairedreads[0].Library);

            Assert.AreEqual(expectedStdDeviation, libraryInfo.StandardDeviationOfInsert.ToString((IFormatProvider)null));
            Assert.AreEqual(expectedLibraray, libraryInfo.LibraryName.ToString((IFormatProvider)null));
            Assert.AreEqual(mean, libraryInfo.MeanLengthOfInsert.ToString((IFormatProvider)null));

            ApplicationLog.WriteLine(@"Padena P1 : Map paired reads has been verified successfully");
        }

        /// <summary>
        /// Validate Add library information in existing libraries.
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        /// <param name="IsLibraryInfo">Is library info?</param>
        internal void AddLibraryInformation(string nodeName, bool IsLibraryInfo)
        {
            var filePath = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode).TestDir();
            var expectedPairedReadsCount = utilityObj.xmlUtil.GetTextValue(
              nodeName, Constants.PairedReadsCountNode);
            var backwardReadsNode = utilityObj.xmlUtil.GetTextValues(nodeName,
              Constants.BackwardReadsNode);
            var forwardReadsNode = utilityObj.xmlUtil.GetTextValues(nodeName,
              Constants.ForwardReadsNode);
            var expectedLibraray = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.LibraryName);
            var expectedStdDeviation = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.StdDeviation);
            var mean = utilityObj.xmlUtil.GetTextValue(nodeName,
              Constants.Mean);

            IList<ISequence> sequenceReads = new List<ISequence>();
            IList<MatePair> pairedreads = new List<MatePair>();

            // Get the input reads 
            var parser = new FastAParser();
            sequenceReads.AddRange(parser.Parse(filePath));

            // Add a new library infomration.
            if (IsLibraryInfo)
            {
                var libraryInfo =
                    new CloneLibraryInformation();
                libraryInfo.LibraryName = expectedLibraray;
                libraryInfo.MeanLengthOfInsert = float.Parse(mean, (IFormatProvider)null);
                libraryInfo.StandardDeviationOfInsert = float.Parse(expectedStdDeviation, (IFormatProvider)null);
                CloneLibrary.Instance.AddLibrary(libraryInfo);
            }
            else
            {
                CloneLibrary.Instance.AddLibrary(expectedLibraray,
                    float.Parse(mean, (IFormatProvider)null), float.Parse(expectedStdDeviation, (IFormatProvider)null));
            }

            // Convert reads to map paired reads.
            var pair = new MatePairMapper();
            pairedreads = pair.Map(sequenceReads);

            // Validate Map paired reads.
            Assert.AreEqual(expectedPairedReadsCount, pairedreads.Count.ToString((IFormatProvider)null));

            for (var index = 0; index < pairedreads.Count; index++)
            {
                Assert.IsTrue(forwardReadsNode.Contains(new string(pairedreads[index].GetForwardRead(sequenceReads).Select(a => (char)a).ToArray())));
                Assert.IsTrue(backwardReadsNode.Contains(new string(pairedreads[index].GetReverseRead(sequenceReads).Select(a => (char)a).ToArray())));
                Assert.AreEqual(expectedStdDeviation,
                    pairedreads[index].StandardDeviationOfLibrary.ToString((IFormatProvider)null));
                Assert.AreEqual(expectedLibraray, pairedreads[index].Library.ToString((IFormatProvider)null));
                Assert.AreEqual(mean, pairedreads[index].MeanLengthOfLibrary.ToString((IFormatProvider)null));
            }

            ApplicationLog.WriteLine(@"Padena P1 : Map paired reads has been verified successfully");
        }

        /// <summary>
        /// Validate building map reads to contigs.
        /// </summary>
        /// <param name="nodeName">xml node name used for a different testcases</param>
        /// <param name="isFullOverlap">True if full overlap else false</param>
        internal void ValidateMapReadsToContig(string nodeName, bool isFullOverlap)
        {
            var filePath = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.FilePathNode).TestDir();
            var kmerLength = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.KmerLengthNode);
            var daglingThreshold = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.DanglingLinkThresholdNode);
            var redundantThreshold = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.RedundantThreshold);
            var readMapLengthString = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.ReadMapLength);
            var readStartPosString = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.ReadStartPos);
            var contigStartPosString = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.ContigStartPos);

            var expectedReadmapLength = readMapLengthString.Split(',');
            var expectedReadStartPos = readStartPosString.Split(',');
            var expectedContigStartPos = contigStartPosString.Split(',');

            // Get the input reads and build kmers
            var parser = new FastAParser();
            IEnumerable<ISequence> sequenceReads = parser.Parse(filePath).ToList();

            // Build kmers from step1,graph in step2 
            // Remove the dangling links from graph in step3
            // Remove bubbles form the graph in step4 
            // Pass the graph and build contigs
            // Validate contig reads
            KmerLength = Int32.Parse(kmerLength, null);
            DanglingLinksThreshold = Int32.Parse(daglingThreshold, null);
            DanglingLinksPurger = new DanglingLinksPurger(Int32.Parse(daglingThreshold, null));
            RedundantPathsPurger = new RedundantPathsPurger(Int32.Parse(redundantThreshold, null));
            RedundantPathLengthThreshold = Int32.Parse(redundantThreshold, null);

            SetSequenceReads(sequenceReads.ToList());
            CreateGraph();
            UnDangleGraph();
            ContigBuilder = new SimplePathContigBuilder();
            RemoveRedundancy();

            var contigs = BuildContigs();

            var sortedContigs = SortContigsData(contigs.ToList());
            var mapper = new ReadContigMapper();
            var maps = mapper.Map(sortedContigs, sequenceReads, KmerLength);

            Assert.AreEqual(maps.Count, sequenceReads.Count());

            var readMaps = maps[sequenceReads.ToList()[0].ID];

            for (var i = 0; i < SortContigsData(readMaps.Keys.ToList()).Count; i++)
            {
                var readMap = readMaps[SortContigsData(readMaps.Keys.ToList())[i]];

                if (isFullOverlap)
                {
                    Assert.AreEqual(expectedReadmapLength[i], readMap[0].Length.ToString((IFormatProvider)null), "readMap failed for pos " + i);
                    Assert.AreEqual(expectedContigStartPos[i], readMap[0].StartPositionOfContig.ToString((IFormatProvider)null), "contigStart failed for pos " + i);
                    Assert.AreEqual(expectedReadStartPos[i], readMap[0].StartPositionOfRead.ToString((IFormatProvider)null), "readStart failed for pos " + i);
                    Assert.AreEqual(readMap[0].ReadOverlap, ContigReadOverlapType.FullOverlap);
                }
                else
                {
                    Assert.AreEqual(readMap[0].ReadOverlap, ContigReadOverlapType.PartialOverlap);
                    break;
                }
            }

            ApplicationLog.WriteLine("PADENA P1 :ReadContigMapper.Map() validation for Padena step6:step2 completed successfully");
        }

        /// <summary>
        /// Validate Filter contig nodes.
        /// </summary>
        /// <param name="nodeName">xml node name used for a differnt testcase.</param>
        /// <param name="isFirstContig">Is First Contig?</param>
        internal void ValidateFilterPaired(string nodeName, bool isFirstContig)
        {
            var filePath = utilityObj.xmlUtil.GetTextValue(
              nodeName, Constants.FilePathNode).TestDir();
            var kmerLength = utilityObj.xmlUtil.GetTextValue(
              nodeName, Constants.KmerLengthNode);
            var daglingThreshold = utilityObj.xmlUtil.GetTextValue(
              nodeName, Constants.DanglingLinkThresholdNode);
            var redundantThreshold = utilityObj.xmlUtil.GetTextValue(
              nodeName, Constants.RedundantThreshold);
            var expectedContigPairedReadsCount = utilityObj.xmlUtil.GetTextValue(
              nodeName, Constants.ContigPairedReadsCount);
            var forwardReadStartPos = utilityObj.xmlUtil.GetTextValue(
              nodeName, Constants.ForwardReadStartPos);
            var reverseReadStartPos = utilityObj.xmlUtil.GetTextValue(
              nodeName, Constants.ReverseReadStartPos);
            var reverseComplementStartPos = utilityObj.xmlUtil.GetTextValue(
              nodeName, Constants.RerverseReadReverseCompPos);
            var expectedForwardReadStartPos = forwardReadStartPos.Split(',');
            var expectedReverseReadStartPos = reverseReadStartPos.Split(',');
            var expectedReverseComplementStartPos = reverseComplementStartPos.Split(',');

            // Get the input reads and build kmers
            IEnumerable<ISequence> sequenceReads = null;
            var parser = new FastAParser();
            sequenceReads = parser.Parse(filePath).ToList();

            // Build kmers from step1,graph in step2 
            // Remove the dangling links from graph in step3
            // Remove bubbles form the graph in step4 
            // Pass the graph and build contigs
            // Validate contig reads
            KmerLength = Int32.Parse(kmerLength, (IFormatProvider)null);
            DanglingLinksThreshold = Int32.Parse(daglingThreshold, (IFormatProvider)null);
            DanglingLinksPurger = new DanglingLinksPurger(Int32.Parse(daglingThreshold, (IFormatProvider)null));
            RedundantPathsPurger =
                new RedundantPathsPurger(Int32.Parse(redundantThreshold, (IFormatProvider)null));
            RedundantPathLengthThreshold = Int32.Parse(redundantThreshold, (IFormatProvider)null);
            SequenceReads.Clear();

            SetSequenceReads(sequenceReads.ToList());
            CreateGraph();
            UnDangleGraph();

            // Build contig.
            ContigBuilder = new SimplePathContigBuilder();
            RemoveRedundancy();
            var contigs = BuildContigs();

            var sortedContigs = SortContigsData(contigs.ToList());
            var mapper = new ReadContigMapper();

            var maps = mapper.Map(
                sortedContigs, sequenceReads, KmerLength);

            // Find map paired reads.
            var mapPairedReads = new MatePairMapper();
            var pairedReads = mapPairedReads.MapContigToMatePairs(sequenceReads, maps);

            // Filter contigs based on the orientation.
            var filter = new OrientationBasedMatePairFilter();
            var contigpairedReads = filter.FilterPairedReads(pairedReads, 0);


            Assert.AreEqual(expectedContigPairedReadsCount,
                contigpairedReads.Values.Count.ToString((IFormatProvider)null));

            Dictionary<ISequence, IList<ValidMatePair>> map = null;
            IList<ValidMatePair> valid = null;
            var firstSeq = sortedContigs[0];
            var secondSeq = sortedContigs[1];
            // Validate Contig paired reads after filtering contig sequences.
            if (isFirstContig)
            {

                map = contigpairedReads[firstSeq];
                valid = SortPairedReads(map[secondSeq], sequenceReads);
            }
            else
            {
                map = contigpairedReads[secondSeq];
                valid = SortPairedReads(map[firstSeq], sequenceReads);
            }

            for (var index = 0; index < valid.Count; index++)
            {
                Assert.IsTrue((expectedForwardReadStartPos[index] ==
                        valid[index].ForwardReadStartPosition[0].ToString((IFormatProvider)null)
                        || (expectedForwardReadStartPos[index] ==
                        valid[index].ForwardReadStartPosition[1].ToString((IFormatProvider)null))));

                if (valid[index].ReverseReadReverseComplementStartPosition.Count > 1)
                {
                    Assert.IsTrue((expectedReverseReadStartPos[index] ==
                        valid[index].ReverseReadReverseComplementStartPosition[0].ToString((IFormatProvider)null)
                        || (expectedReverseReadStartPos[index] ==
                        valid[index].ReverseReadReverseComplementStartPosition[1].ToString((IFormatProvider)null))));
                }

                if (valid[index].ReverseReadStartPosition.Count > 1)
                {
                    Assert.IsTrue((expectedReverseComplementStartPos[index] ==
                        valid[index].ReverseReadStartPosition[0].ToString((IFormatProvider)null)
                        || (expectedReverseComplementStartPos[index] ==
                        valid[index].ReverseReadStartPosition[1].ToString((IFormatProvider)null))));
                }
            }

            ApplicationLog.WriteLine("PADENA P1 : FilterPairedReads() validation for Padena step6:step4 completed successfully");
        }

        /// <summary>
        /// Validate FilterPairedRead.FilterPairedRead() by passing graph object
        /// </summary>
        /// <param name="nodeName">xml node name used for a differnt testcase.</param>
        internal void ValidateContigDistance(string nodeName)
        {
            var filePath = utilityObj.xmlUtil.GetTextValue(
              nodeName, Constants.FilePathNode).TestDir();
            var kmerLength = utilityObj.xmlUtil.GetTextValue(
              nodeName, Constants.KmerLengthNode);
            var daglingThreshold = utilityObj.xmlUtil.GetTextValue(
              nodeName, Constants.DanglingLinkThresholdNode);
            var redundantThreshold = utilityObj.xmlUtil.GetTextValue(
              nodeName, Constants.RedundantThreshold);
            var expectedContigPairedReadsCount = utilityObj.xmlUtil.GetTextValue(
              nodeName, Constants.ContigPairedReadsCount);
            var distanceBetweenFirstContigs = utilityObj.xmlUtil.GetTextValue(
              nodeName, Constants.DistanceBetweenFirstContig);
            var distanceBetweenSecondContigs = utilityObj.xmlUtil.GetTextValue(
              nodeName, Constants.DistanceBetweenSecondContig);
            var firstStandardDeviation = utilityObj.xmlUtil.GetTextValue(
              nodeName, Constants.FirstContigStandardDeviation);
            var secondStandardDeviation = utilityObj.xmlUtil.GetTextValue(
              nodeName, Constants.SecondContigStandardDeviation);

            // Get the input reads and build kmers
            IEnumerable<ISequence> sequenceReads = null;
            var parser = new FastAParser();
            sequenceReads = parser.Parse(filePath).ToList();

            // Build kmers from step1,graph in step2 
            // Remove the dangling links from graph in step3
            // Remove bubbles form the graph in step4 
            // Pass the graph and build contigs
            // Validate contig reads
            KmerLength = Int32.Parse(kmerLength, (IFormatProvider)null);
            DanglingLinksThreshold = Int32.Parse(daglingThreshold, (IFormatProvider)null);
            DanglingLinksPurger =
                new DanglingLinksPurger(Int32.Parse(daglingThreshold, (IFormatProvider)null));
            RedundantPathsPurger =
                new RedundantPathsPurger(Int32.Parse(redundantThreshold, (IFormatProvider)null));
            RedundantPathLengthThreshold = Int32.Parse(redundantThreshold, (IFormatProvider)null);
            SequenceReads.Clear();

            SetSequenceReads(sequenceReads.ToList());
            CreateGraph();
            UnDangleGraph();

            // Build contig.
            ContigBuilder = new SimplePathContigBuilder();
            RemoveRedundancy();
            var contigs = BuildContigs();

            var sortedContigs = SortContigsData(contigs.ToList());
            var mapper = new ReadContigMapper();

            var maps = mapper.Map(sortedContigs, sequenceReads, KmerLength);

            // Find map paired reads.
            var mapPairedReads = new MatePairMapper();
            var pairedReads = mapPairedReads.MapContigToMatePairs(sequenceReads, maps);

            // Filter contigs based on the orientation.
            var filter = new OrientationBasedMatePairFilter();
            var contigpairedReads = filter.FilterPairedReads(pairedReads, 0);

            // Calculate the distance between contigs.
            var calc = new DistanceCalculator(contigpairedReads);
            calc.CalculateDistance();
            Assert.AreEqual(expectedContigPairedReadsCount,
                contigpairedReads.Values.Count.ToString((IFormatProvider)null));

            Dictionary<ISequence, IList<ValidMatePair>> map;
            IList<ValidMatePair> valid;
            var firstSeq = sortedContigs[0];
            var secondSeq = sortedContigs[1];
            if (contigpairedReads.ContainsKey(firstSeq))
            {
                map = contigpairedReads[firstSeq];
            }
            else
            {
                map = contigpairedReads[secondSeq];
            }

            if (map.ContainsKey(firstSeq))
            {
                valid = map[firstSeq];
            }
            else
            {
                valid = map[secondSeq];
            }

            // Validate distance and standard deviation between contigs.
            Assert.AreEqual(float.Parse(distanceBetweenFirstContigs, (IFormatProvider)null),
                valid.First().DistanceBetweenContigs[0]);
            Assert.AreEqual(float.Parse(distanceBetweenSecondContigs, (IFormatProvider)null),
                valid.First().DistanceBetweenContigs[1]);
            Assert.AreEqual(float.Parse(firstStandardDeviation, (IFormatProvider)null),
                valid.First().StandardDeviation[0]);
            Assert.AreEqual(float.Parse(secondStandardDeviation, (IFormatProvider)null),
                valid.First().StandardDeviation[1]);
            
            ApplicationLog.WriteLine("PADENA P1 : DistanceCalculator() validation for Padena step6:step5 completed successfully");
        }

        /// <summary>
        /// Validate Assembled paths for a given input reads.
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateAssembledPath(string nodeName)
        {
            var filePath = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.FilePathNode).TestDir();
            var kmerLength = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.KmerLengthNode);
            var daglingThreshold = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.DanglingLinkThresholdNode);
            var redundantThreshold = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.RedundantThreshold);
            var library = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.LibraryName);
            var StdDeviation = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.StdDeviation);
            var mean = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.Mean);
            var expectedDepth = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.DepthNode);
            var assembledPath = utilityObj.xmlUtil.GetTextValues(nodeName, Constants.SequencePathNode);

            // Get the input reads and build kmers
            var parser = new FastAParser();
            IEnumerable<ISequence> sequenceReads = parser.Parse(filePath).ToList();

            // Build kmers from step1,graph in step2 
            // Remove the dangling links from graph in step3
            // Remove bubbles form the graph in step4 
            // Pass the graph and build contigs
            // Validate contig reads

            KmerLength = Int32.Parse(kmerLength, null);
            DanglingLinksThreshold = Int32.Parse(daglingThreshold, null);
            DanglingLinksPurger = new DanglingLinksPurger(Int32.Parse(daglingThreshold, null));
            RedundantPathsPurger = new RedundantPathsPurger(Int32.Parse(redundantThreshold, null));
            RedundantPathLengthThreshold = Int32.Parse(redundantThreshold, null);
            SequenceReads.Clear();

            SetSequenceReads(sequenceReads.ToList());
            CreateGraph();
            var graph = new ContigGraph();
            UnDangleGraph();

            // Build contig.
            ContigBuilder = new SimplePathContigBuilder();
            RemoveRedundancy();
            var contigs = BuildContigs();

            var sortedContigs = SortContigsData(contigs.ToList());
            var mapper = new ReadContigMapper();

            var maps = mapper.Map(sortedContigs, sequenceReads, KmerLength);

            // Find map paired reads.
            CloneLibrary.Instance.AddLibrary(library, float.Parse(mean, null), float.Parse(StdDeviation, null));
            var mapPairedReads = new MatePairMapper();
            var pairedReads = mapPairedReads.MapContigToMatePairs(sequenceReads, maps);

            // Filter contigs based on the orientation.
            var filter = new OrientationBasedMatePairFilter();
            var contigpairedReads = filter.FilterPairedReads(pairedReads, 0);

            var dist = new DistanceCalculator(contigpairedReads);
            dist.CalculateDistance();
            graph.BuildContigGraph(contigs.ToList(), KmerLength);

            // Validate ScaffoldPath using BFS.
            var trace = new TracePath();
            var paths = trace.FindPaths(graph, contigpairedReads, Int32.Parse(kmerLength, null),
                                                        Int32.Parse(expectedDepth, null));

            // Assemble paths.
            var pathsAssembler = new PathPurger();
            pathsAssembler.PurgePath(paths);

            // Get sequences from assembled path.
            IList<ISequence> seqList = paths.Select(temp => temp.BuildSequenceFromPath(graph, Int32.Parse(kmerLength, null))).ToList();

            //Validate assembled sequence paths.
            foreach (var sequence in seqList.Select(t => t.ConvertToString()))
            {
                Assert.IsTrue(assembledPath.Contains(sequence), "Failed to locate " + sequence);
            }

            ApplicationLog.WriteLine("PADENA P1 : AssemblePath() validation for Padena step6:step7 completed successfully");
        }

        /// <summary>
        /// Sort Contig List based on the contig sequence
        /// </summary>
        /// <param name="contigsList">xml node name used for different testcases</param>
        private static IList<ISequence> SortContigsData(IList<ISequence> contigsList)
        {
            return (from ContigData in contigsList
                    orderby new string(ContigData.Select(a => (char)a).ToArray())
                    select ContigData).ToList();
        }

        ///<summary>
        /// Sort Valid Paired reads based on forward reads.
        /// For consistent output due to parallel implementation.
        /// </summary>
        /// <param name="list">List of Paired Reads</param>
        /// <param name="reads">Input list of reads.</param>
        /// <returns>Sorted List of Paired reads</returns>
        private static IList<ValidMatePair> SortPairedReads(IList<ValidMatePair> list,
            IEnumerable<ISequence> reads)
        {
            return (from valid in list
                    orderby new string(valid.PairedRead.GetForwardRead(reads).Select(a => (char)a).ToArray())
                    select valid).ToList();
        }

        /// <summary>
        /// Validate scaffold paths for a given input reads.
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateScaffoldPath(string nodeName)
        {
            var filePath = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.FilePathNode).TestDir();
            var kmerLength = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.KmerLengthNode);
            var daglingThreshold = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.DanglingLinkThresholdNode);
            var redundantThreshold = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.RedundantThreshold);
            var expectedScaffoldNodes = utilityObj.xmlUtil.GetTextValues(nodeName,Constants.ScaffoldNodes);
            var libraray = utilityObj.xmlUtil.GetTextValue(nodeName,Constants.LibraryName);
            var stdDeviation = utilityObj.xmlUtil.GetTextValue(nodeName,Constants.StdDeviation);
            var mean = utilityObj.xmlUtil.GetTextValue(nodeName,Constants.Mean);
            var expectedDepth = utilityObj.xmlUtil.GetTextValue(nodeName,Constants.DepthNode);

            // Get the input reads and build kmers
            var parser = new FastAParser();
            IEnumerable<ISequence> sequenceReads = parser.Parse(filePath).ToList();

            // Build kmers from step1,graph in step2 
            // Remove the dangling links from graph in step3
            // Remove bubbles form the graph in step4 
            // Pass the graph and build contigs
            // Validate contig reads
            KmerLength = Int32.Parse(kmerLength, null);
            DanglingLinksThreshold = Int32.Parse(daglingThreshold, null);
            DanglingLinksPurger = new DanglingLinksPurger(Int32.Parse(daglingThreshold, null));
            RedundantPathsPurger = new RedundantPathsPurger(Int32.Parse(redundantThreshold, null));
            RedundantPathLengthThreshold = Int32.Parse(redundantThreshold, null);
            SequenceReads.Clear();

            SetSequenceReads(sequenceReads.ToList());
            CreateGraph();
            var graph = new ContigGraph();
            UnDangleGraph();

            // Build contig.
            ContigBuilder = new SimplePathContigBuilder();
            RemoveRedundancy();
            var contigs = BuildContigs();

            var sortedContigs = SortContigsData(contigs.ToList());
            var mapper = new ReadContigMapper();
            var maps = mapper.Map(sortedContigs, sequenceReads, KmerLength);

            // Find map paired reads.
            CloneLibrary.Instance.AddLibrary(libraray, float.Parse(mean, null), float.Parse(stdDeviation, null));

            var mapPairedReads = new MatePairMapper();
            var pairedReads = mapPairedReads.MapContigToMatePairs(sequenceReads, maps);

            // Filter contigs based on the orientation.
            var filter = new OrientationBasedMatePairFilter();
            var contigpairedReads = filter.FilterPairedReads(pairedReads, 0);

            var dist = new DistanceCalculator(contigpairedReads);
            dist.CalculateDistance();

            graph.BuildContigGraph(contigs.ToList(), KmerLength);

            // Validate ScaffoldPath using BFS.
            var trace = new TracePath();
            var paths = trace.FindPaths(graph, contigpairedReads, Int32.Parse(kmerLength, null),
                                                        Int32.Parse(expectedDepth, null));

            var scaffold = paths.First();

            foreach (var kvp in scaffold)
            {
                var seq = graph.GetNodeSequence(kvp.Key);
                var sequence = seq.ConvertToString();
                var reversedSequence = seq.GetReverseComplementedSequence().ConvertToString();

                Assert.IsTrue(expectedScaffoldNodes.Contains(sequence)
                            || expectedScaffoldNodes.Contains(reversedSequence),
                            "Failed to find " + sequence + ", or " + reversedSequence);
            }

            ApplicationLog.WriteLine("PADENA P1 : FindPaths() validation for Padena step6:step6 completed successfully");
        }

        /// <summary>
        /// Validate scaffold sequence for a given input reads.
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateScaffoldSequence(string nodeName)
        {
            var filePath = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.FilePathNode).TestDir();
            var kmerLength = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.KmerLengthNode);
            var daglingThreshold = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.DanglingLinkThresholdNode);
            var redundantThreshold = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.RedundantThreshold);
            var libraray = utilityObj.xmlUtil.GetTextValue(nodeName,Constants.LibraryName);
            var stdDeviation = utilityObj.xmlUtil.GetTextValue(nodeName,Constants.StdDeviation);
            var mean = utilityObj.xmlUtil.GetTextValue(nodeName,Constants.Mean);
            var inputRedundancy = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.InputRedundancy);
            var expectedSeq = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.ScaffoldSeq);
            var scaffoldSeqNodes = expectedSeq.Split(',');

            // Get the input reads and build kmers
            var parser = new FastAParser();
            IEnumerable<ISequence> sequenceReads = parser.Parse(filePath).ToList();

            // Build kmers from step1,graph in step2 
            // Remove the dangling links from graph in step3
            // Remove bubbles form the graph in step4 
            // Pass the graph and build contigs
            // Validate contig reads
            KmerLength = Int32.Parse(kmerLength, null);
            DanglingLinksThreshold = Int32.Parse(daglingThreshold, null);
            DanglingLinksPurger = new DanglingLinksPurger(Int32.Parse(daglingThreshold, null));
            RedundantPathsPurger = new RedundantPathsPurger(Int32.Parse(redundantThreshold, null));
            RedundantPathLengthThreshold = Int32.Parse(redundantThreshold, null);
            SequenceReads.Clear();

            SetSequenceReads(sequenceReads.ToList());
            CreateGraph();
            UnDangleGraph();

            // Build contig.
            RemoveRedundancy();
            var contigs = BuildContigs();

            // Find map paired reads.
            CloneLibrary.Instance.AddLibrary(libraray, float.Parse(mean, null),
                float.Parse(stdDeviation, null));
            IEnumerable<ISequence> scaffoldSeq;

            using (var scaffold = new GraphScaffoldBuilder())
            {
                scaffoldSeq = scaffold.BuildScaffold(
                    sequenceReads, contigs.ToList(), KmerLength, redundancy: Int32.Parse(inputRedundancy, null));
            }

            AlignmentHelpers.CompareSequenceLists(new HashSet<string>(scaffoldSeqNodes), scaffoldSeq.ToList());

            ApplicationLog.WriteLine("PADENA P1 : Scaffold sequence : validation for Padena step6:step8 completed successfully");
        }

        /// <summary>
        /// Validate Parallel Denovo Assembly Assembled sequences.
        /// </summary>
        /// <param name="nodeName">XML node used to validate different test scenarios</param>
        internal void ValidatePadenaAssembledSeqs(string nodeName)
        {
            // Get values from XML node.
            var filePath = utilityObj.xmlUtil.GetTextValue(
               nodeName, Constants.FilePathNode).TestDir();
            var kmerLength = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.KmerLengthNode);
            var daglingThreshold = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.DanglingLinkThresholdNode);
            var redundantThreshold = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.RedundantThreshold);
            var libraray = utilityObj.xmlUtil.GetTextValue(nodeName,
                Constants.LibraryName);
            var stdDeviation = utilityObj.xmlUtil.GetTextValue(nodeName,
                Constants.StdDeviation);
            var mean = utilityObj.xmlUtil.GetTextValue(nodeName,
                Constants.Mean);
            var assembledSequences = utilityObj.xmlUtil.GetTextValue(nodeName,
                Constants.SequencePathNode);
            var assembledSeqCount = utilityObj.xmlUtil.GetTextValue(nodeName,
                Constants.AssembledSeqCountNode);
            var updatedAssembledSeqs = assembledSequences.Split(',');

            // Get the input reads and build kmers
            IEnumerable<ISequence> sequenceReads = null;
            var parser = new FastAParser();
            sequenceReads = parser.Parse(filePath).ToList();
            
            // Create a ParallelDeNovoAssembler instance.
            ParallelDeNovoAssembler denovoObj = null;
            try
            {
                denovoObj = new ParallelDeNovoAssembler();

                denovoObj.KmerLength = Int32.Parse(kmerLength, (IFormatProvider)null);
                denovoObj.DanglingLinksThreshold = Int32.Parse(daglingThreshold, (IFormatProvider)null);
                denovoObj.RedundantPathLengthThreshold = Int32.Parse(redundantThreshold, (IFormatProvider)null);

                CloneLibrary.Instance.AddLibrary(libraray, float.Parse(mean, (IFormatProvider)null),
                float.Parse(stdDeviation, (IFormatProvider)null));

                var symbols = sequenceReads.ElementAt(0).Alphabet.GetSymbolValueMap();

                var assembly =
                    denovoObj.Assemble(sequenceReads.Select(a => new Sequence(Alphabets.DNA, a.Select(b => symbols[b]).ToArray()) { ID = a.ID }), true);

                IList<ISequence> assembledSequenceList = assembly.AssembledSequences.ToList();

                // Validate assembled sequences.
                Assert.AreEqual(assembledSeqCount, assembledSequenceList.Count.ToString((IFormatProvider)null));

                for (var i = 0; i < assembledSequenceList.Count; i++)
                {
                    Assert.IsTrue(assembledSequences.Contains(
                    new string(assembledSequenceList[i].Select(a => (char)a).ToArray()))
                    || updatedAssembledSeqs.Contains(
                    new string(assembledSequenceList[i].GetReverseComplementedSequence().Select(a => (char)a).ToArray())));
                }
            }
            finally
            {
                if (denovoObj != null)
                    denovoObj.Dispose();
            }

            ApplicationLog.WriteLine("Padena P1 : Assemble() validation for Padena step6:step7 completed successfully");
        }

        #endregion
    }
}