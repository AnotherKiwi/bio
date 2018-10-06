using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Bio.Core.Extensions;
using Bio.Extensions;
using Bio.Util;
using static Bio.Properties.Resource;

namespace Bio
{
    /// <summary>
    /// This is the standard implementation of the ISequence interface. It contains
    /// the raw data that defines the contents of a sequence. Since Sequence uses 
    /// enumerable of bytes that can be accessed as follows:
    /// Sequence mySequence = new Sequence(Alphabets.DNA, "GATTC");
    /// foreach (Nucleotide nucleotide in mySequence) { ... }
    /// The results will be based on the Alphabet associated with the
    /// sequence. Common alphabets include those for DNA, RNA, and Amino Acids.
    /// For users who wish to get at the underlying data directly, Sequence provides
    /// a means to do this as well. This may be useful for those writing algorithms
    /// against the sequence where performance is especially important. For these
    /// advanced users access is provided to the encoding classes associated with the
    /// sequence.
    /// </summary>
    public sealed class Sequence : ISequence
    {
        #region Member variables

        /// <summary>
        /// Holds the sequence data.
        /// </summary>
        private byte[] _sequenceData;

        /// <summary>
        /// Metadata is features or references or related things of a sequence.
        /// </summary>
        private Dictionary<string, object> _metadata;

        #endregion Member variables

        #region Constructors
        /// <summary>
        /// Constructor used when copying an existing sequence internally for reverse/complement usage
        /// to avoid double-copying the buffer.
        /// </summary>
        private Sequence()
        {
        }

