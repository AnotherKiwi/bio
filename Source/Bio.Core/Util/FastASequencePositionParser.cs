﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using Bio.IO.FastA;

namespace Bio.Util
{
    /// <summary>
    /// This class is similar to FastAParser except that this class appends the
    /// position of the sequence parsed to its id.
    /// </summary>
    public class FastASequencePositionParser : IDisposable
    {
        /// <summary>
        /// An instance of FastAParser.
        /// </summary>
        private readonly FastAParser fastaParser;

        /// <summary>
        /// Stream to use in GetSequenceAt method.
        /// </summary>
        private Stream stream;

        /// <summary>
        /// Sequence cache to hold sequences.
        /// </summary>
        private SequenceCache sequenceCache;

        /// <summary>
        /// Flag to indicate to get the forward strand sequence of a reverse paired read.
        /// </summary>
        private readonly bool reverseReversePairedRead;

        /// <summary>
        /// Initializes a new instance of the FastASequencePositionParser class by 
        /// loading the specified stream.
        /// </summary>
        /// <param name="stream">Stream to load</param>
        /// <param name="reverseReversePairedRead">Flag to indicate to get the forward strand sequence of a reverse paired read.</param>
        public FastASequencePositionParser(Stream stream, bool reverseReversePairedRead = false)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            fastaParser = new FastAParser();
            this.stream = stream;
            this.reverseReversePairedRead = reverseReversePairedRead;
        }

        /// <summary>
        /// Gets a value indicating whether the sequences are cached or not.
        /// </summary>
        public bool SequencesCached { get; private set; }

        /// <summary>
        /// Stream for this parser.
        /// </summary>
        public Stream Stream
        {
            get { return stream; }
        }

        /// <summary>
        /// Gets or sets the alphabet to use for parsed ISequence objects.  If this is not set, the alphabet will
        /// be determined based on the file being parsed.
        /// </summary>
        public IAlphabet Alphabet
        {
            get
            {
                return fastaParser.Alphabet;
            }

            set
            {
                fastaParser.Alphabet = value;
            }
        }

        /// <summary>
        /// Returns an IEnumerable of sequences in the file being parsed.
        /// </summary>
        /// <returns>Returns ISequence arrays.</returns>
        public IEnumerable<ISequence> Parse()
        {
            if (SequencesCached)
            {
                return sequenceCache.GetAllSequences();
            }
            
            return ParseFromFile();
        }

        /// <summary>
        /// Loads sequences to cache.
        /// This method will ignore the call if sequences are already cached.
        /// </summary>
        public void CacheSequencesForRandomAccess()
        {
            if (SequencesCached)
            {
                return;
            }

            sequenceCache = new SequenceCache();

            foreach (var sequence in Parse())
            {
                var position = long.Parse(sequence.ID.Substring(sequence.ID.LastIndexOf('@') + 1), CultureInfo.InvariantCulture);
                sequenceCache.Add(position, sequence);
            }

            if (sequenceCache.Count > 0)
            {
                SequencesCached = true;
            }
        }

        /// <summary>
        /// Gets the sequence at specified position.
        /// </summary>
        /// <param name="position">Start position of the sequence required in the file.</param>
        /// <returns>Sequence present at the specified position.</returns>
        public ISequence GetSequenceAt(long position)
        {
            if (SequencesCached)
            {
                return sequenceCache.GetSequenceAt(position);
            }

            stream.Position = position;

            var sequence = fastaParser.ParseOne(stream);

            var delim = "@";
            if (sequence.ID.LastIndexOf(Helper.PairedReadDelimiter) == -1)
            {
                delim = "!@";
            }
            
            if (reverseReversePairedRead)
            {
                string originalSequenceId;
                bool forwardRead;
                string pairedReadType;
                string libraryName;
                var pairedRead = Helper.ValidatePairedSequenceId(sequence.ID, out originalSequenceId, out forwardRead, out pairedReadType, out libraryName);
                if (pairedRead && !forwardRead)
                {
                    sequence = sequence.GetReverseComplementedSequence();
                }
            }

            sequence.ID += delim + position.ToString(CultureInfo.InvariantCulture);

            return sequence;
        }

