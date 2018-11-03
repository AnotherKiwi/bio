﻿using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Bio.Algorithms.Alignment;
using Bio.IO;
using Bio.IO.Phylip;
using NUnit.Framework;

namespace Bio.Tests.IO.Phylip
{
    /// <summary>
    /// Phylip format parser.
    /// </summary>
    [TestFixture]
    public class PhylipTests
    {
        /// <summary>
        /// Parse sample FASTA file 186972391.fasta and verify that it is read correctly.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void PhylipParse()
        {
            string filepath = @"TestUtils\Phylip\dna.phy".TestDir();
            Assert.IsTrue(File.Exists(filepath));

            IList<Dictionary<string, string>> expectedOutput = new List<Dictionary<string, string>>();

            Dictionary<string, string> expectedAlignment = new Dictionary<string, string>
            {
                ["Cow"] = "ATGGCATATCCCATACAACTAGGATTCCAAGATGCAACATCACCAATCATAGAAGAACTA"
                    + "CTTCACTTTCATGACCACACGCTAATAATTGTCTTCTTAATTAGCTCATTAGTACTTTAC"
                    + "ATTATTTCACTAATACTAACGACAAAGCTGACCCATACAAGCACGATAGATGCACAAGAA"
                    + "GTAGAGACAATCTGAACCATTCTGCCCGCCATCATCTTAATTCTAATTGCTCTTCCTTCT"
                    + "TTACGAATTCTATACATAATAGATGAAATCAATAACCCATCTCTTACAGTAAAAACCATA"
                    + "GGACATCAGTGATACTGAAGCTATGAGTATACAGATTATGAGGACTTAAGCTTCGACTCC"
                    + "TACATAATTCCAACATCAGAATTAAAGCCAGGGGAGCTACGACTATTAGAAGTCGATAAT"
                    + "CGAGTTGTACTACCAATAGAAATAACAATCCGAATGTTAGTCTCCTCTGAAGACGTATTA"
                    + "CACTCATGAGCTGTGCCCTCTCTAGGACTAAAAACAGACGCAATCCCAGGCCGTCTAAAC"
                    + "CAAACAACCCTTATATCGTCCCGTCCAGGCTTATATTACGGTCAATGCTCAGAAATTTGC"
                    + "GGGTCAAACCACAGTTTCATACCCATTGTCCTTGAGTTAGTCCCACTAAAGTACTTTGAA"
                    + "AAATGATCTGCGTCAATATTA---------------------TAA",

                ["Carp"] = "ATGGCACACCCAACGCAACTAGGTTTCAAGGACGCGGCCATACCCGTTATAGAGGAACTT"
                    + "CTTCACTTCCACGACCACGCATTAATAATTGTGCTCCTAATTAGCACTTTAGTTTTATAT"
                    + "ATTATTACTGCAATGGTATCAACTAAACTTACTAATAAATATATTCTAGACTCCCAAGAA"
                    + "ATCGAAATCGTATGAACCATTCTACCAGCCGTCATTTTAGTACTAATCGCCCTGCCCTCC"
                    + "CTACGCATCCTGTACCTTATAGACGAAATTAACGACCCTCACCTGACAATTAAAGCAATA"
                    + "GGACACCAATGATACTGAAGTTACGAGTATACAGACTATGAAAATCTAGGATTCGACTCC"
                    + "TATATAGTACCAACCCAAGACCTTGCCCCCGGACAATTCCGACTTCTGGAAACAGACCAC"
                    + "CGAATAGTTGTTCCAATAGAATCCCCAGTCCGTGTCCTAGTATCTGCTGAAGACGTGCTA"
                    + "CATTCTTGAGCTGTTCCATCCCTTGGCGTAAAAATGGACGCAGTCCCAGGACGACTAAAT"
                    + "CAAGCCGCCTTTATTGCCTCACGCCCAGGGGTCTTTTACGGACAATGCTCTGAAATTTGT"
                    + "GGAGCTAATCACAGCTTTATACCAATTGTAGTTGAAGCAGTACCTCTCGAACACTTCGAA"
                    + "AACTGATCCTCATTAATACTAGAAGACGCCTCGCTAGGAAGCTAA",

                ["Chicken"] = "ATGGCCAACCACTCCCAACTAGGCTTTCAAGACGCCTCATCCCCCATCATAGAAGAGCTC"
                    + "GTTGAATTCCACGACCACGCCCTGATAGTCGCACTAGCAATTTGCAGCTTAGTACTCTAC"
                    + "CTTCTAACTCTTATACTTATAGAAAAACTATCA---TCAAACACCGTAGATGCCCAAGAA"
                    + "GTTGAACTAATCTGAACCATCCTACCCGCTATTGTCCTAGTCCTGCTTGCCCTCCCCTCC"
                    + "CTCCAAATCCTCTACATAATAGACGAAATCGACGAACCTGATCTCACCCTAAAAGCCATC"
                    + "GGACACCAATGATACTGAACCTATGAATACACAGACTTCAAGGACCTCTCATTTGACTCC"
                    + "TACATAACCCCAACAACAGACCTCCCCCTAGGCCACTTCCGCCTACTAGAAGTCGACCAT"
                    + "CGCATTGTAATCCCCATAGAATCCCCCATTCGAGTAATCATCACCGCTGATGACGTCCTC"
                    + "CACTCATGAGCCGTACCCGCCCTCGGGGTAAAAACAGACGCAATCCCTGGACGACTAAAT"
                    + "CAAACCTCCTTCATCACCACTCGACCAGGAGTGTTTTACGGACAATGCTCAGAAATCTGC"
                    + "GGAGCTAACCACAGCTACATACCCATTGTAGTAGAGTCTACCCCCCTAAAACACTTTGAA"
                    + "GCCTGATCCTCACTA------------------CTGTCATCTTAA",

                ["Human"] = "ATGGCACATGCAGCGCAAGTAGGTCTACAAGACGCTACTTCCCCTATCATAGAAGAGCTT"
                    + "ATCACCTTTCATGATCACGCCCTCATAATCATTTTCCTTATCTGCTTCCTAGTCCTGTAT"
                    + "GCCCTTTTCCTAACACTCACAACAAAACTAACTAATACTAACATCTCAGACGCTCAGGAA"
                    + "ATAGAAACCGTCTGAACTATCCTGCCCGCCATCATCCTAGTCCTCATCGCCCTCCCATCC"
                    + "CTACGCATCCTTTACATAACAGACGAGGTCAACGATCCCTCCCTTACCATCAAATCAATT"
                    + "GGCCACCAATGGTACTGAACCTACGAGTACACCGACTACGGCGGACTAATCTTCAACTCC"
                    + "TACATACTTCCCCCATTATTCCTAGAACCAGGCGACCTGCGACTCCTTGACGTTGACAAT"
                    + "CGAGTAGTACTCCCGATTGAAGCCCCCATTCGTATAATAATTACATCACAAGACGTCTTG"
                    + "CACTCATGAGCTGTCCCCACATTAGGCTTAAAAACAGATGCAATTCCCGGACGTCTAAAC"
                    + "CAAACCACTTTCACCGCTACACGACCGGGGGTATACTACGGTCAATGCTCTGAAATCTGT"
                    + "GGAGCAAACCACAGTTTCATGCCCATCGTCCTAGAATTAATTCCCCTAAAAATCTTTGAA"
                    + "ATA---------------------GGGCCCGTATTTACCCTATAG",

                ["Loach"] = "ATGGCACATCCCACACAATTAGGATTCCAAGACGCGGCCTCACCCGTAATAGAAGAACTT"
                    + "CTTCACTTCCATGACCATGCCCTAATAATTGTATTTTTGATTAGCGCCCTAGTACTTTAT"
                    + "GTTATTATTACAACCGTCTCAACAAAACTCACTAACATATATATTTTGGACTCACAAGAA"
                    + "ATTGAAATCGTATGAACTGTGCTCCCTGCCCTAATCCTCATTTTAATCGCCCTCCCCTCA"
                    + "CTACGAATTCTATATCTTATAGACGAGATTAATGACCCCCACCTAACAATTAAGGCCATG"
                    + "GGGCACCAATGATACTGAAGCTACGAGTATACTGATTATGAAAACTTAAGTTTTGACTCC"
                    + "TACATAATCCCCACCCAGGACCTAACCCCTGGACAATTCCGGCTACTAGAGACAGACCAC"
                    + "CGAATGGTTGTTCCCATAGAATCCCCTATTCGCATTCTTGTTTCCGCCGAAGATGTACTA"
                    + "CACTCCTGGGCCCTTCCAGCCATGGGGGTAAAGATAGACGCGGTCCCAGGACGCCTTAAC"
                    + "CAAACCGCCTTTATTGCCTCCCGCCCCGGGGTATTCTATGGGCAATGCTCAGAAATCTGT"
                    + "GGAGCAAACCACAGCTTTATACCCATCGTAGTAGAAGCGGTCCCACTATCTCACTTCGAA"
                    + "AACTGGTCCACCCTTATACTAAAAGACGCCTCACTAGGAAGCTAA",

                ["Mouse"] = "ATGGCCTACCCATTCCAACTTGGTCTACAAGACGCCACATCCCCTATTATAGAAGAGCTA"
                    + "ATAAATTTCCATGATCACACACTAATAATTGTTTTCCTAATTAGCTCCTTAGTCCTCTAT"
                    + "ATCATCTCGCTAATATTAACAACAAAACTAACACATACAAGCACAATAGATGCACAAGAA"
                    + "GTTGAAACCATTTGAACTATTCTACCAGCTGTAATCCTTATCATAATTGCTCTCCCCTCT"
                    + "CTACGCATTCTATATATAATAGACGAAATCAACAACCCCGTATTAACCGTTAAAACCATA"
                    + "GGGCACCAATGATACTGAAGCTACGAATATACTGACTATGAAGACCTATGCTTTGATTCA"
                    + "TATATAATCCCAACAAACGACCTAAAACCTGGTGAACTACGACTGCTAGAAGTTGATAAC"
                    + "CGAGTCGTTCTGCCAATAGAACTTCCAATCCGTATATTAATTTCATCTGAAGACGTCCTC"
                    + "CACTCATGAGCAGTCCCCTCCCTAGGACTTAAAACTGATGCCATCCCAGGCCGACTAAAT"
                    + "CAAGCAACAGTAACATCAAACCGACCAGGGTTATTCTATGGCCAATGCTCTGAAATTTGT"
                    + "GGATCTAACCATAGCTTTATGCCCATTGTCCTAGAAATGGTTCCACTAAAATATTTCGAA"
                    + "AACTGATCTGCTTCAATAATT---------------------TAA",

                ["Rat"] = "ATGGCTTACCCATTTCAACTTGGCTTACAAGACGCTACATCACCTATCATAGAAGAACTT"
                    + "ACAAACTTTCATGACCACACCCTAATAATTGTATTCCTCATCAGCTCCCTAGTACTTTAT"
                    + "ATTATTTCACTAATACTAACAACAAAACTAACACACACAAGCACAATAGACGCCCAAGAA"
                    + "GTAGAAACAATTTGAACAATTCTCCCAGCTGTCATTCTTATTCTAATTGCCCTTCCCTCC"
                    + "CTACGAATTCTATACATAATAGACGAGATTAATAACCCAGTTCTAACAGTAAAAACTATA"
                    + "GGACACCAATGATACTGAAGCTATGAATATACTGACTATGAAGACCTATGCTTTGACTCC"
                    + "TACATAATCCCAACCAATGACCTAAAACCAGGTGAACTTCGTCTATTAGAAGTTGATAAT"
                    + "CGGGTAGTCTTACCAATAGAACTTCCAATTCGTATACTAATCTCATCCGAAGACGTCCTG"
                    + "CACTCATGAGCCATCCCTTCACTAGGGTTAAAAACCGACGCAATCCCCGGCCGCCTAAAC"
                    + "CAAGCTACAGTCACATCAAACCGACCAGGTCTATTCTATGGCCAATGCTCTGAAATTTGC"
                    + "GGCTCAAATCACAGCTTCATACCCATTGTACTAGAAATAGTGCCTCTAAAATATTTCGAA"
                    + "AACTGATCAGCTTCTATAATT---------------------TAA",

                ["Seal"] = "ATGGCATACCCCCTACAAATAGGCCTACAAGATGCAACCTCTCCCATTATAGAGGAGTTA"
                    + "CTACACTTCCATGACCACACATTAATAATTGTGTTCCTAATTAGCTCATTAGTACTCTAC"
                    + "ATTATCTCACTTATACTAACCACGAAACTCACCCACACAAGTACAATAGACGCACAAGAA"
                    + "GTGGAAACGGTGTGAACGATCCTACCCGCTATCATTTTAATTCTCATTGCCCTACCATCA"
                    + "TTACGAATCCTCTACATAATGGACGAGATCAATAACCCTTCCTTGACCGTAAAAACTATA"
                    + "GGACATCAGTGATACTGAAGCTATGAGTACACAGACTACGAAGACCTGAACTTTGACTCA"
                    + "TATATGATCCCCACACAAGAACTAAAGCCCGGAGAACTACGACTGCTAGAAGTAGACAAT"
                    + "CGAGTAGTCCTCCCAATAGAAATAACAATCCGCATACTAATCTCATCAGAAGATGTACTC"
                    + "CACTCATGAGCCGTACCGTCCCTAGGACTAAAAACTGATGCTATCCCAGGACGACTAAAC"
                    + "CAAACAACCCTAATAACCATACGACCAGGACTGTACTACGGTCAATGCTCAGAAATCTGT"
                    + "GGTTCAAACCACAGCTTCATACCTATTGTCCTCGAATTGGTCCCACTATCCCACTTCGAG"
                    + "AAATGATCTACCTCAATGCTT---------------------TAA",

                ["Whale"] = "ATGGCATATCCATTCCAACTAGGTTTCCAAGATGCAGCATCACCCATCATAGAAGAGCTC"
                    + "CTACACTTTCACGATCATACACTAATAATCGTTTTTCTAATTAGCTCTTTAGTTCTCTAC"
                    + "ATTATTACCCTAATGCTTACAACCAAATTAACACATACTAGTACAATAGACGCCCAAGAA"
                    + "GTAGAAACTGTCTGAACTATCCTCCCAGCCATTATCTTAATTTTAATTGCCTTGCCTTCA"
                    + "TTACGGATCCTTTACATAATAGACGAAGTCAATAACCCCTCCCTCACTGTAAAAACAATA"
                    + "GGTCACCAATGATATTGAAGCTATGAGTATACCGACTACGAAGACCTAAGCTTCGACTCC"
                    + "TATATAATCCCAACATCAGACCTAAAGCCAGGAGAACTACGATTATTAGAAGTAGATAAC"
                    + "CGAGTTGTCTTACCTATAGAAATAACAATCCGAATATTAGTCTCATCAGAAGACGTACTC"
                    + "CACTCATGGGCCGTACCCTCCTTGGGCCTAAAAACAGATGCAATCCCAGGACGCCTAAAC"
                    + "CAAACAACCTTAATATCAACACGACCAGGCCTATTTTATGGACAATGCTCAGAGATCTGC"
                    + "GGCTCAAACCACAGTTTCATACCAATTGTCCTAGAACTAGTACCCCTAGAAGTCTTTGAA"
                    + "AAATGATCTGTATCAATACTA---------------------TAA",

                ["Frog"] = "ATGGCACACCCATCACAATTAGGTTTTCAAGACGCAGCCTCTCCAATTATAGAAGAATTA"
                    + "CTTCACTTCCACGACCATACCCTCATAGCCGTTTTTCTTATTAGTACGCTAGTTCTTTAC"
                    + "ATTATTACTATTATAATAACTACTAAACTAACTAATACAAACCTAATGGACGCACAAGAG"
                    + "ATCGAAATAGTGTGAACTATTATACCAGCTATTAGCCTCATCATAATTGCCCTTCCATCC"
                    + "CTTCGTATCCTATATTTAATAGATGAAGTTAATGATCCACACTTAACAATTAAAGCAATC"
                    + "GGCCACCAATGATACTGAAGCTACGAATATACTAACTATGAGGATCTCTCATTTGACTCT"
                    + "TATATAATTCCAACTAATGACCTTACCCCTGGACAATTCCGGCTGCTAGAAGTTGATAAT"
                    + "CGAATAGTAGTCCCAATAGAATCTCCAACCCGACTTTTAGTTACAGCCGAAGACGTCCTC"
                    + "CACTCGTGAGCTGTACCCTCCTTGGGTGTCAAAACAGATGCAATCCCAGGACGACTTCAT"
                    + "CAAACATCATTTATTGCTACTCGTCCGGGAGTATTTTACGGACAATGTTCAGAAATTTGC"
                    + "GGAGCAAACCACAGCTTTATACCAATTGTAGTTGAAGCAGTACCGCTAACCGACTTTGAA"
                    + "AACTGATCTTCATCAATACTA---GAAGCATCACTA------AGA"
            };

            expectedOutput.Add(expectedAlignment);

            IList<ISequenceAlignment> actualOutput = null;
            ISequenceAlignmentParser parser = new PhylipParser();

            using (parser.Open(filepath))
            {
                actualOutput = parser.Parse().ToList();
            }

            CompareOutput(actualOutput, expectedOutput);
        }

