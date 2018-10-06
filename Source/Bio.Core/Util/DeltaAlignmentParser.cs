using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using Bio.Algorithms.Alignment;
using Bio.Extensions;

namespace Bio.Util
{
    /// <summary>
    /// This parser reads from a source of text that contains DeltaAlignments
    /// and converts the data to in-memory DeltaAlignment objects.  
    /// </summary>
    public sealed class DeltaAlignmentParser : IDisposable
    {
        /// <summary>
        /// Stream for Delta file.
        /// </summary>
        private Stream deltaStream;

        /// <summary>
        /// List holding all the open parsing readers.
        /// </summary>
        private readonly List<StreamReader> parsingReaders = new List<StreamReader>(); 

        /// <summary>
        /// Initializes a new instance of the DeltaAlignmentParser class by 
        /// loading the specified filename.
        /// </summary>
        /// <param name="stream">Name of the File.</param>
        /// <param name="queryParser">FastASequencePositionParser instance.</param>
        public DeltaAlignmentParser(Stream stream, FastASequencePositionParser queryParser)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            if (queryParser == null)
            {
                throw new ArgumentNullException(nameof(queryParser));
            }

            deltaStream = stream;
            QueryParser = queryParser;
        }

        /// <summary>
        /// Gets the query parser.
        /// </summary>
        public FastASequencePositionParser QueryParser { get; private set; }

