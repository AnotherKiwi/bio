using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Bio.IO.FastA
{
    public class FastaAMetadata
    {
        public FastaAMetadata(in string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            int splitAt = id.IndexOf(' ');

            if (splitAt < 0)
            {
                Accession = id;
                OtherAccessions = string.Empty;
                Description = string.Empty;
            }
            else
            {
                Accession = id.Substring(0, splitAt);
                OtherAccessions = string.Empty;
                Description = id.Substring(splitAt).Trim();                
            }
        }

        public string Accession { get; }

        public string OtherAccessions { get; }

        public string Description { get; }
    }
}