        /// <summary>
        /// Parse sample Phylip file dna.phy and verify that it is read correctly.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void PhylipParseOne()
        {
            string filepath = @"TestUtils\Phylip\dna.phy".TestDir();
            Assert.IsTrue(File.Exists(filepath));

            IList<Dictionary<string, string>> expectedOutput = new List<Dictionary<string, string>>();

            Dictionary<string, string> expectedAlignment = new Dictionary<string, string>
            {
                ["Cow"] = "ATGGCATATCCCATACAACTAGGATTCCAAGATGCAACATCACCAATCATAGAAGAACTA"
                    + "CTTCACTTTCATGACCACACGCTAATAATTGTCTTCTTAATTAGCTCATTAGTACTTTAC"
                    + "ATTATTTCACTAATACTAACGACAAAGCTGACCCATACAAGCACGATAGATGCACAAGAA"
                    + "GTAGAGACAATCTGAACCATTCTGCCCGCCATCATCTTAATTCTAATTGCTCTTCCTTCT"
                    + "TTACGAATTCTATACATAATAGATGAAATCAATAACCCATCTCTTACAGTAAAAACCATA"
                    + "GGACATCAGTGATACTGAAGCTATGAGTATACAGATTATGAGGACTTAAGCTTCGACTCC"
                    + "TACATAATTCCAACATCAGAATTAAAGCCAGGGGAGCTACGACTATTAGAAGTCGATAAT"
                    + "CGAGTTGTACTACCAATAGAAATAACAATCCGAATGTTAGTCTCCTCTGAAGACGTATTA"
                    + "CACTCATGAGCTGTGCCCTCTCTAGGACTAAAAACAGACGCAATCCCAGGCCGTCTAAAC"
                    + "CAAACAACCCTTATATCGTCCCGTCCAGGCTTATATTACGGTCAATGCTCAGAAATTTGC"
                    + "GGGTCAAACCACAGTTTCATACCCATTGTCCTTGAGTTAGTCCCACTAAAGTACTTTGAA"
                    + "AAATGATCTGCGTCAATATTA---------------------TAA",

                ["Carp"] = "ATGGCACACCCAACGCAACTAGGTTTCAAGGACGCGGCCATACCCGTTATAGAGGAACTT"
                    + "CTTCACTTCCACGACCACGCATTAATAATTGTGCTCCTAATTAGCACTTTAGTTTTATAT"
                    + "ATTATTACTGCAATGGTATCAACTAAACTTACTAATAAATATATTCTAGACTCCCAAGAA"
                    + "ATCGAAATCGTATGAACCATTCTACCAGCCGTCATTTTAGTACTAATCGCCCTGCCCTCC"
                    + "CTACGCATCCTGTACCTTATAGACGAAATTAACGACCCTCACCTGACAATTAAAGCAATA"
                    + "GGACACCAATGATACTGAAGTTACGAGTATACAGACTATGAAAATCTAGGATTCGACTCC"
                    + "TATATAGTACCAACCCAAGACCTTGCCCCCGGACAATTCCGACTTCTGGAAACAGACCAC"
                    + "CGAATAGTTGTTCCAATAGAATCCCCAGTCCGTGTCCTAGTATCTGCTGAAGACGTGCTA"
                    + "CATTCTTGAGCTGTTCCATCCCTTGGCGTAAAAATGGACGCAGTCCCAGGACGACTAAAT"
                    + "CAAGCCGCCTTTATTGCCTCACGCCCAGGGGTCTTTTACGGACAATGCTCTGAAATTTGT"
                    + "GGAGCTAATCACAGCTTTATACCAATTGTAGTTGAAGCAGTACCTCTCGAACACTTCGAA"
                    + "AACTGATCCTCATTAATACTAGAAGACGCCTCGCTAGGAAGCTAA",

                ["Chicken"] = "ATGGCCAACCACTCCCAACTAGGCTTTCAAGACGCCTCATCCCCCATCATAGAAGAGCTC"
                    + "GTTGAATTCCACGACCACGCCCTGATAGTCGCACTAGCAATTTGCAGCTTAGTACTCTAC"
                    + "CTTCTAACTCTTATACTTATAGAAAAACTATCA---TCAAACACCGTAGATGCCCAAGAA"
                    + "GTTGAACTAATCTGAACCATCCTACCCGCTATTGTCCTAGTCCTGCTTGCCCTCCCCTCC"
                    + "CTCCAAATCCTCTACATAATAGACGAAATCGACGAACCTGATCTCACCCTAAAAGCCATC"
                    + "GGACACCAATGATACTGAACCTATGAATACACAGACTTCAAGGACCTCTCATTTGACTCC"
                    + "TACATAACCCCAACAACAGACCTCCCCCTAGGCCACTTCCGCCTACTAGAAGTCGACCAT"
                    + "CGCATTGTAATCCCCATAGAATCCCCCATTCGAGTAATCATCACCGCTGATGACGTCCTC"
                    + "CACTCATGAGCCGTACCCGCCCTCGGGGTAAAAACAGACGCAATCCCTGGACGACTAAAT"
                    + "CAAACCTCCTTCATCACCACTCGACCAGGAGTGTTTTACGGACAATGCTCAGAAATCTGC"
                    + "GGAGCTAACCACAGCTACATACCCATTGTAGTAGAGTCTACCCCCCTAAAACACTTTGAA"
                    + "GCCTGATCCTCACTA------------------CTGTCATCTTAA",

                ["Human"] = "ATGGCACATGCAGCGCAAGTAGGTCTACAAGACGCTACTTCCCCTATCATAGAAGAGCTT"
                    + "ATCACCTTTCATGATCACGCCCTCATAATCATTTTCCTTATCTGCTTCCTAGTCCTGTAT"
                    + "GCCCTTTTCCTAACACTCACAACAAAACTAACTAATACTAACATCTCAGACGCTCAGGAA"
                    + "ATAGAAACCGTCTGAACTATCCTGCCCGCCATCATCCTAGTCCTCATCGCCCTCCCATCC"
                    + "CTACGCATCCTTTACATAACAGACGAGGTCAACGATCCCTCCCTTACCATCAAATCAATT"
                    + "GGCCACCAATGGTACTGAACCTACGAGTACACCGACTACGGCGGACTAATCTTCAACTCC"
                    + "TACATACTTCCCCCATTATTCCTAGAACCAGGCGACCTGCGACTCCTTGACGTTGACAAT"
                    + "CGAGTAGTACTCCCGATTGAAGCCCCCATTCGTATAATAATTACATCACAAGACGTCTTG"
                    + "CACTCATGAGCTGTCCCCACATTAGGCTTAAAAACAGATGCAATTCCCGGACGTCTAAAC"
                    + "CAAACCACTTTCACCGCTACACGACCGGGGGTATACTACGGTCAATGCTCTGAAATCTGT"
                    + "GGAGCAAACCACAGTTTCATGCCCATCGTCCTAGAATTAATTCCCCTAAAAATCTTTGAA"
                    + "ATA---------------------GGGCCCGTATTTACCCTATAG",

                ["Loach"] = "ATGGCACATCCCACACAATTAGGATTCCAAGACGCGGCCTCACCCGTAATAGAAGAACTT"
                    + "CTTCACTTCCATGACCATGCCCTAATAATTGTATTTTTGATTAGCGCCCTAGTACTTTAT"
                    + "GTTATTATTACAACCGTCTCAACAAAACTCACTAACATATATATTTTGGACTCACAAGAA"
                    + "ATTGAAATCGTATGAACTGTGCTCCCTGCCCTAATCCTCATTTTAATCGCCCTCCCCTCA"
                    + "CTACGAATTCTATATCTTATAGACGAGATTAATGACCCCCACCTAACAATTAAGGCCATG"
                    + "GGGCACCAATGATACTGAAGCTACGAGTATACTGATTATGAAAACTTAAGTTTTGACTCC"
                    + "TACATAATCCCCACCCAGGACCTAACCCCTGGACAATTCCGGCTACTAGAGACAGACCAC"
                    + "CGAATGGTTGTTCCCATAGAATCCCCTATTCGCATTCTTGTTTCCGCCGAAGATGTACTA"
                    + "CACTCCTGGGCCCTTCCAGCCATGGGGGTAAAGATAGACGCGGTCCCAGGACGCCTTAAC"
                    + "CAAACCGCCTTTATTGCCTCCCGCCCCGGGGTATTCTATGGGCAATGCTCAGAAATCTGT"
                    + "GGAGCAAACCACAGCTTTATACCCATCGTAGTAGAAGCGGTCCCACTATCTCACTTCGAA"
                    + "AACTGGTCCACCCTTATACTAAAAGACGCCTCACTAGGAAGCTAA",

                ["Mouse"] = "ATGGCCTACCCATTCCAACTTGGTCTACAAGACGCCACATCCCCTATTATAGAAGAGCTA"
                    + "ATAAATTTCCATGATCACACACTAATAATTGTTTTCCTAATTAGCTCCTTAGTCCTCTAT"
                    + "ATCATCTCGCTAATATTAACAACAAAACTAACACATACAAGCACAATAGATGCACAAGAA"
                    + "GTTGAAACCATTTGAACTATTCTACCAGCTGTAATCCTTATCATAATTGCTCTCCCCTCT"
                    + "CTACGCATTCTATATATAATAGACGAAATCAACAACCCCGTATTAACCGTTAAAACCATA"
                    + "GGGCACCAATGATACTGAAGCTACGAATATACTGACTATGAAGACCTATGCTTTGATTCA"
                    + "TATATAATCCCAACAAACGACCTAAAACCTGGTGAACTACGACTGCTAGAAGTTGATAAC"
                    + "CGAGTCGTTCTGCCAATAGAACTTCCAATCCGTATATTAATTTCATCTGAAGACGTCCTC"
                    + "CACTCATGAGCAGTCCCCTCCCTAGGACTTAAAACTGATGCCATCCCAGGCCGACTAAAT"
                    + "CAAGCAACAGTAACATCAAACCGACCAGGGTTATTCTATGGCCAATGCTCTGAAATTTGT"
                    + "GGATCTAACCATAGCTTTATGCCCATTGTCCTAGAAATGGTTCCACTAAAATATTTCGAA"
                    + "AACTGATCTGCTTCAATAATT---------------------TAA",

                ["Rat"] = "ATGGCTTACCCATTTCAACTTGGCTTACAAGACGCTACATCACCTATCATAGAAGAACTT"
                    + "ACAAACTTTCATGACCACACCCTAATAATTGTATTCCTCATCAGCTCCCTAGTACTTTAT"
                    + "ATTATTTCACTAATACTAACAACAAAACTAACACACACAAGCACAATAGACGCCCAAGAA"
                    + "GTAGAAACAATTTGAACAATTCTCCCAGCTGTCATTCTTATTCTAATTGCCCTTCCCTCC"
                    + "CTACGAATTCTATACATAATAGACGAGATTAATAACCCAGTTCTAACAGTAAAAACTATA"
                    + "GGACACCAATGATACTGAAGCTATGAATATACTGACTATGAAGACCTATGCTTTGACTCC"
                    + "TACATAATCCCAACCAATGACCTAAAACCAGGTGAACTTCGTCTATTAGAAGTTGATAAT"
                    + "CGGGTAGTCTTACCAATAGAACTTCCAATTCGTATACTAATCTCATCCGAAGACGTCCTG"
                    + "CACTCATGAGCCATCCCTTCACTAGGGTTAAAAACCGACGCAATCCCCGGCCGCCTAAAC"
                    + "CAAGCTACAGTCACATCAAACCGACCAGGTCTATTCTATGGCCAATGCTCTGAAATTTGC"
                    + "GGCTCAAATCACAGCTTCATACCCATTGTACTAGAAATAGTGCCTCTAAAATATTTCGAA"
                    + "AACTGATCAGCTTCTATAATT---------------------TAA",

                ["Seal"] = "ATGGCATACCCCCTACAAATAGGCCTACAAGATGCAACCTCTCCCATTATAGAGGAGTTA"
                    + "CTACACTTCCATGACCACACATTAATAATTGTGTTCCTAATTAGCTCATTAGTACTCTAC"
                    + "ATTATCTCACTTATACTAACCACGAAACTCACCCACACAAGTACAATAGACGCACAAGAA"
                    + "GTGGAAACGGTGTGAACGATCCTACCCGCTATCATTTTAATTCTCATTGCCCTACCATCA"
                    + "TTACGAATCCTCTACATAATGGACGAGATCAATAACCCTTCCTTGACCGTAAAAACTATA"
                    + "GGACATCAGTGATACTGAAGCTATGAGTACACAGACTACGAAGACCTGAACTTTGACTCA"
                    + "TATATGATCCCCACACAAGAACTAAAGCCCGGAGAACTACGACTGCTAGAAGTAGACAAT"
                    + "CGAGTAGTCCTCCCAATAGAAATAACAATCCGCATACTAATCTCATCAGAAGATGTACTC"
                    + "CACTCATGAGCCGTACCGTCCCTAGGACTAAAAACTGATGCTATCCCAGGACGACTAAAC"
                    + "CAAACAACCCTAATAACCATACGACCAGGACTGTACTACGGTCAATGCTCAGAAATCTGT"
                    + "GGTTCAAACCACAGCTTCATACCTATTGTCCTCGAATTGGTCCCACTATCCCACTTCGAG"
                    + "AAATGATCTACCTCAATGCTT---------------------TAA",

                ["Whale"] = "ATGGCATATCCATTCCAACTAGGTTTCCAAGATGCAGCATCACCCATCATAGAAGAGCTC"
                    + "CTACACTTTCACGATCATACACTAATAATCGTTTTTCTAATTAGCTCTTTAGTTCTCTAC"
                    + "ATTATTACCCTAATGCTTACAACCAAATTAACACATACTAGTACAATAGACGCCCAAGAA"
                    + "GTAGAAACTGTCTGAACTATCCTCCCAGCCATTATCTTAATTTTAATTGCCTTGCCTTCA"
                    + "TTACGGATCCTTTACATAATAGACGAAGTCAATAACCCCTCCCTCACTGTAAAAACAATA"
                    + "GGTCACCAATGATATTGAAGCTATGAGTATACCGACTACGAAGACCTAAGCTTCGACTCC"
                    + "TATATAATCCCAACATCAGACCTAAAGCCAGGAGAACTACGATTATTAGAAGTAGATAAC"
                    + "CGAGTTGTCTTACCTATAGAAATAACAATCCGAATATTAGTCTCATCAGAAGACGTACTC"
                    + "CACTCATGGGCCGTACCCTCCTTGGGCCTAAAAACAGATGCAATCCCAGGACGCCTAAAC"
                    + "CAAACAACCTTAATATCAACACGACCAGGCCTATTTTATGGACAATGCTCAGAGATCTGC"
                    + "GGCTCAAACCACAGTTTCATACCAATTGTCCTAGAACTAGTACCCCTAGAAGTCTTTGAA"
                    + "AAATGATCTGTATCAATACTA---------------------TAA",

                ["Frog"] = "ATGGCACACCCATCACAATTAGGTTTTCAAGACGCAGCCTCTCCAATTATAGAAGAATTA"
                    + "CTTCACTTCCACGACCATACCCTCATAGCCGTTTTTCTTATTAGTACGCTAGTTCTTTAC"
                    + "ATTATTACTATTATAATAACTACTAAACTAACTAATACAAACCTAATGGACGCACAAGAG"
                    + "ATCGAAATAGTGTGAACTATTATACCAGCTATTAGCCTCATCATAATTGCCCTTCCATCC"
                    + "CTTCGTATCCTATATTTAATAGATGAAGTTAATGATCCACACTTAACAATTAAAGCAATC"
                    + "GGCCACCAATGATACTGAAGCTACGAATATACTAACTATGAGGATCTCTCATTTGACTCT"
                    + "TATATAATTCCAACTAATGACCTTACCCCTGGACAATTCCGGCTGCTAGAAGTTGATAAT"
                    + "CGAATAGTAGTCCCAATAGAATCTCCAACCCGACTTTTAGTTACAGCCGAAGACGTCCTC"
                    + "CACTCGTGAGCTGTACCCTCCTTGGGTGTCAAAACAGATGCAATCCCAGGACGACTTCAT"
                    + "CAAACATCATTTATTGCTACTCGTCCGGGAGTATTTTACGGACAATGTTCAGAAATTTGC"
                    + "GGAGCAAACCACAGCTTTATACCAATTGTAGTTGAAGCAGTACCGCTAACCGACTTTGAA"
                    + "AACTGATCTTCATCAATACTA---GAAGCATCACTA------AGA"
            };

            expectedOutput.Add(expectedAlignment);

            List<ISequenceAlignment> actualOutput = new List<ISequenceAlignment>();
            ISequenceAlignment actualAlignment = null;
            ISequenceAlignmentParser parser = new PhylipParser();

            using (parser.Open(filepath))
            {
                actualAlignment = parser.ParseOne();
            }
            actualOutput.Add(actualAlignment);
            CompareOutput(actualOutput, expectedOutput);
        }

