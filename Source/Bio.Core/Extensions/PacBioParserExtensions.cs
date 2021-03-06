﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Bio.IO.PacBio
{
    
    /// <summary>
    /// Parser extensions for the PacBio BAM parsers.
    /// </summary>
    public static class BAMParserExtensions
    {
        /// <summary>
        /// Returns an iterator over a set of SAMAlignedSequences retrieved from a parsed BAM file.
        /// </summary>
        /// <param name="parser">Parser</param>
        /// <param name="filename">Filename</param>
        /// <returns>IEnumerable SAMAlignedSequence object.</returns>
        public static IEnumerable<PacBioCCSRead> Parse(this PacBioCCSBamReader parser, string filename)
        {
            if (parser == null)
            {
                throw new ArgumentNullException(nameof(parser));
            }
            if (filename == null)
            {
                throw new ArgumentNullException(nameof(filename));
            }

            using (FileStream fs = File.OpenRead(filename))
            {
                foreach (PacBioCCSRead item in PacBioCCSBamReader.Parse(fs))
                    yield return item;
            }
        }

    }
}
