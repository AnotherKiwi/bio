﻿using System;
using System.Collections.Generic;
using System.Globalization;
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
    ///     PairwiseOverlapAlignment algorithm P2 test cases
    /// </summary>
    [TestFixture]
    public class PairwiseOverlapAlignerP2TestCases
    {
        #region Enums

        /// <summary>
        ///     Alignment Parameters which are used for different test cases
        ///     based on which the test cases are executed.
        /// </summary>
        private enum AlignParameters
        {
            AlignList,
            AllParam,
            AlignTwo,
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
        ///     Types of invalid sequence
        /// </summary>
        private enum InvalidSequenceType
        {
            SequenceWithSpecialChars,
            AlphabetMap,
            EmptySequence,
            SequenceWithInvalidChars,
            InvalidSequence,
            SequenceWithSpaces,
            SequenceWithGap,
            SequenceWithUnicodeChars,
            Default
        }

        /// <summary>
        ///     Input sequences to get aligned in different cases.
        /// </summary>
        private enum SequenceCaseType
        {
            LowerCase,
            UpperCase,
            LowerUpperCase,
            Default
        }

        /// <summary>
        ///     Types of invalid similarity matrix
        /// </summary>
        private enum SimilarityMatrixInvalidTypes
        {
            NonMatchingSimilarityMatrix,
            EmptySimilaityMatrix,
            OnlyAlphabetSimilarityMatrix,
            FewAlphabetsSimilarityMatrix,
            ModifiedSimilarityMatrix,
            NullSimilarityMatrix,
            EmptySequence,
            ExpectedErrorMessage,
        }

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

        #region Test Cases

        /// <summary>
        ///     Pass a Valid Sequence(Lower case) with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method Align(sequence1, sequence2)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA File
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void PairwiseOverlapSimpleAlignTwoLowerCaseSequencesFromTextFile()
        {
            ValidatePairwiseOverlapAlignment(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                true,
                SequenceCaseType.LowerCase,
                AlignParameters.AlignTwo);
        }

        /// <summary>
        ///     Pass a Valid Sequence(Upper case) with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method Align(sequence1, sequence2)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA File
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void PairwiseOverlapSimpleAlignTwoUpperCaseSequencesFromTextFile()
        {
            ValidatePairwiseOverlapAlignment(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                true,
                SequenceCaseType.UpperCase,
                AlignParameters.AlignTwo);
        }

        /// <summary>
        ///     Pass a Valid Sequence(Lower and Upper case) with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method Align(sequence1, sequence2)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA File
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void PairwiseOverlapSimpleAlignTwoLowerUpperCaseSequencesFromTextFile()
        {
            ValidatePairwiseOverlapAlignment(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                true,
                SequenceCaseType.LowerUpperCase,
                AlignParameters.AlignTwo);
        }

        /// <summary>
        ///     Pass a Valid Sequence(Lower case) with valid GapPenalty, Similarity Matrix
        ///     from code using the method Align(sequence1, sequence2)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA File
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void PairwiseOverlapSimpleAlignTwoLowerCaseSequencesFromCode()
        {
            ValidatePairwiseOverlapAlignment(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                false,
                SequenceCaseType.LowerCase,
                AlignParameters.AlignTwo);
        }

        /// <summary>
        ///     Pass a Valid Sequence(Upper case) with valid GapPenalty, Similarity Matrix
        ///     from code using the method Align(sequence1, sequence2)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA File
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void PairwiseOverlapSimpleAlignTwoUpperCaseSequencesFromCode()
        {
            ValidatePairwiseOverlapAlignment(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                false,
                SequenceCaseType.UpperCase,
                AlignParameters.AlignTwo);
        }

        /// <summary>
        ///     Pass a Valid Sequence(Lower and Upper case) with valid GapPenalty, Similarity Matrix
        ///     from code using the method Align(sequence1, sequence2)
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA File
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void PairwiseOverlapSimpleAlignTwoLowerUpperCaseSequencesFromCode()
        {
            ValidatePairwiseOverlapAlignment(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                false,
                SequenceCaseType.LowerUpperCase,
                AlignParameters.AlignTwo);
        }

        /// <summary>
        ///     Pass a Valid Sequence(Lower case) with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method AlignList
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA File
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void PairwiseOverlapSimpleAlignListLowerCaseSequencesFromTextFile()
        {
            ValidatePairwiseOverlapAlignment(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                true,
                SequenceCaseType.LowerCase,
                AlignParameters.AlignList);
        }

        /// <summary>
        ///     Pass a Valid Sequence(Upper case) with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method AlignList
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA File
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void PairwiseOverlapSimpleAlignListUpperCaseSequencesFromTextFile()
        {
            ValidatePairwiseOverlapAlignment(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                true,
                SequenceCaseType.UpperCase,
                AlignParameters.AlignList);
        }

        /// <summary>
        ///     Pass a Valid Sequence(Lower and Upper case) with valid GapPenalty, Similarity Matrix
        ///     which is in a text file using the method AlignList
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA File
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void PairwiseOverlapSimpleAlignListLowerUpperCaseSequencesFromTextFile()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapAlignAlgorithmNodeName, true,
                                             SequenceCaseType.LowerUpperCase,
                                             AlignParameters.AlignList);
        }

        /// <summary>
        ///     Pass a Valid Sequence(Lower case)  with valid GapPenalty, Similarity Matrix
        ///     from text file using the method AlignList
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA File
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void PairwiseOverlapSimpleAlignAllParamsLowerCaseSequencesFromTextFile()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapAlignAlgorithmNodeName, true,
                                             SequenceCaseType.LowerCase,
                                             AlignParameters.AllParam);
        }

        /// <summary>
        ///     Pass a Valid Sequence(Upper case) with valid GapPenalty, Similarity Matrix
        ///     from text file using the method AlignList
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA File
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void PairwiseOverlapSimpleAlignAllParamsUpperCaseSequencesFromTextFile()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapAlignAlgorithmNodeName, true,
                                             SequenceCaseType.UpperCase,
                                             AlignParameters.AllParam);
        }

        /// <summary>
        ///     Pass a Valid Sequence(Lower and Upper case) with valid GapPenalty, Similarity Matrix
        ///     from text file using the method AlignList
        ///     and validate if the aligned sequence is as expected and
        ///     also validate the score for the same
        ///     Input : FastA File
        ///     Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void PairwiseOverlapSimpleAlignAllParamsLowerUpperCaseSequencesFromTextFile()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapAlignAlgorithmNodeName, true,
                                             SequenceCaseType.LowerUpperCase, AlignParameters.AllParam);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix (Non Matching)
        ///     from text file and validate if Align(se1,seq2) throws expected exception
        ///     Input : Two Input sequence and Non Matching similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignTwoSequencesWithNonMatchingSimilarityMatrix()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSimilarityMatrix(
                Constants.PairwiseOverlapAlignAlgorithmNodeName, true,
                SimilarityMatrixInvalidTypes.NonMatchingSimilarityMatrix, AlignParameters.AlignTwo,
                AlignmentType.SimpleAlign);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix (Non Matching)
        ///     from text file and validate if Align using List throws expected exception
        ///     Input : Input sequence List and Non Matching similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignListSequencesWithNonMatchingSimilarityMatrix()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSimilarityMatrix(
                Constants.PairwiseOverlapAlignAlgorithmNodeName, true,
                SimilarityMatrixInvalidTypes.NonMatchingSimilarityMatrix, AlignParameters.AlignList,
                AlignmentType.SimpleAlign);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix (Non Matching)
        ///     from text file and validate if Align using all params throws expected exception
        ///     Input : Input sequence and Non Matching similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignAllParamsSequencesWithNonMatchingSimilarityMatrix()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSimilarityMatrix(
                Constants.PairwiseOverlapAlignAlgorithmNodeName, true,
                SimilarityMatrixInvalidTypes.NonMatchingSimilarityMatrix, AlignParameters.AllParam,
                AlignmentType.SimpleAlign);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix (Empty)
        ///     from text file and validate if Align(se1,seq2) throws expected exception
        ///     Input : Two Input sequence and Empty similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignTwoSequencesWithEmptySimilarityMatrix()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSimilarityMatrix(
                Constants.PairwiseOverlapAlignAlgorithmNodeName, true,
                SimilarityMatrixInvalidTypes.EmptySimilaityMatrix, AlignParameters.AlignTwo,
                AlignmentType.SimpleAlign);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix (Empty)
        ///     from text file and validate if Align using List throws expected exception
        ///     Input : Input sequence List and Empty similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignListSequencesWithEmptySimilarityMatrix()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSimilarityMatrix(
                Constants.PairwiseOverlapAlignAlgorithmNodeName, true,
                SimilarityMatrixInvalidTypes.EmptySimilaityMatrix,
                AlignParameters.AlignList,
                AlignmentType.SimpleAlign);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix (Empty)
        ///     from text file and validate if Align using all params throws expected exception
        ///     Input : Input sequence and Empty similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignAllParamsSequencesWithEmptySimilarityMatrix()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSimilarityMatrix(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                true,
                SimilarityMatrixInvalidTypes.EmptySimilaityMatrix,
                AlignParameters.AllParam,
                AlignmentType.SimpleAlign);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix (Only Alphabet)
        ///     from text file and validate if Align(se1,seq2) throws expected exception
        ///     Input : Two Input sequence and Only Alphabet similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignTwoSequencesWithOnlyAlphabetSimilarityMatrix()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSimilarityMatrix(
                Constants.PairwiseOverlapAlignAlgorithmNodeName, true,
                SimilarityMatrixInvalidTypes.OnlyAlphabetSimilarityMatrix,
                AlignParameters.AlignTwo,
                AlignmentType.SimpleAlign);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix (Only Alphabet)
        ///     from text file and validate if Align using List throws expected exception
        ///     Input : Input sequence List and Only Alphabet similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignListSequencesWithOnlyAlphabetSimilarityMatrix()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSimilarityMatrix(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                true,
                SimilarityMatrixInvalidTypes.OnlyAlphabetSimilarityMatrix,
                AlignParameters.AlignList,
                AlignmentType.SimpleAlign);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix (Only Alphabet)
        ///     from text file and validate if Align using all params throws expected exception
        ///     Input : Input sequence and Only Alphabet similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignAllParamsSequencesWithOnlyAlphabetSimilarityMatrix()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSimilarityMatrix(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                true,
                SimilarityMatrixInvalidTypes.OnlyAlphabetSimilarityMatrix,
                AlignParameters.AllParam,
                AlignmentType.SimpleAlign);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix (Modified)
        ///     from text file and validate if Align(se1,seq2) throws expected exception
        ///     Input : Two Input sequence and Modified similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignTwoSequencesWithModifiedSimilarityMatrix()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSimilarityMatrix(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                true,
                SimilarityMatrixInvalidTypes.ModifiedSimilarityMatrix,
                AlignParameters.AlignTwo,
                AlignmentType.SimpleAlign);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix (Modified)
        ///     from text file and validate if Align using list throws expected exception
        ///     Input : Input sequence list and Modified similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignListSequencesWithModifiedSimilarityMatrix()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSimilarityMatrix(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                true,
                SimilarityMatrixInvalidTypes.ModifiedSimilarityMatrix,
                AlignParameters.AlignList,
                AlignmentType.SimpleAlign);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix (Modified)
        ///     from text file and validate if Align using all params throws expected exception
        ///     Input : Input sequence and Modified similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignAllParamsSequencesWithModifiedSimilarityMatrix()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSimilarityMatrix(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                true,
                SimilarityMatrixInvalidTypes.ModifiedSimilarityMatrix,
                AlignParameters.AllParam,
                AlignmentType.SimpleAlign);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix (Few Alphabet)
        ///     from text file and validate if Align(se1,seq2) throws expected exception
        ///     Input : Two Input sequence and Few Alphabet similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignTwoSequencesWithFewAlphabetSimilarityMatrix()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSimilarityMatrix(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                true,
                SimilarityMatrixInvalidTypes.FewAlphabetsSimilarityMatrix,
                AlignParameters.AlignTwo,
                AlignmentType.SimpleAlign);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix (Few Alphabet)
        ///     from text file and validate if Align using list throws expected exception
        ///     Input : Input sequence list and Few Alphabet similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignListSequencesWithFewAlphabetSimilarityMatrix()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSimilarityMatrix(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                true,
                SimilarityMatrixInvalidTypes.FewAlphabetsSimilarityMatrix,
                AlignParameters.AlignList,
                AlignmentType.SimpleAlign);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix (Few Alphabet)
        ///     from text file and validate if Align using all params throws expected exception
        ///     Input : Input sequence and Few Alphabet similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignAllParamsSequencesWithFewAlphabetSimilarityMatrix()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSimilarityMatrix(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                true,
                SimilarityMatrixInvalidTypes.FewAlphabetsSimilarityMatrix,
                AlignParameters.AllParam,
                AlignmentType.SimpleAlign);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix (Empty)
        ///     from code and validate if Align(se1,seq2) throws expected exception
        ///     Input : Two Input sequence and Empty similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignTwoSequencesFromCodeWithEmptySimilarityMatrix()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSimilarityMatrix(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                false,
                SimilarityMatrixInvalidTypes.EmptySimilaityMatrix,
                AlignParameters.AlignTwo,
                AlignmentType.SimpleAlign);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix (Empty)
        ///     from text file and validate if Align using List throws expected exception
        ///     Input : Input sequence List and Empty similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignListSequencesFromCodeWithEmptySimilarityMatrix()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSimilarityMatrix(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                false,
                SimilarityMatrixInvalidTypes.EmptySimilaityMatrix,
                AlignParameters.AlignList,
                AlignmentType.SimpleAlign);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix (Empty)
        ///     from code and validate if Align using all params throws expected exception
        ///     Input : Input sequence List and Empty similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignAllParamsSequencesFromCodeWithEmptySimilarityMatrix()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSimilarityMatrix(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                false,
                SimilarityMatrixInvalidTypes.EmptySimilaityMatrix,
                AlignParameters.AllParam,
                AlignmentType.SimpleAlign);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix (Only Alphabet)
        ///     from code and validate if Align(seq1,seq2) throws expected exception
        ///     Input : Input sequence List and Only Alphabet similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignTwoSequencesFromCodeWithOnlyAlphabetSimilarityMatrix()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSimilarityMatrix(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                false,
                SimilarityMatrixInvalidTypes.OnlyAlphabetSimilarityMatrix,
                AlignParameters.AlignTwo,
                AlignmentType.SimpleAlign);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix (Only Alphabet)
        ///     from code and validate if Align using list throws expected exception
        ///     Input : Input sequence List and Only Alphabet similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignListSequencesFromCodeWithOnlyAlphabetSimilarityMatrix()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSimilarityMatrix(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                false,
                SimilarityMatrixInvalidTypes.OnlyAlphabetSimilarityMatrix,
                AlignParameters.AlignList,
                AlignmentType.SimpleAlign);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix (Only Alphabet)
        ///     from code and validate if Align using all params throws expected exception
        ///     Input : Input sequence List and Only Alphabet similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignAllParamsSequencesFromCodeWithOnlyAlphabetSimilarityMatrix()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSimilarityMatrix(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                false,
                SimilarityMatrixInvalidTypes.OnlyAlphabetSimilarityMatrix,
                AlignParameters.AllParam,
                AlignmentType.SimpleAlign);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix (Null)
        ///     from code and validate if Align using all params throws expected exception
        ///     Input : Input sequence and Few Alphabet similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignTwoSequencesFromCodeWithNullSimilarityMatrix()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSimilarityMatrix(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                false,
                SimilarityMatrixInvalidTypes.NullSimilarityMatrix,
                AlignParameters.AlignTwo,
                AlignmentType.SimpleAlign);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix (Null)
        ///     from code and validate if Align using all params throws expected exception
        ///     Input : Input sequence and Few Alphabet similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignListSequencesFromCodeWithNullSimilarityMatrix()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSimilarityMatrix(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                false,
                SimilarityMatrixInvalidTypes.NullSimilarityMatrix,
                AlignParameters.AlignList,
                AlignmentType.SimpleAlign);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Similarity Matrix (Null)
        ///     from code and validate if Align using all params throws expected exception
        ///     Input : Input sequence and Few Alphabet similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignAllParamsSequencesFromCodeWithNullSimilarityMatrix()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSimilarityMatrix(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                false,
                SimilarityMatrixInvalidTypes.NullSimilarityMatrix,
                AlignParameters.AllParam,
                AlignmentType.SimpleAlign);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Invalid DiagonalSimilarityMatrix
        ///     from text file and validate if Align using all params throws expected exception
        ///     Input : Input sequence and Few Alphabet similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignTwoSequencesWithInvalidDiagonalSimilarityMatrix()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSimilarityMatrix(
                Constants.PairwiseOverlapDiagonalSimMatAlignAlgorithmNodeName,
                true,
                SimilarityMatrixInvalidTypes.NonMatchingSimilarityMatrix,
                AlignParameters.AlignTwo,
                AlignmentType.SimpleAlign);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Invalid DiagonalSimilarityMatrix
        ///     from text file and validate if Align using all params throws expected exception
        ///     Input : Input sequence and Invalid DiagonalSimilarityMatrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignListSequencesWithInvalidDiagonalSimilarityMatrix()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSimilarityMatrix(
                Constants.PairwiseOverlapDiagonalSimMatAlignAlgorithmNodeName,
                true,
                SimilarityMatrixInvalidTypes.NonMatchingSimilarityMatrix,
                AlignParameters.AlignList,
                AlignmentType.SimpleAlign);
        }

        /// <summary>
        ///     Pass a Valid Sequence with valid GapPenalty, Invalid DiagonalSimilarityMatrix
        ///     from text file and validate if Align using all params throws expected exception
        ///     Input : Input sequence and Invalid DiagonalSimilarityMatrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignAllParamsSequencesWithInvalidDiagonalSimilarityMatrix()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSimilarityMatrix(
                Constants.PairwiseOverlapDiagonalSimMatAlignAlgorithmNodeName,
                true,
                SimilarityMatrixInvalidTypes.NonMatchingSimilarityMatrix,
                AlignParameters.AllParam,
                AlignmentType.SimpleAlign);
        }

        /// <summary>
        ///     Pass a In Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     from text file and validate if Parser throws expected exception
        ///     Input : Input sequence List and Only Alphabet similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignTwoWithInvalidSequencesFromTextFile()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSequence(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                true,
                InvalidSequenceType.SequenceWithSpecialChars,
                AlignParameters.AlignTwo,
                AlignmentType.SimpleAlign,
                InvalidSequenceType.InvalidSequence);
        }

        /// <summary>
        ///     Pass a In Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     from text file and validate if Parser throws expected exception
        ///     Input : Input sequence List and Only Alphabet similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignListWithInvalidSequencesFromTextFile()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSequence(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                true,
                InvalidSequenceType.SequenceWithSpecialChars,
                AlignParameters.AlignList,
                AlignmentType.SimpleAlign,
                InvalidSequenceType.InvalidSequence);
        }

        /// <summary>
        ///     Pass a In Valid Sequence with valid GapPenalty, Similarity Matrix
        ///     from text file and validate if Parser throws expected exception
        ///     Input : Input sequence List and Only Alphabet similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignAllParamsWithInvalidSequencesFromTextFile()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSequence(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                true,
                InvalidSequenceType.SequenceWithSpecialChars,
                AlignParameters.AllParam,
                AlignmentType.SimpleAlign,
                InvalidSequenceType.InvalidSequence);
        }

        /// <summary>
        ///     Pass Empty Sequence with valid GapPenalty, Similarity Matrix
        ///     from text file and validate if Parser throws expected exception
        ///     Input : Input sequence List and Only Alphabet similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignTwoWithEmptySequencesFromTextFile()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSequence(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                true,
                InvalidSequenceType.EmptySequence,
                AlignParameters.AlignTwo,
                AlignmentType.SimpleAlign,
                InvalidSequenceType.SequenceWithInvalidChars);
        }

        /// <summary>
        ///     Pass Empty Sequence with valid GapPenalty, Similarity Matrix
        ///     from text file and validate if Parser throws expected exception
        ///     Input : Input sequence List and Only Alphabet similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignListWithEmptySequencesFromTextFile()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSequence(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                true,
                InvalidSequenceType.EmptySequence,
                AlignParameters.AlignList,
                AlignmentType.SimpleAlign,
                InvalidSequenceType.SequenceWithInvalidChars);
        }

        /// <summary>
        ///     Pass Empty Sequence with valid GapPenalty, Similarity Matrix
        ///     from text file and validate if Parser throws expected exception
        ///     Input : Input sequence List and Only Alphabet similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignAllParamsWithEmptySequencesFromTextFile()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSequence(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                true,
                InvalidSequenceType.EmptySequence,
                AlignParameters.AllParam,
                AlignmentType.SimpleAlign,
                InvalidSequenceType.SequenceWithInvalidChars);
        }

        /// <summary>
        ///     Pass invalid Sequence(Contains Gap) with valid GapPenalty, Similarity Matrix
        ///     from text file and validate if Parser throws expected exception
        ///     Input : Input sequence List and Only Alphabet similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignTwoWithGapSequencesFromTextFile()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSequence(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                true,
                InvalidSequenceType.SequenceWithGap,
                AlignParameters.AlignTwo,
                AlignmentType.SimpleAlign,
                InvalidSequenceType.Default);
        }

        /// <summary>
        ///     Pass invalid Sequence(Contains Gap) with valid GapPenalty, Similarity Matrix
        ///     from text file and validate if Align using all params throws expected exception
        ///     Input : Input sequence List and Only Alphabet similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignListWithGapSequencesFromTextFile()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSequence(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                true,
                InvalidSequenceType.SequenceWithGap,
                AlignParameters.AlignList,
                AlignmentType.SimpleAlign,
                InvalidSequenceType.Default);
        }

        /// <summary>
        ///     Pass invalid Sequence(Contains Gap) with valid GapPenalty, Similarity Matrix
        ///     from text file and validate if Align using all params throws expected exception
        ///     Input : Input sequence List and Only Alphabet similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignAllParamsWithGapSequencesFromTextFile()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSequence(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                true,
                InvalidSequenceType.SequenceWithGap,
                AlignParameters.AllParam,
                AlignmentType.SimpleAlign,
                InvalidSequenceType.Default);
        }

        /// <summary>
        ///     Pass invalid Sequence(Unicode) with valid GapPenalty, Similarity Matrix
        ///     from text file and validate if Align using all params throws expected exception
        ///     Input : Input sequence List and Only Alphabet similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignTwoWithUnicodeSequencesFromTextFile()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSequence(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                true,
                InvalidSequenceType.SequenceWithUnicodeChars,
                AlignParameters.AlignTwo,
                AlignmentType.SimpleAlign,
                InvalidSequenceType.SequenceWithUnicodeChars);
        }

        /// <summary>
        ///     Pass invalid Sequence(Unicode) with valid GapPenalty, Similarity Matrix
        ///     from text file and validate if Align using all params throws expected exception
        ///     Input : Input sequence List and Only Alphabet similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignListWithUnicodeSequencesFromTextFile()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSequence(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                true,
                InvalidSequenceType.SequenceWithUnicodeChars,
                AlignParameters.AlignList,
                AlignmentType.SimpleAlign,
                InvalidSequenceType.SequenceWithUnicodeChars);
        }

        /// <summary>
        ///     Pass invalid Sequence(Unicode) with valid GapPenalty, Similarity Matrix
        ///     from text file and validate if Align using all params throws expected exception
        ///     Input : Input sequence List and Only Alphabet similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignAllParamsWithUnicodeSequencesFromTextFile()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSequence(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                true,
                InvalidSequenceType.SequenceWithUnicodeChars,
                AlignParameters.AllParam,
                AlignmentType.SimpleAlign,
                InvalidSequenceType.SequenceWithUnicodeChars);
        }

        /// <summary>
        ///     Pass invalid Sequence(Spaces) with valid GapPenalty, Similarity Matrix
        ///     from text file and validate if Align using all params throws expected exception
        ///     Input : Input sequence List and Only Alphabet similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignTwoSequencesWithSpacesFromTextFile()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSequence(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                true,
                InvalidSequenceType.SequenceWithSpaces,
                AlignParameters.AlignTwo,
                AlignmentType.SimpleAlign,
                InvalidSequenceType.SequenceWithSpaces);
        }

        /// <summary>
        ///     Pass invalid Sequence(Spaces) with valid GapPenalty, Similarity Matrix
        ///     from text file and validate if Align using all params throws expected exception
        ///     Input : Input sequence List and Only Alphabet similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignListSequencesWithSpacesFromTextFile()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSequence(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                true,
                InvalidSequenceType.SequenceWithSpaces,
                AlignParameters.AlignList,
                AlignmentType.SimpleAlign,
                InvalidSequenceType.SequenceWithSpaces);
        }

        /// <summary>
        ///     Pass invalid Sequence(Spaces) with valid GapPenalty, Similarity Matrix
        ///     from text file and validate if Align using all params throws expected exception
        ///     Input : Input sequence List and Only Alphabet similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        public void InValidatePOSimpleAlignParamsSequencesWithSpacesFromTextFile()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSequence(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                true,
                InvalidSequenceType.SequenceWithSpaces,
                AlignParameters.AllParam,
                AlignmentType.SimpleAlign,
                InvalidSequenceType.SequenceWithSpaces);
        }

        /// <summary>
        ///     Pass invalid Sequence with valid GapPenalty, Similarity Matrix
        ///     from code and validate if Align using all params throws expected exception
        ///     Input : Input sequence List and Only Alphabet similarity matrix
        ///     Validation : Exception should be thrown
        /// </summary>
        [Test]
        [Category("Priority2")]
        [Ignore("The underlying exception messages differ between mono and .NET")]
        public void InValidatePOSimpleAlignTwoWithInvalidSequencesFromCode()
        {
            InValidatePairwiseOverlapAlignmentWithInvalidSequence(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                false,
                InvalidSequenceType.SequenceWithSpecialChars,
                AlignParameters.AllParam,
                AlignmentType.SimpleAlign,
                InvalidSequenceType.AlphabetMap);
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
        [Category("Priority2")]
        public void ValidatePairwiseOverlapAlignTwoDnaSequences()
        {
            ValidatePairwiseOverlapAlignment(
                Constants.PairwiseOverlapDnaAlignAlgorithmNodeName,
                true,
                SequenceCaseType.LowerCase,
                AlignParameters.AlignTwo,
                AlignmentType.Align);
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
        [Category("Priority2")]
        public void ValidatePairwiseOverlapAlignTwoRnaSequences()
        {
            ValidatePairwiseOverlapAlignment(
                Constants.PairwiseOverlapRnaAlignAlgorithmNodeName,
                true,
                SequenceCaseType.LowerCase,
                AlignParameters.AlignTwo,
                AlignmentType.Align);
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
        [Category("Priority2")]
        public void ValidatePairwiseOverlapAlignTwoProteinSequences()
        {
            ValidatePairwiseOverlapAlignment(
                Constants.PairwiseOverlapProAlignAlgorithmNodeName,
                true,
                SequenceCaseType.LowerCase,
                AlignParameters.AlignTwo,
                AlignmentType.Align);
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
        [Category("Priority2")]
        public void ValidatePairwiseOverlapAlignTwoSequencesGapCostMax()
        {
            ValidatePairwiseOverlapAlignment(
                Constants.PairwiseOverlapGapCostMaxAlignAlgorithmNodeName,
                true,
                SequenceCaseType.LowerCase,
                AlignParameters.AlignTwo,
                AlignmentType.Align);
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
        [Category("Priority2")]
        public void ValidatePairwiseOverlapAlignTwoSequencesGapCostMin()
        {
            ValidatePairwiseOverlapAlignment(
                Constants.PairwiseOverlapGapCostMinAlignAlgorithmNodeName,
                true,
                SequenceCaseType.LowerCase,
                AlignParameters.AlignTwo,
                AlignmentType.Align);
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
        [Category("Priority2")]
        public void ValidatePairwiseOverlapAlignTwoSequencesWithBlosomSimilarityMatrix()
        {
            ValidatePairwiseOverlapAlignment(
                Constants.PairwiseOverlapBlosumAlignAlgorithmNodeName,
                true,
                SequenceCaseType.LowerCase,
                AlignParameters.AlignTwo,
                AlignmentType.Align);
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
        [Category("Priority2")]
        public void ValidatePairwiseOverlapAlignTwoSequencesWithPamSimilarityMatrix()
        {
            ValidatePairwiseOverlapAlignment(
                Constants.PairwiseOverlapPamAlignAlgorithmNodeName,
                true,
                SequenceCaseType.LowerCase,
                AlignParameters.AlignTwo,
                AlignmentType.Align);
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
        [Category("Priority2")]
        public void ValidatePairwiseOverlapAlignTwoSequencesWithTextReaderSimilarityMatrix()
        {
            ValidatePairwiseOverlapAlignment(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                true,
                SequenceCaseType.LowerCase,
                AlignParameters.AlignTwo,
                AlignmentType.Align,
                SimilarityMatrixParameters.TextReader);
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
        [Category("Priority2")]
        public void ValidatePairwiseOverlapAlignTwoSequencesWithDiagonalSimilarityMatrix()
        {
            ValidatePairwiseOverlapAlignment(
                Constants.PairwiseOverlapDiagonalSimMatAlignAlgorithmNodeName,
                true, SequenceCaseType.LowerCase,
                AlignParameters.AlignTwo,
                AlignmentType.Align,
                SimilarityMatrixParameters.DiagonalMatrix);
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
        [Category("Priority2")]
        public void ValidatePairwiseOverlapAlignTwoSequencesWithEqualGapOpenAndExtensionCost()
        {
            ValidatePairwiseOverlapAlignment(
                Constants.PairwiseOverlapEqualAlignAlgorithmNodeName,
                true, SequenceCaseType.LowerCase,
                AlignParameters.AlignTwo,
                AlignmentType.Align);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        ///     Validates PairwiseOverlapAlignment algorithm for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Xml node name</param>
        /// <param name="isTextFile">Is text file an input.</param>
        /// <param name="caseType">Case Type</param>
        /// <param name="additionalParameter">parameter based on which certain validations are done.</param>
        private void ValidatePairwiseOverlapAlignment(string nodeName,
                                                      bool isTextFile, SequenceCaseType caseType,
                                                      AlignParameters additionalParameter)
        {
            ValidatePairwiseOverlapAlignment(nodeName, isTextFile,
                                             caseType, additionalParameter, AlignmentType.SimpleAlign);
        }

        /// <summary>
        ///     Validates PairwiseOverlapAlignment algorithm for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Xml node name</param>
        /// <param name="isTextFile">Is text file an input.</param>
        /// <param name="caseType">Case Type</param>
        /// <param name="additionalParameter">parameter based on which certain validations are done.</param>
        /// <param name="alignType">Is the Align type Simple or Align with Gap Extension cost?</param>
        private void ValidatePairwiseOverlapAlignment(string nodeName, bool isTextFile,
                                                      SequenceCaseType caseType, AlignParameters additionalParameter,
                                                      AlignmentType alignType)
        {
            ValidatePairwiseOverlapAlignment(nodeName, isTextFile,
                                             caseType, additionalParameter, alignType,
                                             SimilarityMatrixParameters.Default);
        }

        /// <summary>
        ///     Validates PairwiseOverlapAlignment algorithm for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Xml node name</param>
        /// <param name="isTextFile">Is text file an input.</param>
        /// <param name="caseType">Case Type</param>
        /// <param name="additionalParameter">parameter based on which certain validations are done.</param>
        /// <param name="alignType">Is the Align type Simple or Align with Gap Extension cost?</param>
        /// <param name="similarityMatrixParam">Similarity Matrix</param>
        private void ValidatePairwiseOverlapAlignment(string nodeName, bool isTextFile,
                                                      SequenceCaseType caseType, AlignParameters additionalParameter,
                                                      AlignmentType alignType,
                                                      SimilarityMatrixParameters similarityMatrixParam)
        {
            Sequence aInput = null;
            Sequence bInput = null;

            var alphabet = Utility.GetAlphabet(utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                                     Constants.AlphabetNameNode));

            if (isTextFile)
            {
                // Read the xml file for getting both the files for aligning.
                var filePath1 = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                   Constants.FilePathNode1).TestDir();
                var filePath2 = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                   Constants.FilePathNode2).TestDir();

                var parser1 = new FastAParser();
                var originalSequence1 = parser1.Parse(filePath1).ElementAt(0);
                var originalSequence2 = parser1.Parse(filePath2).ElementAt(0);

                // Create input sequence for sequence string in different cases.
                GetSequenceWithCaseType(new string(originalSequence1.Select(a => (char) a).ToArray()),
                                        new string(originalSequence2.Select(a => (char) a).ToArray()), alphabet,
                                        caseType, out aInput, out bInput);
            }
            else
            {
                var originalSequence1 = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.SequenceNode1);
                var originalSequence2 = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.SequenceNode2);

                // Create input sequence for sequence string in different cases.
                GetSequenceWithCaseType(
                    originalSequence1,
                    originalSequence2,
                    alphabet,
                    caseType,
                    out aInput,
                    out bInput);
            }

            var aInputString = new string(aInput.Select(a => (char) a).ToArray());
            var bInputString = new string(bInput.Select(a => (char) a).ToArray());

            ApplicationLog.WriteLine(string.Format(null,
                                                   "PairwiseOverlapAligner P2 : First sequence used is '{0}'.",
                                                   aInputString));
            ApplicationLog.WriteLine(string.Format(null,
                                                   "PairwiseOverlapAligner P2 : Second sequence used is '{0}'.",
                                                   bInputString));

            // Create similarity matrix object for a given file.
            var blosumFilePath = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.BlosumFilePathNode).TestDir();

            SimilarityMatrix sm = null;

            switch (similarityMatrixParam)
            {
                case SimilarityMatrixParameters.TextReader:
                    using (TextReader reader = new StreamReader(blosumFilePath))
                        sm = new SimilarityMatrix(reader);
                    break;
                case SimilarityMatrixParameters.DiagonalMatrix:
                    var matchValue = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                        Constants.MatchScoreNode);
                    var misMatchValue = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                           Constants.MisMatchScoreNode);
                    sm = new DiagonalSimilarityMatrix(int.Parse(matchValue, null),
                                                      int.Parse(misMatchValue, null));
                    break;
                default:
                    sm = new SimilarityMatrix(new StreamReader(blosumFilePath));
                    break;
            }

            var gapOpenCost = int.Parse(utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                        Constants.GapOpenCostNode), null);

            var gapExtensionCost = int.Parse(utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                             Constants.GapExtensionCostNode), null);

            // Create PairwiseOverlapAligner instance and set its values.
            var pairwiseOverlapObj = new PairwiseOverlapAligner();
            if (additionalParameter != AlignParameters.AllParam)
            {
                pairwiseOverlapObj.SimilarityMatrix = sm;
                pairwiseOverlapObj.GapOpenCost = gapOpenCost;
                pairwiseOverlapObj.GapExtensionCost = gapExtensionCost;
            }
            IList<IPairwiseSequenceAlignment> result = null;

            // Align the input sequences.
            switch (additionalParameter)
            {
                case AlignParameters.AlignList:
                    var sequences = new List<ISequence>
                    {
                        aInput,
                        bInput
                    };
                    switch (alignType)
                    {
                        case AlignmentType.Align:
                            result = pairwiseOverlapObj.Align(sequences);
                            break;
                        default:
                            result = pairwiseOverlapObj.AlignSimple(sequences);
                            break;
                    }
                    break;
                case AlignParameters.AlignTwo:
                    switch (alignType)
                    {
                        case AlignmentType.Align:
                            result = pairwiseOverlapObj.Align(aInput, bInput);
                            break;
                        default:
                            result = pairwiseOverlapObj.AlignSimple(aInput, bInput);
                            break;
                    }
                    break;
                case AlignParameters.AllParam:
                    switch (alignType)
                    {
                        case AlignmentType.Align:
                            result = pairwiseOverlapObj.Align(sm, gapOpenCost,
                                                              gapExtensionCost, aInput, bInput);
                            break;
                        default:
                            result = pairwiseOverlapObj.AlignSimple(sm, gapOpenCost, aInput, bInput);
                            break;
                    }
                    break;
                default:
                    break;
            }

            aInput = null;
            bInput = null;
            sm = null;

            // Get the expected sequence and scorde from xml config.
            var expectedSequence1 = string.Empty;
            var expectedSequence2 = string.Empty;
            var expectedScore = string.Empty;

            switch (alignType)
            {
                case AlignmentType.Align:
                    expectedScore = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                    Constants.ExpectedGapExtensionScoreNode);
                    expectedSequence1 = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                        Constants.ExpectedGapExtensionSequence1Node);
                    expectedSequence2 = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                        Constants.ExpectedGapExtensionSequence2Node);
                    break;
                default:
                    expectedScore = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                    Constants.ExpectedScoreNode);
                    expectedSequence1 = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                        Constants.ExpectedSequenceNode1);
                    expectedSequence2 = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                        Constants.ExpectedSequenceNode2);
                    break;
            }

            IList<IPairwiseSequenceAlignment> expectedOutput = new List<IPairwiseSequenceAlignment>();
            string[] expectedSequences1, expectedSequences2;
            var seperators = new char[1] {';'};
            expectedSequences1 = expectedSequence1.Split(seperators);
            expectedSequences2 = expectedSequence2.Split(seperators);

            IPairwiseSequenceAlignment align = new PairwiseSequenceAlignment();
            PairwiseAlignedSequence alignedSeq;
            for (var i = 0; i < expectedSequences1.Length; i++)
            {
                alignedSeq = new PairwiseAlignedSequence
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
            Assert.IsTrue(AlignmentHelpers.CompareAlignment(result, expectedOutput,true));

            ApplicationLog.WriteLine(string.Format(null, "PairwiseOverlapAligner P2 : Final Score '{0}'.", expectedScore));
            ApplicationLog.WriteLine(string.Format(null, "PairwiseOverlapAligner P2 : Aligned First Sequence is '{0}'.", expectedSequence1));
            ApplicationLog.WriteLine(string.Format(null, "PairwiseOverlapAligner P2 : Aligned Second Sequence is '{0}'.", expectedSequence2));
        }

        /// <summary>
        ///     InValidates PairwiseOverlapAlignment with invalid sequence.
        /// </summary>
        /// <param name="nodeName"></param>
        /// <param name="isTextFile">Is text file an input.</param>
        /// <param name="invalidSequenceType"></param>
        /// <param name="additionalParameter">parameter based on which certain validations are done.</param>
        /// <param name="alignType">Is the Align type Simple or Align with Gap Extension cost?</param>
        /// <param name="sequenceType"></param>
        private void InValidatePairwiseOverlapAlignmentWithInvalidSequence(string nodeName,
                                                                           bool isTextFile,
                                                                           InvalidSequenceType invalidSequenceType,
                                                                           AlignParameters additionalParameter,
                                                                           AlignmentType alignType,
                                                                           InvalidSequenceType sequenceType)
        {
            var alphabet = Utility.GetAlphabet(utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                                     Constants.AlphabetNameNode));
            Exception actualException = null;
            Sequence aInput = null;

            if (isTextFile)
            {
                // Read the xml file for getting both the files for aligning.
                var filepath = GetInputFileNameWithInvalidType(nodeName, invalidSequenceType);

                // Create input sequence for sequence string in different cases.
                try
                {
                    // Parse the files and get the sequence.                    
                    var parser = new FastAParser();
                    aInput = new Sequence(alphabet, new string(parser.Parse(filepath).ElementAt(0).
                                                                          Select(a => (char) a).ToArray()));
                }
                catch (Exception ex)
                {
                    actualException = ex;
                }
            }
            else
            {
                var originalSequence = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                          Constants.InvalidSequence1);

                // Create input sequence for sequence string in different cases.
                try
                {
                    aInput = new Sequence(alphabet, originalSequence);
                }
                catch (ArgumentException ex)
                {
                    actualException = ex;
                }
            }

            if (null == actualException)
            {
                var bInput = aInput;

                // Create similarity matrix object for a given file.
                var blosumFilePath = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                        Constants.BlosumFilePathNode).TestDir();

                var sm = new SimilarityMatrix(new StreamReader(blosumFilePath));

                var gapOpenCost = int.Parse(utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                            Constants.GapOpenCostNode), null);

                var gapExtensionCost = int.Parse(utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                                 Constants.GapExtensionCostNode), null);

                // Create PairwiseOverlapAligner instance and set its values.
                var pairwiseOverlapObj = new PairwiseOverlapAligner();
                if (additionalParameter != AlignParameters.AllParam)
                {
                    pairwiseOverlapObj.SimilarityMatrix = sm;
                    pairwiseOverlapObj.GapOpenCost = gapOpenCost;
                    pairwiseOverlapObj.GapExtensionCost = gapExtensionCost;
                }

                // Align the input sequences and catch the exception.
                switch (additionalParameter)
                {
                    case AlignParameters.AlignList:
                        var sequences = new List<ISequence> {aInput, bInput};
                        switch (alignType)
                        {
                            case AlignmentType.Align:
                                try
                                {
                                    pairwiseOverlapObj.Align(sequences);
                                }
                                catch (ArgumentException ex)
                                {
                                    actualException = ex;
                                }
                                break;
                            default:
                                try
                                {
                                    pairwiseOverlapObj.AlignSimple(sequences);
                                }
                                catch (ArgumentException ex)
                                {
                                    actualException = ex;
                                }
                                break;
                        }
                        break;
                    case AlignParameters.AlignTwo:
                        switch (alignType)
                        {
                            case AlignmentType.Align:
                                try
                                {
                                    pairwiseOverlapObj.Align(aInput, bInput);
                                }
                                catch (ArgumentException ex)
                                {
                                    actualException = ex;
                                }
                                break;
                            default:
                                try
                                {
                                    pairwiseOverlapObj.AlignSimple(aInput, bInput);
                                }
                                catch (ArgumentException ex)
                                {
                                    actualException = ex;
                                }
                                break;
                        }
                        break;
                    case AlignParameters.AllParam:
                        switch (alignType)
                        {
                            case AlignmentType.Align:
                                try
                                {
                                    pairwiseOverlapObj.Align(sm, gapOpenCost,
                                                             gapExtensionCost, aInput, bInput);
                                }
                                catch (ArgumentException ex)
                                {
                                    actualException = ex;
                                }
                                break;
                            default:
                                try
                                {
                                    pairwiseOverlapObj.AlignSimple(sm, gapOpenCost,
                                                                   aInput, bInput);
                                }
                                catch (ArgumentException ex)
                                {
                                    actualException = ex;
                                }
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }

            // Validate Error messages for Invalid Sequence types.
            var expectedErrorMessage = GetExpectedErrorMeesageWithInvalidSequenceType(nodeName,
                                                                                         sequenceType);

            Assert.AreEqual(expectedErrorMessage.Replace("\\r", "").Replace("\\n", "").Replace("\t", ""),
                            actualException.Message.Replace("\r", "").Replace("\n", "").Replace("\t", ""));

            ApplicationLog.WriteLine("PairwiseOverlapAligner P2 : Expected Error message is thrown ", expectedErrorMessage);
        }

        /// <summary>
        ///     Validates PairwiseOverlapAlignment algorithm for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Xml node name</param>
        /// <param name="isTextFile">Is text file an input.</param>
        /// <param name="invalidType">Invalid type</param>
        /// <param name="additionalParameter">parameter based on which certain validations are done.</param>
        /// <param name="alignType">Is the Align type Simple or Align with Gap Extension cost?</param>
        private void InValidatePairwiseOverlapAlignmentWithInvalidSimilarityMatrix(string nodeName,
                                                                                   bool isTextFile,
                                                                                   SimilarityMatrixInvalidTypes
                                                                                       invalidType,
                                                                                   AlignParameters additionalParameter,
                                                                                   AlignmentType alignType)
        {
            Sequence aInput = null;
            Sequence bInput = null;

            var alphabet = Utility.GetAlphabet(utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                                     Constants.AlphabetNameNode));

            if (isTextFile)
            {
                // Read the xml file for getting both the files for aligning.
                var firstInputFilepath = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                            Constants.FilePathNode1).TestDir();
                var secondInputFilepath = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                             Constants.FilePathNode2).TestDir();

                // Parse the files and get the sequence.              
                var parser1 = new FastAParser();
                var inputSequence1 = parser1.Parse(firstInputFilepath).ElementAt(0);
                var inputSequence2 = parser1.Parse(secondInputFilepath).ElementAt(0);

                // Create input sequence for sequence string in different cases.
                GetSequenceWithCaseType(new string(inputSequence1.Select(a => (char) a).ToArray()),
                                        new string(inputSequence2.Select(a => (char) a).ToArray()), alphabet,
                                        SequenceCaseType.LowerCase, out aInput, out bInput);
            }
            else
            {
                var firstInputSequence = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.SequenceNode1);
                var secondInputSequence = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.SequenceNode2);

                // Create input sequence for sequence string in different cases.
                GetSequenceWithCaseType(firstInputSequence, secondInputSequence,
                                        alphabet, SequenceCaseType.LowerCase, out aInput, out bInput);
            }

            var aInputString = new string(aInput.Select(a => (char) a).ToArray());
            var bInputString = new string(bInput.Select(a => (char) a).ToArray());

            ApplicationLog.WriteLine(string.Format(null, "PairwiseOverlapAligner P2 : First sequence used is '{0}'.", aInputString));
            ApplicationLog.WriteLine(string.Format(null, "PairwiseOverlapAligner P2 : Second sequence used is '{0}'.", bInputString));

            // Create similarity matrix object for a invalid file.
            var blosumFilePath = GetSimilarityMatrixFileWithInvalidType(nodeName, invalidType);
            Exception actualExpection = null;

            // For invalid similarity matrix data format; exception will be thrown while instantiating
            SimilarityMatrix sm = null;
            try
            {
                if (invalidType != SimilarityMatrixInvalidTypes.NullSimilarityMatrix)
                {
                    sm = new SimilarityMatrix(new StreamReader(blosumFilePath));
                }
            }
            catch (InvalidDataException ex)
            {
                actualExpection = ex;
            }

            // For non matching similarity matrix exception will be thrown while alignment
            if (actualExpection == null)
            {
                var gapOpenCost = int.Parse(utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                            Constants.GapOpenCostNode), null);

                var gapExtensionCost = int.Parse(utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                                 Constants.GapExtensionCostNode), null);

                // Create PairwiseOverlapAligner instance and set its values.
                var pairwiseOverlapObj = new PairwiseOverlapAligner();
                if (additionalParameter != AlignParameters.AllParam)
                {
                    pairwiseOverlapObj.SimilarityMatrix = sm;
                    pairwiseOverlapObj.GapOpenCost = gapOpenCost;
                    pairwiseOverlapObj.GapExtensionCost = gapExtensionCost;
                }

                // Align the input sequences and catch the exception.
                switch (additionalParameter)
                {
                    case AlignParameters.AlignList:
                        var sequences = new List<ISequence>
                        {
                            aInput,
                            bInput
                        };
                        switch (alignType)
                        {
                            case AlignmentType.Align:
                                try
                                {
                                    pairwiseOverlapObj.Align(sequences);
                                }
                                catch (ArgumentException ex)
                                {
                                    actualExpection = ex;
                                }
                                break;
                            default:
                                try
                                {
                                    pairwiseOverlapObj.AlignSimple(sequences);
                                }
                                catch (ArgumentException ex)
                                {
                                    actualExpection = ex;
                                }
                                break;
                        }
                        break;
                    case AlignParameters.AlignTwo:
                        switch (alignType)
                        {
                            case AlignmentType.Align:
                                try
                                {
                                    pairwiseOverlapObj.Align(aInput, bInput);
                                }
                                catch (ArgumentException ex)
                                {
                                    actualExpection = ex;
                                }
                                break;
                            default:
                                try
                                {
                                    pairwiseOverlapObj.AlignSimple(aInput, bInput);
                                }
                                catch (ArgumentException ex)
                                {
                                    actualExpection = ex;
                                }
                                break;
                        }
                        break;
                    case AlignParameters.AllParam:
                        switch (alignType)
                        {
                            case AlignmentType.Align:
                                try
                                {
                                    pairwiseOverlapObj.Align(sm, gapOpenCost, gapExtensionCost,
                                                             aInput, bInput);
                                }
                                catch (ArgumentException ex)
                                {
                                    actualExpection = ex;
                                }
                                break;
                            default:
                                try
                                {
                                    pairwiseOverlapObj.AlignSimple(sm, gapOpenCost, aInput, bInput);
                                }
                                catch (ArgumentException ex)
                                {
                                    actualExpection = ex;
                                }
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }

            // Validate that expected exception is thrown using error message.
            var expectedErrorMessage = GetExpectedErrorMeesageWithInvalidSimilarityMatrixType(nodeName, invalidType);
            Assert.AreEqual(expectedErrorMessage, actualExpection.Message);
            ApplicationLog.WriteLine("PairwiseOverlapAligner P2 : Expected Error message is thrown ", expectedErrorMessage);
        }

        /// <summary>
        ///     Gets the expected error message for invalid similarity matrix type.
        /// </summary>
        /// <param name="nodeName">xml node</param>
        /// <param name="invalidType">similarity matrix invalid type.</param>
        /// <returns>Returns expected error message</returns>
        private string GetExpectedErrorMeesageWithInvalidSimilarityMatrixType(string nodeName,
                                                                              SimilarityMatrixInvalidTypes invalidType)
        {
            var expectedErrorMessage = string.Empty;
            switch (invalidType)
            {
                case SimilarityMatrixInvalidTypes.FewAlphabetsSimilarityMatrix:
                case SimilarityMatrixInvalidTypes.NonMatchingSimilarityMatrix:
                    expectedErrorMessage = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                           Constants.ExpectedErrorMessage);
                    break;
                case SimilarityMatrixInvalidTypes.EmptySimilaityMatrix:
                    expectedErrorMessage = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                           Constants.EmptyMatrixErrorMessage);
                    break;
                case SimilarityMatrixInvalidTypes.OnlyAlphabetSimilarityMatrix:
                    expectedErrorMessage = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                           Constants.SimilarityMatrixFewerLinesException);
                    break;
                case SimilarityMatrixInvalidTypes.ModifiedSimilarityMatrix:
                    expectedErrorMessage = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                           Constants.ModifiedMatrixErrorMessage);
                    break;
                case SimilarityMatrixInvalidTypes.NullSimilarityMatrix:
                    expectedErrorMessage = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                           Constants.NullErrorMessage);
                    break;
                case SimilarityMatrixInvalidTypes.EmptySequence:
                    expectedErrorMessage = utilityObj.xmlUtil.GetFileTextValue(nodeName,
                                                                               Constants.EmptySequenceErrorMessage);
                    break;
                case SimilarityMatrixInvalidTypes.ExpectedErrorMessage:
                    expectedErrorMessage = utilityObj.xmlUtil.GetFileTextValue(nodeName,
                                                                               Constants.ExpectedErrorMessage);
                    break;
                default:
                    break;
            }

            return expectedErrorMessage;
        }

        /// <summary>
        ///     Gets the expected error message for invalid sequence type.
        /// </summary>
        /// <param name="nodeName">xml node</param>
        /// <param name="sequenceType">invalid sequence type.</param>
        /// <returns>Returns expected error message</returns>
        private string GetExpectedErrorMeesageWithInvalidSequenceType(string nodeName,
                                                                      InvalidSequenceType sequenceType)
        {
            var expectedErrorMessage = string.Empty;
            switch (sequenceType)
            {
                case InvalidSequenceType.SequenceWithInvalidChars:
                    expectedErrorMessage = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                           Constants.EmptySequenceErrorMessage);
                    break;
                case InvalidSequenceType.InvalidSequence:
                    expectedErrorMessage = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                           Constants.InvalidSequenceErrorMessage);
                    break;
                case InvalidSequenceType.SequenceWithUnicodeChars:
                    expectedErrorMessage = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                           Constants.UnicodeSequenceErrorMessage);
                    break;
                case InvalidSequenceType.SequenceWithSpaces:
                    expectedErrorMessage = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                           Constants.SequenceWithSpaceErrorMessage);
                    break;
                case InvalidSequenceType.AlphabetMap:
                    expectedErrorMessage = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                           Constants.InvalidAlphabetErrorMessage);
                    break;
                default:
                    expectedErrorMessage = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                           Constants.ExpectedErrorMessage);
                    break;
            }

            return expectedErrorMessage;
        }

        /// <summary>
        ///     Gets the similarity matrix file name for a given invalid similarity matrix type.
        /// </summary>
        /// <param name="nodeName">xml node.</param>
        /// <param name="invalidType">similarity matrix invalid type.</param>
        /// <returns>Returns similarity matrix file name.</returns>
        private string GetSimilarityMatrixFileWithInvalidType(string nodeName,
                                                              SimilarityMatrixInvalidTypes invalidType)
        {
            var invalidFileNode = string.Empty;
            var invalidFilePath = string.Empty;
            switch (invalidType)
            {
                case SimilarityMatrixInvalidTypes.NonMatchingSimilarityMatrix:
                    invalidFileNode = Constants.BlosumInvalidFilePathNode;
                    break;
                case SimilarityMatrixInvalidTypes.EmptySimilaityMatrix:
                    invalidFileNode = Constants.BlosumEmptyFilePathNode;
                    break;
                case SimilarityMatrixInvalidTypes.OnlyAlphabetSimilarityMatrix:
                    invalidFileNode = Constants.BlosumOnlyAlphabetFilePathNode;
                    break;
                case SimilarityMatrixInvalidTypes.FewAlphabetsSimilarityMatrix:
                    invalidFileNode = Constants.BlosumFewAlphabetsFilePathNode;
                    break;
                case SimilarityMatrixInvalidTypes.ModifiedSimilarityMatrix:
                    invalidFileNode = Constants.BlosumModifiedFilePathNode;
                    break;
                default:
                    break;
            }
            if (1 == string.Compare(invalidFileNode, string.Empty, StringComparison.CurrentCulture))
            {
                invalidFilePath = utilityObj.xmlUtil.GetTextValue(nodeName, invalidFileNode);
            }

            return invalidFilePath.TestDir();
        }

        /// <summary>
        ///     Gets the input file name for a given invalid sequence type.
        /// </summary>
        /// <param name="nodeName">xml node.</param>
        /// <param name="invalidSequenceType">sequence invalid type.</param>
        /// <returns>Returns input file name.</returns>
        private string GetInputFileNameWithInvalidType(string nodeName,
                                                       InvalidSequenceType invalidSequenceType)
        {
            var invalidFilePath = string.Empty;
            switch (invalidSequenceType)
            {
                case InvalidSequenceType.SequenceWithSpecialChars:
                    invalidFilePath = utilityObj.xmlUtil.GetTextValue(nodeName,
                                                                      Constants.InvalidFilePathNode1);
                    break;
                case InvalidSequenceType.EmptySequence:
                    invalidFilePath = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.EmptyFilePath1);
                    break;
                case InvalidSequenceType.SequenceWithSpaces:
                    invalidFilePath = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.SpacesFilePath1);
                    break;
                case InvalidSequenceType.SequenceWithGap:
                    invalidFilePath = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.GapFilePath1);
                    break;
                case InvalidSequenceType.SequenceWithUnicodeChars:
                    invalidFilePath = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.UnicodeFilePath1);
                    break;
                default:
                    break;
            }

            return invalidFilePath.TestDir();
        }

        /// <summary>
        ///     Creates the sequence object with sequences in different cases
        /// </summary>
        /// <param name="firstSequenceString">First sequence string.</param>
        /// <param name="secondSequenceString">Second sequence string.</param>
        /// <param name="alphabet">alphabet type.</param>
        /// <param name="caseType">Sequence case type</param>
        /// <param name="firstInputSequence">First input sequence object.</param>
        /// <param name="secondInputSequence">Second input sequence object.</param>
        private static void GetSequenceWithCaseType(string firstSequenceString,
                                                    string secondSequenceString, IAlphabet alphabet,
                                                    SequenceCaseType caseType,
                                                    out Sequence firstInputSequence, out Sequence secondInputSequence)
        {
            switch (caseType)
            {
                case SequenceCaseType.LowerCase:
                    firstInputSequence = new Sequence(alphabet,
                                                      firstSequenceString.ToString(null)
                                                                         .ToLower(CultureInfo.CurrentCulture));
                    secondInputSequence = new Sequence(alphabet,
                                                       secondSequenceString.ToString(null)
                                                                           .ToLower(CultureInfo.CurrentCulture));
                    break;
                case SequenceCaseType.UpperCase:
                    firstInputSequence = new Sequence(alphabet,
                                                      firstSequenceString.ToString(null)
                                                                         .ToUpper(CultureInfo.CurrentCulture));
                    secondInputSequence = new Sequence(alphabet,
                                                       secondSequenceString.ToString(null)
                                                                           .ToLower(CultureInfo.CurrentCulture));
                    break;
                case SequenceCaseType.LowerUpperCase:
                    firstInputSequence = new Sequence(alphabet,
                                                      firstSequenceString.ToString(null)
                                                                         .ToLower(CultureInfo.CurrentCulture));
                    secondInputSequence = new Sequence(alphabet,
                                                       secondSequenceString.ToString(null)
                                                                           .ToUpper(CultureInfo.CurrentCulture));
                    break;
                case SequenceCaseType.Default:
                default:
                    firstInputSequence = new Sequence(alphabet, firstSequenceString.ToString(null));
                    secondInputSequence = new Sequence(alphabet, secondSequenceString.ToString(null));
                    break;
            }
        }

        #endregion
    }
}