﻿using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Bio;
using Bio.IO.Bed;
using NUnit.Framework;

namespace Bio.Tests.IO.Bed
{
    /// <summary>
    /// Class to test BED Operations like Merge, intersect etc.
    /// </summary>
    [TestFixture]
    public class BEDOperationsTests
    {
        /// <summary>
        /// Method to test Merge operation.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void MergeOperationTest()
        {
            var direc = Path.Combine ("TestUtils", "BED", "Merge");
            var filepath = Path.Combine(direc, "Merge_single.BED").TestDir();
            var resultfilepath = "tmp_mergeresult.bed";
            var expectedresultpath = Path.Combine(direc, "Result_Merge_Single_MinLength0.BED").TestDir();
            var parser = new BedParser();
            var formatter = new BedFormatter();
            SequenceRangeGrouping seqGrouping = null;
            SequenceRangeGrouping result = null;
            var resultvalue = false;
            resultfilepath = "tmp_mergeresult.bed";
            expectedresultpath = Path.Combine(direc, "Result_Merge_Single_MinLength0.BED").TestDir();
            seqGrouping = parser.ParseRangeGrouping(filepath);

            result = seqGrouping.MergeOverlaps();
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, seqGrouping, null, false);
            Assert.IsTrue(resultvalue);

            expectedresultpath = Path.Combine(direc, "Result_Merge_Single_MinLength0.BED").TestDir();
            result = seqGrouping.MergeOverlaps(0, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, seqGrouping, null, true);
            Assert.IsTrue(resultvalue);

            expectedresultpath = Path.Combine(direc, "Result_Merge_Single_MinLength0.BED").TestDir();
            result = seqGrouping.MergeOverlaps(0, false);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, seqGrouping, null, false);
            Assert.IsTrue(resultvalue);

