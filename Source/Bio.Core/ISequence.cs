using System.Collections.Generic;

namespace Bio
{
    /// <summary>
    /// Implementations of ISequence make up the one of the core sets
    /// of data structures in Bio. It is these sequences that store
    /// data relevant to DNA, RNA, and Amino Acid structures. Several
    /// algorithms for alignment, assembly, and analysis take these items
    /// as their basic data inputs and outputs.
    /// </summary>
    public interface ISequence : IEnumerable<byte>
    {
        /// <summary>
        /// Gets or sets an identification provided to distinguish the sequence to others
        /// being worked with.
        /// </summary>
        string ID { get; set; }

         /// <summary>
        /// Gets alphabet to which this sequence should conform.
        /// </summary>
        IAlphabet Alphabet { get; }

        ///// <summary>
        ///// 	Determines whether the current sequence contains the specified sub-sequence.
        ///// </summary>
        ///// <param name="subSequence">The sub-sequence to find.</param>
        ///// <param name="startIndex">The index at which to start the search.</param>
        ///// <returns>
        /////  	<see cref="bool"/>:
        /////  	<c>true</c> if the current sequence contains the specified sub-sequence; otherwise, <c>false</c>.
        ///// </returns>
        //bool Contains(ISequence subSequence, long startIndex = 0);

        //IReadOnlyList<ISequence> FindMatches(IEnumerable<ISequence> sequences);
        //IReadOnlyList<ISequence> FindMatches2(IEnumerable<ISequence> sequences);

        ///// <summary>
        ///// 	Returns the zero-based index of the sub-sequence in the current sequence, if found. If not, -1 is returned.
        ///// </summary>
        ///// <param name="subSequence">The sub-sequence to find in the current sequence.</param>
        ///// <param name="startIndex">The index at which to start the search.</param>
        ///// <returns>
        /////  	<see cref="System.Int64"/>: 
        /////  	<see cref="long" />: the zero-based index of the sub-sequence in the current sequence, if found; -1 otherwise.
        ///// </returns>
        //long IndexOf(ISequence subSequence, long startIndex = 0);
        //long IndexOf2(ISequence subSequence, long startIndex = 0);

        /// <summary>
        /// Gets the number of sequence items contained in the Sequence.
        /// </summary>
        long Count { get; }

        /// <summary>
        /// Allows the sequence to function like an array, getting and setting
        /// the sequence item at the particular index specified. Note that the
        /// index value starts its count at 0.
        /// </summary>
        /// <param name="index">The index value.</param>
        /// <returns>A byte which represents the symbol.</returns>
        byte this[long index]
        {
            get;
        }

        /// <summary>
        /// 	Gets a copy of the specified number of items from the raw sequence data,
        ///     starting at the specified zero-based start index.
        /// </summary>
        /// <param name="startIndex">Zero-based index of the first item to copy.</param>
        /// <param name="length">Total numbers of elements to be copied.</param>
        /// <returns>
        ///  	<see cref="T:byte[]"/>: the specified items from the raw sequence data.
        /// </returns>
        /// 
        byte[] GetSymbols(long startIndex = 0, long length = -1);

        /// <summary>
        /// Many sequence representations when saved to file also contain
        /// information about that sequence. Unfortunately there is no standard
        /// around what that data may be from format to format. This property
        /// allows a place to put structured metadata that can be accessed by
        /// a particular key. <br/>
        /// 
        /// For example, if species information is stored in a particular Species
        /// class, you could add it to the dictionary by:
        /// 
        /// <c>mySequence.Metadata["SpeciesInfo"] = mySpeciesInfo</c>;
        /// 
        /// To fetch the data you would use:
        /// 
        /// <c>Species mySpeciesInfo = mySequence.Metadata["SpeciesInfo"]</c>;<br/>
        /// 
        /// Particular formats may create their own data model class for information
        /// unique to their format as well. Such as:<br/>
        /// 
        /// <c>GenBankMetadata genBankData = new GenBankMetadata();
        /// // ... add population code
        /// mySequence.MetaData["GenBank"] = genBankData;</c>
        /// </summary>
        Dictionary<string, object> Metadata { get; }

        /// <summary>
        /// Return a sequence representing this sequence with the orientation reversed.
        /// </summary>
        ISequence GetReversedSequence();

        /// <summary>
        /// Return a sequence representing the complement of this sequence.
        /// </summary>
        ISequence GetComplementedSequence();

        /// <summary>
        /// Return a sequence representing the reverse complement of this sequence.
        /// </summary>
        ISequence GetReverseComplementedSequence();

        /// <summary>
        /// Return a sequence representing a range (subsequence) of this sequence.
        /// </summary>
        /// <param name="start">The index of the first symbol in the range.</param>
        /// <param name="length">The number of symbols in the range.</param>
        /// <returns>The virtual sequence.</returns>
        ISequence GetSubSequence(long start, long length);

        /// <summary>
        /// Gets the index of first non gap symbol.
        /// </summary>
        /// <returns>If found returns an zero based index of the first non gap symbol, otherwise returns -1.</returns>
        long IndexOfNonGap();

        /// <summary>
        /// Returns the position of the first item beyond startPos that does not 
        /// have a Gap symbol.
        /// </summary>
        /// <param name="startPos">Index value above which to search for non-Gap symbol.</param>
        /// <returns>If found returns an zero based index of the first non gap symbol, otherwise returns -1.</returns>
        long IndexOfNonGap(long startPos);

        /// <summary>
        /// Gets the index of last non gap symbol.
        /// </summary>
        /// <returns>If found returns an zero based index of the last non gap symbol, otherwise returns -1.</returns>
        long LastIndexOfNonGap();

        /// <summary>
        /// Gets the index of last non gap symbol before the specified end position.
        /// </summary>
        /// <param name="endPos">Index value below which to search for non-Gap symbol.</param>
        /// <returns>If found returns an zero based index of the last non gap symbol, otherwise returns -1.</returns>
        long LastIndexOfNonGap(long endPos);
    }
}