        /// <summary>
        /// Gets the position of DeltaAlignments in the specified file.
        /// </summary>
        public IEnumerable<long> GetPositions()
        {
            using (var streamReader = deltaStream.OpenRead(leaveOpen:false))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    if (line.StartsWith("@"))
                    {
                        line = line.Substring(1);
                        yield return long.Parse(line);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the DeltaAlignment at specified position of the file.
        /// </summary>
        /// <param name="position">Position at which delta alignment is required.</param>
        /// <returns>Delta alignment.</returns>
        public DeltaAlignment GetDeltaAlignmentAt(long position)
        {
            using (var reader = deltaStream.OpenRead())
            {
                long deltaPosition = -1;
                var line = ReadNextLine(reader);
                if (line == null || !line.StartsWith("@", StringComparison.OrdinalIgnoreCase))
                {
                    throw new FormatException(string.Format(CultureInfo.CurrentCulture, Properties.Resource.CorruptedDeltaAlignmentFile, position));
                }

                deltaPosition = long.Parse(line.Substring(1), CultureInfo.InvariantCulture);
                if (position != deltaPosition)
                {
                    throw new FormatException(string.Format(CultureInfo.CurrentCulture, Properties.Resource.DeltaAlignmentIDDoesnotMatch, deltaPosition, position));
                }

                line = ReadNextLine(reader);
                if (line == null || !line.StartsWith(">", StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception(Properties.Resource.INVALID_INPUT_FILE);
                }

                var referenceId = line.Substring(1);

                // Read next line.
                line = ReadNextLine(reader);

                // Second line - Query sequence id
                var queryId = line;

                // fetch the query sequence from the query file
                ISequence querySequence = null;
                Sequence refEmpty = null;

                if (!string.IsNullOrEmpty(queryId))
                {
                    // Get the id and remove any alphas - this can happen because the delta might
                    // have "Reverse" appended to it when it's a reversed sequence.
                    var id = queryId.Substring(queryId.LastIndexOf('@') + 1);
                    var idx = Array.FindIndex(id.ToCharArray(), c => !Char.IsDigit(c));
                    if (idx > 0)
                        id = id.Substring(0, idx);

                    var sequencePosition = long.Parse(id, CultureInfo.InvariantCulture);
                    querySequence = QueryParser.GetSequenceAt(sequencePosition);
                    refEmpty = new Sequence(querySequence.Alphabet, "A", false) { ID = referenceId };
                }

                var deltaAlignment = new DeltaAlignment(refEmpty, querySequence) { Id = deltaPosition };
                line = ReadNextLine(reader);
                var deltaAlignmentProperties = line.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (deltaAlignmentProperties != null && deltaAlignmentProperties.Length == 7)
                {
                    long temp;
                    deltaAlignment.FirstSequenceStart = long.TryParse(deltaAlignmentProperties[0], out temp) ? temp : 0;
                    deltaAlignment.FirstSequenceEnd = long.TryParse(deltaAlignmentProperties[1], out temp) ? temp : 0;
                    deltaAlignment.SecondSequenceStart = long.TryParse(deltaAlignmentProperties[2], out temp) ? temp : 0;
                    deltaAlignment.SecondSequenceEnd = long.TryParse(deltaAlignmentProperties[3], out temp) ? temp : 0;

                    // Look for a reversed sequence
                    if (deltaAlignment.SecondSequenceEnd < deltaAlignment.SecondSequenceStart)
                    {
                        temp = deltaAlignment.SecondSequenceEnd;
                        deltaAlignment.SecondSequenceEnd = deltaAlignment.SecondSequenceStart;
                        deltaAlignment.SecondSequenceStart = temp;
                        deltaAlignment.QueryDirection = Cluster.ReverseDirection;
                    }

                    int error;
                    deltaAlignment.Errors = int.TryParse(deltaAlignmentProperties[4], out error) ? error : 0;
                    deltaAlignment.SimilarityErrors = int.TryParse(deltaAlignmentProperties[5], out error) ? error : 0;
                    deltaAlignment.NonAlphas = int.TryParse(deltaAlignmentProperties[6], out error) ? error : 0;
                }

                // Fifth line - either a 0 - marks the end of the delta alignment or they are deltas
                while (line != null && !line.StartsWith("*", StringComparison.OrdinalIgnoreCase))
                {
                    long temp;
                    if (long.TryParse(line, out temp))
                    {
                        deltaAlignment.Deltas.Add(temp);
                    }

                    // Read next line.
                    line = reader.ReadLine();

                    // Continue reading if blank line found.
                    while (line != null && string.IsNullOrEmpty(line))
                    {
                        line = reader.ReadLine();
                    }
                }
                return deltaAlignment;
            }

        }

        /// <summary>
        /// Gets the query sequence id in the DeltaAlignment at specified position.
        /// </summary>
        /// <param name="position">Position of the delta alignment.</param>
        public string GetQuerySeqIdAt(long position)
        {
            using (var reader = deltaStream.OpenRead())
            {
                reader.BaseStream.Position = position;
                reader.DiscardBufferedData();

                long deltaPosition = -1;
                var line = ReadNextLine(reader);
                if (line == null || !line.StartsWith("@", StringComparison.OrdinalIgnoreCase))
                {
                    throw new FormatException(string.Format(CultureInfo.CurrentCulture, Properties.Resource.CorruptedDeltaAlignmentFile, position));
                }

                deltaPosition = long.Parse(line.Substring(1), CultureInfo.InvariantCulture);
                if (position != deltaPosition)
                {
                    throw new FormatException(string.Format(CultureInfo.CurrentCulture, Properties.Resource.DeltaAlignmentIDDoesnotMatch, deltaPosition, position));
                }

                line = ReadNextLine(reader);
                if (line == null || !line.StartsWith(">", StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception(Properties.Resource.INVALID_INPUT_FILE);
                }

                // Read next line.
                return ReadNextLine(reader);
            }
        }

        /// <summary>
        /// Gets Delta alignment id and query sequence ids pairs.
        /// </summary>
        public IEnumerable<Tuple<string, string>> GetQuerySeqIds()
        {
            using (var reader = deltaStream.OpenRead())
            {
                reader.BaseStream.Position = 0;
                reader.DiscardBufferedData();

                while (!reader.EndOfStream)
                {
                    var line = ReadNextLine(reader);
                    if (line == null || !line.StartsWith("@", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new Exception(Properties.Resource.INVALID_INPUT_FILE);
                    }

                    var id = line;
                    line = ReadNextLine(reader);
                    if (line == null || !line.StartsWith(">", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new Exception(Properties.Resource.INVALID_INPUT_FILE);
                    }

                    line = ReadNextLine(reader);
                    yield return new Tuple<string, string>(id, line);
                } 
            }
        }

        /// <summary>
        /// Returns an IEnumerable of DeltaAlignment in the file being parsed.
        /// </summary>
        /// <returns>Returns DeltaAlignment collection.</returns>
        public IEnumerable<DeltaAlignment> Parse()
        {
            using (var reader = deltaStream.OpenRead())
            {
                return ParseFrom(reader);
            }
        }

        /// <summary>
        /// Starts parsing of delta alignments from the specified position of the file.
        /// </summary>
        /// <param name="position">Position from which to start parsing.</param>
        /// <returns>IEnumerable of DeltaAlignments.</returns>
        public IEnumerable<DeltaAlignment> ParseFrom(long position)
        {
            using (var reader = deltaStream.OpenRead())
            {
                reader.BaseStream.Position = position;
                reader.DiscardBufferedData();
                return ParseFrom(reader);
            }
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
        /// Dispose the stream.
        /// </summary>
        /// <param name="disposing"></param>
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (deltaStream != null)
                {
                    deltaStream.Dispose();
                    deltaStream = null;
                }

                if (QueryParser != null)
                {
                    QueryParser.Dispose();
                    QueryParser = null;
                }
            }            
        }

        /// <summary>
        /// Gets the next line skipping the blank lines.
        /// </summary>
        /// <param name="streamReader">Stream reader.</param>
        private static string ReadNextLine(StreamReader streamReader)
        {
            // Read next line.
            var line = streamReader.ReadLine();
            var message = string.Empty;

            // Continue reading if blank line found.
            while (line != null && string.IsNullOrEmpty(line))
            {
                line = streamReader.ReadLine();
            }

            if (line == null)
            {
                message = string.Format(
                    CultureInfo.InvariantCulture,
                    Properties.Resource.InvalidSymbolInString,
                    string.Empty);
                throw new FormatException(message);
            }

            return line;
        }

        /// <summary>
        /// Starts parsing from the specified StreamReader.
        /// </summary>
        /// <param name="streamReader">Stream reader to parse.</param>
        /// <returns>IEnumerable of DeltaAlignments.</returns>
        private IEnumerable<DeltaAlignment> ParseFrom(StreamReader streamReader)
        {
            parsingReaders.Add(streamReader);

            var lastReadQuerySequenceId = string.Empty;
            ISequence sequence = null;

            if (streamReader.EndOfStream)
            {
                throw new Exception(Properties.Resource.INVALID_INPUT_FILE);
            }

            var line = ReadNextLine(streamReader);
            do
            {
                if (line == null || !line.StartsWith("@", StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception(Properties.Resource.INVALID_INPUT_FILE);
                }

                var deltaPosition = long.Parse(line.Substring(1));
                line = ReadNextLine(streamReader);
                if (line == null || !line.StartsWith(">", StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception(Properties.Resource.INVALID_INPUT_FILE);
                }

                DeltaAlignment deltaAlignment = null;

                // First line - reference id
                var referenceId = line.Substring(1);

                // Read next line.
                line = ReadNextLine(streamReader);

                // Second line - Query sequence id
                var queryId = line;

                // fetch the query sequence from the query file
                if (!string.IsNullOrEmpty(queryId))
                {
                    if (queryId != lastReadQuerySequenceId)
                    {
                        // Get the id and remove any alphas - this can happen because the delta might
                        // have "Reverse" appended to it when it's a reversed sequence.
                        var id = queryId.Substring(queryId.LastIndexOf('@') + 1);
                        var idx = Array.FindIndex(id.ToCharArray(), c => !Char.IsDigit(c));
                        if (idx > 0)
                            id = id.Substring(0, idx);
                        
                        var seqPosition = long.Parse(id, CultureInfo.InvariantCulture);
                        sequence = QueryParser.GetSequenceAt(seqPosition);
                        lastReadQuerySequenceId = queryId;
                    }

                    var refEmpty = new Sequence(sequence.Alphabet, "A", false) {ID = referenceId};
                    deltaAlignment = new DeltaAlignment(refEmpty, sequence);
                }

                deltaAlignment.Id = deltaPosition;
                // Fourth line - properties of delta alignment
                // Read next line.
                line = ReadNextLine(streamReader);

                var deltaAlignmentProperties = line.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (deltaAlignmentProperties != null && deltaAlignmentProperties.Length == 7)
                {
                    long temp;
                    deltaAlignment.FirstSequenceStart = long.TryParse(deltaAlignmentProperties[0], out temp) ? temp : 0;
                    deltaAlignment.FirstSequenceEnd = long.TryParse(deltaAlignmentProperties[1], out temp) ? temp : 0;
                    deltaAlignment.SecondSequenceStart = long.TryParse(deltaAlignmentProperties[2], out temp) ? temp : 0;
                    deltaAlignment.SecondSequenceEnd = long.TryParse(deltaAlignmentProperties[3], out temp) ? temp : 0;

                    // Look for a reversed sequence
                    if (deltaAlignment.SecondSequenceEnd < deltaAlignment.SecondSequenceStart)
                    {
                        temp = deltaAlignment.SecondSequenceEnd;
                        deltaAlignment.SecondSequenceEnd = deltaAlignment.SecondSequenceStart;
                        deltaAlignment.SecondSequenceStart = temp;
                        deltaAlignment.QueryDirection = Cluster.ReverseDirection;
                    }

                    int error;
                    deltaAlignment.Errors = int.TryParse(deltaAlignmentProperties[4], out error) ? error : 0;
                    deltaAlignment.SimilarityErrors = int.TryParse(deltaAlignmentProperties[5], out error) ? error : 0;
                    deltaAlignment.NonAlphas = int.TryParse(deltaAlignmentProperties[6], out error) ? error : 0;
                }

                // Fifth line - either a 0 - marks the end of the delta alignment or they are deltas
                while (line != null && !line.StartsWith("*", StringComparison.OrdinalIgnoreCase))
                {
                    long temp;
                    if (long.TryParse(line, out temp))
                    {
                        deltaAlignment.Deltas.Add(temp);
                    }

                    // Read next line.
                    line = streamReader.ReadLine();

                    // Continue reading if blank line found.
                    while (line != null && string.IsNullOrEmpty(line))
                    {
                        line = streamReader.ReadLine();
                    }
                }

                yield return deltaAlignment;

                // Read the next line
                line = streamReader.ReadLine();
            }
            while (line != null);
        }
    }
}