            expectedresultpath = Path.Combine(direc, "Result_Merge_Single_MinLength0.BED").TestDir();
            result = seqGrouping.MergeOverlaps(0);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, seqGrouping, null, false);
            Assert.IsTrue(resultvalue);

            expectedresultpath = Path.Combine(direc, "Result_Merge_Single_MinLength0.BED").TestDir();
            result = seqGrouping.MergeOverlaps(0, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, seqGrouping, null, true);
            Assert.IsTrue(resultvalue);

            expectedresultpath = Path.Combine(direc, "Result_Merge_Single_MinLength0.BED").TestDir();
            result = seqGrouping.MergeOverlaps(0, false);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, seqGrouping, null, false);
            Assert.IsTrue(resultvalue);

            expectedresultpath = Path.Combine(direc, "Result_Merge_Single_MinLength1.BED").TestDir();
            result = seqGrouping.MergeOverlaps(1);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, seqGrouping, null, false);
            Assert.IsTrue(resultvalue);


            expectedresultpath = Path.Combine(direc, "Result_Merge_Single_MinLength-1.BED").TestDir();
            result = seqGrouping.MergeOverlaps(-1);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, seqGrouping, null, false);
            Assert.IsTrue(resultvalue);


            expectedresultpath = Path.Combine(direc, "Result_Merge_Single_MinLength-3.BED").TestDir();
            result = seqGrouping.MergeOverlaps(-3);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, seqGrouping, null, false);
            Assert.IsTrue(resultvalue);

            var firstFile = Path.Combine(direc, "Merge_twofiles_1.BED").TestDir();
            var secondFile = Path.Combine(direc, "Merge_twofiles_2.BED").TestDir();
            var refSeqRange = parser.ParseRangeGrouping(firstFile);
            var querySeqRange = parser.ParseRangeGrouping(secondFile);

            expectedresultpath = Path.Combine(direc, "Result_Merge_Two_MinLength0.BED").TestDir();
            result = refSeqRange.MergeOverlaps(querySeqRange);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, false);
            Assert.IsTrue(resultvalue);

            result = refSeqRange.MergeOverlaps(querySeqRange, 0, false);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, false);
            Assert.IsTrue(resultvalue);

            result = refSeqRange.MergeOverlaps(querySeqRange, 0, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, true);
            Assert.IsTrue(resultvalue);

            result = refSeqRange.MergeOverlaps(querySeqRange, 0);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, false);
            Assert.IsTrue(resultvalue);

            result = refSeqRange.MergeOverlaps(querySeqRange, 0, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, true);
            Assert.IsTrue(resultvalue);

            result = refSeqRange.MergeOverlaps(querySeqRange, 0, false);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, false);
            Assert.IsTrue(resultvalue);

            expectedresultpath = Path.Combine(direc, "Result_Merge_Two_MinLength1.BED").TestDir();
            result = refSeqRange.MergeOverlaps(querySeqRange, 1, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, true);
            Assert.IsTrue(resultvalue);

            expectedresultpath = Path.Combine(direc, "Result_Merge_Two_MinLength-1.BED").TestDir();
            result = refSeqRange.MergeOverlaps(querySeqRange, -1, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, true);
            Assert.IsTrue(resultvalue);

            expectedresultpath = Path.Combine(direc, "Result_Merge_Two_MinLength-3.BED").TestDir();
            result = refSeqRange.MergeOverlaps(querySeqRange, -3, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, true);
            Assert.IsTrue(resultvalue);

            expectedresultpath = Path.Combine(direc, "Result_Merge_Two_MinLength2.BED").TestDir();
            result = refSeqRange.MergeOverlaps(querySeqRange, 2, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, true);
            Assert.IsTrue(resultvalue);

            expectedresultpath = Path.Combine(direc, "Result_Merge_Two_MinLength6.BED").TestDir();
            result = refSeqRange.MergeOverlaps(querySeqRange, 6, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, true);
            Assert.IsTrue(resultvalue);
            File.Delete(resultfilepath);
        }

        /// <summary>
        /// Method to test Intersect operation.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void IntersectOperationTest()
        {
            var resultfilepath = "tmp_mergeresult.bed";
            var expectedresultpath = string.Empty;
            var parser = new BedParser();
            var formatter = new BedFormatter();

            SequenceRangeGrouping result = null;
            var resultvalue = false;
            resultfilepath = "tmp_mergeresult.bed";
            var direc = Path.Combine("TestUtils", "BED", "Intersect");

            var reffile =  Path.Combine(direc, "Intersect_ref.BED").TestDir();
            var queryFile = Path.Combine(direc, "Intersect_query.BED").TestDir();
            var refSeqRange = parser.ParseRangeGrouping(reffile);
            var querySeqRange = parser.ParseRangeGrouping(queryFile);

            expectedresultpath = Path.Combine(direc, "Result_Intersect_MinOverlap1_OverLappingBases.BED").TestDir();
            result = refSeqRange.Intersect(querySeqRange, 1, IntersectOutputType.OverlappingPiecesOfIntervals);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, true);
            Assert.IsTrue(resultvalue);

            expectedresultpath = Path.Combine(direc, "Result_Intersect_MinOverlap1.BED").TestDir();
            result = refSeqRange.Intersect(querySeqRange, 1, IntersectOutputType.OverlappingIntervals);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, false);
            Assert.IsTrue(resultvalue);

            expectedresultpath = Path.Combine(direc, "Result_Intersect_MinOverlap0_OverLappingBases.BED").TestDir();
            result = refSeqRange.Intersect(querySeqRange, 0, IntersectOutputType.OverlappingPiecesOfIntervals);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, true);
            Assert.IsTrue(resultvalue);

            expectedresultpath = Path.Combine(direc, "Result_Intersect_MinOverlap0.BED").TestDir();
            result = refSeqRange.Intersect(querySeqRange, 0, IntersectOutputType.OverlappingIntervals);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, false);
            Assert.IsTrue(resultvalue);

            expectedresultpath = Path.Combine(direc, "Result_Intersect_MinOverlap1_OverLappingBases.BED").TestDir();
            result = refSeqRange.Intersect(querySeqRange, 1, IntersectOutputType.OverlappingPiecesOfIntervals, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, true);
            Assert.IsTrue(resultvalue);

            expectedresultpath = Path.Combine(direc, "Result_Intersect_MinOverlap1.BED").TestDir();
            result = refSeqRange.Intersect(querySeqRange, 1, IntersectOutputType.OverlappingIntervals, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, true);
            Assert.IsTrue(resultvalue);

            expectedresultpath = Path.Combine(direc, "Result_Intersect_MinOverlap0_OverLappingBases.BED").TestDir();
            result = refSeqRange.Intersect(querySeqRange, 0, IntersectOutputType.OverlappingPiecesOfIntervals, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, true);
            Assert.IsTrue(resultvalue);


            expectedresultpath = Path.Combine(direc, "Result_Intersect_MinOverlap0.BED").TestDir();
            result = refSeqRange.Intersect(querySeqRange, 0, IntersectOutputType.OverlappingIntervals, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, true);
            Assert.IsTrue(resultvalue);
            File.Delete(resultfilepath);
        }

        /// <summary>
        /// Test Subtract operation.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void SubtractTest()
        {
            var direc = Path.Combine ("TestUtils", "BED", "Subtract");
            var refSeqRangefile = Path.Combine (direc, "Subtract_ref.BED").TestDir();
            var querySeqRangefile = Path.Combine (direc, "Subtract_query.BED").TestDir();
            var resultfilepath = "tmp_mergeresult.bed";
            var parser = new BedParser();
            var formatter = new BedFormatter();
            SequenceRangeGrouping result = null;
            var resultvalue = false;

            var refSeqRange = parser.ParseRangeGrouping(refSeqRangefile);
            var querySeqRange = parser.ParseRangeGrouping(querySeqRangefile);

            const string MinOverlap1 = "Result_Subtract_minoverlap1.bed";
            var expectedresultpath = Path.Combine (direc, MinOverlap1).TestDir();
            result = refSeqRange.Subtract(querySeqRange, 1, SubtractOutputType.IntervalsWithNoOverlap);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, true);
            Assert.IsTrue(resultvalue);

            const string MinOverlap0 = "Result_subtract_minoverlap0.bed";
            expectedresultpath = Path.Combine (direc, MinOverlap0).TestDir();
            result = refSeqRange.Subtract(querySeqRange, 0, SubtractOutputType.IntervalsWithNoOverlap);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, true);
            Assert.IsTrue(resultvalue);

            expectedresultpath = Path.Combine (direc, MinOverlap1).TestDir();
            result = refSeqRange.Subtract(querySeqRange, 1, SubtractOutputType.IntervalsWithNoOverlap, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, true);
            Assert.IsTrue(resultvalue);

            expectedresultpath = Path.Combine (direc, MinOverlap0).TestDir();
            result = refSeqRange.Subtract(querySeqRange, 0, SubtractOutputType.IntervalsWithNoOverlap, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, true);
            Assert.IsTrue(resultvalue);


            expectedresultpath = Path.Combine (direc, MinOverlap1).TestDir();
            result = refSeqRange.Subtract(querySeqRange, 1, SubtractOutputType.IntervalsWithNoOverlap, false);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, false);
            Assert.IsTrue(resultvalue);

            expectedresultpath = Path.Combine (direc, MinOverlap0).TestDir();
            result = refSeqRange.Subtract(querySeqRange, 0, SubtractOutputType.IntervalsWithNoOverlap, false);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, false);
            Assert.IsTrue(resultvalue);

            const string MinNoOverlap0 = "Result_Subtract_minoverlap0_NOnOverlappingPieces.BED";
            expectedresultpath = Path.Combine (direc, MinNoOverlap0).TestDir();
            result = refSeqRange.Subtract(querySeqRange, 0, SubtractOutputType.NonOverlappingPiecesOfIntervals, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, true);
            Assert.IsTrue(resultvalue);

            const string MinNoOverlap1 = "Result_Subtract_minoverlap1_NOnOverlappingPieces.BED";
            expectedresultpath = Path.Combine (direc, MinNoOverlap1).TestDir();
            result = refSeqRange.Subtract(querySeqRange, 1, SubtractOutputType.NonOverlappingPiecesOfIntervals, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, true);
            Assert.IsTrue(resultvalue);
        }

        /// <summary>
        /// Compare BED output
        /// </summary>
        /// <param name="resultfile">Actual result</param>
        /// <param name="expectedresultfile">Expected result</param>
        /// <returns>True if results are equal, else false</returns>
        private static bool CompareBEDOutput(string resultfile, string expectedresultfile)
        {
            return TestAutomation.Util.Utility.CompareFiles(resultfile, expectedresultfile);
        }

        /// <summary>
        /// Method to validate parent seq ranges in result.
        /// </summary>
        /// <param name="resultSeqRange">Result seq range group.</param>
        /// <param name="refSeqRange">Reference seq range group.</param>
        /// <param name="querySeqRange">Query seq range group.</param>
        /// <param name="isParentSeqRangeRequired">Flag to indicate whether result should contain parent seq ranges or not.</param>
        /// <returns>Returns true if the parent seq ranges are valid; otherwise returns false.</returns>
        private static bool ValidateParentSeqRange(SequenceRangeGrouping resultSeqRange, SequenceRangeGrouping refSeqRange,
            SequenceRangeGrouping querySeqRange, bool isParentSeqRangeRequired)
        {
            IList<ISequenceRange> refSeqRangeList = new List<ISequenceRange>();
            IList<ISequenceRange> querySeqRangeList = new List<ISequenceRange>();

            foreach (var groupid in resultSeqRange.GroupIDs)
            {
                if (refSeqRange != null)
                {
                    refSeqRangeList = refSeqRange.GetGroup(groupid);
                }

                if (querySeqRange != null)
                {
                    querySeqRangeList = querySeqRange.GetGroup(groupid);
                }


                foreach (var resultRange in resultSeqRange.GetGroup(groupid))
                {
                    if (!isParentSeqRangeRequired)
                    {
                        if (resultRange.ParentSeqRanges.Count != 0)
                        {
                            return false;
                        }
                    }
                    else
                    {

                        var refCount = refSeqRangeList.Where(R => resultRange.ParentSeqRanges.Contains(R)).Count();
                        var queryCount = querySeqRangeList.Where(R => resultRange.ParentSeqRanges.Contains(R)).Count();


                        if (refCount + queryCount != resultRange.ParentSeqRanges.Count)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