        /// <summary>
        /// Initializes a new instance of the Sequence class with specified alphabet and string sequence.
        /// Symbols in the sequence are validated with the specified alphabet.
        /// </summary>
        /// <param name="alphabet">Alphabet to which this class should conform.</param>
        /// <param name="sequence">The sequence in string form.</param>
        public Sequence(IAlphabet alphabet, string sequence)
            : this(alphabet, sequence, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Sequence class with specified alphabet and string sequence.
        /// </summary>
        /// <param name="alphabet">Alphabet to which this class should conform.</param>
        /// <param name="sequence">The sequence in string form.</param>
        /// <param name="validate">If this flag is true then validation will be done to see whether the data is valid or not,
        /// else validation will be skipped.</param>
        public Sequence(IAlphabet alphabet, string sequence, bool validate)
        {
            // validate the inputs
            if (sequence == null)
            {
                throw new ArgumentNullException(nameof(sequence));
            }

            if (alphabet == null)
            {
                throw new ArgumentNullException(nameof(alphabet));
            }

            Alphabet = alphabet;
            ID = string.Empty;
            var values = Encoding.UTF8.GetBytes(sequence);

            if (validate)
            {
                // Validate sequence data
                if (!alphabet.ValidateSequence(values, 0, values.GetLongLength()))
                {
                    throw Helper.GenerateAlphabetCheckFailureException(alphabet, values);
                }
            }

            _sequenceData = values;
            Count = _sequenceData.GetLongLength();
        }

        /// <summary>
        /// Initializes a new instance of the Sequence class with specified alphabet and bytes.
        /// Bytes representing Symbols in the values are validated with the specified alphabet.
        /// </summary>
        /// <param name="alphabet">Alphabet to which this instance should conform.</param>
        /// <param name="values">An array of bytes representing the symbols.</param>
        public Sequence(IAlphabet alphabet, byte[] values)
            : this(alphabet, values, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Sequence class with specified alphabet and bytes.
        /// </summary>
        /// <param name="alphabet">Alphabet to which this instance should conform.</param>
        /// <param name="values">An array of bytes representing the symbols.</param>
        /// <param name="validate">If this flag is true then validation will be done to see whether the data is valid or not,
        /// else validation will be skipped.</param>
        public Sequence(IAlphabet alphabet, byte[] values, bool validate)
        {
            // validate the inputs
            if (alphabet == null)
            {
                throw new ArgumentNullException(nameof(alphabet));
            }

            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (validate)
            {
                // Validate sequence data
                if (!alphabet.ValidateSequence(values, 0, values.GetLongLength()))
                {
                    throw Helper.GenerateAlphabetCheckFailureException(alphabet, values);
                }
            }

            _sequenceData = new byte[values.GetLongLength()];
            ID = string.Empty;

            Helper.Copy(values, _sequenceData, values.GetLongLength());

            Alphabet = alphabet;
            Count = _sequenceData.GetLongLength();
        }

        /// <summary>
        /// Initializes a new instance of the Sequence class with passed new Sequence. Creates a copy of the sequence.
        /// </summary>
        /// <param name="newSequence">The New sequence for which the copy has to be made.</param>
        public Sequence(ISequence newSequence)
        {
            if (newSequence == null)
            {
                throw new ArgumentNullException(nameof(newSequence));
            }

            ID = newSequence.ID;
            Alphabet = newSequence.Alphabet;
            Count = newSequence.Count;
            _metadata = new Dictionary<string, object>(newSequence.Metadata);

            var realSequence = newSequence as Sequence;
            if (realSequence != null)
            {
                _sequenceData = new byte[newSequence.Count];
                Helper.Copy(realSequence._sequenceData, _sequenceData, realSequence._sequenceData.GetLongLength());
            }
            else
            {
                _sequenceData = newSequence.ToArray();
            }
        }
        #endregion Constructors

        #region Properties
        /// <summary>
        /// Gets or sets an identifier for this instance of sequence class.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Gets the number of bytes contained in the Sequence.
        /// </summary>
        public long Count { get; private set; }

        /// <summary>
        /// Gets or sets the alphabet to which symbols in this sequence belongs to.
        /// </summary>
        public IAlphabet Alphabet { get; set; }

        /// <summary>
        /// Gets or sets the Metadata of this instance.
        /// Many sequence representations when saved to file also contain
        /// information about that sequence. Unfortunately there is no standard
        /// around what that data may be from format to format. This property
        /// allows a place to put structured metadata that can be accessed by
        /// a particular key.
        /// <para>
        /// For example, if species information is stored in a particular Species
        /// class, you could add it to the dictionary by:
        /// </para>
        /// <para>
        /// mySequence.Metadata["SpeciesInfo"] = mySpeciesInfo;
        /// </para>
        /// <para>
        /// To fetch the data you would use:
        /// </para>
        /// <para>
        /// Species mySpeciesInfo = mySequence.Metadata["SpeciesInfo"];
        /// </para>
        /// <para>
        /// Particular formats may create their own data model class for information
        /// unique to their format as well. Such as:
        /// </para>
        /// <para>
        /// GenBankMetadata genBankData = new GenBankMetadata();
        /// // ... add population code
        /// mySequence.MetaData["GenBank"] = genBankData;.
        /// </para>
        /// </summary>
        public Dictionary<string, object> Metadata
        {
            get { return _metadata ?? (_metadata = new Dictionary<string, object>()); }
            set { _metadata = value; }
        }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Returns the byte found at the specified index if within bounds. Note 
        /// that the index value starts at 0.
        /// </summary>
        /// <param name="index">Index at which the symbol is required.</param>
        /// <returns>Byte value at the given index.</returns>
        public byte this[long index]
        {
            get
            {
                return _sequenceData[index];
            }
        }

        /// <summary>
        /// Return a new sequence representing this sequence with the orientation reversed.
        /// </summary>
        public ISequence GetReversedSequence()
        {
            var values = new byte[Count];

            Helper.Copy(_sequenceData, values, _sequenceData.GetLongLength());

            Array.Reverse(values);
            var seq = new Sequence { _sequenceData = values, Alphabet = Alphabet, ID = ID, Count = Count };
            if (_metadata != null)
                seq._metadata = new Dictionary<string, object>(_metadata);

            return seq;
        }

        /// <summary>
        /// Return a new sequence representing the complement of this sequence.
        /// </summary>
        public ISequence GetComplementedSequence()
        {
            if (!Alphabet.IsComplementSupported)
            {
                throw new InvalidOperationException(ComplementNotFound);
            }

            var complemented = new byte[Count];
            Alphabet.TryGetComplementSymbol(_sequenceData, out complemented);

            var seq = new Sequence { _sequenceData = complemented, Alphabet = Alphabet, ID = ID, Count = Count };
            if (_metadata != null)
                seq._metadata = new Dictionary<string, object>(_metadata);

            return seq;
        }

        /// <summary>
        /// Return a new sequence representing the reverse complement of this sequence.
        /// </summary>
        public ISequence GetReverseComplementedSequence()
        {
            if (!Alphabet.IsComplementSupported)
            {
                throw new InvalidOperationException(ComplementNotFound);
            }

            var reverseComplemented = new byte[Count];
            Alphabet.TryGetComplementSymbol(_sequenceData, out reverseComplemented);
            Array.Reverse(reverseComplemented);
            var seq = new Sequence { _sequenceData = reverseComplemented, Alphabet = Alphabet, ID = ID, Count = Count };
            if (_metadata != null)
                seq._metadata = new Dictionary<string, object>(_metadata);

            return seq;
        }

        /// <summary>
        /// Return a new sequence representing a range (subsequence) of this sequence.
        /// </summary>
        /// <param name="start">The index of the first symbol in the range.</param>
        /// <param name="length">The number of symbols in the range.</param>
        /// <returns>The sub-sequence.</returns>
        public ISequence GetSubSequence(long start, long length)
        {
            if (start >= Count)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }

            if (start + length > Count)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            var subSequence = new byte[length];
            for (long index = 0; index < length; index++)
            {
                subSequence[index] = _sequenceData[start + index];
            }

            var seq = new Sequence { _sequenceData = subSequence, Alphabet = Alphabet, ID = ID, Count = subSequence.Length };
            if (_metadata != null)
                seq._metadata = new Dictionary<string, object>(_metadata);
            return seq;
        }

        /// <summary>
        /// Gets the index of first non-gap symbol.
        /// </summary>
        /// <returns>If found returns a zero based index of the first non-gap symbol, otherwise returns -1.</returns>
        public long IndexOfNonGap()
        {
            return IndexOfNonGap(0);
        }

        /// <summary>
        /// Returns the position of the first symbol beyond startPos that does not 
        /// have a Gap symbol.
        /// </summary>
        /// <param name="startPos">Index value beyond which the non-gap symbol is searched for.</param>
        /// <returns>If found returns a zero based index of the first non-gap symbol, otherwise returns -1.</returns>
        public long IndexOfNonGap(long startPos)
        {
            if (startPos >= _sequenceData.GetLongLength())
            {
                throw new ArgumentOutOfRangeException(nameof(startPos));
            }

            HashSet<byte> gapSymbols;
            if (!Alphabet.TryGetGapSymbols(out gapSymbols))
            {
                return startPos;
            }

            var aliasSymbolsMap = Alphabet.GetSymbolValueMap();

            for (var index = startPos; index < Count; index++)
            {
                var symbol = aliasSymbolsMap[_sequenceData[index]];
                if (!gapSymbols.Contains(symbol))
                {
                    return index;
                }
            }

            return -1;
        }

        /// <summary>
        /// Gets the index of last non-gap symbol.
        /// </summary>
        /// <returns>If found returns a zero based index of the last non-gap symbol, otherwise returns -1.</returns>
        public long LastIndexOfNonGap()
        {
            return LastIndexOfNonGap(Count - 1);
        }

        /// <summary>
        /// Returns the index of last non-gap symbol before the specified end position.
        /// </summary>
        /// <param name="endPos">Index value up to which the non-Gap symbol is searched for.</param>
        /// <returns>If found returns a zero based index of the last non-gap symbol, otherwise returns -1.</returns>
        public long LastIndexOfNonGap(long endPos)
        {
            HashSet<byte> gapSymbols;

            if (!Alphabet.TryGetGapSymbols(out gapSymbols))
            {
                return endPos;
            }

            var aliasSymbolsMap = Alphabet.GetSymbolValueMap();
            for (var index = endPos; index >= 0; index--)
            {
                var symbol = aliasSymbolsMap[_sequenceData[index]];
                if (!gapSymbols.Contains(symbol))
                {
                    return index;
                }
            }

            return -1;
        }

        /// <summary>
        /// Gets an enumerator to the bytes present in this sequence.
        /// </summary>
        /// <returns>An IEnumerator of bytes.</returns>
        public IEnumerator<byte> GetEnumerator()
        {
            for (long index = 0; index < Count; index++)
            {
                yield return _sequenceData[index];
            }
        }

        /// <summary>
        /// Returns a string representation of the sequence data. This representation
        /// will come from the symbols in the alphabet defined for the sequence.
        /// 
        /// Thus a Sequence whose Alphabet is Alphabets.DNA may return a value like
        /// 'GATTCCA'
        /// </summary>
        public override string ToString()
        {
            if (Count > Helper.AlphabetsToShowInToString)
            {
                return string.Format(CultureInfo.CurrentCulture, ToStringFormat,
                                     new string(_sequenceData.Take(Helper.AlphabetsToShowInToString).Select((a => (char)a)).ToArray()),
                                     (Count - Helper.AlphabetsToShowInToString));
            }
            return new string(_sequenceData.Take(_sequenceData.Length).Select((a => (char)a)).ToArray());
        }

        /// <summary>
        /// Converts part of the sequence to a string.
        /// </summary>
        /// <param name="startIndex">Start position of the sequence.</param>
        /// <param name="length">Number of symbols to return.</param>
        /// <returns>Part of the sequence in string format.</returns>
        public string ConvertToString(long startIndex = 0, long length = -1)
        {
            if (startIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex), @"> 0");
            }

