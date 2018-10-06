using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bio;
using Bio.Algorithms.Alignment.MultipleSequenceAlignment;
using NUnit.Framework;

namespace Bio.Pamsam.Tests
{
    /// <summary>
    /// Test for KmerDistanceMatrixGenerator Class and KmerDistanceScoreCalculator class
    /// </summary>
    [TestFixture]
    public class KmerDistanceMatrixGeneratorTests
    {
        /// <summary>
        /// Test KmerDistanceMatrixGenerator Class and KmerDistanceScoreCalculator class
        /// </summary>
        [Test]
        public void TestKimuraDistanceMatrixGenerator()
        {
            var sequences = new List<ISequence>
            {
                new Sequence(Alphabets.DNA, "ACGTAA"),
                new Sequence(Alphabets.DNA, "GGGAATCAATCAG"),
                new Sequence(Alphabets.DNA, "GGGAATCTTATCAG"),
                new Sequence(Alphabets.DNA, "GGGACAAAATCAG")
            };

            var kmerLength = 3;

            // test kmer counting
            var kmerDistanceScoreCalculator =
                new KmerDistanceScoreCalculator(kmerLength, Alphabets.AmbiguousDNA);

            var countDictionaryA =
                KmerDistanceScoreCalculator.CalculateKmerCounting(sequences[0], kmerLength);
            var countDictionaryB =
                KmerDistanceScoreCalculator.CalculateKmerCounting(sequences[1], kmerLength);

            var expectedCountDictionaryA = new Dictionary<String, float>
            {
                { "ACG", 1 },
                { "CGT", 1 },
                { "GTA", 1 },
                { "TAA", 1 }
            };

            Assert.AreEqual(countDictionaryA["ACG"], expectedCountDictionaryA["ACG"]);
            Assert.AreEqual(countDictionaryA["CGT"], expectedCountDictionaryA["CGT"]);
            Assert.AreEqual(countDictionaryA["GTA"], expectedCountDictionaryA["GTA"]);
            Assert.AreEqual(countDictionaryA["TAA"], expectedCountDictionaryA["TAA"]);

            var expectedCountDictionaryB = new Dictionary<String, float>
            {
                { "GGG", 1 },
                { "GGA", 1 },
                { "GAA", 1 },
                { "AAT", 2 },
                { "ATC", 2 },
                { "TCA", 2 },
                { "CAA", 1 },
                { "CAG", 1 }
            };

            Assert.AreEqual(countDictionaryB["GGG"], expectedCountDictionaryB["GGG"]);
            Assert.AreEqual(countDictionaryB["GGA"], expectedCountDictionaryB["GGA"]);
            Assert.AreEqual(countDictionaryB["GAA"], expectedCountDictionaryB["GAA"]);
            Assert.AreEqual(countDictionaryB["AAT"], expectedCountDictionaryB["AAT"]);
            Assert.AreEqual(countDictionaryB["ATC"], expectedCountDictionaryB["ATC"]);
            Assert.AreEqual(countDictionaryB["TCA"], expectedCountDictionaryB["TCA"]);
            Assert.AreEqual(countDictionaryB["CAA"], expectedCountDictionaryB["CAA"]);
            Assert.AreEqual(countDictionaryB["CAG"], expectedCountDictionaryB["CAG"]);

            foreach (var pair in countDictionaryA)
            {
                foreach (var s in pair.Key)
                {
                    Console.Write(s + " ");
                }
                Console.WriteLine(pair.Value);
            }
            foreach (var pair in countDictionaryB)
            {
                foreach (var s in pair.Key)
                {
                    Console.Write(s + " ");
                }
                Console.WriteLine(pair.Value);
            }

            var distanceScore = kmerDistanceScoreCalculator.CalculateDistanceScore(countDictionaryA, countDictionaryB);
            Console.WriteLine(distanceScore);

            PAMSAMMultipleSequenceAligner.ParallelOption = new ParallelOptions { MaxDegreeOfParallelism = 2 };
            var kmerDistanceMatrixGenerator = new KmerDistanceMatrixGenerator(sequences, kmerLength, Alphabets.AmbiguousDNA);

            for (var i = 0; i < sequences.Count - 1; ++i)
            {
                for (var j = i + 1; j < sequences.Count; ++j)
                {
                    Console.WriteLine("Kmer Distance of sequence {0}, and {1} is: {2}", i, j, kmerDistanceMatrixGenerator.DistanceMatrix[i, j]);
                }
            }


            // test kmer counting CoVariance
            var kmerDistanceScoreCalculatorB = new KmerDistanceScoreCalculator(kmerLength, Alphabets.AmbiguousDNA, DistanceFunctionTypes.CoVariance);

            countDictionaryA = KmerDistanceScoreCalculator.CalculateKmerCounting(sequences[0], kmerLength);
            countDictionaryB = KmerDistanceScoreCalculator.CalculateKmerCounting(sequences[1], kmerLength);

            distanceScore = kmerDistanceScoreCalculatorB.CalculateDistanceScore(countDictionaryA, countDictionaryB);
            Console.WriteLine(distanceScore);

            var kmerDistanceMatrixGeneratorB = new KmerDistanceMatrixGenerator(sequences, kmerLength, Alphabets.AmbiguousDNA, DistanceFunctionTypes.CoVariance);

            for (var i = 0; i < sequences.Count - 1; ++i)
            {
                for (var j = i + 1; j < sequences.Count; ++j)
                {
                    Console.WriteLine("Kmer Distance of sequence {0}, and {1} is: {2}", i, j, kmerDistanceMatrixGeneratorB.DistanceMatrix[i, j]);
                }
            }


            // test kmer counting ModifiedMUSCLE
            var kmerDistanceScoreCalculatorC = new KmerDistanceScoreCalculator(kmerLength, Alphabets.AmbiguousDNA, DistanceFunctionTypes.ModifiedMUSCLE);

            countDictionaryA = KmerDistanceScoreCalculator.CalculateKmerCounting(sequences[0], kmerLength);
            countDictionaryB = KmerDistanceScoreCalculator.CalculateKmerCounting(sequences[1], kmerLength);

            distanceScore = kmerDistanceScoreCalculatorC.CalculateDistanceScore(countDictionaryA, countDictionaryB);
            Console.WriteLine(distanceScore);

            var kmerDistanceMatrixGeneratorC = new KmerDistanceMatrixGenerator(sequences, kmerLength, Alphabets.AmbiguousDNA, DistanceFunctionTypes.ModifiedMUSCLE);

            for (var i = 0; i < sequences.Count - 1; ++i)
            {
                for (var j = i + 1; j < sequences.Count; ++j)
                {
                    Console.WriteLine("Kmer Distance of sequence {0}, and {1} is: {2}", i, j, kmerDistanceMatrixGeneratorC.DistanceMatrix[i, j]);
                }
            }

            // test kmer counting PearsonCorrelation
            var kmerDistanceScoreCalculatorD = new KmerDistanceScoreCalculator(kmerLength, Alphabets.AmbiguousDNA, DistanceFunctionTypes.PearsonCorrelation);

            countDictionaryA = KmerDistanceScoreCalculator.CalculateKmerCounting(sequences[0], kmerLength);
            countDictionaryB = KmerDistanceScoreCalculator.CalculateKmerCounting(sequences[1], kmerLength);

            distanceScore = kmerDistanceScoreCalculatorD.CalculateDistanceScore(countDictionaryA, countDictionaryB);
            Console.WriteLine(distanceScore);

            var kmerDistanceMatrixGeneratorD = new KmerDistanceMatrixGenerator(sequences, kmerLength, Alphabets.AmbiguousDNA, DistanceFunctionTypes.PearsonCorrelation);

            for (var i = 0; i < sequences.Count - 1; ++i)
            {
                for (var j = i + 1; j < sequences.Count; ++j)
                {
                    Console.WriteLine("Kmer Distance of sequence {0}, and {1} is: {2}", i, j, kmerDistanceMatrixGeneratorD.DistanceMatrix[i, j]);
                }
            }


            // Test for case 2
            sequences.Clear();
            sequences.Add(new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGACAAAATCAG"));

            // test kmer counting

            countDictionaryA = KmerDistanceScoreCalculator.CalculateKmerCounting(sequences[0], kmerLength);
            countDictionaryB = KmerDistanceScoreCalculator.CalculateKmerCounting(sequences[1], kmerLength);

            foreach (var pair in countDictionaryA)
            {
                foreach (var s in pair.Key)
                {
                    Console.Write(s + " ");
                }
                Console.WriteLine(pair.Value);
            }
            foreach (var pair in countDictionaryB)
            {
                foreach (var s in pair.Key)
                {
                    Console.Write(s + " ");
                }
                Console.WriteLine(pair.Value);
            }

            distanceScore = kmerDistanceScoreCalculator.CalculateDistanceScore(countDictionaryA, countDictionaryB);
            Console.WriteLine(distanceScore);

            kmerDistanceMatrixGenerator = new KmerDistanceMatrixGenerator(sequences, kmerLength, Alphabets.AmbiguousDNA);

            for (var i = 0; i < sequences.Count - 1; ++i)
            {
                for (var j = i + 1; j < sequences.Count; ++j)
                {
                    Console.WriteLine("Kmer Distance of sequence {0}, and {1} is: {2}", i, j, kmerDistanceMatrixGenerator.DistanceMatrix[i, j]);
                }
            }

            // Test on larger dataset
            sequences = new List<ISequence>
            {
                new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT"),
                new Sequence(Alphabets.DNA, "GGGAATCAAAATCAG"),
                new Sequence(Alphabets.DNA, "GGGAATCAAAATCAG"),
                new Sequence(Alphabets.DNA, "GGGAAATCG"),
                new Sequence(Alphabets.DNA, "GGGAATCAATCAG"),
                new Sequence(Alphabets.DNA, "GGGAATCTTATCAG"),
                new Sequence(Alphabets.DNA, "GGGACAAAATCAG"),
                new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT"),
                new Sequence(Alphabets.DNA, "GGGAATCAAAATCAG"),
                new Sequence(Alphabets.DNA, "GGGACAAAATCAG")
            };

            kmerLength = 4;
            kmerDistanceMatrixGenerator =
                new KmerDistanceMatrixGenerator(sequences, kmerLength, Alphabets.AmbiguousDNA, DistanceFunctionTypes.EuclideanDistance);

            kmerDistanceScoreCalculator = new KmerDistanceScoreCalculator(kmerLength, Alphabets.AmbiguousDNA, DistanceFunctionTypes.EuclideanDistance);
            for (var i = 0; i < kmerDistanceMatrixGenerator.DistanceMatrix.Dimension - 1; ++i)
            {
                for (var j = i + 1; j < kmerDistanceMatrixGenerator.DistanceMatrix.Dimension; ++j)
                {
                    countDictionaryA = KmerDistanceScoreCalculator.CalculateKmerCounting(sequences[i], kmerLength);
                    countDictionaryB = KmerDistanceScoreCalculator.CalculateKmerCounting(sequences[j], kmerLength);
                    MsaUtils.Normalize(countDictionaryA);
                    MsaUtils.Normalize(countDictionaryB);
                    var score = kmerDistanceScoreCalculator.CalculateDistanceScore(countDictionaryA, countDictionaryB);
                    Console.WriteLine("{0}-{1}: {2}", i, j, score);
                    Console.WriteLine("{0}-{1}: {2}", i, j, kmerDistanceMatrixGenerator.DistanceMatrix[i, j]);
                    // Assert.AreEqual(score, kmerDistanceMatrixGenerator.DistanceMatrix[i, j]);
                }
            }
        }
    }
}