        /// <summary>
        /// Parse sample Phylip file primates.phy and verify that it is read correctly.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void PhylipParsePrimates()
        {
            string filepath = @"TestUtils\Phylip\primates.phy".TestDir();
            Assert.IsTrue(File.Exists(filepath));

            IList<Dictionary<string, string>> expectedOutput = new List<Dictionary<string, string>>();

            Dictionary<string, string> expectedAlignment = new Dictionary<string, string>
            {
                ["Mouse"] = "ACCAAAAAAACATCCAAACACCAACCCCAGCCCTTACGCAATAGCCATACAAAGAATATT"
                    + "ATACTACTAAAAACTCAAATTAACTCTTTAATCTTTATACAACATTCCACCAACCTATCC"
                    + "ACACAAAAAAACTCATATTTATCTAAATACGAACTTCACACAACCTTAACACATAAACAT"
                    + "ACCCCAGCCCAACACCCTTCCACAAATCCTTAATATACGCACCATAAATAAC",

                ["Bovine"] = "ACCAAACCTGTCCCCACCATCTAACACCAACCCACATATACAAGCTAAACCAAAAATACC"
                    + "ATACAACCATAAATAAGACTAATCTATTAAAATAACCCATTACGATACAAAATCCCTTTC"
                    + "GTCTAGATACAAACCACAACACACAATTAATACACACCACAATTACAATACTAAACTCCC"
                    + "ATCCCACCAAATCACCCTCCATCAAATCCACAAATTACACAACCATTAACCC",

                ["Lemur"] = "ACCAAACTAACATCTAACAACTACCTCCAACTCTAAAAAAGCACTCTTACCAAACCCATC"
                    + "ACAACTCTATCAACCTAACCAAACTATCAACATGCCCTCTCCTAATTAAAAACATTGCCA"
                    + "CACTAAACCTACACACCTCATCACCATTAACGCATAACTCCTCAGTCATATCTACTACAC"
                    + "ACCCTAACAATTTATCCCTCCCATAATCCAAAAACTCCATAAACACAAATTC",

                ["Tarsier"] = "ATCTACCTTATCTCCCCCAATCAATACCAACCTAAAAACTCTACAATTAAAAACCCCACC"
                    + "GCTCAATTACTAGCAAAAATAGACATTCAACTCCTCCCATCATAACATAAAACATTCCTC"
                    + "GCTCCAATAAACACATCACAATCCCAATAACGCATATACCTAAATACATCATTTAATAAT"
                    + "AATACTCCAACTCCCATAACACAGCATACATAAACTCCATAAGTTTGAACAC",

                ["Squir Monk"] = "ACCCCAGCAACTCGTTGTGACCAACATCAATCCAAAATTAGCAAACGTACCAACAATCTC"
                    + "CCAAATTTAAAAACACATCCTACCTTTACAATTAATAACCATTGTCTAGATATACCCCTA"
                    + "AAATAAATGAATATAAACCCTCGCCGATAACATA-ACCCCTAAAATCAAGACATCCTCTC"
                    + "ACAACGCCAAACCCCCCTCTCATAACTCTACAAAATACACAATCACCAACAC",

                ["Jpn Macaq"] = "ACTCCACCTGCTCACCTCATCCACTACTACTCCTCAAGCAATACATAAACTAAAAACTTC"
                    + "TCACCTCTAATACTACACACCACTCCTGAAATCAATGCCCTCCACTAAAAAACATCACCA"
                    + "GCCCAAACAAACACCTATCTACCCCCCCGGTCCACGCCCCTAACTCCATCATTCCCCCTC"
                    + "AATACATCAAACAATTCCCCCCAATACCCACAAACTGCATAAGCAAACAGAC",

                ["Rhesus Mac"] = "ACTTCACCCGTTCACCTCATCCACTACTACTCCTCAAGCGATACATAAATCAAAAACTTC"
                    + "TCACCTCCAATACTACGCACCGCTCCTAAAATCAATGCCCCCCACCAAAAAACATCACCA"
                    + "ACCCAAACAAACACCTACCCATCCCCCCGGTTCACGCCTCAAACTCCATCATTCCCCCTC"
                    + "AATACATCAAACAATTCCCCCCAATACCCACAAACTACATAAACAAACAAAC",

                ["Crab-E.Mac"] = "ACCCCACCTACCCGCCTCGTCCGCTACTGCTTCTCAAACAATATATAGACCAACAACTTC"
                    + "TCACCTTTAACACTACATATCACTCCTGAGCTTAACACCCTCCGCTAAAAAACACCACTA"
                    + "ACCCAAACAAACACCTATCTATCCCCCCGGTCCACGCCCCAAACCCCGCTATTCCCCCCT"
                    + "AATACACCAAACAATTTTCTCCAACACCCACAAACTGTATAAACAAACAAAC",

                ["BarbMacaq"] = "ACCCTATCTATCTACCTCACCCGCCACCACCCCCCAAACAACACACAAACCAACAACTTT"
                    + "TTATCTTTAGCACCACACATCACCCCCAAAAGCAATACCCTTCACCAAAAAGCACCATCA"
                    + "AATCAAACAAACACCTATTTATTCCCCTAATTCACGTCCCAAATCCCATTATCTCTCCCC"
                    + "AACATACCAAACAATTCTCCCTAATATACACAAACCACGCAAACAAACAAAC",

                ["Gibbon"] = "ACTATACCCACCCAACTCGACCTACACCAATCCCCACATAGCACACAGACCAACAACCTC"
                    + "CCACCTTCCATACCAAGCCCCGACTTTACCGCCAACGCACCTCATCAAAACATACCTACA"
                    + "ACACAAACAAATGCCCCCCCACCCTCCTTCTTCAAGCCCACTAGACCATCCTACCTTCCT"
                    + "AGCACGCCAAGCTCTCTACCATCAAACGCACAACTTACACATACAGAACCAC",

                ["Orang"] = "ACCCCACCCGTCTACACCAGCCAACACCAACCCCCACCTACTATACCAACCAATAACCTC"
                    + "TCAACCCCTAAACCAAACACTATCCCCAAAACCAACACACTCTACCAAAATACACCCCCA"
                    + "ATTCACATCCGCACACCCCCACCCCCCCTGCCCACGTCCATCCCATCACCCTCTCCTCCC"
                    + "AACACCCTAAGCCACCTTCCTCAAAATCCAAAACCCACACAACCGAAACAAC",

                ["Gorilla"] = "ACCCCATTTATCCATAAAAACCAACACCAACCCCCATCTAACACACAAACTAATGACCCC"
                    + "CCACCCTCAAAGCCAAACACCAACCCTATAATCAATACGCCTTATCAAAACACACCCCCA"
                    + "ACATAAACCCACGCACCCCCACCCCTTCCGCCCATGCTCACCACATCATCTCTCCCCTTC"
                    + "AACACCTCAATCCACCTCCCCCCAAATACACAATTCACACAAACAATACCAC",

                ["Chimp"] = "ACCCCATCCACCCATACAAACCAACATTACCCTCCATCCAATATACAAACTAACAACCTC"
                    + "CCACTCTTCAGACCGAACACCAATCTCACAACCAACACGCCCCGTCAAAACACCCCTTCA"
                    + "GCACAAATTCATACACCCCTACCTTTCCTACCCACGTTCACCACATCATCCCCCCCTCTC"
                    + "AACATCTTGACTCGCCTCTCTCCAAACACACAATTCACGCAAACAACGCCAC",

                ["Human"] = "ACCCCACTCACCCATACAAACCAACACCACTCTCCACCTAATATACAAATTAATAACCTC"
                    + "CCACCTTCAGAACTGAACGCCAATCTCATAACCAACACACCCCATCAAAGCACCCCTCCA"
                    + "ACACAAACCCGCACACCTCCACCCCCCTCGTCTACGCTTACCACGTCATCCCTCCCTCTC"
                    + "AACACCTTAACTCACCTTCTCCCAAACGCACAATTCGCACACACAACGCCAC"
            };

            expectedOutput.Add(expectedAlignment);

            IList<ISequenceAlignment> actualOutput = null;
            ISequenceAlignmentParser parser = new PhylipParser();

            using (parser.Open(filepath))
            {
                actualOutput = parser.Parse().ToList();
            }

            CompareOutput(actualOutput, expectedOutput);
        }

        /// <summary>
        /// Compare the actual output to expected output
        /// </summary>
        /// <param name="actualOutput">Actual output</param>
        /// <param name="expectedOutput">Expected output</param>
        /// <returns></returns>
        private static bool CompareOutput(
                IList<ISequenceAlignment> actualOutput,
                IList<Dictionary<string, string>> expectedOutput)
        {
            if (actualOutput.Count != expectedOutput.Count)
            {
                return false;
            }

            int alignmentIndex = 0;
            foreach (ISequenceAlignment alignment in actualOutput)
            {
                Dictionary<string, string> expcetedAlignment = expectedOutput[alignmentIndex];

                foreach (Sequence actualSequence in alignment.AlignedSequences[0].Sequences)
                {
                    if (0 != string.Compare(new string(actualSequence.Select(a => (char)a).ToArray()),
                            expcetedAlignment[actualSequence.ID],
                            true, CultureInfo.CurrentCulture))
                    {
                        return false;
                    }
                }

                alignmentIndex++;
            }

            return true;
        }
    }
}
