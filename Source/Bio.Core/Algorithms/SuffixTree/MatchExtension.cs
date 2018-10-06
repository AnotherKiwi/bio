using System.Globalization;

namespace Bio.Algorithms.SuffixTree
{
    /// <summary>
    /// Maximum Unique Match Class.
    /// </summary>
    public class MatchExtension
    {
        /// <summary>
        /// Gets or sets the length of match.
        /// </summary>
        public long Length;

        /// <summary>
        /// Gets or sets the start index of this match in reference sequence.
        /// </summary>
        public long ReferenceSequenceOffset;

        /// <summary>
        /// Gets or sets start index of this match in query sequence.
        /// </summary>
        public long QuerySequenceOffset;

        /// <summary>
        /// Gets or sets Sequence one's MaxUniqueMatch order. 
        /// </summary>
        public long ReferenceSequenceMumOrder;

        /// <summary>
        /// Gets or sets Sequence Two's MaxUniqueMatch order.
        /// </summary>
        public long QuerySequenceMumOrder;

        /// <summary>
        /// Gets or sets the Query sequence.
        /// </summary>
        public ISequence Query;

        /// <summary>
        /// Gets or sets cluster Identifier.
        /// </summary>
        public long ID;

        /// <summary>
        /// Gets or sets a value indicating whether MUM is Good candidate.
        /// </summary>
        public bool IsGood;

        /// <summary>
        /// Gets or sets a value indicating whether MUM is Tentative candidate.
        /// </summary>
        public bool IsTentative;

        /// <summary>
        /// Gets or sets score of MUM.
        /// </summary>
        public long Score;

        /// <summary>
        /// Gets or sets offset to adjacent MUM.
        /// </summary>
        public long Adjacent;

        /// <summary>
        /// Gets or sets From (index representing the previous MUM to form LIS) of MUM.
        /// </summary>
        public long From;

        /// <summary>
        /// Gets or sets wrap score.
        /// </summary>
        public long WrapScore;

        /// <summary>
        /// Initializes a new instance of the MaxUniqueMatchExtension class
        /// </summary>
        public MatchExtension()
        {
        }

        /// <summary>
        /// Initializes a new instance of the MaxUniqueMatchExtension class
        /// </summary>
        /// <param name="mum">Maximum Unique Match</param>
        public MatchExtension(Match mum)
        {
            ReferenceSequenceOffset = mum.ReferenceSequenceOffset;
            QuerySequenceOffset = mum.QuerySequenceOffset;
            Length = mum.Length;
            IsGood = false;
            IsTentative = false;
        }

        /// <summary>
        /// Copy the content to MUM.
        /// </summary>
        /// <param name="match">Maximum unique match.</param>
        public void CopyTo(MatchExtension match)
        {
            if (match != null)
            {
                match.ReferenceSequenceMumOrder = ReferenceSequenceMumOrder;
                match.ReferenceSequenceOffset = ReferenceSequenceOffset;
                match.QuerySequenceMumOrder = QuerySequenceMumOrder;
                match.QuerySequenceOffset = QuerySequenceOffset;
                match.Length = Length;
                match.Query = Query;

                match.ID = ID;
                match.IsGood = IsGood;
                match.IsTentative = IsTentative;
                match.Score = Score;
                match.Adjacent = Adjacent;
                match.From = From;
                match.WrapScore = WrapScore;
            }
        }

        /// <summary>
        /// Converts RefStart, QueryStart, Length, Score, WrapScore, IsGood of MatchExtension to string.
        /// </summary>
        /// <returns>RefStart, QueryStart, Length, Score, WrapScore, IsGood.</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, Properties.Resource.MatchExtensionToStringFormat,
                              ReferenceSequenceOffset, QuerySequenceOffset,
                              Length,
                              Score, WrapScore, IsGood);
        }

    }
}
