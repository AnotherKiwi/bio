using System.Collections.Generic;
using System.Linq;

namespace Bio.Padena.Tests
{
    /// <summary>
    /// Contains test inputs for steps in PaDeNA algorithm. 
    /// </summary>
    public static class TestInputs
    {
        /// <summary>
        /// Sequence Reads for unit tests
        /// </summary>
        /// <returns>List of sequences</returns>
        public static List<ISequence> GetTinyReads()
        {
            var reads = new List<string>
            {
                "ATGCC",
                "TCCTA",
                "CCTATC",
                "TGCCTCC",
                "CCTCCT"
            };
            return new List<ISequence>(reads.Select(r => new Sequence(Alphabets.DNA, r.Select(a => (byte)a).ToArray())));
        }

        /// <summary>
        /// Sequence Reads for unit tests
        /// </summary>
        /// <returns>List of sequences</returns>
        public static List<ISequence> GetSmallReads()
        {
            // Sequence to assemble: GATGCCTCCTATCGATCGTCGATGC
            var reads = new List<string>
            {
                "GATGCCTCCTAT",
                "CCTCCTATCGA",
                "TCCTATCGATCGT",
                "ATCGTCGATGC",
                "TCCTATCGATCGTC",
                "TGCCTCCTATCGA",
                "TATCGATCGTCGA",
                "TCGATCGTCGATGC",
                "GCCTCCTATCGA"
            };
            return new List<ISequence>(reads.Select(r => new Sequence(Alphabets.DNA, r.Select(a => (byte)a).ToArray())));
        }

        /// <summary>
        /// Sequence reads for testing dangling links purger
        /// </summary>
        /// <returns>List of sequences</returns>
        public static List<ISequence> GetDanglingReads()
        {
            // Sequence to assemble: ATCGCTAGCATCGAACGATCATTA
            var reads = new List<string>
            {
                "ATCGCTAGCATCG",
                "CTAGCATCGAAC",
                "CATCGAACGATCATT",
                "GCTAGCATCGAAC",
                "CGCTAGCATCGAA",
                "ATCGAACGATGA", // ATCGAACGATCA: SNP introduced to create dangling link
                "CTAGCATCGAACGATC",
                "ATCGCTAGCATCGAA",
                "GCTAGCATCGAACGAT",
                "AGCATCGAACGATCAT"
            };
            return new List<ISequence>(reads.Select(r => (ISequence)new Sequence(Alphabets.DNA, r.Select(a => (byte)a).ToArray())));
        }

        /// <summary>
        /// Sequence reads for testing redundant paths purger
        /// </summary>
        /// <returns>List of sequence</returns>
        public static List<ISequence> GetRedundantPathReads()
        {
            // Sequence to assemble: ATGCCTCCTATCTTAGCGATGCGGTGT
            var reads = new List<string>
            {
                "ATGCCTCCTAT",
                "CCTCCTATCTT",
                "TCCTATCTT",
                "TGCCTCCTATC",
                "GCCTCCTATCTT",
                "CTTAGCGATG",
                "CTATCTTAGCGAT",
                "CTATCTTAGC",
                "GCCTCGTATCT", // GCCTCCTATCT: SNP introduced to create bubble
                "AGCGATGCGGTGT",
                "TATCTTAGCGATGC",
                "ATCTTAGCGATGC",
                "TTAGCGATGCGG"
            };
            return new List<ISequence>(reads.Select(r => (ISequence)new Sequence(Alphabets.DNA, r.Select(a => (byte)a).ToArray())));
        }

        /// <summary>
        /// Gets reads required for scaffolds.
        /// </summary>
        public static List<ISequence> GetReadsForScaffolds()
        {
            var sequences = new List<ISequence>();
            var seq = new Sequence(Alphabets.DNA, "ATGCCTC".Select(a => (byte)a).ToArray());
            seq.ID = ">10.x1:abc";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "CCTCCTAT".Select(a => (byte)a).ToArray());
            seq.ID = "1";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TCCTATC".Select(a => (byte)a).ToArray());
            seq.ID = "2";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TGCCTCCT".Select(a => (byte)a).ToArray());
            seq.ID = "3";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "ATCTTAGC".Select(a => (byte)a).ToArray());
            seq.ID = "4";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "CTATCTTAG".Select(a => (byte)a).ToArray());
            seq.ID = "5";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "CTTAGCG".Select(a => (byte)a).ToArray());
            seq.ID = "6";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "GCCTCCTAT".Select(a => (byte)a).ToArray());
            seq.ID = ">8.x1:abc";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TAGCGCGCTA".Select(a => (byte)a).ToArray());
            seq.ID = ">8.y1:abc";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "AGCGCGC".Select(a => (byte)a).ToArray());
            seq.ID = ">9.x1:abc";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TTTTTT".Select(a => (byte)a).ToArray());
            seq.ID = "7";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TTTTTAAA".Select(a => (byte)a).ToArray());
            seq.ID = "8";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TAAAAA".Select(a => (byte)a).ToArray());
            seq.ID = "9";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TTTTAG".Select(a => (byte)a).ToArray());
            seq.ID = "10";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TTTAGC".Select(a => (byte)a).ToArray());
            seq.ID = "11";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "GCGCGCCGCGCG".Select(a => (byte)a).ToArray());
            seq.ID = "12";
            sequences.Add(seq);
            return sequences;
        }
    }
}
