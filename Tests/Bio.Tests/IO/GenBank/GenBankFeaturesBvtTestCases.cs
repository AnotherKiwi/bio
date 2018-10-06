/****************************************************************************
 * GenBankFeaturesBvtTestCases.cs
 * 
 *   This file contains the GenBank Features Bvt test cases.
 * 
***************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Bio.IO;
using Bio.IO.GenBank;
using Bio.TestAutomation.Util;
using Bio.Tests;
using Bio.Util.Logging;
using NUnit.Framework;

#if (SILVERLIGHT == false)
namespace Bio.TestAutomation.IO.GenBank
#else
namespace Bio.Silverlight.TestAutomation.IO.GenBank
#endif
{
    /// <summary>
    ///     GenBank Features Bvt test case implementation.
    /// </summary>
    [TestFixture]
    public class GenBankFeaturesBvtTestCases
    {
        #region Enums

        /// <summary>
        ///     GenBank location operator used for different testcases.
        /// </summary>
        private enum LocationOperatorParameter
        {
            Join,
            Complement,
            Order,
            Default
        };

        #endregion Enums

        #region Global Variables

        private readonly Utility utilityObj = new Utility(@"TestUtils\GenBankFeaturesTestConfig.xml");

        #endregion Global Variables

        #region Genbank Features Bvt test cases

        /// <summary>
        ///     Format a valid DNA sequence to GenBank file
        ///     and validate GenBank features.
        ///     Input : DNA Sequence
        ///     Output : Validate GenBank features.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateGenBankFeaturesForDNASequence()
        {
            ValidateGenBankFeatures(Constants.SimpleGenBankDnaNodeName,
                                    "DNA");
        }

        /// <summary>
        ///     Format a valid Protein sequence to GenBank file
        ///     and validate GenBank features.
        ///     Input : Protein Sequence
        ///     Output : Validate GenBank features.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateGenBankFeaturesForPROTEINSequence()
        {
            ValidateGenBankFeatures(Constants.SimpleGenBankProNodeName,
                                    "Protein");
        }

        /// <summary>
        ///     Format a valid RNA sequence to GenBank file
        ///     and validate GenBank features.
        ///     Input : RNA Sequence
        ///     Output : Validate GenBank features.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateGenBankFeaturesForRNASequence()
        {
            ValidateGenBankFeatures(Constants.SimpleGenBankRnaNodeName,
                                    "RNA");
        }

        /// <summary>
        ///     Format a valid DNA sequence to GenBank file,
        ///     add new features and validate GenBank features.
        ///     Input : DNA Sequence
        ///     Output : Validate addition of new GenBank features.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateAdditionOfGenBankFeaturesForDNASequence()
        {
            ValidateAdditionGenBankFeatures(Constants.SimpleGenBankDnaNodeName);
        }

        /// <summary>
        ///     Format a valid Protein sequence to GenBank file,
        ///     add new features and validate GenBank features.
        ///     Input : Protein Sequence
        ///     Output : Validate addition of new GenBank features.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateAdditionOfGenBankFeaturesForPROTEINSequence()
        {
            ValidateAdditionGenBankFeatures(Constants.SimpleGenBankProNodeName);
        }

        /// <summary>
        ///     Format a valid RNA sequence to GenBank file
        ///     add new features and validate GenBank features.
        ///     Input : RNA Sequence
        ///     Output : Validate addition of new GenBank features.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateAdditionOfGenBankFeaturesForRNASequence()
        {
            ValidateAdditionGenBankFeatures(Constants.RNAGenBankFeaturesNode);
        }

        /// <summary>
        ///     Format a valid DNA sequence to GenBank file
        ///     and validate GenBank DNA sequence standard features.
        ///     Input : Valid DNA sequence.
        ///     Output : Validation
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateDNASeqStandardFeaturesKey()
        {
            ValidateStandardFeaturesKey(Constants.DNAStandardFeaturesKeyNode,
                                        "DNA");
        }

        /// <summary>
        ///     Format a valid Protein sequence to GenBank file
        ///     and validate GenBank Protein seq standard features.
        ///     Input : Valid Protein sequence.
        ///     Output : Validation
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidatePROTEINSeqStandardFeaturesKey()
        {
            ValidateStandardFeaturesKey(Constants.SimpleGenBankProNodeName,
                                        "Protein");
        }

        /// <summary>
        ///     Format a valid sequence to
        ///     GenBank file using GenBankFormatter(File-Path) constructor and
        ///     validate GenBank Features.
        ///     Input : MultiSequence GenBank DNA file.
        ///     Validation : Validate GenBank Features.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateGenBankFeaturesForMultipleDNASequence()
        {
            ValidateGenBankFeatures(Constants.MultiSequenceGenBankDNANode,
                                    "DNA");
        }

        /// <summary>
        ///     Format a valid sequence to
        ///     GenBank file using GenBankFormatter(File-Path) constructor and
        ///     validate GenBank Features.
        ///     Input : MultiSequence GenBank Protein file.
        ///     Validation : Validate GenBank Features.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateGenBankFeaturesForMultiplePROTEINSequence()
        {
            ValidateGenBankFeatures(Constants.MultiSeqGenBankProteinNode,
                                    "Protein");
        }


        /// <summary>
        ///     Parse a Valid DNA Sequence and Validate Features
        ///     within specified range.
        ///     Input : Valid DNA Sequence and specified range.
        ///     Ouput : Validation of features count within specified range.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateFeaturesWithinRangeForDNASequence()
        {
            ValidateGetFeatures(Constants.DNAStandardFeaturesKeyNode, null);
        }

        /// <summary>
        ///     Parse a Valid RNA Sequence and Validate Features
        ///     within specified range.
        ///     Input : Valid RNA Sequence and specified range.
        ///     Ouput : Validation of features count within specified range.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateFeaturesWithinRangeForRNASequence()
        {
            ValidateGetFeatures(Constants.RNAGenBankFeaturesNode, null);
        }

        /// <summary>
        ///     Parse a Valid Protein Seq and Validate features
        ///     within specified range.
        ///     Input : Valid Protein Sequence and specified range.
        ///     Ouput : Validation of features count within specified range.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateFeaturesWithinRangeForPROTEINSequence()
        {
            ValidateGetFeatures(Constants.SimpleGenBankProNodeName, null);
        }

        /// <summary>
        ///     Parse a Valid DNA Sequence and Validate CDS Qualifiers.
        ///     Input : Valid DNA Sequence.
        ///     Ouput : Validation of all CDS Qualifiers..
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateDNASequenceCDSQualifiers()
        {
            ValidateCDSQualifiers(Constants.DNAStandardFeaturesKeyNode, "DNA");
        }

        /// <summary>
        ///     Parse a Valid Protein Sequence and Validate CDS Qualifiers.
        ///     Input : Valid Protein Sequence.
        ///     Ouput : Validation of all CDS Qualifiers..
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidatePROTEINSequenceCDSQualifiers()
        {
            ValidateCDSQualifiers(Constants.SimpleGenBankProNodeName, "Protein");
        }

        /// <summary>
        ///     Parse a Valid RNA Sequence and Validate CDS Qualifiers.
        ///     Input : Valid RNA Sequence.
        ///     Ouput : Validation of all CDS Qualifiers..
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateRNASequenceCDSQualifiers()
        {
            ValidateCDSQualifiers(Constants.RNAGenBankFeaturesNode, "RNA");
        }

        /// <summary>
        ///     Parse a Valid DNA Sequence and Validate CDS Qualifiers.
        ///     Input : Valid DNA Sequence and accession number.
        ///     Ouput : Validation of all CDS Qualifiers..
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateFeaturesUsingAccessionForDNASequence()
        {
            ValidateGetFeatures(Constants.DNAStandardFeaturesKeyNode, "Accession");
        }

        /// <summary>
        ///     Parse a Valid RNA Sequence and Validate CDS Qualifiers.
        ///     Input : Valid RNA Sequence and accession number.
        ///     Ouput : Validation of all CDS Qualifiers..
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateFeaturesUsingAccessionForRNASequence()
        {
            ValidateGetFeatures(Constants.RNAGenBankFeaturesNode, "Accession");
        }

        /// <summary>
        ///     Parse a Valid Protein Sequence and Validate CDS Qualifiers.
        ///     Input : Valid Protein Sequence and accession number.
        ///     Ouput : Validation of all CDS Qualifiers..
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateFeaturesUsingAccessionForPROTEINSequence()
        {
            ValidateGetFeatures(Constants.SimpleGenBankProNodeName, "Accession");
        }

        /// <summary>
        ///     Parse a Valid DNA Sequence and validate citation referenced
        ///     present in GenBank metadata.
        ///     Input : Valid DNA Sequence and specified range.
        ///     Ouput : Validation of citation referneced.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateCitationReferencedForDNASequence()
        {
            ValidateCitationReferenced(Constants.DNAStandardFeaturesKeyNode);
        }

        /// <summary>
        ///     Parse a Valid RNA Sequence and validate citation referenced
        ///     present in GenBank metadata.
        ///     Input : Valid RNA Sequence and specified range.
        ///     Ouput : Validation of citation referneced.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateCitationReferencedForRNASequence()
        {
            ValidateCitationReferenced(Constants.RNAGenBankFeaturesNode);
        }

        /// <summary>
        ///     Parse a Valid Protein Sequence and validate citation referenced
        ///     present in GenBank metadata.
        ///     Input : Valid Protein Sequence and specified range.
        ///     Ouput : Validation of citation referneced.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateCitationReferencedForPROTEINSequence()
        {
            ValidateCitationReferenced(Constants.SimpleGenBankProNodeName);
        }

        /// <summary>
        ///     Parse a Valid DNA Sequence and validate citation referenced
        ///     present in GenBank metadata by passing featureItem.
        ///     Input : Valid DNA Sequence and specified range.
        ///     Ouput : Validation of citation referneced.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateCitationReferencedForDNASequenceUsingFeatureItem()
        {
            ValidateCitationReferencedUsingFeatureItem(
                Constants.DNAStandardFeaturesKeyNode);
        }

        /// <summary>
        ///     Parse a Valid RNA Sequence and validate citation referenced
        ///     present in GenBank metadata by passing featureItem.
        ///     Input : Valid RNA Sequence and specified range.
        ///     Ouput : Validation of citation referneced.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateCitationReferencedForRNASequenceUsingFeatureItem()
        {
            ValidateCitationReferencedUsingFeatureItem(Constants.RNAGenBankFeaturesNode);
        }

        /// <summary>
        ///     Parse a Valid Protein Sequence and validate citation referenced
        ///     present in GenBank metadata by passing featureItem.
        ///     Input : Valid Protein Sequence and specified range.
        ///     Ouput : Validation of citation referneced.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateCitationReferencedForPROTEINSequenceUsingFeatureItem()
        {
            ValidateCitationReferencedUsingFeatureItem(Constants.SimpleGenBankProNodeName);
        }

        /// <summary>
        ///     Vaslidate Genbank Properties.
        ///     Input : Genbank sequence.
        ///     Output : validation of GenBank features.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateGenBankFeatureProperties()
        {
            // Get Values from XML node.
            var filePath = utilityObj.xmlUtil.GetTextValue(
                Constants.DNAStandardFeaturesKeyNode, Constants.FilePathNode).TestDir();
            var mRNAFeatureCount = utilityObj.xmlUtil.GetTextValue(
                Constants.DNAStandardFeaturesKeyNode, Constants.mRNACount);
            var exonFeatureCount = utilityObj.xmlUtil.GetTextValue(
                Constants.DNAStandardFeaturesKeyNode, Constants.ExonCount);
            var intronFeatureCount = utilityObj.xmlUtil.GetTextValue(
                Constants.DNAStandardFeaturesKeyNode, Constants.IntronCount);
            var cdsFeatureCount = utilityObj.xmlUtil.GetTextValue(
                Constants.DNAStandardFeaturesKeyNode, Constants.CDSCount);
            var allFeaturesCount = utilityObj.xmlUtil.GetTextValue(
                Constants.DNAStandardFeaturesKeyNode, Constants.GenBankFeaturesCount);
            var GenesCount = utilityObj.xmlUtil.GetTextValue(
                Constants.DNAStandardFeaturesKeyNode, Constants.GeneCount);
            var miscFeaturesCount = utilityObj.xmlUtil.GetTextValue(
                Constants.DNAStandardFeaturesKeyNode, Constants.MiscFeatureCount);
            var rRNACount = utilityObj.xmlUtil.GetTextValue(
                Constants.DNAStandardFeaturesKeyNode, Constants.rRNACount);
            var tRNACount = utilityObj.xmlUtil.GetTextValue(
                Constants.DNAStandardFeaturesKeyNode, Constants.tRNACount);
            var zeroValue = utilityObj.xmlUtil.GetTextValue(
                Constants.DNAStandardFeaturesKeyNode, Constants.emptyCount);

            ISequenceParser parserObj = new GenBankParser();
            var seq = parserObj.Parse(filePath);

            // Get all metada features. Hitting all the properties in the metadata feature.
            var metadata = (GenBankMetadata) seq.ElementAt(0).Metadata[Constants.GenBank];
            var allFeatures = metadata.Features.All;
            var minus10Signal = metadata.Features.Minus10Signals;
            var minus35Signal = metadata.Features.Minus35Signals;
            var threePrimeUTR = metadata.Features.ThreePrimeUTRs;
            var fivePrimeUTR = metadata.Features.FivePrimeUTRs;
            var attenuator = metadata.Features.Attenuators;
            var caatSignal = metadata.Features.CAATSignals;
            var CDS = metadata.Features.CodingSequences;
            var displacementLoop = metadata.Features.DisplacementLoops;
            var enhancer = metadata.Features.Enhancers;
            var exonList = metadata.Features.Exons;
            var gcsSignal = metadata.Features.GCSignals;
            var genesList = metadata.Features.Genes;
            var interveningDNA = metadata.Features.InterveningDNAs;
            var intronList = metadata.Features.Introns;
            var LTR = metadata.Features.LongTerminalRepeats;
            var matPeptide = metadata.Features.MaturePeptides;
            var miscBinding = metadata.Features.MiscBindings;
            var miscDifference = metadata.Features.MiscDifferences;
            var miscFeatures = metadata.Features.MiscFeatures;
            var miscRecobination =
                metadata.Features.MiscRecombinations;
            var miscRNA = metadata.Features.MiscRNAs;
            var miscSignal = metadata.Features.MiscSignals;
            var miscStructure = metadata.Features.MiscStructures;
            var modifierBase = metadata.Features.ModifiedBases;
            var mRNA = metadata.Features.MessengerRNAs;
            var nonCodingRNA = metadata.Features.NonCodingRNAs;
            var operonRegion = metadata.Features.OperonRegions;
            var polySignal = metadata.Features.PolyASignals;
            var polySites = metadata.Features.PolyASites;
            var precursorRNA = metadata.Features.PrecursorRNAs;
            var proteinBindingSites =
                metadata.Features.ProteinBindingSites;
            var rBindingSites =
                metadata.Features.RibosomeBindingSites;
            var repliconOrigin =
                metadata.Features.ReplicationOrigins;
            var repeatRegion = metadata.Features.RepeatRegions;
            var rRNA = metadata.Features.RibosomalRNAs;
            var signalPeptide = metadata.Features.SignalPeptides;
            var stemLoop = metadata.Features.StemLoops;
            var tataSignals = metadata.Features.TATASignals;
            var terminator = metadata.Features.Terminators;
            var tmRNA =
                metadata.Features.TransferMessengerRNAs;
            var transitPeptide = metadata.Features.TransitPeptides;
            var tRNA = metadata.Features.TransferRNAs;
            var unSecureRegion =
                metadata.Features.UnsureSequenceRegions;
            var variations = metadata.Features.Variations;

            // Validate GenBank Features.
            Assert.AreEqual(minus10Signal.Count, Convert.ToInt32(zeroValue, null));
            Assert.AreEqual(minus35Signal.Count, Convert.ToInt32(zeroValue, null));
            Assert.AreEqual(threePrimeUTR.Count, Convert.ToInt32(zeroValue, null));
            Assert.AreEqual(fivePrimeUTR.Count, Convert.ToInt32(zeroValue, null));
            Assert.AreEqual(caatSignal.Count, Convert.ToInt32(zeroValue, null));
            Assert.AreEqual(attenuator.Count, Convert.ToInt32(zeroValue, null));
            Assert.AreEqual(displacementLoop.Count, Convert.ToInt32(zeroValue, null));

            Assert.AreEqual(enhancer.Count, Convert.ToInt32(zeroValue, null));
            Assert.AreEqual(gcsSignal.Count, Convert.ToInt32(zeroValue, null));
            Assert.AreEqual(genesList.Count.ToString((IFormatProvider) null), GenesCount);
            Assert.AreEqual(interveningDNA.Count, Convert.ToInt32(zeroValue, null));
            Assert.AreEqual(LTR.Count, Convert.ToInt32(zeroValue, null));
            Assert.AreEqual(matPeptide.Count, Convert.ToInt32(zeroValue, null));
            Assert.AreEqual(miscBinding.Count, Convert.ToInt32(zeroValue, null));


            Assert.AreEqual(miscDifference.Count, Convert.ToInt32(zeroValue, null));
            Assert.AreEqual(miscFeatures.Count.ToString((IFormatProvider) null), miscFeaturesCount);
            Assert.AreEqual(miscRecobination.Count, Convert.ToInt32(zeroValue, null));
            Assert.AreEqual(miscSignal.Count, Convert.ToInt32(zeroValue, null));
            Assert.AreEqual(modifierBase.Count, Convert.ToInt32(zeroValue, null));
            Assert.AreEqual(miscRNA.Count, Convert.ToInt32(zeroValue, null));
            Assert.AreEqual(miscStructure.Count, Convert.ToInt32(zeroValue, null));
            Assert.AreEqual(mRNA.Count.ToString((IFormatProvider) null), mRNAFeatureCount);
            Assert.AreEqual(nonCodingRNA.Count, Convert.ToInt32(zeroValue, null));
            Assert.AreEqual(operonRegion.Count, Convert.ToInt32(zeroValue, null));

            Assert.AreEqual(polySignal.Count, Convert.ToInt32(zeroValue, null));
            Assert.AreEqual(polySites.Count, Convert.ToInt32(zeroValue, null));
            Assert.AreEqual(precursorRNA.Count, Convert.ToInt32(zeroValue, null));
            Assert.AreEqual(proteinBindingSites.Count, Convert.ToInt32(zeroValue, null));
            Assert.AreEqual(rBindingSites.Count, Convert.ToInt32(zeroValue, null));
            Assert.AreEqual(repliconOrigin.Count, Convert.ToInt32(zeroValue, null));

            Assert.AreEqual(rRNA.Count.ToString((IFormatProvider) null), rRNACount);
            Assert.AreEqual(signalPeptide.Count, Convert.ToInt32(zeroValue, null));
            Assert.AreEqual(stemLoop.Count, Convert.ToInt32(zeroValue, null));
            Assert.AreEqual(tataSignals.Count, Convert.ToInt32(zeroValue, null));
            Assert.AreEqual(repeatRegion.Count, Convert.ToInt32(zeroValue, null));
            Assert.AreEqual(terminator.Count, Convert.ToInt32(zeroValue, null));
            Assert.AreEqual(tmRNA.Count, Convert.ToInt32(zeroValue, null));
            Assert.AreEqual(variations.Count, Convert.ToInt32(zeroValue, null));

            Assert.AreEqual(tRNA.Count.ToString((IFormatProvider) null), tRNACount);
            Assert.AreEqual(transitPeptide.Count, Convert.ToInt32(zeroValue, null));
            Assert.AreEqual(unSecureRegion.Count, Convert.ToInt32(zeroValue, null));
            Assert.AreEqual(stemLoop.Count, Convert.ToInt32(zeroValue, null));

            Assert.AreEqual(allFeatures.Count, Convert.ToInt32(allFeaturesCount, null));
            Assert.AreEqual(CDS.Count, Convert.ToInt32(cdsFeatureCount, null));
            Assert.AreEqual(exonList.Count, Convert.ToInt32(exonFeatureCount, null));
            Assert.AreEqual(intronList.Count, Convert.ToInt32(intronFeatureCount, null));
        }

        /// <summary>
        ///     Validate location builder with normal string.
        ///     Input Data : Location string "345678910";
        ///     Output Data : Validation of Location start,end position,seperator
        ///     and operators.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateNormalStringLocationBuilder()
        {
            ValidateLocationBuilder(Constants.NormalLocationBuilderNode,
                                    LocationOperatorParameter.Default, false);
        }

        /// <summary>
        ///     Validate location builder with Single dot seperator string.
        ///     Input Data : Location string "1098945.2098765";
        ///     Output Data : Validation of Location start,end position,seperator
        ///     and operators.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateSingleDotSeperatorLocationBuilder()
        {
            ValidateLocationBuilder(Constants.SingleDotLocationBuilderNode,
                                    LocationOperatorParameter.Default, false);
        }

        /// <summary>
        ///     Validate location builder with Join Opperator.
        ///     Input Data : Location string "join(26300..26395)";
        ///     Output Data : Validation of Location start,end position,seperator
        ///     and operators.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateJoinOperatorLocationBuilder()
        {
            ValidateLocationBuilder(Constants.JoinOperatorLocationBuilderNode,
                                    LocationOperatorParameter.Join, true);
        }

        /// <summary>
        ///     Validate location builder with Join Opperator.
        ///     Input Data : Location string "complement(45745..50256)";
        ///     Output Data : Validation of Location start,end position,seperator
        ///     and operators.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateComplementOperatorLocationBuilder()
        {
            ValidateLocationBuilder(Constants.ComplementOperatorLocationBuilderNode,
                                    LocationOperatorParameter.Complement, true);
        }

        /// <summary>
        ///     Validate location builder with Order Opperator.
        ///     Input Data : Location string "order(9214567.50980256)";
        ///     Output Data : Validation of Location start,end position,seperator
        ///     and operators.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateOrderOperatorLocationBuilder()
        {
            ValidateLocationBuilder(Constants.OrderOperatorLocationBuilderNode,
                                    LocationOperatorParameter.Order, true);
        }

        /// <summary>
        ///     Validate CDS feature location builder by passing GenBank file.
        ///     Input Data : Location string "join(136..202,AF032048.1:67..345,
        ///     AF032048.1:1162..1175)";
        ///     Output Data : Validation of Location start,end position,seperator
        ///     and operators.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateSubSequenceGenBankFile()
        {
            ValidateLocationBuilder(Constants.GenBankFileLocationBuilderNode,
                                    LocationOperatorParameter.Join, true);
        }

        /// <summary>
        ///     Validate SubSequence start, end and range of sequence.
        ///     Input Data : GenBank file;
        ///     Output Data : Validation of Location start,end position,seperator
        ///     and operators.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateSequenceFeatureForRna()
        {
            ValidateSequenceFeature(Constants.GenBankFileSubSequenceNode);
        }

        /// <summary>
        ///     Validate SubSequence start, end and range of sequence.
        ///     Input Data : Dna GenBank file;
        ///     Output Data : Validation of Location start,end position,seperator
        ///     and operators.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateSequenceFeatureForDna()
        {
            ValidateSequenceFeature(Constants.GenBankFileSubSequenceDnaNode);
        }

        /// <summary>
        ///     Validate SubSequence start, end and range of sequence.
        ///     Input Data :Protein GenBank file;
        ///     Output Data : Validation of Location start,end position,seperator
        ///     and operators.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateSequenceFeatureForProteinA()
        {
            ValidateSequenceFeature(Constants.GenBankFileSubSequenceProteinNode);
        }

        /// <summary>
        ///     Validate SubSequence start, end and range of sequence.
        ///     Input Data : GenBank file;
        ///     Output Data : Validation of Location start,end position,seperator
        ///     and operators.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateSequenceFeatureUsingReferencedSequence()
        {
            // Get Values from XML node.
            var filePath = utilityObj.xmlUtil.GetTextValue(
                Constants.GenBankFileSubSequenceNode, Constants.FilePathNode).TestDir();
            var subSequence = utilityObj.xmlUtil.GetTextValue(
                Constants.GenBankFileSubSequenceNode, Constants.ExpectedSubSequence);
            var subSequenceStart = utilityObj.xmlUtil.GetTextValue(
                Constants.GenBankFileSubSequenceNode, Constants.SequenceStart);
            var subSequenceEnd = utilityObj.xmlUtil.GetTextValue(
                Constants.GenBankFileSubSequenceNode, Constants.SequenceEnd);
            var referenceSeq = utilityObj.xmlUtil.GetTextValue(
                Constants.GenBankFileSubSequenceNode, Constants.referenceSeq);

            ISequence sequence;
            ISequence firstFeatureSeq = null;

            // Parse a genBank file.
            var refSequence = new Sequence(Alphabets.RNA, referenceSeq);
            var parserObj = new GenBankParser();
            sequence = parserObj.Parse(filePath).FirstOrDefault();

            var metadata =
                sequence.Metadata[Constants.GenBank] as GenBankMetadata;

            // Get Subsequence feature,start and end postions.
            var referenceSequences =
                new Dictionary<string, ISequence>
                {
                    { Constants.Reference, refSequence }
                };
            firstFeatureSeq = metadata.Features.All[0].GetSubSequence(sequence,
                                                                      referenceSequences);

            var sequenceString = new string(firstFeatureSeq.Select(a => (char) a).ToArray());

            // Validate SubSequence.            
            Assert.AreEqual(sequenceString, subSequence);
            Assert.AreEqual(metadata.Features.All[0].Location.LocationStart.ToString((IFormatProvider) null),
                            subSequenceStart);
            Assert.AreEqual(metadata.Features.All[0].Location.LocationEnd.ToString((IFormatProvider) null),
                            subSequenceEnd);
            Assert.IsNull(metadata.Features.All[0].Location.Accession);
            Assert.AreEqual(metadata.Features.All[0].Location.StartData,
                            subSequenceStart);
            Assert.AreEqual(metadata.Features.All[0].Location.EndData,
                            subSequenceEnd);

            // Log to VSTest GUI
            ApplicationLog.WriteLine(string.Format(null,
                                                   "GenBank Features BVT: Successfully validated the Subsequence feature '{0}'",
                                                   sequenceString));
            ApplicationLog.WriteLine(string.Format(null,
                                                   "GenBank Features BVT: Successfully validated the start of subsequence'{0}'",
                                                   metadata.Features.All[0].Location.LocationStart.ToString(
                                                       (IFormatProvider) null)));
        }

        /// <summary>
        ///     Validate Sequence features for Exon,CDS,Intron..
        ///     Input Data : Sequence feature item and location.
        ///     Output Data : Validation of created sequence features.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateGenBankSubFeatures()
        {
            ValidateGenBankSubFeatures(Constants.GenBankSequenceFeaturesNode);
        }

        /// <summary>
        ///     Validate Sequence features for features with Join operator.
        ///     Input Data : Sequence feature item and location.
        ///     Output Data : Validation of created sequence features.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateGenBankSubFeatureswithOperator()
        {
            ValidateGenBankSubFeatures(Constants.GenBankSequenceFeaturesForMRNA);
        }

        /// <summary>
        ///     Validate Sequence features for features with Empty sub-operator.
        ///     Input Data : Sequence feature item and location.
        ///     Output Data : Validation of created sequence features.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateGenBankSubFeatureswithEmptyOperator()
        {
            ValidateAdditionGenBankFeatures(
                Constants.OperatorGenBankFileNode);
        }

        #endregion Genbank Features Bvt test cases

        #region Supporting Methods

        /// <summary>
        ///     Validate GenBank features.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        /// <param name="methodName">Name of the method</param>
        private void ValidateGenBankFeatures(string nodeName,
                                             string methodName)
        {
            // Get Values from XML node.
            var filePath = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode).TestDir();
            var alphabetName = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode);
            var expectedSequence = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequenceNode);
            var mRNAFeatureCount = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.mRNACount);
            var exonFeatureCount = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.ExonCount);
            var intronFeatureCount = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.IntronCount);
            var cdsFeatureCount = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.CDSCount);
            var allFeaturesCount = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.GenBankFeaturesCount);
            var expectedCDSKey = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.CDSKey);
            var expectedIntronKey = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.IntronKey);
            var expectedExonKey = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.ExonKey);
            var mRNAKey = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.mRNAKey);
            var sourceKeyName = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.SourceKey);
            var proteinKey = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.ProteinKeyName);
            var tempFileName = Path.GetTempFileName();
            ISequenceParser parserObj = new GenBankParser();
            var sequenceList = parserObj.Parse(filePath);

            if (sequenceList.Count() == 1)
            {
                var expectedUpdatedSequence =
                    expectedSequence.Replace("\r", "").Replace("\n", "").Replace(" ", "");
                var orgSeq = new Sequence(Utility.GetAlphabet(alphabetName), expectedUpdatedSequence);
                orgSeq.ID = sequenceList.ElementAt(0).ID;

                orgSeq.Metadata.Add(Constants.GenBank,
                                    sequenceList.ElementAt(0).Metadata[Constants.GenBank]);

                ISequenceFormatter formatterObj = new GenBankFormatter();
                formatterObj.Format(orgSeq, tempFileName);
            }
            else
            {
                var expectedUpdatedSequence =
                    expectedSequence.Replace("\r", "").Replace("\n", "").Replace(" ", "");
                var orgSeq =
                    new Sequence(Utility.GetAlphabet(alphabetName), expectedUpdatedSequence)
                    {
                        ID = sequenceList.ElementAt(1).ID
                    };

                orgSeq.Metadata.Add(Constants.GenBank,
                                    sequenceList.ElementAt(1).Metadata[Constants.GenBank]);
                ISequenceFormatter formatterObj = new GenBankFormatter();
                formatterObj.Format(orgSeq, tempFileName);
            }

            // parse a temporary file.
            var tempParserObj = new GenBankParser();
            {
                var tempFileSeqList = tempParserObj.Parse(tempFileName);
                var sequence = tempFileSeqList.ElementAt(0);

                var metadata = (GenBankMetadata) sequence.Metadata[Constants.GenBank];

                // Validate formatted temporary file GenBank Features.
                Assert.AreEqual(metadata.Features.All.Count,
                                Convert.ToInt32(allFeaturesCount, null));
                Assert.AreEqual(metadata.Features.CodingSequences.Count,
                                Convert.ToInt32(cdsFeatureCount, null));
                Assert.AreEqual(metadata.Features.Exons.Count,
                                Convert.ToInt32(exonFeatureCount, null));
                Assert.AreEqual(metadata.Features.Introns.Count,
                                Convert.ToInt32(intronFeatureCount, null));
                Assert.AreEqual(metadata.Features.MessengerRNAs.Count,
                                Convert.ToInt32(mRNAFeatureCount, null));
                Assert.AreEqual(metadata.Features.Attenuators.Count, 0);
                Assert.AreEqual(metadata.Features.CAATSignals.Count, 0);
                Assert.AreEqual(metadata.Features.DisplacementLoops.Count, 0);
                Assert.AreEqual(metadata.Features.Enhancers.Count, 0);
                Assert.AreEqual(metadata.Features.Genes.Count, 0);

                if ((0 == string.Compare(methodName, "DNA",
                                         CultureInfo.CurrentCulture, CompareOptions.IgnoreCase))
                    || (0 == string.Compare(methodName, "RNA",
                                            CultureInfo.CurrentCulture, CompareOptions.IgnoreCase)))
                {
                    IList<FeatureItem> featureList = metadata.Features.All;
                    Assert.AreEqual(featureList[0].Key.ToString(null), sourceKeyName);
                    Assert.AreEqual(featureList[1].Key.ToString(null), mRNAKey);
                    Assert.AreEqual(featureList[3].Key.ToString(null), expectedCDSKey);
                    Assert.AreEqual(featureList[5].Key.ToString(null), expectedExonKey);
                    Assert.AreEqual(featureList[6].Key.ToString(null), expectedIntronKey);
                    ApplicationLog.WriteLine(
                        "GenBank Features BVT: Successfully validated the GenBank Features");
                    ApplicationLog.WriteLine(string.Format(null,
                                                           "GenBank Features BVT: Successfully validated the CDS feature '{0}'",
                                                           featureList[3].Key.ToString(null)));
                    ApplicationLog.WriteLine(string.Format(null,
                                                           "GenBank Features BVT: Successfully validated the Exon feature '{0}'",
                                                           featureList[5].Key.ToString(null)));
                }
                else
                {
                    IList<FeatureItem> proFeatureList = metadata.Features.All;
                    Assert.AreEqual(proFeatureList[0].Key.ToString(null), sourceKeyName);
                    Assert.AreEqual(proFeatureList[1].Key.ToString(null), proteinKey);
                    Assert.AreEqual(proFeatureList[2].Key.ToString(null), expectedCDSKey);
                    ApplicationLog.WriteLine(
                        "GenBank Features BVT: Successfully validated the GenBank Features");
                    ApplicationLog.WriteLine(string.Format(null,
                                                           "GenBank Features BVT: Successfully validated the CDS feature '{0}'",
                                                           proFeatureList[2].Key.ToString(null)));
                    ApplicationLog.WriteLine(string.Format(null,
                                                           "GenBank Features BVT: Successfully validated the Source feature '{0}'",
                                                           proFeatureList[0].Key.ToString(null)));
                }
            }
            File.Delete(tempFileName);
        }

        /// <summary>
        ///     Validate addition of GenBank features.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        private void ValidateAdditionGenBankFeatures(string nodeName)
        {
            // Get Values from XML node.
            var filePath = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode).TestDir();
            var alphabetName = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode);
            var expectedSequence = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequenceNode);
            var addFirstKey = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.FirstKey);
            var addSecondKey = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.SecondKey);
            var addFirstLocation = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.FirstLocation);
            var addSecondLocation = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.SecondLocation);
            var addFirstQualifier = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.FirstQualifier);
            var addSecondQualifier = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.SecondQualifier);

            ISequenceParser parser1 = new GenBankParser();
            {
                var seqList1 = parser1.Parse(filePath);
                var localBuilderObj = new LocationBuilder();

                var tempFileName = Path.GetTempFileName();
                var expectedUpdatedSequence =
                    expectedSequence.Replace("\r", "").Replace("\n", "").Replace(" ", "");
                var orgSeq = new Sequence(Utility.GetAlphabet(alphabetName),
                                          expectedUpdatedSequence);
                orgSeq.ID = seqList1.ElementAt(0).ID;

                orgSeq.Metadata.Add(Constants.GenBank,
                                    seqList1.ElementAt(0).Metadata[Constants.GenBank]);

                ISequenceFormatter formatterObj = new GenBankFormatter();
                {
                    formatterObj.Format(orgSeq, tempFileName);

                    // parse GenBank file.
                    var parserObj = new GenBankParser();
                    {
                        var seqList = parserObj.Parse(tempFileName);

                        var seq = seqList.ElementAt(0);
                        var metadata = (GenBankMetadata) seq.Metadata[Constants.GenBank];

                        // Add a new features to Genbank features list.
                        metadata.Features = new SequenceFeatures();
                        var feature = new FeatureItem(addFirstKey, addFirstLocation);
                        var qualifierValues = new List<string>
                        {
                            addFirstQualifier,
                            addFirstQualifier
                        };
                        feature.Qualifiers.Add(addFirstQualifier, qualifierValues);
                        metadata.Features.All.Add(feature);

                        feature = new FeatureItem(addSecondKey, addSecondLocation);
                        qualifierValues = new List<string>
                        {
                            addSecondQualifier,
                            addSecondQualifier
                        };
                        feature.Qualifiers.Add(addSecondQualifier, qualifierValues);
                        metadata.Features.All.Add(feature);

                        // Validate added GenBank features.
                        Assert.AreEqual(metadata.Features.All[0].Key.ToString(null), addFirstKey);
                        Assert.AreEqual(
                            localBuilderObj.GetLocationString(metadata.Features.All[0].Location),
                            addFirstLocation);
                        Assert.AreEqual(metadata.Features.All[1].Key.ToString(null), addSecondKey);
                        Assert.AreEqual(localBuilderObj.GetLocationString(metadata.Features.All[1].Location),
                                        addSecondLocation);

                        parserObj.Close();
                    }

                    File.Delete(tempFileName);
                }
            }
        }

        /// <summary>
        ///     Validate GenBank standard features key.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        /// <param name="methodName">Name of the method</param>
        private void ValidateStandardFeaturesKey(string nodeName, string methodName)
        {
            // Get Values from XML node.
            var filePath = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode).TestDir();
            var expectedCondingSeqCount = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.CDSCount);
            var exonFeatureCount = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.ExonCount);
            var expectedtRNA = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.tRNACount);
            var expectedGeneCount = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.GeneCount);
            var miscFeatureCount = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.MiscFeatureCount);
            var expectedCDSKey = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.CDSKey);
            var expectedIntronKey = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.IntronKey);
            var mRNAKey = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.mRNAKey);
            var allFeaturesCount = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.StandardFeaturesCount);

            // Parse a file.
            ISequenceParser parserObj = new GenBankParser();
            var seq = parserObj.Parse(filePath);

            var metadata =
                seq.ElementAt(0).Metadata[Constants.GenBank] as GenBankMetadata;

            if ((0 == string.Compare(methodName, "DNA",
                                     CultureInfo.CurrentCulture, CompareOptions.IgnoreCase))
                || (0 == string.Compare(methodName, "RNA",
                                        CultureInfo.CurrentCulture, CompareOptions.IgnoreCase)))
            {
                // Validate standard features keys.
                Assert.AreEqual(metadata.Features.CodingSequences.Count.ToString((IFormatProvider) null),
                                expectedCondingSeqCount);
                Assert.AreEqual(metadata.Features.Exons.Count.ToString((IFormatProvider) null),
                                exonFeatureCount);
                Assert.AreEqual(metadata.Features.TransferRNAs.Count.ToString((IFormatProvider) null),
                                expectedtRNA);
                Assert.AreEqual(metadata.Features.Genes.Count.ToString((IFormatProvider) null),
                                expectedGeneCount);
                Assert.AreEqual(metadata.Features.MiscFeatures.Count.ToString((IFormatProvider) null),
                                miscFeatureCount);
                Assert.AreEqual(StandardFeatureKeys.CodingSequence.ToString(null),
                                expectedCDSKey);
                Assert.AreEqual(StandardFeatureKeys.Intron.ToString(null),
                                expectedIntronKey);
                Assert.AreEqual(StandardFeatureKeys.MessengerRna.ToString(null),
                                mRNAKey);
                Assert.AreEqual(StandardFeatureKeys.All.Count.ToString((IFormatProvider) null),
                                allFeaturesCount);
            }
            else
            {
                Assert.AreEqual(metadata.Features.CodingSequences.Count.ToString((IFormatProvider) null),
                                expectedCondingSeqCount);
                Assert.AreEqual(StandardFeatureKeys.CodingSequence.ToString(null),
                                expectedCDSKey);
            }
        }

        /// <summary>
        ///     Validate GenBank Get features with specified range.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        /// <param name="methodName">name of method</param>
        private void ValidateGetFeatures(string nodeName, string methodName)
        {
            // Get Values from XML node.
            var filePath = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode).TestDir();
            var expectedFirstRangeStartPoint = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.FirstRangeStartPoint);
            var expectedSecondRangeStartPoint = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.SecondRangeStartPoint);
            var expectedFirstRangeEndPoint = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.FirstRangeEndPoint);
            var expectedSecondRangeEndPoint = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.SecondRangeEndPoint);
            var expectedCountWithinSecondRange = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.FeaturesWithinSecondRange);
            var expectedCountWithinFirstRange = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.FeaturesWithinFirstRange);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            {
                var seq = parserObj.Parse(filePath);
                var metadata =
                    seq.ElementAt(0).Metadata[Constants.GenBank] as GenBankMetadata;
                var cdsList = metadata.Features.CodingSequences;
                var accessionNumber = cdsList[0].Location.Accession;

                if ((0 == string.Compare(methodName, "Accession",
                                         CultureInfo.CurrentCulture, CompareOptions.IgnoreCase)))
                {
                    // Validate GetFeature within specified range.
                    Assert.AreEqual(metadata.GetFeatures(accessionNumber,
                                                         Convert.ToInt32(expectedFirstRangeStartPoint, null),
                                                         Convert.ToInt32(expectedFirstRangeEndPoint, null))
                                            .Count.ToString((IFormatProvider) null),
                                    expectedCountWithinFirstRange);
                    Assert.AreEqual(metadata.GetFeatures(accessionNumber,
                                                         Convert.ToInt32(expectedSecondRangeStartPoint, null),
                                                         Convert.ToInt32(expectedSecondRangeEndPoint, null))
                                            .Count.ToString((IFormatProvider) null),
                                    expectedCountWithinSecondRange);
                }
                else
                {
                    // Validate GetFeature within specified range.
                    Assert.AreEqual(metadata.GetFeatures(
                        Convert.ToInt32(expectedFirstRangeStartPoint, null),
                        Convert.ToInt32(expectedFirstRangeEndPoint, null)).Count.ToString((IFormatProvider) null),
                                    expectedCountWithinFirstRange);
                    Assert.AreEqual(metadata.GetFeatures(
                        Convert.ToInt32(expectedSecondRangeStartPoint, null),
                        Convert.ToInt32(expectedSecondRangeEndPoint, null)).Count.ToString((IFormatProvider) null),
                                    expectedCountWithinSecondRange);
                }
            }
        }

        /// <summary>
        ///     Validate GenBank Citation referenced present in GenBank Metadata.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        private void ValidateCitationReferenced(string nodeName)
        {
            // Get Values from XML node.
            var filePath = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode).TestDir();
            var expectedCitationReferenced = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.citationReferencedCount);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            {
                var seq = parserObj.Parse(filePath);

                var metadata =
                    seq.ElementAt(0).Metadata[Constants.GenBank] as GenBankMetadata;

                // Get a list citationReferenced present in GenBank file.
                var citationReferenceList =
                    metadata.GetCitationsReferredInFeatures();

                // Validate citation referenced present in GenBank features.
                Assert.AreEqual(citationReferenceList.Count.ToString((IFormatProvider) null),
                                expectedCitationReferenced);
            }
        }

        /// <summary>
        ///     Validate GenBank Citation referenced by passing featureItem present in GenBank Metadata.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        private void ValidateCitationReferencedUsingFeatureItem(string nodeName)
        {
            // Get Values from XML node.
            var filePath = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode).TestDir();
            var expectedCitationReferenced = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.citationReferencedCount);

            // Parse a GenBank file.           
            ISequenceParser parserObj = new GenBankParser();
            {
                var seq = parserObj.Parse(filePath);
                var metadata =
                    seq.ElementAt(0).Metadata[Constants.GenBank] as GenBankMetadata;
                IList<FeatureItem> featureList = metadata.Features.All;

                // Get a list citationReferenced present in GenBank file.
                var citationReferenceList =
                    metadata.GetCitationsReferredInFeature(featureList[0]);

                Assert.AreEqual(citationReferenceList.Count.ToString((IFormatProvider) null),
                                expectedCitationReferenced);
            }
        }

        /// <summary>
        ///     Validate All qualifiers in CDS feature.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        private void ValidateCDSQualifiers(string nodeName, string methodName)
        {
            // Get Values from XML node.
            var filePath = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode).TestDir();
            var expectedCDSProduct = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.CDSProductQualifier);
            var expectedCDSException = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.CDSException);
            var expectedCDSCodonStart = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.CDSCodonStart);
            var expectedCDSLabel = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.CDSLabel);
            var expectedCDSDBReference = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.CDSDBReference);
            var expectedGeneSymbol = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.GeneSymbol);

            // Parse a GenBank file.            
            ISequenceParser parserObj = new GenBankParser();
            {
                var seq = parserObj.Parse(filePath);
                var metadata =
                    seq.ElementAt(0).Metadata[Constants.GenBank] as GenBankMetadata;

                // Get CDS qaulifier.value.
                var cdsQualifiers = metadata.Features.CodingSequences;
                var codonStartValue = cdsQualifiers[0].CodonStart;
                var productValue = cdsQualifiers[0].Product;
                var DBReferenceValue = cdsQualifiers[0].DatabaseCrossReference;


                // validate CDS qualifiers.
                if ((0 == string.Compare(methodName, "DNA",
                                         CultureInfo.CurrentCulture, CompareOptions.IgnoreCase))
                    || (0 == string.Compare(methodName, "RNA",
                                            CultureInfo.CurrentCulture, CompareOptions.IgnoreCase)))
                {
                    Assert.AreEqual(cdsQualifiers[0].Label,
                                    expectedCDSLabel);
                    Assert.AreEqual(cdsQualifiers[0].Exception.ToString(null),
                                    expectedCDSException);
                    Assert.AreEqual(productValue[0],
                                    expectedCDSProduct);
                    Assert.AreEqual(codonStartValue[0],
                                    expectedCDSCodonStart);
                    Assert.IsTrue(string.IsNullOrEmpty(cdsQualifiers[0].Allele));
                    Assert.IsFalse(string.IsNullOrEmpty(cdsQualifiers[0].Citation.ToString()));
                    Assert.AreEqual(DBReferenceValue[0],
                                    expectedCDSDBReference);
                    Assert.AreEqual(cdsQualifiers[0].GeneSymbol,
                                    expectedGeneSymbol);
                }
                else
                {
                    Assert.AreEqual(cdsQualifiers[0].Label, expectedCDSLabel);
                    Assert.AreEqual(cdsQualifiers[0].Exception.ToString(null), expectedCDSException);
                    Assert.IsTrue(string.IsNullOrEmpty(cdsQualifiers[0].Allele));
                    Assert.IsFalse(string.IsNullOrEmpty(cdsQualifiers[0].Citation.ToString()));
                    Assert.AreEqual(DBReferenceValue[0], expectedCDSDBReference);
                    Assert.AreEqual(cdsQualifiers[0].GeneSymbol, expectedGeneSymbol);
                }
            }
        }

        /// <summary>
        ///     Validate general Location builder.
        /// </summary>
        /// <param name="operatorPam">Different operator parameter name</param>
        /// <param name="nodeName">Different location string node name</param>
        /// <param name="isOperator">True if operator else false.</param>
        private void ValidateLocationBuilder(string nodeName,
                                             LocationOperatorParameter operatorPam, bool isOperator)
        {
            // Get Values from XML node.
            var locationString = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.LocationStringValue);
            var locationStartPosition = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.LoocationStartNode);
            var locationEndPosition = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.LoocationEndNode);
            var locationSeperatorNode = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.LocationSeperatorNode);
            var expectedLocationString = string.Empty;
            var sublocationStartPosition = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.SubLocationStart);
            var sublocationEndPosition = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.SubLocationEnd);
            var sublocationSeperatorNode = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.SubLocationSeperator);
            var subLocationsCount = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.SubLocationCount);

            // Build a new location 
            ILocationBuilder locationBuilderObj = new LocationBuilder();

            var location = locationBuilderObj.GetLocation(locationString);
            expectedLocationString = locationBuilderObj.GetLocationString(location);

            // Validate constructed location starts,end and location string.
            Assert.AreEqual(locationStartPosition, location.LocationStart.ToString((IFormatProvider) null));
            Assert.AreEqual(locationString, expectedLocationString);
            Assert.AreEqual(locationEndPosition, location.LocationEnd.ToString((IFormatProvider) null));

            switch (operatorPam)
            {
                case LocationOperatorParameter.Join:
                    Assert.AreEqual(LocationOperator.Join, location.Operator);
                    break;
                case LocationOperatorParameter.Complement:
                    Assert.AreEqual(LocationOperator.Complement, location.Operator);
                    break;
                case LocationOperatorParameter.Order:
                    Assert.AreEqual(LocationOperator.Order, location.Operator);
                    break;
                default:
                    Assert.AreEqual(LocationOperator.None, location.Operator);
                    Assert.AreEqual(locationSeperatorNode,
                                    location.Separator.ToString(null));
                    Assert.IsTrue(string.IsNullOrEmpty(location.Accession));
                    Assert.IsNotNull(location.SubLocations);
                    break;
            }

            if (isOperator)
            {
                Assert.IsTrue(string.IsNullOrEmpty(location.Separator));
                Assert.AreEqual(sublocationEndPosition,
                                location.SubLocations[0].LocationEnd.ToString((IFormatProvider) null));
                Assert.AreEqual(sublocationSeperatorNode,
                                location.SubLocations[0].Separator.ToString(null));
                Assert.AreEqual(Convert.ToInt32(subLocationsCount, null),
                                location.SubLocations.Count);
                Assert.AreEqual(sublocationStartPosition,
                                location.SubLocations[0].LocationStart.ToString((IFormatProvider) null));
                Assert.AreEqual(LocationOperator.None,
                                location.SubLocations[0].Operator);
                Assert.AreEqual(0,
                                location.SubLocations[0].SubLocations.Count);
            }
        }

        /// <summary>
        ///     Validate addition of GenBank features.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        private void ValidateGenBankSubFeatures(string nodeName)
        {
            // Get Values from XML node.
            var firstKey = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.FirstKey);
            var secondKey = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.SecondKey);
            var thirdKey = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.ThirdFeatureKey);
            var fourthKey = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.FourthKey);
            var fifthKey = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.FifthKey);
            var firstLocation = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.FirstLocation);
            var secondLocation = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.SecondLocation);
            var thirdLocation = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.ThirdLocation);
            var fourthLocation = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.FourthLocation);
            var fifthLocation = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.FifthLocation);
            var featuresCount = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.MainFeaturesCount);
            var secondCount = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.SecondCount);
            var thirdCount = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.ThirdCount);

            // Create a feature items
            var seqFeatures = new SequenceFeatures();
            var firstItem = new FeatureItem(firstKey, firstLocation);
            var secondItem = new FeatureItem(secondKey, secondLocation);
            var thirdItem = new FeatureItem(thirdKey, thirdLocation);
            var fourthItem = new FeatureItem(fourthKey, fourthLocation);
            var fifthItem = new FeatureItem(fifthKey, fifthLocation);

            seqFeatures.All.Add(firstItem);
            seqFeatures.All.Add(secondItem);
            seqFeatures.All.Add(thirdItem);
            seqFeatures.All.Add(fourthItem);
            seqFeatures.All.Add(fifthItem);

            // Validate sub features .
            var subFeatures = firstItem.GetSubFeatures(seqFeatures);
            Assert.AreEqual(Convert.ToInt32(featuresCount, null), subFeatures.Count);
            subFeatures = secondItem.GetSubFeatures(seqFeatures);
            Assert.AreEqual(Convert.ToInt32(secondCount, null), subFeatures.Count);
            subFeatures = thirdItem.GetSubFeatures(seqFeatures);
            Assert.AreEqual(Convert.ToInt32(thirdCount, null), subFeatures.Count);
        }

        /// <summary>
        ///     Validate Seqeunce feature of GenBank file.
        /// </summary>
        /// <param name="nodeName">xml node name. for different alphabet</param>
        private void ValidateSequenceFeature(string nodeName)
        {
            // Get Values from XML node.
            var filePath = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode).TestDir();
            var subSequence = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSubSequence);
            var subSequenceStart = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.SequenceStart);
            var subSequenceEnd = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.SequenceEnd);
            ISequence firstFeatureSeq = null;

            // Parse a genBank file.           
            ISequenceParser parserObj = new GenBankParser();
            {
                var seq = parserObj.Parse(filePath);
                var metadata = seq.ElementAt(0).Metadata[Constants.GenBank] as GenBankMetadata;

                // Get Subsequence feature,start and end postions.
                firstFeatureSeq = metadata.Features.All[0].GetSubSequence(seq.ElementAt(0));
                var sequenceString = new string(firstFeatureSeq.Select(a => (char) a).ToArray());
                // Validate SubSequence.
                Assert.AreEqual(sequenceString, subSequence);
                Assert.AreEqual(metadata.Features.All[0].Location.LocationStart.ToString((IFormatProvider) null),
                                subSequenceStart);
                Assert.AreEqual(metadata.Features.All[0].Location.LocationEnd.ToString((IFormatProvider) null),
                                subSequenceEnd);
                Assert.IsNull(metadata.Features.All[0].Location.Accession);
                Assert.AreEqual(metadata.Features.All[0].Location.StartData,
                                subSequenceStart);
                Assert.AreEqual(metadata.Features.All[0].Location.EndData,
                                subSequenceEnd);
            }
        }

        #endregion Supporting Methods
    }
}