            if (length == -1)
            {
                length = Count;
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), @"> 0");
            }

            if (startIndex + length > Count)
            {
                throw new ArgumentOutOfRangeException(nameof(length), LengthPlusStartCannotExceedCount);
            }

            var sb = new StringBuilder();
            try
            {
                for (var index = startIndex; index < startIndex + length; index++)
                {
                    sb.Append((char)_sequenceData[index]);
                }
            }
            catch (IndexOutOfRangeException rangeEx)
            {
                throw new ArgumentOutOfRangeException(nameof(length), rangeEx.Message);
            }

            return sb.ToString();
        }

        /// <summary>
        /// This is used by some of the built-in algorithms which access the data in a read-only fashion
        /// to quickly grab a sequence of data without copying it.  It cannot be used outside Bio.dll
        /// For outside users, use the CopyTo method.
        /// </summary>
        /// <returns></returns>
        internal byte[] GetInternalArray()
        {
            return _sequenceData;
        }

        // GetData() method added by Stephen Haines.
        // TODO: write test.
        /// <inheritdoc />
        public byte[] GetData(long startIndex = 0, long length = -1)
        {
            // Perform same checks as in existing ConvertToString() method
            if (startIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex), @"> 0");
            }

            if (length == -1)
            {
                length = Count;
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), @"> 0");
            }

            if (startIndex + length > Count)
            {
                throw new ArgumentOutOfRangeException(nameof(length), LengthPlusStartCannotExceedCount);
            }

            var rawData = new byte[length - startIndex];
            CopyTo(rawData, startIndex, length);
            return rawData;
        }

        /// <summary>
        ///     Copies items from the sequence to a pre allocated array.
        /// </summary>
        /// <param name="byteArray">Array to fill the items to.</param>
        /// <param name="start">Index of the first item to copy.</param>
        /// <param name="count">Total numbers of elements to be copied.</param>
        public void CopyTo(byte[] byteArray, long start, long count)
        {
            if (byteArray == null)
            {
                throw new ArgumentNullException(ParameterNameArray);
            }

            if ((start + count) > Count)
            {
                throw new ArgumentException(DestArrayNotLargeEnough);
            }

            if (start < 0)
            {
                throw new ArgumentException(StartCannotBeLessThanZero);
            }

            if (count < 0)
            {
                throw new ArgumentException(CountCannotBeLessThanZero);
            }

            Helper.Copy(_sequenceData, start, byteArray, 0, count);
        }

        /// <summary>
        /// Gets an enumerator to the bytes present in this sequence.
        /// </summary>
        /// <returns>An IEnumerator of bytes.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _sequenceData.GetEnumerator();
        }
        #endregion
    }
}
