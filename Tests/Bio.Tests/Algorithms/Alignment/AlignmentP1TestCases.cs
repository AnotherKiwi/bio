﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

using Bio.Algorithms.Alignment;
using Bio.Extensions;
using Bio.IO.FastA;
using Bio.SimilarityMatrices;
using Bio.TestAutomation.Util;
using Bio.Tests.Framework;
using Bio.Util.Logging;

using NUnit.Framework;

namespace Bio.Tests.Algorithms.Alignment
{
    /// <summary>
    ///     Alignment P1 Test case implementation.
    /// </summary>
    [TestFixture]
    public class AlignmentP1TestCases
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
        ///     SequenceAlignment methods name which are used for different cases.
        /// </summary>
        private enum SeqAlignmentMethods
        {
            Add,
            Clear,
            Contains,
            CopyTo,
            Remove,
            AddSequence,
            GetEnumerator
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

        #region NeedlemanWunschAligner P1 Test cases

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
        public void NeedlemanWunschSimpleAlignListSequencesDna()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschDnaAlignAlgorithmNodeName,
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
        public void NeedlemanWunschSimpleAlignAllParamDna()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschDnaAlignAlgorithmNodeName,
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
        public void NeedlemanWunschSimpleAlignListSequencesPro()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschProAlignAlgorithmNodeName,
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
        public void NeedlemanWunschSimpleAlignAllParamPro()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschProAlignAlgorithmNodeName,
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
        public void NeedlemanWunschSimpleAlignListSequencesRna()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschRnaAlignAlgorithmNodeName,
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
        public void NeedlemanWunschSimpleAlignAllParamRna()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschRnaAlignAlgorithmNodeName,
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
        public void NeedlemanWunschSimpleAlignListSequencesGapCostMin()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschGapCostMinAlignAlgorithmNodeName,
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
        public void NeedlemanWunschSimpleAlignAllParamGapCostMin()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschGapCostMinAlignAlgorithmNodeName,
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
        public void NeedlemanWunschSimpleAlignListSequencesBlosum()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschBlosumAlignAlgorithmNodeName,
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
        public void NeedlemanWunschSimpleAlignAllParamBlosum()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschBlosumAlignAlgorithmNodeName,
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
        public void NeedlemanWunschSimpleAlignListSequencesPam()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschPamAlignAlgorithmNodeName,
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
        public void NeedlemanWunschSimpleAlignAllParamPam()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschPamAlignAlgorithmNodeName,
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
        public void NeedlemanWunschSimpleAlignListSequencesSimMatTextRead()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschProAlignAlgorithmNodeName,
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
        public void NeedlemanWunschSimpleAlignAllParamSimMatTextRead()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschProAlignAlgorithmNodeName,
                                             AlignParameters.AllParam,
                                             SimilarityMatrixParameters.TextReader);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method Align(ListofSequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Dna File Diagonal Matrix
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void NeedlemanWunschSimpleAlignListSequencesDiagonalSimMat()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschDiagonalSimMatAlignAlgorithmNodeName,
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
        public void NeedlemanWunschSimpleAlignAllParamDiagonalSimMat()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschDiagonalSimMatAlignAlgorithmNodeName,
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
        public void NeedlemanWunschSimpleAlignTwoDnaSequences()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschDnaAlignAlgorithmNodeName,
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
        public void NeedlemanWunschSimpleAlignTwoDnaSequencesFromXml()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschDnaAlignAlgorithmNodeName,
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
        public void NeedlemanWunschSimpleAlignListDnaSequencesFromXml()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschDnaAlignAlgorithmNodeName,
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
        public void NeedlemanWunschSimpleAlignAllParamDnaFromXml()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschDnaAlignAlgorithmNodeName,
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
        public void NeedlemanWunschSimpleAlignTwoRnaSequences()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschRnaAlignAlgorithmNodeName,
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
        public void NeedlemanWunschSimpleAlignTwoRnaSequencesFromXml()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschRnaAlignAlgorithmNodeName,
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
        public void NeedlemanWunschSimpleAlignListRnaSequencesFromXml()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschRnaAlignAlgorithmNodeName,
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
        public void NeedlemanWunschSimpleAlignAllParamRnaFromXml()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschRnaAlignAlgorithmNodeName,
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
        public void NeedlemanWunschSimpleAlignTwoProSequences()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschProAlignAlgorithmNodeName,
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
        public void NeedlemanWunschSimpleAlignTwoProSequencesFromXml()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschProAlignAlgorithmNodeName,
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
        public void NeedlemanWunschSimpleAlignListProSequencesFromXml()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschProAlignAlgorithmNodeName,
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
        public void NeedlemanWunschSimpleAlignAllParamProFromXml()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschProAlignAlgorithmNodeName,
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
        public void NeedlemanWunschSimpleAlignTwoSequencesGapCostMin()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschGapCostMinAlignAlgorithmNodeName,
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
        public void NeedlemanWunschSimpleAlignTwoSequencesGapCostMinFromXml()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschGapCostMinAlignAlgorithmNodeName,
                                             AlignParameters.AlignTwoCode);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a xml file using the method Align(list of sequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein Sequence with Min Gap Cost
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void NeedlemanWunschSimpleAlignSequenceListGapCostMinFromXml()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschGapCostMinAlignAlgorithmNodeName,
                                             AlignParameters.AlignListCode);
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
        public void NeedlemanWunschSimpleAlignAllParamGapCostMinFromXml()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschGapCostMinAlignAlgorithmNodeName,
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
        public void NeedlemanWunschSimpleAlignTwoSequencesBlosum()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschBlosumAlignAlgorithmNodeName,
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
        public void NeedlemanWunschSimpleAlignTwoSequencesPam()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschPamAlignAlgorithmNodeName,
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
        public void NeedlemanWunschSimpleAlignTwoSequencesSimMatTextRead()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschProAlignAlgorithmNodeName,
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
        public void NeedlemanWunschSimpleAlignTwoSequencesDiagonalSimMat()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschDiagonalSimMatAlignAlgorithmNodeName,
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
        public void NeedlemanWunschAlignListSequencesDna()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschDnaAlignAlgorithmNodeName,
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
        public void NeedlemanWunschAlignAllParamDna()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschDnaAlignAlgorithmNodeName,
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
        public void NeedlemanWunschAlignListSequencesPro()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschProAlignAlgorithmNodeName,
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
        public void NeedlemanWunschAlignAllParamPro()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschProAlignAlgorithmNodeName,
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
        public void NeedlemanWunschAlignListSequencesRna()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschRnaAlignAlgorithmNodeName,
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
        public void NeedlemanWunschAlignAllParamRna()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschRnaAlignAlgorithmNodeName,
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
        public void NeedlemanWunschAlignListSequencesGapCostMin()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschGapCostMinAlignAlgorithmNodeName,
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
        public void NeedlemanWunschAlignAllParamGapCostMin()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschGapCostMinAlignAlgorithmNodeName,
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
        public void NeedlemanWunschAlignListSequencesBlosum()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschBlosumAlignAlgorithmNodeName,
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
        public void NeedlemanWunschAlignAllParamBlosum()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschBlosumAlignAlgorithmNodeName,
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
        public void NeedlemanWunschAlignListSequencesPam()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschPamAlignAlgorithmNodeName,
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
        public void NeedlemanWunschAlignAllParamPam()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschPamAlignAlgorithmNodeName,
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
        public void NeedlemanWunschAlignListSequencesSimMatTextRead()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschProAlignAlgorithmNodeName,
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
        public void NeedlemanWunschAlignAllParamSimMatTextRead()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschProAlignAlgorithmNodeName,
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
        public void NeedlemanWunschAlignListSequencesDiagonalSimMat()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschDiagonalSimMatAlignAlgorithmNodeName,
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
        public void NeedlemanWunschAlignAllParamDiagonalSimMat()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschDiagonalSimMatAlignAlgorithmNodeName,
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
        public void NeedlemanWunschAlignListSequencesGapCostGapExtensionEqual()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschEqualAlignAlgorithmNodeName,
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
        public void NeedlemanWunschAlignAllParamGapCostGapExtensionEqual()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschEqualAlignAlgorithmNodeName,
                                             AlignParameters.AllParam,
                                             SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        #endregion Gap Extension Cost inclusion Test cases

