﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Bio.Algorithms.Alignment;
using Bio.IO.FastA;
using Bio.SimilarityMatrices;
using Bio.TestAutomation.Util;
using Bio.Tests.Framework;
using Bio.Util.Logging;

using NUnit.Framework;

namespace Bio.Tests.Algorithms.Alignment
{
    /// <summary>
    ///     Pairwise Overlap Aligner algorithm Bvt test cases
    /// </summary>
    [TestFixture]
    public class PairwiseOverlapAlignerP1TestCases
    {
        #region Enums

        /// <summary>
        ///     Alignment Parameters which are used for different test cases
        ///     based on which the test cases are executed.
        /// </summary>
        private enum AlignParameters
        {
            AlignList,
            AlignListCode,
            AllParam,
            AllParamCode,
            AlignTwo,
            AlignTwoCode
        };

        /// <summary>
        ///     Alignment Type Parameters which are used for different test cases
        ///     based on which the test cases are executed.
        /// </summary>
        private enum AlignmentType
        {
            SimpleAlign,
            Align,
        };

        /// <summary>
        ///     Similarity Matrix Parameters which are used for different test cases
        ///     based on which the test cases are executed with different Similarity Matrixes.
        /// </summary>
        private enum SimilarityMatrixParameters
        {
            TextReader,
            DiagonalMatrix,
            Default
        };

        #endregion Enums

        #region Global Variables

        private readonly Utility utilityObj = new Utility(@"TestUtils\TestsConfig.xml");

        #endregion Global Variables

