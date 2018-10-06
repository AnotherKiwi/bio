using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

using Bio.Core.Extensions;
using Bio.Extensions;
using Bio.Properties;

namespace Bio.IO.FastQ
{
    /// <summary>
    ///     A FastQParser reads from a source of text that is formatted according to the FASTQ
    ///     file specification and converts the data to in-memory QualitativeSequence objects.
    /// </summary>
    public class FastQParser : IParserWithAlphabet<IQualitativeSequence>
    {
        /// <summary>
        ///     Initializes a new instance of the FastQParser class.
        /// </summary>
        public FastQParser()
        {
            FormatType = FastQFormatType.Illumina_v1_8;
        }

        /// <summary>
        ///     Gets or sets the format type to be used.
        ///     The FastQFormatType to be used for parsed QualitativeSequence objects.
        ///     Default value is Illumina_v1_8
        /// </summary>
        public FastQFormatType FormatType { get; set; }

        /// <summary>
        ///     Gets the type of parser.
        ///     This is intended to give developers name of the parser.
        /// </summary>
        public virtual string Name
        {
            get
            {
                return Resource.FastQName;
            }
        }

        /// <summary>
        ///     Gets the description of the parser.
        ///     This is intended to give developers some information
        ///     of the parser class. This property returns a simple description of what this
        ///     class achieves.
        /// </summary>
        public virtual string Description
        {
            get
            {
                return Resource.FASTQPARSER_DESCRIPTION;
            }
        }

        /// <summary>
        ///     Gets a comma separated values of the possible FastQ
        ///     file extensions.
        /// </summary>
        public virtual string SupportedFileTypes
        {
            get
            {
                return Resource.FASTQ_FILEEXTENSION;
            }
        }

        /// <summary>
        ///     Gets or sets the alphabet to use for parsed ISequence objects.  If this is not set, the alphabet will
        ///     be determined based on the file being parsed.
        /// </summary>
        public IAlphabet Alphabet { get; set; }

        /// <summary>
        ///     Gets the IEnumerable of QualitativeSequences from the steam being parsed.
        /// </summary>
        /// <param name="stream">Stream to be parsed.</param>
        /// <returns>Returns the QualitativeSequences.</returns>
        public IEnumerable<IQualitativeSequence> Parse(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var formatType = FormatType;

            using (var reader = stream.OpenRead())
            {
                while (reader.Peek() != -1)
                {
                    var seq = ParseOne(reader, formatType);
                    if (seq != null)
                    {
                        yield return seq;
                    }
                }
            }
        }

        /// <summary>
        ///     Returns a single QualitativeSequence from the FASTQ data.
        /// </summary>
        /// <param name="stream">Reader to be parsed.</param>
        /// <returns>Returns a QualitativeSequence.</returns>
        public IQualitativeSequence ParseOne(Stream stream)
        {
            return ParseOne(stream, FormatType);
        }

        /// <summary>
        ///     Returns a single QualitativeSequence from the FASTQ data.
        /// </summary>
        /// <param name="stream">Reader to be parsed.</param>
        /// <param name="formatType">FASTQ format type.</param>
        /// <returns>Returns a QualitativeSequence.</returns>
        public IQualitativeSequence ParseOne(Stream stream, FastQFormatType formatType)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            using (var reader = stream.OpenRead())
            {
                return ParseOne(reader, formatType);
            }
        }

        /// <summary>
        ///     Returns a single QualitativeSequence from the FASTQ data.
        /// </summary>
        /// <param name="reader">Reader to be parsed.</param>
        /// <param name="formatType">FASTQ format type.</param>
        /// <returns>Returns a QualitativeSequence.</returns>
        private IQualitativeSequence ParseOne(StreamReader reader, FastQFormatType formatType)
        {
            if (reader.EndOfStream)
            {
                return null;
            }

            var line = ReadNextLine(reader, true);
            if (line == null || !line.StartsWith("@", StringComparison.Ordinal))
            {
                var message = string.Format(CultureInfo.CurrentCulture, Resource.INVALID_INPUT_FILE, Name);
                throw new Exception(message);
            }

            // Process header line.
            var id = line.Substring(1).Trim();

            line = ReadNextLine(reader, true);
            if (string.IsNullOrEmpty(line))
            {
                var details = string.Format(CultureInfo.CurrentCulture, Resource.FastQ_InvalidSequenceLine, id);
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    Resource.IOFormatErrorMessage,
                    Name,
                    details);
                throw new Exception(message);
            }

            // Get sequence from second line.
            var sequenceData = Encoding.ASCII.GetBytes(line);

            // Goto third line.
            line = ReadNextLine(reader, true);

            // Check for '+' symbol in the third line.
            if (line == null || !line.StartsWith("+", StringComparison.Ordinal))
            {
                var details = string.Format(
                    CultureInfo.CurrentCulture,
                    Resource.FastQ_InvalidQualityScoreHeaderLine,
                    id);
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    Resource.IOFormatErrorMessage,
                    Name,
                    details);
                throw new Exception(message);
            }

            var qualScoreId = line.Substring(1).Trim();

            if (!string.IsNullOrEmpty(qualScoreId) && !id.Equals(qualScoreId))
            {
                var details = string.Format(
                    CultureInfo.CurrentCulture,
                    Resource.FastQ_InvalidQualityScoreHeaderData,
                    id);
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    Resource.IOFormatErrorMessage,
                    Name,
                    details);
                throw new Exception(message);
            }

            // Goto fourth line.
            line = ReadNextLine(reader, true);

            if (string.IsNullOrEmpty(line))
            {
                var details = string.Format(CultureInfo.CurrentCulture, Resource.FastQ_EmptyQualityScoreLine, id);
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    Resource.IOFormatErrorMessage,
                    Name,
                    details);
                throw new Exception(message);
            }

            // Get the quality scores from the fourth line.
            var qualScores = Encoding.ASCII.GetBytes(line);

            // Check for sequence length and quality score length.
            if (sequenceData.GetLongLength() != qualScores.GetLongLength())
            {
                var details = string.Format(
                    CultureInfo.CurrentCulture,
                    Resource.FastQ_InvalidQualityScoresLength,
                    id);
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    Resource.IOFormatErrorMessage,
                    Name,
                    details);
                throw new Exception(message);
            }

            // Auto detect alphabet if alphabet is set to null, else validate with already set alphabet
            var alphabet = Alphabet;
            if (alphabet == null)
            {
                alphabet = Alphabets.AutoDetectAlphabet(sequenceData, 0, sequenceData.GetLongLength(), alphabet);
                if (alphabet == null)
                {
                    throw new Exception(Resource.CouldNotIdentifyAlphabetType);
                }
            }
            else
            {
                if (!alphabet.ValidateSequence(sequenceData, 0, sequenceData.GetLongLength()))
                {
                    throw new Exception(Resource.InvalidAlphabetType);
                }
            }

            return new QualitativeSequence(alphabet, formatType, sequenceData, qualScores, false) { ID = id };
        }

        /// <summary>
        ///     Gets the next available line from the specified stream reader.
        /// </summary>
        /// <param name="reader">Stream reader.</param>
        /// <param name="skipBlankLine">
        ///     Flag to skip blank lines. If true this method returns
        ///     first non blank line available from the current position, else returns next available line.
        /// </param>
        /// <returns></returns>
        private static string ReadNextLine(TextReader reader, bool skipBlankLine)
        {
            // Continue reading if blank line found.
            var line = reader.ReadLine();
            while (skipBlankLine && line != null && string.IsNullOrEmpty(line))
            {
                line = reader.ReadLine();
            }

            return line;
        }
    }
}