        #endregion NeedlemanWunschAligner P1 Test cases

        #region SmithWatermanAligner P1 Test cases

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
        public void SmithWatermanSimpleAlignListSequencesDna()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanDnaAlignAlgorithmNodeName,
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
        public void SmithWatermanSimpleAlignAllParamDna()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanDnaAlignAlgorithmNodeName,
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
        public void SmithWatermanSimpleAlignListSequencesPro()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanProAlignAlgorithmNodeName,
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
        public void SmithWatermanSimpleAlignAllParamPro()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanProAlignAlgorithmNodeName,
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
        public void SmithWatermanSimpleAlignListSequencesRna()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanRnaAlignAlgorithmNodeName,
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
        public void SmithWatermanSimpleAlignAllParamRna()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanRnaAlignAlgorithmNodeName,
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
        public void SmithWatermanSimpleAlignListSequencesGapCostMin()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanGapCostMinAlignAlgorithmNodeName,
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
        public void SmithWatermanSimpleAlignAllParamGapCostMin()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanGapCostMinAlignAlgorithmNodeName,
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
        public void SmithWatermanSimpleAlignListSequencesBlosum()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanBlosumAlignAlgorithmNodeName,
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
        public void SmithWatermanSimpleAlignAllParamBlosum()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanBlosumAlignAlgorithmNodeName,
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
        public void SmithWatermanSimpleAlignListSequencesPam()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanPamAlignAlgorithmNodeName,
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
        public void SmithWatermanSimpleAlignAllParamPam()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanPamAlignAlgorithmNodeName,
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
        public void SmithWatermanSimpleAlignListSequencesSimMatTextRead()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanProAlignAlgorithmNodeName,
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
        public void SmithWatermanSimpleAlignAllParamSimMatTextRead()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanProAlignAlgorithmNodeName,
                                           AlignParameters.AllParam,
                                           SimilarityMatrixParameters.TextReader);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method Align(ListofSequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Dna File Diagonal Matrix
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void SmithWatermanSimpleAlignListSequencesDiagonalSimMat()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanDiagonalSimMatAlignAlgorithmNodeName,
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
        public void SmithWatermanSimpleAlignAllParamDiagonalSimMat()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanDiagonalSimMatAlignAlgorithmNodeName,
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
        public void SmithWatermanSimpleAlignTwoDnaSequences()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanDnaAlignAlgorithmNodeName,
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
        public void SmithWatermanSimpleAlignTwoDnaSequencesFromXml()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanDnaAlignAlgorithmNodeName,
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
        public void SmithWatermanSimpleAlignListDnaSequencesFromXml()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanDnaAlignAlgorithmNodeName,
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
        public void SmithWatermanSimpleAlignAllParamDnaFromXml()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanDnaAlignAlgorithmNodeName,
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
        public void SmithWatermanSimpleAlignTwoRnaSequences()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanRnaAlignAlgorithmNodeName,
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
        public void SmithWatermanSimpleAlignTwoRnaSequencesFromXml()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanRnaAlignAlgorithmNodeName,
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
        public void SmithWatermanSimpleAlignListRnaSequencesFromXml()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanRnaAlignAlgorithmNodeName,
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
        public void SmithWatermanSimpleAlignAllParamRnaFromXml()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanRnaAlignAlgorithmNodeName,
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
        public void SmithWatermanSimpleAlignTwoProSequences()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanProAlignAlgorithmNodeName,
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
        public void SmithWatermanSimpleAlignTwoProSequencesFromXml()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanProAlignAlgorithmNodeName,
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
        public void SmithWatermanSimpleAlignListProSequencesFromXml()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanProAlignAlgorithmNodeName,
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
        public void SmithWatermanSimpleAlignAllParamProFromXml()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanProAlignAlgorithmNodeName,
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
        public void SmithWatermanSimpleAlignTwoSequencesGapCostMin()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanGapCostMinAlignAlgorithmNodeName,
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
        public void SmithWatermanSimpleAlignTwoSequencesGapCostMinFromXml()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanGapCostMinAlignAlgorithmNodeName,
                                           AlignParameters.AlignTwoCode);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     which is in a xml file using the method Align(list of sequences)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA Protein Sequence with Min Gap Cost
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void SmithWatermanSimpleAlignSequenceListGapCostMinFromXml()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanGapCostMinAlignAlgorithmNodeName,
                                           AlignParameters.AlignListCode);
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
        public void SmithWatermanSimpleAlignAllParamGapCostMinFromXml()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanGapCostMinAlignAlgorithmNodeName,
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
        public void SmithWatermanSimpleAlignTwoSequencesBlosum()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanBlosumAlignAlgorithmNodeName,
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
        public void SmithWatermanSimpleAlignTwoSequencesPam()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanPamAlignAlgorithmNodeName,
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
        public void SmithWatermanSimpleAlignTwoSequencesSimMatTextRead()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanProAlignAlgorithmNodeName,
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
        public void SmithWatermanSimpleAlignTwoSequencesDiagonalSimMat()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanDiagonalSimMatAlignAlgorithmNodeName,
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
        public void SmithWatermanAlignListSequencesDna()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanDnaAlignAlgorithmNodeName,
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
        public void SmithWatermanAlignAllParamDna()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanDnaAlignAlgorithmNodeName,
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
        public void SmithWatermanAlignListSequencesPro()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanProAlignAlgorithmNodeName,
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
        public void SmithWatermanAlignAllParamPro()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanProAlignAlgorithmNodeName,
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
        public void SmithWatermanAlignListSequencesRna()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanRnaAlignAlgorithmNodeName,
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
        public void SmithWatermanAlignAllParamRna()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanRnaAlignAlgorithmNodeName,
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
        public void SmithWatermanAlignListSequencesGapCostMin()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanGapCostMinAlignAlgorithmNodeName,
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
        public void SmithWatermanAlignAllParamGapCostMin()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanGapCostMinAlignAlgorithmNodeName,
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
        public void SmithWatermanAlignListSequencesBlosum()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanBlosumAlignAlgorithmNodeName,
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
        public void SmithWatermanAlignAllParamBlosum()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanBlosumAlignAlgorithmNodeName,
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
        public void SmithWatermanAlignListSequencesPam()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanPamAlignAlgorithmNodeName,
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
        public void SmithWatermanAlignAllParamPam()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanPamAlignAlgorithmNodeName,
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
        public void SmithWatermanAlignListSequencesSimMatTextRead()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanProAlignAlgorithmNodeName,
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
        public void SmithWatermanAlignAllParamSimMatTextRead()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanProAlignAlgorithmNodeName,
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
        public void SmithWatermanAlignListSequencesDiagonalSimMat()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanDiagonalSimMatAlignAlgorithmNodeName,
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
        public void SmithWatermanAlignAllParamDiagonalSimMat()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanDiagonalSimMatAlignAlgorithmNodeName,
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
        public void SmithWatermanAlignListSequencesGapCostGapExtensionEqual()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanEqualAlignAlgorithmNodeName,
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
        public void SmithWatermanAlignAllParamGapCostGapExtensionEqual()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanEqualAlignAlgorithmNodeName,
                                           AlignParameters.AllParam,
                                           SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        #endregion Gap Extension Cost inclusion Test cases

        #endregion SmithWatermanAligner P1 Test cases

        #region Sequence Alignment P1 Test cases

        /// <summary>
        ///     Pass a valid Dna sequences to AddSequence() method and validate the same.
        ///     Input : Dna Sequence read from xml file.
        ///     Validation : Added sequences are got back and validated.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void SequenceAlignmentAddDnaSequence()
        {
            ValidateGeneralSequenceAlignment(Constants.AlignDnaAlgorithmNodeName, false);
        }

        /// <summary>
        ///     Pass a valid Rna sequences to AddSequence() method and validate the same.
        ///     Input : Rna Sequence read from xml file.
        ///     Validation : Added sequences are got back and validated.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void SequenceAlignmentAddRnaSequence()
        {
            ValidateGeneralSequenceAlignment(Constants.AlignRnaAlgorithmNodeName, false);
        }

        /// <summary>
        ///     Pass a valid Protein sequences to AddSequence() method and validate the same.
        ///     Input : Dna Sequence read from xml file.
        ///     Validation : Added sequences are got back and validated.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void SequenceAlignmentAddProteinSequence()
        {
            ValidateGeneralSequenceAlignment(Constants.AlignProteinAlgorithmNodeName, false);
        }

        /// <summary>
        ///     Pass a valid sequences to AddSequence() method and validate the properties.
        ///     Input : Dna Sequence read from xml file.
        ///     Validation : Added sequences are got back and validated.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void SequenceAlignmentValidateProperties()
        {
            ValidateGeneralSequenceAlignment(Constants.AlignAlgorithmNodeName, true);
        }

        /// <summary>
        ///     Validate all SequenceAlignment public properties
        ///     Input : FastA DNA File
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Category("Priority1")]
        public void ValidateSequenceAlignmentProperties()
        {
            // Read the xml file for getting both the files for aligning.
            string origSequence1 = utilityObj.xmlUtil.GetTextValue(Constants.AlignDnaAlgorithmNodeName,
                                                                   Constants.SequenceNode1);
            string origSequence2 = utilityObj.xmlUtil.GetTextValue(Constants.AlignDnaAlgorithmNodeName,
                                                                   Constants.SequenceNode2);
            IAlphabet alphabet = Utility.GetAlphabet(utilityObj.xmlUtil.GetTextValue(
                Constants.AlignDnaAlgorithmNodeName,
                Constants.AlphabetNameNode));
            string seqCount = utilityObj.xmlUtil.GetTextValue(
                Constants.AlignDnaAlgorithmNodeName,
                Constants.SequenceCountNode);

            // Create two sequences
            ISequence aInput = new Sequence(alphabet, origSequence1);
            ISequence bInput = new Sequence(alphabet, origSequence2);

            // Add the sequences to the Sequence alignment object using AddSequence() method.
            IList<IPairwiseSequenceAlignment> sequenceAlignmentObj = new List<IPairwiseSequenceAlignment>();

            PairwiseAlignedSequence alignSeq = new PairwiseAlignedSequence();

            alignSeq.FirstSequence = aInput;
            alignSeq.SecondSequence = bInput;
            IPairwiseSequenceAlignment seqAlignObj = new PairwiseSequenceAlignment(aInput, bInput)
            {
                alignSeq
            };
            sequenceAlignmentObj.Add(seqAlignObj);

            // Validate all properties of sequence alignment class. 
            Assert.AreEqual(seqCount, seqAlignObj.Count.ToString((IFormatProvider) null));
            Assert.AreEqual(origSequence1, new string(seqAlignObj.FirstSequence.Select(a => (char) a).ToArray()));
            Assert.AreEqual(origSequence2, new string(seqAlignObj.SecondSequence.Select(a => (char) a).ToArray()));
            Assert.IsFalse(seqAlignObj.IsReadOnly);
            Assert.IsNull(seqAlignObj.Documentation);
            Assert.AreEqual(seqCount, seqAlignObj.PairwiseAlignedSequences.Count.ToString((IFormatProvider) null));

            ApplicationLog.WriteLine("SequenceAlignment P1 : Successfully validated the IsRead Property");
            ApplicationLog.WriteLine("SequenceAlignment P1 : Successfully validated the Count Property");
            ApplicationLog.WriteLine("SequenceAlignment P1 : Successfully validated the Sequences Property");
        }

        /// <summary>
        ///     Validate SequenceAlignment Add() method.
        ///     Input : Dna Sequence read from xml file.
        ///     Validation : Added sequences are got back and validated.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void ValidateAddSequenceToSequenceAlignment()
        {
            ValidateSequenceAlignmentGeneralMethods(Constants.AlignAlgorithmNodeName,
                                                    SeqAlignmentMethods.Add, false);
        }

        /// <summary>
        ///     Validate SequenceAlignment Clear() method.
        ///     Input : Dna Sequence read from xml file.
        ///     Validation : Added sequences are got back and validated.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void ValidateDeletingSequenceAlignment()
        {
            ValidateSequenceAlignmentGeneralMethods(Constants.AlignAlgorithmNodeName,
                                                    SeqAlignmentMethods.Clear, false);
        }

        /// <summary>
        ///     Validate SequenceAlignment Contains() method.
        ///     Input : Dna Sequence read from xml file.
        ///     Validation : Validate whether SequenceAlignment contains Aligned sequence or not.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void ValidateSequenceAlignmentContainsMethod()
        {
            ValidateSequenceAlignmentGeneralMethods(Constants.AlignAlgorithmNodeName,
                                                    SeqAlignmentMethods.Contains, false);
        }

        /// <summary>
        ///     Validate copying SequenceAlignment values to array.
        ///     Input : Dna Sequence read from xml file.
        ///     Validation : Validate copying SequenceAlignment values to array.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void ValidateCopiedSeqAlinmentItems()
        {
            ValidateSequenceAlignmentGeneralMethods(Constants.AlignAlgorithmNodeName,
                                                    SeqAlignmentMethods.CopyTo, false);
        }

        /// <summary>
        ///     Validate Remove Aligned Sequence from Sequence Alignment
        ///     Input : Dna Sequence read from xml file.
        ///     Validation : Validate Sequence Alignment.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void ValidateRemoveAlignedSeqItem()
        {
            ValidateSequenceAlignmentGeneralMethods(Constants.AlignAlgorithmNodeName,
                                                    SeqAlignmentMethods.Remove, false);
        }

        /// <summary>
        ///     Validate Sequence Alignment default constructor
        ///     Input : Dna Sequence read from xml file.
        ///     Validation : Validate Sequence Alignment default constructor
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void ValidateSeqAlignmentDefaultCtr()
        {
            ValidateSequenceAlignmentGeneralMethods(Constants.AlignAlgorithmNodeName,
                                                    SeqAlignmentMethods.Remove, true);
        }

        /// <summary>
        ///     Validate SequenceAlignment AddSequence() method.
        ///     Input : Dna Sequence read from xml file.
        ///     Validation : Added sequences are got back and validated.
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void ValidateAddSequenceToAlignedSeqList()
        {
            ValidateSequenceAlignmentGeneralMethods(Constants.AlignAlgorithmNodeName,
                                                    SeqAlignmentMethods.AddSequence, false);
        }

        /// <summary>
        ///     Validate GetEnumerator() method.
        ///     Input : Dna Sequence read from xml file.
        ///     Validation : Validate GetEnumerator() method
        /// </summary>
        [Test]
        [Category("Priority1")]
        public void ValidateAlignedSeqGetEnumerator()
        {
            ValidateSequenceAlignmentGeneralMethods(Constants.AlignAlgorithmNodeName,
                                                    SeqAlignmentMethods.GetEnumerator, false);
        }

        #endregion Sequence Alignment P1 Test cases

        #region Supporting Methods

        /// <summary>
        ///     Validates NeedlemanWunschAlignment algorithm for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node Name in the xml.</param>
        /// <param name="alignParam">parameter based on which certain validations are done.</param>
        private void ValidateNeedlemanWunschAlignment(string nodeName, AlignParameters alignParam)
        {
            ValidateNeedlemanWunschAlignment(nodeName, alignParam, SimilarityMatrixParameters.Default);
        }

        /// <summary>
        ///     Validates NeedlemanWunschAlignment algorithm for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node Name in the xml.</param>
        /// <param name="alignParam">parameter based on which certain validations are done.</param>
        /// <param name="similarityMatrixParam">Similarity Matrix Parameter.</param>
        private void ValidateNeedlemanWunschAlignment(string nodeName, AlignParameters alignParam,
                                                      SimilarityMatrixParameters similarityMatrixParam)
        {
            ValidateNeedlemanWunschAlignment(nodeName, alignParam, similarityMatrixParam, AlignmentType.SimpleAlign);
        }

        /// <summary>
        ///     Validates NeedlemanWunschAlignment algorithm for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node Name in the xml.</param>
        /// <param name="alignParam">parameter based on which certain validations are done.</param>
        /// <param name="similarityMatrixParam">Similarity Matrix Parameter.</param>
        /// <param name="alignType">Is the Align type Simple or Align with Gap Extension cost?</param>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity"),
         SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        private void ValidateNeedlemanWunschAlignment(string nodeName, AlignParameters alignParam,
                                                      SimilarityMatrixParameters similarityMatrixParam,
                                                      AlignmentType alignType)
        {
            ISequence aInput, bInput;

            IAlphabet alphabet =
                Utility.GetAlphabet(utilityObj.xmlUtil.GetTextValue(nodeName, Constants.AlphabetNameNode));

            // Parse the files and get the sequence.
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
                FastAParser parseObjectForFile1 = new FastAParser { Alphabet = alphabet };
                ISequence originalSequence1 = parseObjectForFile1.Parse(filePath1).FirstOrDefault();
                Assert.IsNotNull(originalSequence1);
                aInput = new Sequence(alphabet, originalSequence1.ConvertToString());

                FastAParser parseObjectForFile2 = new FastAParser { Alphabet = alphabet };
                ISequence originalSequence2 = parseObjectForFile2.Parse(filePath2).FirstOrDefault();
                Assert.IsNotNull(originalSequence2);
                bInput = new Sequence(alphabet, originalSequence2.ConvertToString());
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
                    string matchValue = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                        Constants.MatchScoreNode);
                    string misMatchValue = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                           Constants.MisMatchScoreNode);
                    sm = new DiagonalSimilarityMatrix(int.Parse(matchValue, null),
                                                      int.Parse(misMatchValue, null));
                    break;
                default:
                    sm = new SimilarityMatrix(new StreamReader(blosumFilePath));
                    break;
            }

            int gapOpenCost = int.Parse(utilityObj.xmlUtil.GetTextValue(nodeName, Constants.GapOpenCostNode), null);
            int gapExtensionCost = int.Parse(utilityObj.xmlUtil.GetTextValue(nodeName, Constants.GapExtensionCostNode),
                                             null);

            NeedlemanWunschAligner needlemanWunschObj = new NeedlemanWunschAligner();
            if (AlignParameters.AllParam != alignParam)
            {
                needlemanWunschObj.SimilarityMatrix = sm;
                needlemanWunschObj.GapOpenCost = gapOpenCost;
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
                            needlemanWunschObj.GapExtensionCost = gapExtensionCost;
                            result = needlemanWunschObj.Align(sequences);
                            break;
                        default:
                            result = needlemanWunschObj.AlignSimple(sequences);
                            break;
                    }
                    break;
                case AlignParameters.AllParam:
                case AlignParameters.AllParamCode:
                    switch (alignType)
                    {
                        case AlignmentType.Align:
                            needlemanWunschObj.GapExtensionCost = gapExtensionCost;
                            result = needlemanWunschObj.Align(sm,
                                                              gapOpenCost, gapExtensionCost, aInput, bInput);
                            break;
                        default:
                            result = needlemanWunschObj.AlignSimple(sm, gapOpenCost, aInput, bInput);
                            break;
                    }
                    break;
                case AlignParameters.AlignTwo:
                case AlignParameters.AlignTwoCode:
                    switch (alignType)
                    {
                        case AlignmentType.Align:
                            needlemanWunschObj.GapExtensionCost = gapExtensionCost;
                            result = needlemanWunschObj.Align(aInput, bInput);
                            break;
                        default:
                            result = needlemanWunschObj.AlignSimple(aInput, bInput);
                            break;
                    }
                    break;
                default:
                    break;
            }

            // Read the xml file for getting both the files for aligning.
            string expectedSequence1, expectedSequence2, expectedScore;

            switch (alignType)
            {
                case AlignmentType.Align:
                    expectedScore = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.ExpectedGapExtensionScoreNode);
                    expectedSequence1 = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                        Constants.ExpectedGapExtensionSequence1Node);
                    expectedSequence2 = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                        Constants.ExpectedGapExtensionSequence2Node);
                    break;
                default:
                    expectedScore = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.ExpectedScoreNode);
                    expectedSequence1 = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequenceNode1);
                    expectedSequence2 = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequenceNode2);
                    break;
            }

            IList<IPairwiseSequenceAlignment> expectedOutput = new List<IPairwiseSequenceAlignment>();

            IPairwiseSequenceAlignment align = new PairwiseSequenceAlignment(aInput, bInput);
            PairwiseAlignedSequence alignedSeq = new PairwiseAlignedSequence
                                 {
                                     FirstSequence = new Sequence(alphabet, expectedSequence1),
                                     SecondSequence = new Sequence(alphabet, expectedSequence2),
                                     Score = Convert.ToInt32(expectedScore, null)
                                 };
            align.PairwiseAlignedSequences.Add(alignedSeq);
            expectedOutput.Add(align);

            ApplicationLog.WriteLine(string.Format("NeedlemanWunschAligner P1 : Final Score '{0}'.", expectedScore));
            ApplicationLog.WriteLine(string.Format("NeedlemanWunschAligner P1 : Aligned First Sequence is '{0}'.", expectedSequence1));
            ApplicationLog.WriteLine(string.Format("NeedlemanWunschAligner P1 : Aligned Second Sequence is '{0}'.", expectedSequence2));

            Assert.IsTrue(CompareAlignment(result, expectedOutput));
        }

        /// <summary>
        ///     Validates SmithWatermanAlignment algorithm for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node Name in the xml.</param>
        /// <param name="alignParam">parameter based on which certain validations are done.</param>
        private void ValidateSmithWatermanAlignment(string nodeName, AlignParameters alignParam)
        {
            ValidateSmithWatermanAlignment(nodeName, alignParam, SimilarityMatrixParameters.Default);
        }

        /// <summary>
        ///     Validates SmithWatermanAlignment algorithm for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node Name in the xml.</param>
        /// <param name="alignParam">parameter based on which certain validations are done.</param>
        /// <param name="similarityMatrixParam">Similarity Matrix Parameter.</param>
        private void ValidateSmithWatermanAlignment(string nodeName, AlignParameters alignParam,
                                                    SimilarityMatrixParameters similarityMatrixParam)
        {
            ValidateSmithWatermanAlignment(nodeName, alignParam, similarityMatrixParam, AlignmentType.SimpleAlign);
        }

        /// <summary>
        ///     Validates SmithWatermanAlignment algorithm for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node Name in the xml.</param>
        /// <param name="alignParam">parameter based on which certain validations are done.</param>
        /// <param name="similarityMatrixParam">Similarity Matrix Parameter.</param>
        /// <param name="alignType">Is the Align type Simple or Align with Gap Extension cost?</param>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        private void ValidateSmithWatermanAlignment(string nodeName, AlignParameters alignParam,
                                                    SimilarityMatrixParameters similarityMatrixParam,
                                                    AlignmentType alignType)
        {
            ISequence aInput, bInput;
            IAlphabet alphabet =
                Utility.GetAlphabet(utilityObj.xmlUtil.GetTextValue(nodeName, Constants.AlphabetNameNode));

            // Parse the files and get the sequence.
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

                FastAParser parseObjectForFile1 = new FastAParser { Alphabet = alphabet };
                ISequence originalSequence1 = parseObjectForFile1.Parse(filePath1).FirstOrDefault();
                Assert.IsNotNull(originalSequence1);
                aInput = new Sequence(alphabet, originalSequence1.ConvertToString());

                FastAParser parseObjectForFile2 = new FastAParser { Alphabet = alphabet };
                ISequence originalSequence2 = parseObjectForFile2.Parse(filePath2).FirstOrDefault();
                Assert.IsNotNull(originalSequence2);
                bInput = new Sequence(alphabet, originalSequence2.ConvertToString());
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
                    string matchValue = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                        Constants.MatchScoreNode);
                    string misMatchValue = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                           Constants.MisMatchScoreNode);
                    sm = new DiagonalSimilarityMatrix(int.Parse(matchValue, null),
                                                      int.Parse(misMatchValue, null));
                    break;
                default:
                    sm = new SimilarityMatrix(new StreamReader(blosumFilePath));
                    break;
            }

            int gapOpenCost = int.Parse(utilityObj.xmlUtil.GetTextValue(nodeName, Constants.GapOpenCostNode), null);
            int gapExtensionCost = int.Parse(utilityObj.xmlUtil.GetTextValue(nodeName, Constants.GapExtensionCostNode),
                                             null);

            SmithWatermanAligner smithWatermanObj = new SmithWatermanAligner();

            if (AlignParameters.AllParam != alignParam)
            {
                smithWatermanObj.SimilarityMatrix = sm;
                smithWatermanObj.GapOpenCost = gapOpenCost;
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
                            smithWatermanObj.GapExtensionCost = gapExtensionCost;
                            result = smithWatermanObj.Align(sequences);
                            break;
                        default:
                            result = smithWatermanObj.AlignSimple(sequences);
                            break;
                    }
                    break;
                case AlignParameters.AllParam:
                case AlignParameters.AllParamCode:
                    switch (alignType)
                    {
                        case AlignmentType.Align:
                            smithWatermanObj.GapExtensionCost = gapExtensionCost;
                            result = smithWatermanObj.Align(sm, gapOpenCost, gapExtensionCost, aInput, bInput);
                            break;
                        default:
                            result = smithWatermanObj.AlignSimple(sm, gapOpenCost, aInput, bInput);
                            break;
                    }
                    break;
                case AlignParameters.AlignTwo:
                case AlignParameters.AlignTwoCode:
                    switch (alignType)
                    {
                        case AlignmentType.Align:
                            smithWatermanObj.GapExtensionCost = gapExtensionCost;
                            result = smithWatermanObj.Align(aInput, bInput);
                            break;
                        default:
                            result = smithWatermanObj.AlignSimple(aInput, bInput);
                            break;
                    }
                    break;
                default:
                    break;
            }
            // Read the xml file for getting both the files for aligning.
            string expectedSequence1, expectedSequence2, expectedScore;

            switch (alignType)
            {
                case AlignmentType.Align:
                    expectedScore = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.ExpectedGapExtensionScoreNode);
                    expectedSequence1 = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                        Constants.ExpectedGapExtensionSequence1Node);
                    expectedSequence2 = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                        Constants.ExpectedGapExtensionSequence2Node);
                    break;
                default:
                    expectedScore = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.ExpectedScoreNode);
                    expectedSequence1 = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequenceNode1);
                    expectedSequence2 = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequenceNode2);
                    break;
            }

            IList<IPairwiseSequenceAlignment> expectedOutput = new List<IPairwiseSequenceAlignment>();

            IPairwiseSequenceAlignment align = new PairwiseSequenceAlignment();
            PairwiseAlignedSequence alignedSeq = new PairwiseAlignedSequence
                                 {
                                     FirstSequence = new Sequence(alphabet, expectedSequence1),
                                     SecondSequence = new Sequence(alphabet, expectedSequence2),
                                     Score = Convert.ToInt32(expectedScore, null),
                                     FirstOffset = Int32.MinValue,
                                     SecondOffset = Int32.MinValue,
                                 };
            align.PairwiseAlignedSequences.Add(alignedSeq);
            expectedOutput.Add(align);

            ApplicationLog.WriteLine(string.Format("SmithWatermanAligner P1 : Final Score '{0}'.", expectedScore));
            ApplicationLog.WriteLine(string.Format("SmithWatermanAligner P1 : Aligned First Sequence is '{0}'.",
                                                   expectedSequence1));
            ApplicationLog.WriteLine(string.Format("SmithWatermanAligner P1 : Aligned Second Sequence is '{0}'.",
                                                   expectedSequence2));

            Assert.IsTrue(CompareAlignment(result, expectedOutput));
        }


        /// <summary>
        ///     Validates Sequence Alignment test cases for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node Name in the xml.</param>
        /// <param name="validateProperty">Is validation of properties required?</param>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        private void ValidateGeneralSequenceAlignment(string nodeName, bool validateProperty)
        {
            // Read the xml file for getting both the files for aligning.
            string origSequence1 = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.SequenceNode1);
            string origSequence2 = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.SequenceNode2);
            IAlphabet alphabet =
                Utility.GetAlphabet(utilityObj.xmlUtil.GetTextValue(nodeName, Constants.AlphabetNameNode));

            ApplicationLog.WriteLine(string.Format("SequenceAlignment P1 : First sequence used is '{0}'.",
                                                   origSequence1));
            ApplicationLog.WriteLine(string.Format("SequenceAlignment P1 : Second sequence used is '{0}'.",
                                                   origSequence2));

            // Create two sequences
            ISequence aInput = new Sequence(alphabet, origSequence1);
            ISequence bInput = new Sequence(alphabet, origSequence2);

            // Add the sequences to the Sequence alignment object using AddSequence() method.
            IList<IPairwiseSequenceAlignment> sequenceAlignmentObj = new List<IPairwiseSequenceAlignment>();

            PairwiseAlignedSequence alignSeq = new PairwiseAlignedSequence {FirstSequence = aInput, SecondSequence = bInput};
            IPairwiseSequenceAlignment seqAlignObj = new PairwiseSequenceAlignment
            {
                alignSeq
            };
            sequenceAlignmentObj.Add(seqAlignObj);

            // Read the output back and validate the same.
            IList<PairwiseAlignedSequence> newAlignedSequences =
                sequenceAlignmentObj[0].PairwiseAlignedSequences;

            ApplicationLog.WriteLine(string.Format("SequenceAlignment P1 : First sequence read is '{0}'.",
                                                   origSequence1));
            ApplicationLog.WriteLine(string.Format("SequenceAlignment P1 : Second sequence read is '{0}'.",
                                                   origSequence2));

            if (validateProperty)
            {
                string score = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.MatchScoreNode);
                string seqCount = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.SequenceCountNode);

                Assert.IsFalse(sequenceAlignmentObj.IsReadOnly);
                Assert.AreEqual(sequenceAlignmentObj.Count.ToString((IFormatProvider) null), seqCount);
                Assert.AreEqual(
                    sequenceAlignmentObj[0].PairwiseAlignedSequences[0].Score.ToString((IFormatProvider) null), score);
                Assert.AreEqual(sequenceAlignmentObj.Count.ToString((IFormatProvider) null), seqCount);

                ApplicationLog.WriteLine("SequenceAlignment P1 : Successfully validated the IsRead Property");
                ApplicationLog.WriteLine("SequenceAlignment P1 : Successfully validated the Count Property");
                ApplicationLog.WriteLine("SequenceAlignment P1 : Successfully validated the Sequences Property");
            }
            else
            {
                Assert.AreEqual(new String(newAlignedSequences[0].FirstSequence.Select(a => (char) a).ToArray()),
                                origSequence1);
                Assert.AreEqual(new String(newAlignedSequences[0].SecondSequence.Select(a => (char) a).ToArray()),
                                origSequence2);
            }
        }

        /// <summary>
        ///     Compare the alignment of mummer and defined alignment
        /// </summary>
        /// <param name="actualAlignment"></param>
        /// <param name="expectedAlignment">expected output</param>
        /// <returns>Compare result of alignments</returns>
        private static bool CompareAlignment(IList<IPairwiseSequenceAlignment> actualAlignment,
                                             IList<IPairwiseSequenceAlignment> expectedAlignment)
        {
            return AlignmentHelpers.CompareAlignment(actualAlignment, expectedAlignment);
        }

        /// <summary>
        ///     Validates Sequence Alignment Class General methods.
        /// </summary>
        /// <param name="nodeName">Node Name in the xml.</param>
        /// <param name="methodName">Name of the SequenceAlignment method to be validated</param>
        /// <param name="isSeqAlignDefCtr">Is sequence alignment Def Constructor</param>
        private void ValidateSequenceAlignmentGeneralMethods(string nodeName, SeqAlignmentMethods methodName,
                                                             bool isSeqAlignDefCtr)
        {
            // Read the xml file for getting both the files for aligning.
            string origSequence1 = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.SequenceNode1);
            string origSequence2 = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.SequenceNode2);
            IAlphabet alphabet =
                Utility.GetAlphabet(utilityObj.xmlUtil.GetTextValue(nodeName, Constants.AlphabetNameNode));
            string seqCount = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.SeqCountNode);
            string alignedSeqCountAfterAddSeq = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                                Constants
                                                                                    .AlignedSeqCountAfterAddAlignedSeqNode);
            string arrayLength = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.ArraySizeNode);

            PairwiseAlignedSequence[] alignedSeqItems = new PairwiseAlignedSequence[int.Parse(arrayLength, null)];
            const int Index = 0;

            // Create two sequences
            ISequence aInput = new Sequence(alphabet, origSequence1);
            ISequence bInput = new Sequence(alphabet, origSequence2);

            // Add the sequences to the Sequence alignment object using AddSequence() method.
            IList<IPairwiseSequenceAlignment> sequenceAlignmentObj = new List<IPairwiseSequenceAlignment>();

            PairwiseAlignedSequence alignSeq = new PairwiseAlignedSequence {FirstSequence = aInput, SecondSequence = bInput};
            IPairwiseSequenceAlignment seqAlignObj = isSeqAlignDefCtr
                                                         ? new PairwiseSequenceAlignment()
                                                         : new PairwiseSequenceAlignment(aInput, bInput);

            seqAlignObj.Add(alignSeq);
            sequenceAlignmentObj.Add(seqAlignObj);

            IList<PairwiseAlignedSequence> newAlignedSequences =
                sequenceAlignmentObj[0].PairwiseAlignedSequences;

            switch (methodName)
            {
                case SeqAlignmentMethods.Add:
                    seqAlignObj.Add(alignSeq);
                    Assert.AreEqual(seqCount,
                                    seqAlignObj.PairwiseAlignedSequences.Count.ToString((IFormatProvider) null));
                    break;
                case SeqAlignmentMethods.Clear:
                    seqAlignObj.Clear();
                    Assert.AreEqual(0, seqAlignObj.PairwiseAlignedSequences.Count);
                    break;
                case SeqAlignmentMethods.Contains:
                    Assert.IsTrue(seqAlignObj.Contains(newAlignedSequences[0]));
                    break;
                case SeqAlignmentMethods.CopyTo:
                    seqAlignObj.CopyTo(alignedSeqItems, Index);

                    // Validate Copied array.
                    Assert.AreEqual(alignedSeqItems[Index].FirstSequence, seqAlignObj.FirstSequence);
                    Assert.AreEqual(alignedSeqItems[Index].SecondSequence, seqAlignObj.SecondSequence);
                    break;
                case SeqAlignmentMethods.Remove:
                    seqAlignObj.Remove(newAlignedSequences[0]);

                    // Validate whether removed item is deleted from SequenceAlignment.
                    Assert.AreEqual(0, newAlignedSequences.Count);
                    break;
                case SeqAlignmentMethods.AddSequence:
                    seqAlignObj.AddSequence(newAlignedSequences[0]);

                    // Validate SeqAlignObj after adding aligned sequence.
                    Assert.AreEqual(alignedSeqCountAfterAddSeq, seqAlignObj.Count.ToString((IFormatProvider) null));
                    break;
                case SeqAlignmentMethods.GetEnumerator:
                    IEnumerator<PairwiseAlignedSequence> alignedSeqList = seqAlignObj.GetEnumerator();

                    // Aligned Sequence list after iterating through ailgnedSeq collection.
                    Assert.IsNotNull(alignedSeqList);
                    break;
                default:
                    break;
            }

            ApplicationLog.WriteLine("SequenceAlignment P1 : Successfully validated the IsRead Property");
            ApplicationLog.WriteLine("SequenceAlignment P1 : Successfully validated the Count Property");
            ApplicationLog.WriteLine("SequenceAlignment P1 : Successfully validated the Sequences Property");
        }

        #endregion Supporting Methods
    }
}