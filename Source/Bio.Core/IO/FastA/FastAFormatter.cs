using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

using Bio.Extensions;

namespace Bio.IO.FastA
{
    /// <summary>
    /// Writes an ISequence the file specified while creating an instance of this class.
    /// The output is formatter according to the FastA format.
    /// </summary>
    public sealed class FastAFormatter : ISequenceFormatter
    {
        /// <summary>
        /// Buffer used while writing to stream.
        /// </summary>
        private byte[] buffer;

        /// <summary>
        /// Default Maximum symbols allowed per line. 
        /// As per FastA format, recommended value is 80.
        /// </summary>
        private const int DefaultMaxSymbolsAllowedPerLine = 80;

        /// <summary>
        /// Gets the name of this formatter.
        /// This is intended to give developers name of the formatter.
        /// </summary>
        public string Name
        {
            get
            {
                return Properties.Resource.FastAName;
            }
        }

        /// <summary>
        /// Gets the description of this formatter.
        /// This is intended to give developers some information 
        /// of the formatter class. This property returns a simple description of what this
        ///  class achieves.
        /// </summary>
        public string Description
        {
            get
            {
                return Properties.Resource.FASTAFORMATTER_DESCRIPTION;
            }
        }

        /// <summary>
        /// Gets the file extension supported by this formatter.
        /// </summary>
        public string SupportedFileTypes
        {
            get
            {
                return Properties.Resource.FASTA_FILEEXTENSION;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the FastAFormatter will flush its buffer 
        /// to the underlying stream after every call to Write(ISequence).
        /// </summary>
        public bool AutoFlush { get; set; }

        /// <summary>
        /// Gets or sets the maximum symbols allowed per line.
        /// Default value is 80.
        /// Note that the FastA format recommends that all lines 
        /// should be less than 80 symbol in length.
        /// </summary>
        public int MaxSymbolsAllowedPerLine { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public FastAFormatter()
        {
            MaxSymbolsAllowedPerLine = DefaultMaxSymbolsAllowedPerLine;
            AutoFlush = true;
        }

        /// <summary>
        /// Writes the Multiple sequence in FastA format to the file.
        /// Note that if the sequence contains more than the MaxSymbolsAllowedPerLine 
        /// value then it will split the symbols in the specified sequence in to multiple lines, 
        /// where each line will contain maximum of MaxSymbolsAllowedPerLine symbols.
        /// </summary>
        /// <param name="stream">Stream to write to, it will be closed at the end.</param>
        /// <param name="sequences">Sequences to write.</param>
        public void Format(Stream stream, IEnumerable<ISequence> sequences)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (sequences == null)
            {
                throw new ArgumentNullException(nameof(sequences));
            }

            using (var writer = stream.OpenWrite())
            {
                foreach (var sequence in sequences)
                {
                    Write(writer, sequence);
                }
            }
        }

        /// <summary>
        /// Writes the specified sequence in FastA format to the file.
        /// Note that if the sequence contains more than the MaxSymbolsAllowedPerLine 
        /// value then it will split the symbols in the specified sequence in to multiple lines, 
        /// where each line will contain maximum of MaxSymbolsAllowedPerLine symbols.
        /// </summary>
        /// <param name="stream">Stream to write to, it will be left open at the end.</param>
        /// <param name="data">Sequence to write.</param>
        public void Format(Stream stream, ISequence data)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            using (var writer = stream.OpenWrite())
            {
                Write(writer, data);    
            }
        }

        /// <summary>
        /// Writes the specified sequence in FastA format to the file.
        /// Note that if the sequence contains more than the MaxSymbolsAllowedPerLine 
        /// value then it will split the symbols in the specified sequence in to multiple lines, 
        /// where each line will contain maximum of MaxSymbolsAllowedPerLine symbols.
        /// </summary>
        /// <param name="writer">Stream to write to, it will be left open at the end.</param>
        /// <param name="data">Sequence to write.</param>
        void Write(TextWriter writer, ISequence data)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var maxLineSize = MaxSymbolsAllowedPerLine;
            if (buffer == null)
            {
                buffer = new byte[maxLineSize];
            }

            // Buffer resize is required as MaxSymbolsAllowedPerLine can be modified 
            if (buffer.Length < maxLineSize)
            {
                Array.Resize(ref buffer, maxLineSize);
            }

            writer.WriteLine(">" + data.ID);

            for (long index = 0; index < data.Count; index += maxLineSize)
            {
                int bufferIndex;
                for (bufferIndex = 0; bufferIndex < maxLineSize && index + bufferIndex < data.Count; bufferIndex++)
                {
                    buffer[bufferIndex] = data[index + bufferIndex];
                }

                var line = Encoding.UTF8.GetString(buffer, 0, bufferIndex);
                writer.WriteLine(line);
            }

            if (AutoFlush)
            {
                writer.Flush();
            }
        }
    }
}