        /// <summary>
        /// Disposes the underlying stream.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the underlying stream.
        /// </summary>
        /// <param name="disposing">Flag to indicate whether disposing or not.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (stream != null)
                {
                    stream.Dispose();
                    stream = null;
                }
                sequenceCache = null;
                SequencesCached = false;
            }
        }

        /// <summary>
        /// Parses sequences from the file.
        /// </summary>
        private IEnumerable<ISequence> ParseFromFile()
        {
            var sequences = fastaParser.Parse(stream);
            var positions = GetNextSequenceStartPosition(stream).GetEnumerator();

            foreach (var sequence in sequences)
            {
                var seq = sequence;
                long position = -1;
                if (positions.MoveNext())
                {
                    position = positions.Current;
                }

                var delim = "@";
                if (seq.ID.LastIndexOf(Helper.PairedReadDelimiter) == -1)
                {
                    delim = "!@";
                }
                    
                if (reverseReversePairedRead)
                {
                    string originalSequenceId;
                    bool forwardRead;
                    string pairedReadType;
                    string libraryName;
                    var pairedRead = Helper.ValidatePairedSequenceId(seq.ID, out originalSequenceId, out forwardRead, out pairedReadType, out libraryName);
                    if (pairedRead && !forwardRead)
                    {
                        seq = seq.GetReverseComplementedSequence();
                    }
                }

                seq.ID = seq.ID + delim + position;

                yield return seq;
            }
        }

        /// <summary>
        /// Gets the next sequence start position in the file.
        /// </summary>
        /// <param name="stream">FastA file stream.</param>
        /// <returns>Position of the next sequence in the stream.</returns>
        private static IEnumerable<long> GetNextSequenceStartPosition(Stream stream)
        {
            // 4k at a time.
            var readBuffer = new byte[4 * 1024];
            var lastReadLength = 0;
            var startIndex = 0;
            var startChar = (byte)'>';
            long position = 0;
            while (stream.Position != stream.Length)
            {
                if (startIndex >= lastReadLength)
                {
                    startIndex = 0;
                    position += lastReadLength;
                    lastReadLength = stream.Read(readBuffer, 0, 4 * 1024);
                }

                while (startIndex < lastReadLength)
                {
                    if (readBuffer[startIndex++] == startChar)
                    {
                        yield return position + startIndex - 1;
                    }
                }
            }
        }

        /// <summary>
        /// Class to hold sequences.
        /// </summary>
        private class SequenceCache
        {
            private int sequenceLength = 1;
            private int bucketSize = 1000;
            private List<SequenceHolder> buckets = new List<SequenceHolder>();

            /// <summary>
            /// Gets total sequences present in this instance.
            /// </summary>
            public long Count { get; private set; }

            /// <summary>
            /// Adds the specified sequence with position.
            /// </summary>
            /// <param name="position">Position of the sequence in file.</param>
            /// <param name="sequence">Sequence to cache.</param>
            public void Add(long position, ISequence sequence)
            {
                if (Count == 0)
                {
                    sequenceLength = (int)sequence.Count;
                }

                var index = (int)(position / (sequenceLength * bucketSize));
                var newHolder = new SequenceHolder() { Position = position, Sequence = sequence };

                if (index >= buckets.Count)
                {
                    while (index >= buckets.Count)
                    {
                        buckets.Add(null);
                    }

                    buckets[index] = newHolder;
                    Count++;
                    return;
                }

                var holder = buckets[index];
                if (holder == null)
                {
                    buckets[index] = newHolder;
                    Count++;
                    return;
                }

                if (holder.Position > position)
                {
                    buckets[index] = newHolder;
                    buckets[index].Next = holder;
                }
                else
                {
                    while (holder.Next != null)
                    {
                        if (holder.Next.Position > position)
                        {
                            break;
                        }

                        holder = holder.Next;
                    }

                    newHolder.Next = holder.Next;
                    holder.Next = newHolder;
                }

                Count++;
            }

            /// <summary>
            /// Gets the sequence for the specified position.
            /// </summary>
            /// <param name="position">Position.</param>
            public ISequence GetSequenceAt(long position)
            {
                var index = (int)(position / (sequenceLength * bucketSize));
                if (index >= buckets.Count)
                {
                    return null;
                }

                var holder = buckets[index];
                while (holder != null)
                {
                    if (holder.Position == position)
                    {
                        return holder.Sequence;
                    }

                    holder = holder.Next;
                }

                return null;
            }

            /// <summary>
            /// Gets all sequences present in this instance.
            /// </summary>
            public IEnumerable<ISequence> GetAllSequences()
            {
                for (var i = 0; i < buckets.Count; i++)
                {
                    var holder = buckets[i];
                    while (holder != null)
                    {
                        yield return holder.Sequence;
                        holder = holder.Next;
                    }
                }
            }

            /// <summary>
            /// Class to hold sequence along with its position.
            /// </summary>
            private class SequenceHolder
            {
                /// <summary>
                /// Position of the sequence in file.
                /// </summary>
                public long Position { get; set; }

                /// <summary>
                /// Reference to next SequenceHolder
                /// </summary>
                public SequenceHolder Next { get; set; }

                /// <summary>
                /// Sequence.
                /// </summary>
                public ISequence Sequence { get; set; }
            }
        }
    }
}