        #region PairwiseOverlapAligner P1 Test cases

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method Align(ListofSequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA DNA File
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignListSequencesDna()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapDnaAlignAlgorithmNodeName,
                                             AlignParameters.AlignList);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method Align(all parameters)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA DNA File
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignAllParamDna()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapDnaAlignAlgorithmNodeName,
                                             AlignParameters.AllParam);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method Align(ListofSequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein File
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignListSequencesPro()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapProAlignAlgorithmNodeName,
                                             AlignParameters.AlignList);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method Align(all parameters)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein File
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignAllParamPro()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapProAlignAlgorithmNodeName,
                                             AlignParameters.AllParam);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method Align(ListofSequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Rna File
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignListSequencesRna()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapRnaAlignAlgorithmNodeName,
                                             AlignParameters.AlignList);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method Align(all parameters)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Rna File
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignAllParamRna()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapRnaAlignAlgorithmNodeName,
                                             AlignParameters.AllParam);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method Align(ListofSequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein File with Max Gap Cost
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignListSequencesGapCostMax()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapGapCostMaxAlignAlgorithmNodeName,
                                             AlignParameters.AlignList);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method Align(all parameters)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein File with Max Gap Cost
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignAllParamGapCostMax()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapGapCostMaxAlignAlgorithmNodeName,
                                             AlignParameters.AllParam);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method Align(ListofSequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein File with Min Gap Cost
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignListSequencesGapCostMin()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapGapCostMinAlignAlgorithmNodeName,
                                             AlignParameters.AlignList);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method Align(all parameters)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein File with Min Gap Cost
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignAllParamGapCostMin()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapGapCostMinAlignAlgorithmNodeName,
                                             AlignParameters.AllParam);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method Align(ListofSequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein File with blosum SM
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignListSequencesBlosum()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapBlosumAlignAlgorithmNodeName,
                                             AlignParameters.AlignList);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method Align(all parameters)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein File with blosum SM
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignAllParamBlosum()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapBlosumAlignAlgorithmNodeName,
                                             AlignParameters.AllParam);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method Align(ListofSequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein File with Pam SM
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignListSequencesPam()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapPamAlignAlgorithmNodeName,
                                             AlignParameters.AlignList);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method Align(all parameters)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein File with Pam SM
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignAllParamPam()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapPamAlignAlgorithmNodeName,
                                             AlignParameters.AllParam);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method Align(ListofSequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein File with Similarity Matrix passed as Text reader
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignListSequencesSimMatTextRead()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapProAlignAlgorithmNodeName,
                                             AlignParameters.AlignList,
                                             SimilarityMatrixParameters.TextReader);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method Align(all parameters)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein File with Similarity Matrix passed as Text Reader
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignAllParamSimMatTextRead()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapProAlignAlgorithmNodeName,
                                             AlignParameters.AllParam,
                                             SimilarityMatrixParameters.TextReader);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method Align(ListofSequences)
        ///     and validate if the aligned sequence is as expected and 6
        ///     also validate the score for the same
        ///     Input : FastA Dna File Diagonal Matrix
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignListSequencesDiagonalSimMat()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapDiagonalSimMatAlignAlgorithmNodeName,
                                             AlignParameters.AlignList,
                                             SimilarityMatrixParameters.DiagonalMatrix);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method Align(all parameters)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Dna File Diagonal Matrix
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignAllParamDiagonalSimMat()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapDiagonalSimMatAlignAlgorithmNodeName,
                                             AlignParameters.AllParam,
                                             SimilarityMatrixParameters.DiagonalMatrix);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method Align(two sequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA DNA File
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignTwoDnaSequences()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapDnaAlignAlgorithmNodeName,
                                             AlignParameters.AlignTwo);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a xml file using the method Align(two sequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA DNA sequence
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignTwoDnaSequencesFromXml()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapDnaAlignAlgorithmNodeName,
                                             AlignParameters.AlignTwoCode);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a xml file using the method Align(list of sequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA DNA sequence
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignListDnaSequencesFromXml()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapDnaAlignAlgorithmNodeName,
                                             AlignParameters.AlignListCode);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a xml file using the method Align(all parameters)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA DNA sequence
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignAllParamDnaFromXml()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapDnaAlignAlgorithmNodeName,
                                             AlignParameters.AllParamCode);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method Align(two sequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA RNA File
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignTwoRnaSequences()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapRnaAlignAlgorithmNodeName,
                                             AlignParameters.AlignTwo);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a xml file using the method Align(two sequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA RNA sequence
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignTwoRnaSequencesFromXml()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapRnaAlignAlgorithmNodeName,
                                             AlignParameters.AlignTwoCode);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a xml file using the method Align(list of sequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA RNA sequence
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignListRnaSequencesFromXml()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapRnaAlignAlgorithmNodeName,
                                             AlignParameters.AlignListCode);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a xml file using the method Align(all parameters)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA RNA sequence
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignAllParamRnaFromXml()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapRnaAlignAlgorithmNodeName,
                                             AlignParameters.AllParamCode);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method Align(two sequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein File
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignTwoProSequences()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapProAlignAlgorithmNodeName,
                                             AlignParameters.AlignTwo);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a xml file using the method Align(two sequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein sequence
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignTwoProSequencesFromXml()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapProAlignAlgorithmNodeName,
                                             AlignParameters.AlignTwoCode);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a xml file using the method Align(list of sequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein sequence
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignListProSequencesFromXml()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapProAlignAlgorithmNodeName,
                                             AlignParameters.AlignListCode);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a xml file using the method Align(all parameters)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein sequence
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignAllParamProFromXml()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapProAlignAlgorithmNodeName,
                                             AlignParameters.AllParamCode);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method Align(two sequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein File with Max Gap Cost
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignTwoSequencesGapCostMax()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapGapCostMaxAlignAlgorithmNodeName,
                                             AlignParameters.AlignTwo);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a xml file using the method Align(two sequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein Sequence with Max Gap Cost
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignTwoSequencesGapCostMaxFromXml()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapGapCostMaxAlignAlgorithmNodeName,
                                             AlignParameters.AlignTwoCode);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a xml file using the method Align(list of sequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein Sequence with Max Gap Cost
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignSequenceListGapCostMaxFromXml()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapGapCostMaxAlignAlgorithmNodeName,
                                             AlignParameters.AlignListCode);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a xml file using the method Align(all parameters)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein Sequence with Max Gap Cost
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignAllParamGapCostMaxFromXml()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapGapCostMaxAlignAlgorithmNodeName,
                                             AlignParameters.AllParamCode);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method Align(two sequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein File with Min Gap Cost
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignTwoSequencesGapCostMin()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapGapCostMinAlignAlgorithmNodeName,
                                             AlignParameters.AlignTwo);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a xml file using the method Align(two sequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein Sequence with Min Gap Cost
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignTwoSequencesGapCostMinFromXml()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapGapCostMinAlignAlgorithmNodeName,
                                             AlignParameters.AlignTwoCode);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a xml file using the method Align(all parameters)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein Sequence with Min Gap Cost
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignAllParamGapCostMinFromXml()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapGapCostMinAlignAlgorithmNodeName,
                                             AlignParameters.AllParamCode);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method Align(Two Sequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein File with blosum SM
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignTwoSequencesBlosum()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapBlosumAlignAlgorithmNodeName,
                                             AlignParameters.AlignTwo);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method Align(two Sequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein File with Pam SM
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignTwoSequencesPam()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapPamAlignAlgorithmNodeName,
                                             AlignParameters.AlignTwo);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method Align(two sequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein File with Similarity Matrix passed as Text reader
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignTwoSequencesSimMatTextRead()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapProAlignAlgorithmNodeName,
                                             AlignParameters.AlignTwo,
                                             SimilarityMatrixParameters.TextReader);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method Align(two Sequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Dna File Diagonal Matrix
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapSimpleAlignTwoSequencesDiagonalSimMat()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapDiagonalSimMatAlignAlgorithmNodeName,
                                             AlignParameters.AlignTwo,
                                             SimilarityMatrixParameters.DiagonalMatrix);
        }

        #region Gap Extension Cost inclusion Test cases

        /// <summary>
        ///     Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix
        ///     which is in a text file using the method Align(ListofSequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA DNA File
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapAlignListSequencesDna()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapDnaAlignAlgorithmNodeName,
                                             AlignParameters.AlignList, SimilarityMatrixParameters.Default,
                                             AlignmentType.Align);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix
        ///     which is in a text file using the method Align(all parameters)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA DNA File
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapAlignAllParamDna()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapDnaAlignAlgorithmNodeName,
                                             AlignParameters.AllParam, SimilarityMatrixParameters.Default,
                                             AlignmentType.Align);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix
        ///     which is in a text file using the method Align(ListofSequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein File
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapAlignListSequencesPro()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapProAlignAlgorithmNodeName,
                                             AlignParameters.AlignList, SimilarityMatrixParameters.Default,
                                             AlignmentType.Align);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix
        ///     which is in a text file using the method Align(all parameters)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein File
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapAlignAllParamPro()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapProAlignAlgorithmNodeName,
                                             AlignParameters.AllParam, SimilarityMatrixParameters.Default,
                                             AlignmentType.Align);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix
        ///     which is in a text file using the method Align(ListofSequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Rna File
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapAlignListSequencesRna()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapRnaAlignAlgorithmNodeName,
                                             AlignParameters.AlignList, SimilarityMatrixParameters.Default,
                                             AlignmentType.Align);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix
        ///     which is in a text file using the method Align(all parameters)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Rna File
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapAlignAllParamRna()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapRnaAlignAlgorithmNodeName,
                                             AlignParameters.AllParam, SimilarityMatrixParameters.Default,
                                             AlignmentType.Align);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix
        ///     which is in a text file using the method Align(ListofSequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein File with Max Gap Cost
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapAlignListSequencesGapCostMax()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapGapCostMaxAlignAlgorithmNodeName,
                                             AlignParameters.AlignList, SimilarityMatrixParameters.Default,
                                             AlignmentType.Align);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix
        ///     which is in a text file using the method Align(all parameters)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein File with Max Gap Cost
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapAlignAllParamGapCostMax()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapGapCostMaxAlignAlgorithmNodeName,
                                             AlignParameters.AllParam, SimilarityMatrixParameters.Default,
                                             AlignmentType.Align);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix
        ///     which is in a text file using the method Align(ListofSequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein File with Min Gap Cost
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapAlignListSequencesGapCostMin()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapGapCostMinAlignAlgorithmNodeName,
                                             AlignParameters.AlignList, SimilarityMatrixParameters.Default,
                                             AlignmentType.Align);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix
        ///     which is in a text file using the method Align(all parameters)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein File with Min Gap Cost
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapAlignAllParamGapCostMin()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapGapCostMinAlignAlgorithmNodeName,
                                             AlignParameters.AllParam, SimilarityMatrixParameters.Default,
                                             AlignmentType.Align);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix
        ///     which is in a text file using the method Align(ListofSequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein File with blosum SM
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapAlignListSequencesBlosum()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapBlosumAlignAlgorithmNodeName,
                                             AlignParameters.AlignList, SimilarityMatrixParameters.Default,
                                             AlignmentType.Align);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix
        ///     which is in a text file using the method Align(all parameters)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein File with blosum SM
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapAlignAllParamBlosum()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapBlosumAlignAlgorithmNodeName,
                                             AlignParameters.AllParam, SimilarityMatrixParameters.Default,
                                             AlignmentType.Align);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix
        ///     which is in a text file using the method Align(ListofSequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein File with Pam SM
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapAlignListSequencesPam()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapPamAlignAlgorithmNodeName,
                                             AlignParameters.AlignList, SimilarityMatrixParameters.Default,
                                             AlignmentType.Align);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix
        ///     which is in a text file using the method Align(all parameters)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein File with Pam SM
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapAlignAllParamPam()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapPamAlignAlgorithmNodeName,
                                             AlignParameters.AllParam, SimilarityMatrixParameters.Default,
                                             AlignmentType.Align);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix
        ///     which is in a text file using the method Align(ListofSequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein File with Similarity Matrix passed as Text reader
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapAlignListSequencesSimMatTextRead()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapProAlignAlgorithmNodeName,
                                             AlignParameters.AlignList,
                                             SimilarityMatrixParameters.TextReader, AlignmentType.Align);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix
        ///     which is in a text file using the method Align(all parameters)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein File with Similarity Matrix passed as Text Reader
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapAlignAllParamSimMatTextRead()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapProAlignAlgorithmNodeName,
                                             AlignParameters.AllParam,
                                             SimilarityMatrixParameters.TextReader, AlignmentType.Align);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix
        ///     which is in a text file using the method Align(ListofSequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Dna File Diagonal Matrix
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapAlignListSequencesDiagonalSimMat()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapDiagonalSimMatAlignAlgorithmNodeName,
                                             AlignParameters.AlignList,
                                             SimilarityMatrixParameters.DiagonalMatrix, AlignmentType.Align);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix
        ///     which is in a text file using the method Align(all parameters)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Dna File Diagonal Matrix
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapAlignAllParamDiagonalSimMat()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapDiagonalSimMatAlignAlgorithmNodeName,
                                             AlignParameters.AllParam,
                                             SimilarityMatrixParameters.DiagonalMatrix, AlignmentType.Align);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid Gap Cost = Gap Extension, Similarity Matrix
        ///     which is in a text file using the method Align(ListofSequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Dna File
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapAlignListSequencesGapCostGapExtensionEqual()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapEqualAlignAlgorithmNodeName,
                                             AlignParameters.AlignList,
                                             SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid Gap Cost = Gap Extension, Similarity Matrix
        ///     which is in a text file using the method Align(all parameters)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Dna File
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void PairwiseOverlapAlignAllParamGapCostGapExtensionEqual()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapEqualAlignAlgorithmNodeName,
                                             AlignParameters.AllParam,
                                             SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        #endregion Gap Extension Cost inclusion Test cases

        #endregion PairwiseOverlapAligner P1 Test cases

        #region Supporting Methods

        /// <summary>
        ///     Validates PairwiseOverlapAlignment algorithm for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node Name in the xml.</param>
        /// <param name="alignParam">parameter based on which certain validations are done.</param>
        private void ValidatePairwiseOverlapAlignment(string nodeName, AlignParameters alignParam)
        {
            ValidatePairwiseOverlapAlignment(nodeName, alignParam, SimilarityMatrixParameters.Default);
        }

        /// <summary>
        ///     Validates PairwiseOverlapAlignment algorithm for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node Name in the xml.</param>
        /// <param name="alignParam">parameter based on which certain validations are done.</param>
        /// <param name="similarityMatrixParam">Similarity Matrix Parameter.</param>
        private void ValidatePairwiseOverlapAlignment(string nodeName, AlignParameters alignParam,
                                                      SimilarityMatrixParameters similarityMatrixParam)
        {
            ValidatePairwiseOverlapAlignment(nodeName, alignParam,
                                             similarityMatrixParam, AlignmentType.SimpleAlign);
        }

        /// <summary>
        ///     Validates PairwiseOverlapAlignment algorithm for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node Name in the xml.</param>
        /// <param name="alignParam">parameter based on which certain validations are done.</param>
        /// <param name="similarityMatrixParam">Similarity Matrix Parameter.</param>
        /// <param name="alignType">Alignment Type</param>
        private void ValidatePairwiseOverlapAlignment(string nodeName, AlignParameters alignParam,
                                                      SimilarityMatrixParameters similarityMatrixParam,
                                                      AlignmentType alignType)
        {
            ISequence aInput;
            ISequence bInput;

            IAlphabet alphabet = Utility.GetAlphabet(utilityObj.xmlUtil.GetTextValue(nodeName, Constants.AlphabetNameNode));

            if (alignParam.ToString().Contains("Code"))
            {
                string sequence1 = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.SequenceNode1);
                string sequence2 = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.SequenceNode2);

                aInput = new Sequence(alphabet, sequence1);
                bInput = new Sequence(alphabet, sequence2);
            }
            else
            {
                // Read the xml file for getting both the files for aligning.
                string filePath1 = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.FilePathNode1).TestDir();
                string filePath2 = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.FilePathNode2).TestDir();

                FastAParser parser1 = new FastAParser { Alphabet = alphabet };
                aInput = parser1.Parse(filePath1).ElementAt(0);
                bInput = parser1.Parse(filePath2).ElementAt(0);
            }

            string blosumFilePath = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.BlosumFilePathNode).TestDir();
            SimilarityMatrix sm;

            switch (similarityMatrixParam)
            {
                case SimilarityMatrixParameters.TextReader:
                    using (TextReader reader = new StreamReader(blosumFilePath))
                        sm = new SimilarityMatrix(reader);
                    break;
                case SimilarityMatrixParameters.DiagonalMatrix:
                    string matchValue = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.MatchScoreNode);
                    string misMatchValue = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.MisMatchScoreNode);
                    sm = new DiagonalSimilarityMatrix(int.Parse(matchValue, null), int.Parse(misMatchValue, null));
                    break;
                default:
                    sm = new SimilarityMatrix(new StreamReader(blosumFilePath));
                    break;
            }

            int gapOpenCost = int.Parse(utilityObj.xmlUtil.GetTextValue(nodeName, Constants.GapOpenCostNode), null);
            int gapExtensionCost = int.Parse(utilityObj.xmlUtil.GetTextValue(nodeName, Constants.GapExtensionCostNode), null);

            PairwiseOverlapAligner pairwiseOverlapObj = new PairwiseOverlapAligner();
            if (AlignParameters.AllParam != alignParam)
            {
                pairwiseOverlapObj.SimilarityMatrix = sm;
                pairwiseOverlapObj.GapOpenCost = gapOpenCost;
            }

            IList<IPairwiseSequenceAlignment> result = null;

            switch (alignParam)
            {
                case AlignParameters.AlignList:
                case AlignParameters.AlignListCode:
                    List<ISequence> sequences = new List<ISequence> {aInput, bInput};
                    switch (alignType)
                    {
                        case AlignmentType.Align:
                            pairwiseOverlapObj.GapExtensionCost = gapExtensionCost;
                            result = pairwiseOverlapObj.Align(sequences);
                            break;
                        default:
                            result = pairwiseOverlapObj.AlignSimple(sequences);
                            break;
                    }
                    break;
                case AlignParameters.AllParam:
                case AlignParameters.AllParamCode:
                    switch (alignType)
                    {
                        case AlignmentType.Align:
                            pairwiseOverlapObj.GapExtensionCost = gapExtensionCost;
                            result = pairwiseOverlapObj.Align(sm, gapOpenCost, gapExtensionCost, aInput, bInput);
                            break;
                        default:
                            result = pairwiseOverlapObj.AlignSimple(sm, gapOpenCost, aInput, bInput);
                            break;
                    }
                    break;
                case AlignParameters.AlignTwo:
                case AlignParameters.AlignTwoCode:
                    switch (alignType)
                    {
                        case AlignmentType.Align:
                            pairwiseOverlapObj.GapExtensionCost = gapExtensionCost;
                            result = pairwiseOverlapObj.Align(aInput, bInput);
                            break;
                        default:
                            result = pairwiseOverlapObj.AlignSimple(aInput, bInput);
                            break;
                    }
                    break;
                default:
                    break;
            }

            // Read the xml file for getting both the files for aligning.
            string expectedSequence1;
            string expectedSequence2;
            string expectedScore;

            switch (alignType)
            {
                case AlignmentType.Align:
                    expectedScore = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.ExpectedGapExtensionScoreNode);
                    expectedSequence1 = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.ExpectedGapExtensionSequence1Node);
                    expectedSequence2 = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.ExpectedGapExtensionSequence2Node);
                    break;
                default:
                    expectedScore = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.ExpectedScoreNode);
                    expectedSequence1 = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequenceNode1);
                    expectedSequence2 = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequenceNode2);
                    break;
            }

            IList<IPairwiseSequenceAlignment> expectedOutput = new List<IPairwiseSequenceAlignment>();
            char[] seperators = new [] {';'};
            string[] expectedSequences1 = expectedSequence1.Split(seperators);
            string[] expectedSequences2 = expectedSequence2.Split(seperators);

            IPairwiseSequenceAlignment align = new PairwiseSequenceAlignment();
            for (int i = 0; i < expectedSequences1.Length; i++)
            {
                PairwiseAlignedSequence alignedSeq = new PairwiseAlignedSequence
                {
                    FirstSequence = new Sequence(alphabet, expectedSequences1[i]),
                    SecondSequence = new Sequence(alphabet, expectedSequences2[i]),
                    Score = Convert.ToInt32(expectedScore, null),
                    FirstOffset = Int32.MinValue,
                    SecondOffset = Int32.MinValue,
                };
                align.PairwiseAlignedSequences.Add(alignedSeq);
            }
            expectedOutput.Add(align);

            Assert.IsTrue(AlignmentHelpers.CompareAlignment(result, expectedOutput, true));

            ApplicationLog.WriteLine(string.Format(null, "PairwiseOverlapAligner P1 : Final Score '{0}'.", expectedScore));
            ApplicationLog.WriteLine(string.Format(null, "PairwiseOverlapAligner P1 : Aligned First Sequence is '{0}'.", expectedSequence1));
            ApplicationLog.WriteLine(string.Format(null, "PairwiseOverlapAligner P1 : Aligned Second Sequence is '{0}'.", expectedSequence2));
        }
        #endregion Supporting Methods
    }
}