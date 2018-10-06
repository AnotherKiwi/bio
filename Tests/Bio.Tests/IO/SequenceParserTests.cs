using System.IO;
using Bio.IO;
using Bio.IO.FastA;
using Bio.IO.FastQ;
using Bio.IO.GenBank;
using Bio.IO.Gff;
using Bio.IO.SFF;
using Bio.IO.Text;
using NUnit.Framework;

namespace Bio.Tests.IO
{
    /// <summary>
    /// Unit tests for the SequenceParser class
    /// </summary>
    [TestFixture]
    public class SequenceParserTests
    {
        /// <summary>
        /// Tests a normal .FASTA file extension.
        /// </summary>
        [Test]
        public void TestFastAFileExtension()
        {
            string[] extensions = {".fa", ".fas", ".fasta", ".fna", ".fsa", ".mpfa"};
            var filepath = @"TestUtils\Simple_Fasta_DNA";

            foreach (var ext in extensions)
            {
                var foundParser = SequenceParsers.FindParserByFileName(filepath + ext);
                Assert.IsNotNull(foundParser);
                Assert.IsInstanceOf<FastAParser>(foundParser);
            }
        }

        /// <summary>
        /// Tests a normal .fastq file extension.
        /// </summary>
        [Test]
        public void TestFastQFileExtension()
        {
            string[] extensions = { ".fq", ".fastq" };
            var filepath = @"TestUtils\SimpleDnaIllumina";

            foreach (var ext in extensions)
            {
                var foundParser = SequenceParsers.FindParserByFileName(filepath + ext);
                Assert.IsNotNull(foundParser);
                Assert.AreEqual(SequenceParsers.FastQ.Name, foundParser.Name);
            }
        }

        /// <summary>
        /// Tests a normal .genbank file extension.
        /// </summary>
        [Test]
        public void TestGenBankFileExtension()
        {
            string[] extensions = { ".gbk", ".genbank" };
            var filepath = @"TestUtils\Simple_GenBank_DNA";

            foreach (var ext in extensions)
            {
                var foundParser = SequenceParsers.FindParserByFileName(filepath + ext);
                Assert.IsNotNull(foundParser);
                Assert.IsInstanceOf<GenBankParser>(foundParser);
            }
        }

        /// <summary>
        /// Tests a normal .gff file extension.
        /// </summary>
        [Test]
        public void TestGffFileExtension()
        {
            var filepath = @"TestUtils\Simple_Gff_Dna.gff";

            var foundParser = SequenceParsers.FindParserByFileName(filepath);
            Assert.IsNotNull(foundParser);
            Assert.IsInstanceOf<GffParser>(foundParser);
        }

        /// <summary>
        /// Test an unknown parser type
        /// </summary>
        [Test]
        public void TestUnknownFileExtension()
        {
            var filepath = @"Test.ukn";

            var foundParser = SequenceParsers.FindParserByFileName(filepath);
            Assert.IsNull(foundParser);
        }

        /// <summary>
        /// Identify parser when no file exists.
        /// </summary>
        [Test]
        public void TestMissingFile()
        {
            var filepath = @"TestUtils\NoFileHere.fa";
            var foundParser = SequenceParsers.FindParserByFileName(filepath);
            Assert.AreEqual(SequenceParsers.Fasta, foundParser);
        }

        /// <summary>
        /// Identify parser when part of path does not exist.
        /// </summary>
        [Test]
        public void TestMissingDirectory()
        {
            var filepath = @"NoDirectoryHere\NoFileHere.fa";
            var foundParser = SequenceParsers.FindParserByFileName(filepath);
            Assert.AreEqual(SequenceParsers.Fasta, foundParser);
        }

        /// <summary>
        /// Tests the .txt extension
        /// </summary>
        [Test]
        public void TestTxtFileExtension()
        {
            var filepath = @"TestUtils\BLOSUM50.txt";

            var foundParser = SequenceParsers.FindParserByFileName(filepath);
            Assert.IsNull(foundParser);
            // Should not auto-locate FieldTextParser.
        }

        /// <summary>
        /// Tests the .sff extension from Bio.IO.dll
        /// </summary>
        [Test]
        public void TestSffFileExtension()
        {
            var filepath = @"TestUtils\dummy.sff";

            var foundParser = SequenceParsers.FindParserByFileName(filepath);
            Assert.IsNotNull(foundParser);
            Assert.IsInstanceOf<SFFParser>(foundParser);
        }
    }
}
