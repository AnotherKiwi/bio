﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bio.TestAutomation.Util;
using Bio.Util.Logging;
using NUnit.Framework;

namespace Bio.Tests
{
    /// <summary>
    /// Test Automation code for Bio Alphabets and BVT level validations.
    /// </summary>
    [TestFixture]
    public class AlphabetsBvtTestCases
    {
        /// <summary>
        /// Alphabets type for validating different test cases
        /// </summary>
        private enum AlphabetsTypes
        {
            Dna,
            Rna,
            Protein
        }

        readonly Utility utilityObj = new Utility(@"TestUtils\TestsConfig.xml");        

        #region Alphabets Bvt TestCases

        /// <summary>
        /// Validate All property.
        /// Input Data : Valid All Alphabets.
        /// Output Data : Validate all Alphabet types.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateAll()
        {
            var refAlphabets = new List<IAlphabet>
                {
                    DnaAlphabet.Instance,
                    AmbiguousDnaAlphabet.Instance,
                    RnaAlphabet.Instance,
                    AmbiguousRnaAlphabet.Instance,
                    ProteinAlphabet.Instance,
                    AmbiguousProteinAlphabet.Instance
                };
            var allAphabets = Alphabets.All;

            for (var i = 0; i < refAlphabets.Count; i++)
            {
                Assert.AreEqual(refAlphabets[i].Name, allAphabets[i].Name);
            }

            ApplicationLog.WriteLine(string.Concat(
                  "Alphabets BVT: Validation of All property completed successfully."));
        }

        /// <summary>
        /// Validate Auto Detect Alphabet method.
        /// Input Data : Valid Dna,Rna and Protein Alphabet. 
        /// Output Data : Validate detection of alphabets
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateAutoDetectAlphabet()
        {
            var alphabetName = utilityObj.xmlUtil.GetTextValue(
                Constants.DnaDerivedSequenceNode, Constants.AlphabetNameNode);
            var dnaSequence = utilityObj.xmlUtil.GetTextValue(
                Constants.DnaDerivedSequenceNode, Constants.ExpectedDerivedSequence);
            var dnaArray = Encoding.UTF8.GetBytes(dnaSequence);

            //Validating for Dna.
            var dnaAplhabet = Alphabets.AutoDetectAlphabet(dnaArray, 0, 4, null);
            Assert.AreEqual(dnaAplhabet.Name, alphabetName);
            ApplicationLog.WriteLine("Alphabets BVT: Validation of Auto Detect method for Dna completed successfully.");

            //Validating for Rna.
            alphabetName = "";
            alphabetName = utilityObj.xmlUtil.GetTextValue(
                Constants.RnaDerivedSequenceNode, Constants.AlphabetNameNode);
            var rnaSequence = utilityObj.xmlUtil.GetTextValue(
                Constants.RnaDerivedSequenceNode, Constants.ExpectedDerivedSequence);
            var rnaArray = Encoding.UTF8.GetBytes(rnaSequence);

            var rnaAplhabet = Alphabets.AutoDetectAlphabet(rnaArray, 0, 4, null);
            Assert.AreEqual(rnaAplhabet.Name, alphabetName);
            ApplicationLog.WriteLine("Alphabets BVT: Validation of Auto Detect method for Rna completed successfully.");

            //Validating for Protein.
            alphabetName = "";
            alphabetName = utilityObj.xmlUtil.GetTextValue(
                Constants.ProteinDerivedSequenceNode, Constants.AlphabetNameNode);
            var proteinSequence = utilityObj.xmlUtil.GetTextValue(
                Constants.ProteinDerivedSequenceNode, Constants.ExpectedDerivedSequence);
            var proteinArray = Encoding.UTF8.GetBytes(proteinSequence);
            var proteinAplhabet = Alphabets.AutoDetectAlphabet(proteinArray, 0, 4, null);
            Assert.AreEqual(proteinAplhabet.Name, alphabetName);
            ApplicationLog.WriteLine("Alphabets BVT: Validation of Auto Detect method for Protein completed successfully.");

        }

        #endregion Alphabets Bvt TestCases

        #region DNA Alphabets Bvt TestCases

        /// <summary>
        /// Validate of Alphabet() static constructor.
        /// Input Data : Valid Dna Alphabet.
        /// Output Data : Validate Sequences.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void AlphabetStaticCtorValidatePhaseOne()
        {
            var seq =
                 new Sequence(Alphabets.DNA, "ATAGC");
                Assert.AreEqual(seq.Count, 5);
                Assert.AreEqual(new string(seq.Select(a => (char)a).ToArray()), "ATAGC");

            ApplicationLog.WriteLine("Alphabets BVT: Validation of Static Constructor completed successfully.");
        }

        /// <summary>
        /// Validate CompareSymbols() method.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateDnaAlphabetCompareSymbols()
        {
            var alp = DnaAlphabet.Instance;
            Assert.IsTrue(alp.CompareSymbols(65, 65));

            ApplicationLog.WriteLine(
                "Alphabets BVT: Validation of CompareSymbols() method completed successfully.");
        }

        /// <summary>
        /// Validates ValidateGetAmbiguousCharacters method for Dna type.
        /// Input: Dna Alphabet type.
        /// Output:Ambiguous characters corresponding to Dna type.
        [Test]
        [Category("Priority0")]
        public void ValidateGetAmbiguousCharactersforDna()
        {
            ValidateGetAmbiguousCharacters(AlphabetsTypes.Dna);
        }

        /// <summary>
        /// Validates GetValidSymbols method for Dna type.
        /// Input: Dna Alphabet type.
        /// Output:Valid Symbols corresponding to Dna type.
        [Test]
        [Category("Priority0")]
        public void GetValidSymbolsforDna()
        {
            ValidateGetValidSymbols(AlphabetsTypes.Dna);
        }

        /// <summary>
        /// Validates CompareSymbols method for Dna type.
        /// Input: Dna Alphabet type.
        /// Output:Validate Symbols corresponding to Dna type.
        [Test]
        [Category("Priority0")]
        public void ValidateCompareSymbolsforDna()
        {
            ValidateCompareSymbols(AlphabetsTypes.Dna);
        }

        /// <summary>
        /// Validates TryGetComplementSymbol method for Dna type.
        /// Input: Dna Alphabet type.
        /// Output:Validate Complement corresponding to Dna type.
        [Test]
        [Category("Priority0")]
        public void ValidateTryGetComplementSymbolforDna()
        {
            TryGetComplementSymbol(AlphabetsTypes.Dna);
        }

        /// <summary>
        /// Validate TryGetDefaultGapSymbol and TryGetGapSymbols method.
        /// Input Data : Valid Dna Alphabet instance. 
        /// Output Data : Validate Gap symbol for Dna alphabet.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateTryGetDefaultGapSymbolforDna()
        {
            ValidateTryGetDefaultGapSymbol(AlphabetsTypes.Dna);
        }

        /// <summary>
        /// Validate GetDefaultTerminationSymbol method.
        /// Input Data : Valid Dna Alphabet instance. 
        /// Output Data : Validate Default termination symbol for Dna alphabet.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateGetDefaultTerminationSymbolforDna()
        {
            GetDefaultTerminationSymbol(AlphabetsTypes.Dna);
        }

        /// <summary>
        /// Validate TryGetBasicSymbols() method.
        /// Input Data  : Valid Dna Alphabet instance. 
        /// Output Data : Validate Basic symbols for Dna alphabet.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateTryGetBasicSymbolsforDna()
        {
            ValidateTryGetBasicSymbols(AlphabetsTypes.Dna);
        }

        /// <summary>
        /// Validate TryGetAmbiguousSymbol() method.
        /// Input Data  : Valid Dna Alphabet instance. 
        /// Output Data : Validate Ambiguous symbols for Dna alphabets.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateTryGetAmbiguousSymbolforDna()
        {
            ValidateTryGetAmbiguousSymbol(AlphabetsTypes.Dna);
        }

        /// <summary>
        /// Validate ValidateSequenceTypes() method.
        /// Input Data  : Valid Dna Sequence.
        /// Output Data : Validate Sequence type.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateSequenceTypesforDna()
        {
            ValidateSequenceTypes(AlphabetsTypes.Dna);
        }

        /// <summary>
        /// Validate Dna's public properties.
        /// Input Data  : Valid Dna Sequence.
        /// Output Data : Validate Public properties like HasAmbiguity,HasGaps,HasTermination etc.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidatePublicPropertiesforDna()
        {
            ValidatePublicProperties(AlphabetsTypes.Dna);
        }

        /// <summary>
        /// Validate GetSymbolValueMap method.
        /// Input Data  : Valid Dna Sequence and expected Dictionary output..
        /// Output Data : Expected Dictionary output.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateGetSymbolValueMapforDna()
        {
            ValidateGetSymbolValueMap(AlphabetsTypes.Dna);
        }

        /// <summary>
        /// Validate friendly names for Dna Alphabet .
        /// Input Data  : Symbol names.
        /// Output Data : Friendly names for the symbols 
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateFriendlyNameForDna()
        {
            var friendlyName = utilityObj.xmlUtil.GetTextValue(Constants.FriendlyNameNode,
                                    Constants.ExpectedFriendlyNamesDnaNode);
            var friendlyNames = friendlyName.Split(',');

            Assert.AreEqual(DnaAlphabet.Instance.GetFriendlyName(DnaAlphabet.Instance.A), friendlyNames[0]);
            Assert.AreEqual(DnaAlphabet.Instance.GetFriendlyName(DnaAlphabet.Instance.C), friendlyNames[1]);
            Assert.AreEqual(DnaAlphabet.Instance.GetFriendlyName(DnaAlphabet.Instance.G), friendlyNames[2]);
            Assert.AreEqual(DnaAlphabet.Instance.GetFriendlyName(DnaAlphabet.Instance.T), friendlyNames[3]);
            Assert.AreEqual(DnaAlphabet.Instance.GetFriendlyName(DnaAlphabet.Instance.Gap), friendlyNames[4]);
        }

        /// <summary>
        /// Validate public properties of Ambiguous Dna Alphabet method.
        /// Input Data  : Ambiguous characters.
        /// Output Data : Expected values corresponding to Ambiguous characters.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateAmbiguousDnaAplhabet()
        {
            var ambiguousCharacters = new char[16] { 'A', 'M', 'R', 'S', 'W', 'Y', 'K', 'V', 'H', 'D', 'B', 'N', 'C', 'G', '-', 'T' };
            var dnaAlphabetInstance = AmbiguousDnaAlphabet.Instance;

            Assert.AreEqual((char)dnaAlphabetInstance.A, ambiguousCharacters[0]);
            Assert.AreEqual((char)dnaAlphabetInstance.AC, ambiguousCharacters[1]);
            Assert.AreEqual((char)dnaAlphabetInstance.ACT, ambiguousCharacters[8]);
            Assert.AreEqual((char)dnaAlphabetInstance.AT, ambiguousCharacters[4]);
            Assert.AreEqual((char)dnaAlphabetInstance.C, ambiguousCharacters[12]);
            Assert.AreEqual((char)dnaAlphabetInstance.G, ambiguousCharacters[13]);
            Assert.AreEqual((char)dnaAlphabetInstance.GA, ambiguousCharacters[2]);
            Assert.AreEqual((char)dnaAlphabetInstance.Gap, ambiguousCharacters[14]);
            Assert.AreEqual((char)dnaAlphabetInstance.GAT, ambiguousCharacters[9]);
            Assert.AreEqual((char)dnaAlphabetInstance.GC, ambiguousCharacters[3]);
            Assert.AreEqual((char)dnaAlphabetInstance.GCA, ambiguousCharacters[7]);
            Assert.AreEqual((char)dnaAlphabetInstance.GT, ambiguousCharacters[6]);
            Assert.AreEqual((char)dnaAlphabetInstance.GTC, ambiguousCharacters[10]);
            Assert.AreEqual((char)dnaAlphabetInstance.T, ambiguousCharacters[15]);
            Assert.AreEqual((char)dnaAlphabetInstance.TC, ambiguousCharacters[5]);
            ApplicationLog.WriteLine(string.Concat(@"Alphabets BVT: Validation of 
                                Ambiguous Dna characters completed successfully."));
        }

       
        /// <summary>
        /// Validate friendly names for Dna Alphabet .
        /// Input Data  : Symbol names.
        /// Output Data : Friendly names for the symbols 
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateFriendlyNameForAmbiguousDna()
        {
            var friendlyName = utilityObj.xmlUtil.GetTextValue(Constants.FriendlyNameNode,
                                      Constants.ExpectedFriendlyNamesAmbiguousDnaNode);
            var friendlyNames = friendlyName.Split(',');

            Assert.AreEqual(AmbiguousDnaAlphabet.Instance.GetFriendlyName(AmbiguousDnaAlphabet.Instance.AC), friendlyNames[0]);
            Assert.AreEqual(AmbiguousDnaAlphabet.Instance.GetFriendlyName(AmbiguousDnaAlphabet.Instance.GA), friendlyNames[1]);
            Assert.AreEqual(AmbiguousDnaAlphabet.Instance.GetFriendlyName(AmbiguousDnaAlphabet.Instance.GC), friendlyNames[2]);
            Assert.AreEqual(AmbiguousDnaAlphabet.Instance.GetFriendlyName(AmbiguousDnaAlphabet.Instance.AT), friendlyNames[3]);
            Assert.AreEqual(AmbiguousDnaAlphabet.Instance.GetFriendlyName(AmbiguousDnaAlphabet.Instance.TC), friendlyNames[4]);
            Assert.AreEqual(AmbiguousDnaAlphabet.Instance.GetFriendlyName(AmbiguousDnaAlphabet.Instance.GT), friendlyNames[5]);
            Assert.AreEqual(AmbiguousDnaAlphabet.Instance.GetFriendlyName(AmbiguousDnaAlphabet.Instance.GCA), friendlyNames[6]);
            Assert.AreEqual(AmbiguousDnaAlphabet.Instance.GetFriendlyName(AmbiguousDnaAlphabet.Instance.ACT), friendlyNames[7]);
            Assert.AreEqual(AmbiguousDnaAlphabet.Instance.GetFriendlyName(AmbiguousDnaAlphabet.Instance.GAT), friendlyNames[8]);
            Assert.AreEqual(AmbiguousDnaAlphabet.Instance.GetFriendlyName(AmbiguousDnaAlphabet.Instance.GTC), friendlyNames[9]);
            Assert.AreEqual(AmbiguousDnaAlphabet.Instance.GetFriendlyName(AmbiguousDnaAlphabet.Instance.Any), friendlyNames[10]);
        }

        #endregion DNA Alphabets Bvt TestCases

        #region RNA Alphabets Bvt TestCases

        /// <summary>
        /// Validates ValidateGetAmbiguousCharacters method for Rna type.
        /// Input: Rna Alphabet type.
        /// Output:Ambiguous characters corresponding to Rna type.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateGetAmbiguousCharactersforRna()
        {
            ValidateGetAmbiguousCharacters(AlphabetsTypes.Rna);
        }

        /// <summary>
        /// Validate CompareSymbols() method.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateRnaAlphabetCompareSymbols()
        {
            var alp = RnaAlphabet.Instance;
            Assert.IsTrue(alp.CompareSymbols(65, 65));

            ApplicationLog.WriteLine(
                "Alphabets BVT: Validation of CompareSymbols() method completed successfully.");
        }

        /// <summary>
        /// Validates GetValidSymbols method for Rna type.
        /// Input: Rna Alphabet type.
        /// Output:Valid Symbols corresponding to Rna type.
        [Test]
        [Category("Priority0")]
        public void GetValidSymbolsforRna()
        {
            ValidateGetValidSymbols(AlphabetsTypes.Rna);
        }

        /// <summary>
        /// Validates CompareSymbols method for Rna type.
        /// Input: Rna Alphabet type.
        /// Output:Validate Symbols corresponding to Rna type.
        [Test]
        [Category("Priority0")]
        public void ValidateCompareSymbolsforRna()
        {
            ValidateCompareSymbols(AlphabetsTypes.Rna);
        }

        /// <summary>
        /// Validates TryGetComplementSymbol method for Rna type.
        /// Input: Rna Alphabet type.
        /// Output:Validate Complement corresponding to Rna type.
        [Test]
        [Category("Priority0")]
        public void ValidateTryGetComplementSymbolforRna()
        {
            TryGetComplementSymbol(AlphabetsTypes.Rna);
        }

        /// <summary>
        /// Validate GetDefaultTerminationSymbol method.
        /// Input Data : Valid Rna Alphabet instance. 
        /// Output Data : Validate Default termination symbol for Rna alphabet.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateGetDefaultTerminationSymbolforRna()
        {
            GetDefaultTerminationSymbol(AlphabetsTypes.Rna);
        }

        /// <summary>
        /// Validate TryGetDefaultGapSymbol and TryGetGapSymbols method.
        /// Input Data : Valid Rna Alphabet instance. 
        /// Output Data : Validate Gap symbol for Rna alphabet.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateTryGetDefaultGapSymbolforRna()
        {
            ValidateTryGetDefaultGapSymbol(AlphabetsTypes.Rna);
        }

        /// <summary>
        /// Validate TryGetBasicSymbols() method.
        /// Input Data  : Valid Rna Alphabet instance. 
        /// Output Data : Validate Basic symbols for Rna alphabet.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateTryGetBasicSymbolsforRna()
        {
            ValidateTryGetBasicSymbols(AlphabetsTypes.Rna);
        }

        /// <summary>
        /// Validate TryGetAmbiguousSymbol() method.
        /// Input Data  : Valid Rna Alphabet instance. 
        /// Output Data : Validate Ambiguous symbols for Rna alphabets.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateTryGetAmbiguousSymbolforRna()
        {
            ValidateTryGetAmbiguousSymbol(AlphabetsTypes.Rna);
        }

        /// <summary>
        /// Validate ValidateSequenceTypes() method.
        /// Input Data  : Valid Rna Sequence.
        /// Output Data : Validate Sequence type.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateSequenceTypesforRna()
        {
            ValidateSequenceTypes(AlphabetsTypes.Rna);
        }

        /// <summary>
        /// Validate Rna's public properties.
        /// Input Data  : Valid Rna Sequence.
        /// Output Data : Validate Public properties like HasAmbiguity,HasGaps,HasTermination etc.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidatePublicPropertiesforRna()
        {
            ValidatePublicProperties(AlphabetsTypes.Rna);
        }

        /// <summary>
        /// Validate GetSymbolValueMap method.
        /// Input Data  : Valid Rna Sequence and expected Dictionary output..
        /// Output Data : Expected Dictionary output.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateGetSymbolValueMapforRna()
        {
            ValidateGetSymbolValueMap(AlphabetsTypes.Rna);
        }

        /// <summary>
        /// Validate friendly names for Rna Alphabet .
        /// Input Data  : Symbol names.
        /// Output Data : Friendly names for the symbols 
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateFriendlyNameForRna()
        {
            var friendlyName = utilityObj.xmlUtil.GetTextValue(Constants.FriendlyNameNode,
                                    Constants.ExpectedFriendlyNamesRnaNode);
            var friendlyNames = friendlyName.Split(',');

            Assert.AreEqual(RnaAlphabet.Instance.GetFriendlyName(RnaAlphabet.Instance.A), friendlyNames[0]);
            Assert.AreEqual(RnaAlphabet.Instance.GetFriendlyName(RnaAlphabet.Instance.C), friendlyNames[1]);
            Assert.AreEqual(RnaAlphabet.Instance.GetFriendlyName(RnaAlphabet.Instance.G), friendlyNames[2]);
            Assert.AreEqual(RnaAlphabet.Instance.GetFriendlyName(RnaAlphabet.Instance.U), friendlyNames[3]);
            Assert.AreEqual(RnaAlphabet.Instance.GetFriendlyName(RnaAlphabet.Instance.Gap), friendlyNames[4]);
        }

        /// <summary>
        /// Validate public properties of Ambiguous Rna Alphabet method.
        /// Input Data  : Ambiguous characters.
        /// Output Data : Expected values corresponding to Ambiguous characters.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateAmbiguousRnaAplhabet()
        {
            var ambiguousCharacters = new char[10] { 'M', 'H', 'W', 'R', 'D', 'S', 'V', 'K', 'B', 'Y' };
            var rnaAlphabetInstance = AmbiguousRnaAlphabet.Instance;

            Assert.AreEqual((char)rnaAlphabetInstance.AC, ambiguousCharacters[0]);
            Assert.AreEqual((char)rnaAlphabetInstance.ACU, ambiguousCharacters[1]);
            Assert.AreEqual((char)rnaAlphabetInstance.AU, ambiguousCharacters[2]);
            Assert.AreEqual((char)rnaAlphabetInstance.GA, ambiguousCharacters[3]);
            Assert.AreEqual((char)rnaAlphabetInstance.GAU, ambiguousCharacters[4]);
            Assert.AreEqual((char)rnaAlphabetInstance.GC, ambiguousCharacters[5]);
            Assert.AreEqual((char)rnaAlphabetInstance.GCA, ambiguousCharacters[6]);
            Assert.AreEqual((char)rnaAlphabetInstance.GU, ambiguousCharacters[7]);
            Assert.AreEqual((char)rnaAlphabetInstance.GUC, ambiguousCharacters[8]);
            Assert.AreEqual((char)rnaAlphabetInstance.UC, ambiguousCharacters[9]);
            ApplicationLog.WriteLine(string.Concat(@"Alphabets BVT: Validation of 
                                Ambiguous Rna characters completed successfully."));
        }

        /// <summary>
        /// Validate friendly names for AmbiguousRna Alphabet .
        /// Input Data  : Symbol names.
        /// Output Data : Friendly names for the symbols 
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateFriendlyNameForAmbiguousRna()
        {
            var friendlyName = utilityObj.xmlUtil.GetTextValue(Constants.FriendlyNameNode,
                                    Constants.ExpectedFriendlyNamesAmbiguousRnaNode);
            var friendlyNames = friendlyName.Split(',');

            Assert.AreEqual(AmbiguousRnaAlphabet.Instance.GetFriendlyName(AmbiguousRnaAlphabet.Instance.Any), friendlyNames[0]);
            Assert.AreEqual(AmbiguousRnaAlphabet.Instance.GetFriendlyName(AmbiguousRnaAlphabet.Instance.AC), friendlyNames[1]);
            Assert.AreEqual(AmbiguousRnaAlphabet.Instance.GetFriendlyName(AmbiguousRnaAlphabet.Instance.GA), friendlyNames[2]);
            Assert.AreEqual(AmbiguousRnaAlphabet.Instance.GetFriendlyName(AmbiguousRnaAlphabet.Instance.GC), friendlyNames[3]);
            Assert.AreEqual(AmbiguousRnaAlphabet.Instance.GetFriendlyName(AmbiguousRnaAlphabet.Instance.AU), friendlyNames[4]);
            Assert.AreEqual(AmbiguousRnaAlphabet.Instance.GetFriendlyName(AmbiguousRnaAlphabet.Instance.UC), friendlyNames[5]);
            Assert.AreEqual(AmbiguousRnaAlphabet.Instance.GetFriendlyName(AmbiguousRnaAlphabet.Instance.GU), friendlyNames[6]);
            Assert.AreEqual(AmbiguousRnaAlphabet.Instance.GetFriendlyName(AmbiguousRnaAlphabet.Instance.GCA), friendlyNames[7]);
            Assert.AreEqual(AmbiguousRnaAlphabet.Instance.GetFriendlyName(AmbiguousRnaAlphabet.Instance.ACU), friendlyNames[8]);
            Assert.AreEqual(AmbiguousRnaAlphabet.Instance.GetFriendlyName(AmbiguousRnaAlphabet.Instance.GAU), friendlyNames[9]);
            Assert.AreEqual(AmbiguousRnaAlphabet.Instance.GetFriendlyName(AmbiguousRnaAlphabet.Instance.GUC), friendlyNames[10]);
        }

        #endregion RNA Alphabets Bvt TestCases

        #region Protein Alphabets Bvt TestCases

        /// <summary>
        /// Validates ValidateGetAmbiguousCharacters method for Protein type.
        /// Input: Protein Alphabet type.
        /// Output:Ambiguous characters corresponding to Protein type.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateGetAmbiguousCharactersforProtein()
        {
            ValidateGetAmbiguousCharacters(AlphabetsTypes.Protein);
        }

        /// <summary>
        /// Validate CompareSymbols() method.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateProAlphabetCompareSymbols()
        {
            var alp = ProteinAlphabet.Instance;
            Assert.IsTrue(alp.CompareSymbols(65, 65));

            ApplicationLog.WriteLine(
                "Alphabets BVT: Validation of CompareSymbols() method completed successfully.");
        }

        /// <summary>
        /// Validates GetValidSymbols method for Protein type.
        /// Input: Rna Alphabet type.
        /// Output:Valid Symbols corresponding to Protein type.
        [Test]
        [Category("Priority0")]
        public void GetValidSymbolsforProtein()
        {
            ValidateGetValidSymbols(AlphabetsTypes.Protein);
        }

        /// <summary>
        /// Validates CompareSymbols method for Protein type.
        /// Input: Protein Alphabet type.
        /// Output:Validate Symbols corresponding to Protein type.
        [Test]
        [Category("Priority0")]
        public void ValidateCompareSymbolsforProtein()
        {
            ValidateCompareSymbols(AlphabetsTypes.Protein);
        }

        /// <summary>
        /// Validates TryGetComplementSymbol method for Protein type.
        /// Input: Protein Alphabet type.
        /// Output:Validate Complement corresponding to Protein type.
        [Test]
        [Category("Priority0")]
        public void ValidateTryGetComplementSymbolforProtein()
        {
            TryGetComplementSymbol(AlphabetsTypes.Protein);
        }

        /// <summary>
        /// Validate GetDefaultTerminationSymbol method.
        /// Input Data : Valid Protein Alphabet instance. 
        /// Output Data : Validate Default termination symbol for Protein alphabet.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateGetDefaultTerminationSymbolforProtein()
        {
            GetDefaultTerminationSymbol(AlphabetsTypes.Protein);
        }

        /// <summary>
        /// Validate TryGetDefaultGapSymbol and TryGetGapSymbols method.
        /// Input Data : Valid Protein Alphabet instance. 
        /// Output Data : Validate Gap symbol for Protein alphabet.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateTryGetDefaultGapSymbolforProtein()
        {
            ValidateTryGetDefaultGapSymbol(AlphabetsTypes.Protein);
        }

        /// <summary>
        /// Validate TryGetBasicSymbols() method.
        /// Input Data  : Valid Protein Alphabet instance. 
        /// Output Data : Validate Basic symbols for Protein alphabet.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateTryGetBasicSymbolsforProtein()
        {
            ValidateTryGetBasicSymbols(AlphabetsTypes.Protein);
        }

        /// <summary>
        /// Validate TryGetAmbiguousSymbol() method.
        /// Input Data  : Valid Protein Alphabet instance. 
        /// Output Data : Validate Ambiguous symbols for Protein alphabets.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateTryGetAmbiguousSymbolforProtein()
        {
            ValidateTryGetAmbiguousSymbol(AlphabetsTypes.Protein);
        }

        /// <summary>
        /// Validate ValidateSequenceTypes() method.
        /// Input Data  : Valid Protein Sequence.
        /// Output Data : Validate Sequence type.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateSequenceTypesforProtein()
        {
            ValidateSequenceTypes(AlphabetsTypes.Protein);
        }

        /// <summary>
        /// Validate Protein public properties.
        /// Input Data  : Valid Protein Sequence.
        /// Output Data : Validate Public properties like HasAmbiguity,HasGaps,HasTermination etc.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidatePublicPropertiesforProtein()
        {
            ValidatePublicProperties(AlphabetsTypes.Protein);
        }

        /// <summary>
        /// Validate GetSymbolValueMap method.
        /// Input Data  : Valid Protein Sequence and expected Dictionary output..
        /// Output Data : Expected Dictionary output.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateGetSymbolValueMapforProtein()
        {
            ValidateGetSymbolValueMap(AlphabetsTypes.Protein);
        }

        /// <summary>
        /// Validate friendly names for Protein Alphabet .
        /// Input Data  : Symbol names.
        /// Output Data : Friendly names for the symbols 
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateFriendlyNameForProtein()
        {
            var friendlyName = utilityObj.xmlUtil.GetTextValue(Constants.FriendlyNameNode,
                                    Constants.ExpectedFriendlyNamesProteinNode);
            var friendlyNames = friendlyName.Split(',');

            Assert.AreEqual(ProteinAlphabet.Instance.GetFriendlyName(ProteinAlphabet.Instance.A), friendlyNames[0]);
            Assert.AreEqual(ProteinAlphabet.Instance.GetFriendlyName(ProteinAlphabet.Instance.C), friendlyNames[1]);
            Assert.AreEqual(ProteinAlphabet.Instance.GetFriendlyName(ProteinAlphabet.Instance.D), friendlyNames[2]);
            Assert.AreEqual(ProteinAlphabet.Instance.GetFriendlyName(ProteinAlphabet.Instance.E), friendlyNames[3]);
            Assert.AreEqual(ProteinAlphabet.Instance.GetFriendlyName(ProteinAlphabet.Instance.F), friendlyNames[4]);
            Assert.AreEqual(ProteinAlphabet.Instance.GetFriendlyName(ProteinAlphabet.Instance.G), friendlyNames[5]);
            Assert.AreEqual(ProteinAlphabet.Instance.GetFriendlyName(ProteinAlphabet.Instance.H), friendlyNames[6]);
            Assert.AreEqual(ProteinAlphabet.Instance.GetFriendlyName(ProteinAlphabet.Instance.I), friendlyNames[7]);
            Assert.AreEqual(ProteinAlphabet.Instance.GetFriendlyName(ProteinAlphabet.Instance.K), friendlyNames[8]);
            Assert.AreEqual(ProteinAlphabet.Instance.GetFriendlyName(ProteinAlphabet.Instance.L), friendlyNames[9]);
            Assert.AreEqual(ProteinAlphabet.Instance.GetFriendlyName(ProteinAlphabet.Instance.M), friendlyNames[10]);
            Assert.AreEqual(ProteinAlphabet.Instance.GetFriendlyName(ProteinAlphabet.Instance.N), friendlyNames[11]);
            Assert.AreEqual(ProteinAlphabet.Instance.GetFriendlyName(ProteinAlphabet.Instance.O), friendlyNames[12]);
            Assert.AreEqual(ProteinAlphabet.Instance.GetFriendlyName(ProteinAlphabet.Instance.P), friendlyNames[13]);
            Assert.AreEqual(ProteinAlphabet.Instance.GetFriendlyName(ProteinAlphabet.Instance.Q), friendlyNames[14]);
            Assert.AreEqual(ProteinAlphabet.Instance.GetFriendlyName(ProteinAlphabet.Instance.R), friendlyNames[15]);
            Assert.AreEqual(ProteinAlphabet.Instance.GetFriendlyName(ProteinAlphabet.Instance.S), friendlyNames[16]);
            Assert.AreEqual(ProteinAlphabet.Instance.GetFriendlyName(ProteinAlphabet.Instance.T), friendlyNames[17]);
            Assert.AreEqual(ProteinAlphabet.Instance.GetFriendlyName(ProteinAlphabet.Instance.U), friendlyNames[18]);
            Assert.AreEqual(ProteinAlphabet.Instance.GetFriendlyName(ProteinAlphabet.Instance.V), friendlyNames[19]);
            Assert.AreEqual(ProteinAlphabet.Instance.GetFriendlyName(ProteinAlphabet.Instance.W), friendlyNames[20]);
            Assert.AreEqual(ProteinAlphabet.Instance.GetFriendlyName(ProteinAlphabet.Instance.Y), friendlyNames[21]);
            Assert.AreEqual(ProteinAlphabet.Instance.GetFriendlyName(ProteinAlphabet.Instance.Gap), friendlyNames[22]);
            Assert.AreEqual(ProteinAlphabet.Instance.GetFriendlyName(ProteinAlphabet.Instance.Ter), friendlyNames[23]);
        }

        /// <summary>
        /// Validate public properties of Ambiguous Protein Alphabet method.
        /// Input Data  : Ambiguous characters.
        /// Output Data : Expected values corresponding to Ambiguous characters.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateAmbiguousProteinAplhabet()
        {
            var proteinAlphabetInstance = AmbiguousProteinAlphabet.Instance;
            Assert.AreEqual(proteinAlphabetInstance.X, (byte)'X');
            Assert.AreEqual(proteinAlphabetInstance.Z, (byte)'Z');
            Assert.AreEqual(proteinAlphabetInstance.B, (byte)'B');
            Assert.AreEqual(proteinAlphabetInstance.J, (byte)'J');
            ApplicationLog.WriteLine(string.Concat(@"Alphabets BVT: Validation of 
                                Ambiguous Protein characters completed successfully."));

        }

        /// <summary>
        /// Validate friendly names for AmbiguousProtein Alphabet .
        /// Input Data  : Symbol names.
        /// Output Data : Friendly names for the symbols 
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateFriendlyNameForAmbiguousProtein()
        {
            var friendlyName = utilityObj.xmlUtil.GetTextValue(Constants.FriendlyNameNode,
                                    Constants.ExpectedFriendlyNamesAmbiguousProteinNode);
            var friendlyNames = friendlyName.Split(',');

            Assert.AreEqual(AmbiguousProteinAlphabet.Instance.GetFriendlyName(AmbiguousProteinAlphabet.Instance.X), friendlyNames[0]);
            Assert.AreEqual(AmbiguousProteinAlphabet.Instance.GetFriendlyName(AmbiguousProteinAlphabet.Instance.Z), friendlyNames[1]);
            Assert.AreEqual(AmbiguousProteinAlphabet.Instance.GetFriendlyName(AmbiguousProteinAlphabet.Instance.B), friendlyNames[2]);
            Assert.AreEqual(AmbiguousProteinAlphabet.Instance.GetFriendlyName(AmbiguousProteinAlphabet.Instance.J), friendlyNames[3]);
        }

        #endregion Protein Alphabets Bvt TestCases

        #region Supporting Method

        /// <summary>
        /// Validates Get Ambiguous Characters for Dna/Rna/Protein.
        /// Input: Alphabet type(Dna/Rna/Protein).
        /// Output:Ambiguous characters corresponding to Alphabet types.
        /// <param name="parentNode">Node containing the sequences </param>
        /// <param name="option">Alphabet type(Dna/Rna/Protein)</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        void ValidateGetAmbiguousCharacters(AlphabetsTypes option)
        {
            var referenceCharacters = "";
            IAlphabet alphabetInstance = null;

            switch (option)
            {
                case AlphabetsTypes.Protein:
                    referenceCharacters = "BZJX";
                    alphabetInstance = AmbiguousProteinAlphabet.Instance;
                    break;
                case AlphabetsTypes.Rna:
                    alphabetInstance = AmbiguousRnaAlphabet.Instance;
                    referenceCharacters = "MRSWYKVHDBN";
                    break;
                case AlphabetsTypes.Dna:
                    alphabetInstance = AmbiguousDnaAlphabet.Instance;
                    referenceCharacters = "MRSWYKVHDBN";
                    break;
            }

            var ambiguousCharacters = new HashSet<byte>();
            ambiguousCharacters = alphabetInstance.GetAmbiguousSymbols();
            var ambiguosCharacters = new string(ambiguousCharacters.Select(a => (char)a).ToArray());

            var refCharacters = referenceCharacters.ToCharArray();

            for (var i = 0; i < ambiguosCharacters.Length; i++)
            {
                Assert.IsTrue(ambiguosCharacters.Contains(refCharacters[i]));
            }

        }

        /// <summary>
        /// Validates Get Valid Symbols for Dna/Rna/Protein.
        /// Input: Alphabet type(Dna/Rna/Protein).
        /// Output:Valid Symbols corresponding to Alphabet types.        
        /// <param name="option">Alphabet type(Dna/Rna/Protein)</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        void ValidateGetValidSymbols(AlphabetsTypes option)
        {
            var referenceCharacters = "";
            IAlphabet alphabetInstance = null;

            switch (option)
            {
                case AlphabetsTypes.Protein:
                    referenceCharacters = "AaCcDdEeFfGgHhIiKkLlMmNnOoPpQqRrSsTtUuVvWwYy-*";
                    alphabetInstance = ProteinAlphabet.Instance;
                    break;
                case AlphabetsTypes.Rna:
                    alphabetInstance = RnaAlphabet.Instance;
                    referenceCharacters = "AaCcGgUu-";
                    break;
                case AlphabetsTypes.Dna:
                    alphabetInstance = DnaAlphabet.Instance;
                    referenceCharacters = "AaCcGgTt-";
                    break;
            }

            var validSymbolsByte = new HashSet<byte>();
            validSymbolsByte = alphabetInstance.GetValidSymbols();
            var validSymbols = new string(validSymbolsByte.Select(a => (char)a).ToArray());
            Assert.AreEqual(referenceCharacters, validSymbols);
            ApplicationLog.WriteLine(string.Concat(
                   "Alphabets BVT: Validation of Alphabets operation ", option, " completed successfully."));
        }

        /// <summary>
        /// Validates Get Valid Symbols for Dna/Rna/Protein.
        /// Input: Alphabet type(Dna/Rna/Protein).
        /// Output:Valid Symbols corresponding to Alphabet types.        
        /// <param name="option">Alphabet type(Dna/Rna/Protein)</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        void ValidateCompareSymbols(AlphabetsTypes option)
        {
            IAlphabet alphabetInstance = null;
            byte byte1 = 0, byte2 = 0, byte3 = 0;

            switch (option)
            {
                case AlphabetsTypes.Protein:
                    alphabetInstance = ProteinAlphabet.Instance;
                    byte1 = (byte)'A';
                    byte2 = (byte)'A';
                    byte3 = (byte)'B';
                    break;
                case AlphabetsTypes.Rna:
                    alphabetInstance = RnaAlphabet.Instance;
                    byte1 = (byte)'U';
                    byte2 = (byte)'U';
                    byte3 = (byte)'A';
                    break;
                case AlphabetsTypes.Dna:
                    alphabetInstance = DnaAlphabet.Instance;
                    byte1 = (byte)'T';
                    byte2 = (byte)'T';
                    byte3 = (byte)'A';
                    break;
            }

            Assert.AreEqual(byte1, byte2);
            Assert.AreNotEqual(byte1, byte3);
            ApplicationLog.WriteLine(string.Concat("Alphabets BVT: Validation of Comparing Symbols operation ",
                                    option, " completed successfully."));
        }

        /// <summary>
        /// Validate sTryGetComplementSymbol method for Dna/Rna/Protein.
        /// Input: Alphabet type(Dna/Rna/Protein).
        /// Output:Complement corresponding to Alphabet types.        
        /// <param name="option">Alphabet type(Dna/Rna/Protein)</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        void TryGetComplementSymbol(AlphabetsTypes option)
        {
            IAlphabet alphabetInstance = null;
            byte inputByte = 0, complementByte = 0, outputByte = 0;

            switch (option)
            {
                case AlphabetsTypes.Protein:
                    alphabetInstance = ProteinAlphabet.Instance;
                    inputByte = (byte)'F';
                    complementByte = (byte)'U';
                    break;
                case AlphabetsTypes.Rna:
                    alphabetInstance = RnaAlphabet.Instance;
                    inputByte = (byte)'A';
                    complementByte = (byte)'U';
                    break;
                case AlphabetsTypes.Dna:
                    alphabetInstance = DnaAlphabet.Instance;
                    inputByte = (byte)'A';
                    complementByte = (byte)'T';
                    break;
            }

            if (!option.Equals(AlphabetsTypes.Protein))
            {
                Assert.AreEqual(true, alphabetInstance.TryGetComplementSymbol(inputByte, out outputByte));
                Assert.AreEqual(complementByte, outputByte);
            }
            else
            {
                Assert.AreEqual(false, alphabetInstance.TryGetComplementSymbol(inputByte, out outputByte));
            }

            ApplicationLog.WriteLine(string.Concat("Alphabets BVT: Validation of Get Complement operation for",
                                    option, " completed successfully."));

        }

        /// <summary>
        /// Validates TryGetComplementSymbol and TryGetTerminationSymbols method for Dna/Rna/Protein.
        /// Input: Alphabet type(Dna/Rna/Protein).
        /// Output:Complement corresponding to Alphabet types.        
        /// <param name="option">Alphabet type(Dna/Rna/Protein)</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        void GetDefaultTerminationSymbol(AlphabetsTypes option)
        {
            IAlphabet alphabetInstance = null;
            byte outputDefaultTerminationSymbol = 0;
            var outputTerminationSymbol = new HashSet<byte>();
            var outputTerminationString = "";

            switch (option)
            {
                case AlphabetsTypes.Protein:
                    alphabetInstance = ProteinAlphabet.Instance;
                    break;
                case AlphabetsTypes.Rna:
                    alphabetInstance = RnaAlphabet.Instance;
                    break;
                case AlphabetsTypes.Dna:
                    alphabetInstance = DnaAlphabet.Instance;
                    break;
            }

            if (option.Equals(AlphabetsTypes.Protein))
            {
                Assert.AreEqual(true, alphabetInstance.TryGetDefaultTerminationSymbol(out outputDefaultTerminationSymbol));
                Assert.AreEqual(true, alphabetInstance.TryGetTerminationSymbols(out outputTerminationSymbol));
                outputTerminationString = new string(outputTerminationSymbol.Select(a => (char)a).ToArray());
                Assert.AreEqual('*', (char)outputDefaultTerminationSymbol);
                Assert.AreEqual("*", outputTerminationString);
            }
            else
            {
                Assert.AreEqual(false, alphabetInstance.TryGetDefaultTerminationSymbol(out outputDefaultTerminationSymbol));
                Assert.AreEqual(false, alphabetInstance.TryGetTerminationSymbols(out outputTerminationSymbol));
            }

            ApplicationLog.WriteLine(string.Concat("Alphabets BVT: Validation of Get Default termination symbol for",
                                    option, " completed successfully."));
        }

        /// <summary>
        /// Validate TryGetDefaultGapSymbol and TryGetGapSymbols method.
        /// Input Data : Valid Dna/Rna/Protein Alphabet instance. 
        /// Output Data : Validate Gap symbol for Alphabet instances.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        void ValidateTryGetDefaultGapSymbol(AlphabetsTypes option)
        {
            IAlphabet alphabetInstance = null;

            switch (option)
            {
                case AlphabetsTypes.Protein:
                    alphabetInstance = ProteinAlphabet.Instance;
                    break;
                case AlphabetsTypes.Rna:
                    alphabetInstance = RnaAlphabet.Instance;
                    break;
                case AlphabetsTypes.Dna:
                    alphabetInstance = DnaAlphabet.Instance;
                    break;
            }

            byte outputByte;
            alphabetInstance.TryGetDefaultGapSymbol(out outputByte);
            Assert.AreEqual('-', (char)outputByte);
            ApplicationLog.WriteLine(string.Concat(@"Alphabets BVT: Validation of 
                                Try Default gap symbol for ", option, " completed successfully."));
            var outputGapSymbol = new HashSet<byte>();
            var outputGapString = "";
            alphabetInstance.TryGetGapSymbols(out outputGapSymbol);
            outputGapString = new string(outputGapSymbol.Select(a => (char)a).ToArray());
            Assert.AreEqual("-", outputGapString);
            ApplicationLog.WriteLine(string.Concat(@"Alphabets BVT: Validation of 
                                Try  Get gap symbol for ", option, " completed successfully."));
        }

        /// <summary>
        /// Validate TryGetBasicSymbols
        /// Input Data : Valid Dna/Rna/Protein Alphabet instance. 
        /// Output Data : Validate Basic Symbols for Alphabet instances.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        void ValidateTryGetBasicSymbols(AlphabetsTypes option)
        {
            IAlphabet alphabetInstance = null;
            byte basicSymbol = 0, expectedSymbol1 = 0, expectedSymbol2 = 0;

            switch (option)
            {
                case AlphabetsTypes.Protein:
                    alphabetInstance = AmbiguousProteinAlphabet.Instance;
                    basicSymbol = (byte)'Z';
                    expectedSymbol1 = (byte)'Q';
                    expectedSymbol2 = (byte)'E';
                    break;
                case AlphabetsTypes.Rna:
                    alphabetInstance = AmbiguousRnaAlphabet.Instance;
                    basicSymbol = (byte)'R';
                    expectedSymbol1 = (byte)'G';
                    expectedSymbol2 = (byte)'A';
                    break;
                case AlphabetsTypes.Dna:
                    basicSymbol = (byte)'M';
                    expectedSymbol1 = (byte)'A';
                    expectedSymbol2 = (byte)'C';
                    alphabetInstance = AmbiguousDnaAlphabet.Instance;
                    break;
            }

            HashSet<byte> basicSymbols;
            Assert.AreEqual(true, alphabetInstance.TryGetBasicSymbols(basicSymbol, out basicSymbols));
            Assert.IsTrue(basicSymbols.All(sy => (sy == expectedSymbol1 || sy == expectedSymbol2)));
            ApplicationLog.WriteLine(string.Concat(@"Alphabets BVT: Validation of 
                                Try  Get Basics symbol for ", option, " completed successfully."));
        }

        /// <summary>
        /// Validate TryGetAmbiguousSymbol method.
        /// Input Data : Valid Dna/Rna/Protein Alphabet instance. 
        /// Output Data : Validate Ambiguous Symbols for all Alphabet instances.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        void ValidateTryGetAmbiguousSymbol(AlphabetsTypes option)
        {
            IAlphabet alphabetInstance = null;
            var basicSymbols = new HashSet<byte>();
            byte ambiguousSymbol = 0, expectedAmbiguousSymbol = 0;

            switch (option)
            {
                case AlphabetsTypes.Protein:
                    alphabetInstance = AmbiguousProteinAlphabet.Instance;
                    basicSymbols.Add((byte)'Q');
                    basicSymbols.Add((byte)'E');
                    expectedAmbiguousSymbol = (byte)'Z';
                    break;
                case AlphabetsTypes.Rna:
                    alphabetInstance = AmbiguousRnaAlphabet.Instance;
                    basicSymbols.Add((byte)'G');
                    basicSymbols.Add((byte)'C');
                    expectedAmbiguousSymbol = (byte)'S';
                    break;
                case AlphabetsTypes.Dna:
                    alphabetInstance = AmbiguousDnaAlphabet.Instance;
                    basicSymbols.Add((byte)'G');
                    basicSymbols.Add((byte)'A');
                    expectedAmbiguousSymbol = (byte)'R';
                    break;
            }

            Assert.IsTrue(alphabetInstance.TryGetAmbiguousSymbol(basicSymbols, out ambiguousSymbol));
            Assert.AreEqual(expectedAmbiguousSymbol, ambiguousSymbol);
            ApplicationLog.WriteLine(string.Concat(@"Alphabets BVT: Validation of 
                                Try  Get Ambiguous symbol for ", option, " completed successfully."));
        }

        /// <summary>
        /// Validate ValidateSequence method.
        /// Input Data : Valid Dna/Rna/Protein Sequences.
        /// Output Data : Validate Sequences for all Alphabet instances.
        /// </summary>
        void ValidateSequenceTypes(AlphabetsTypes option)
        {
            IAlphabet alphabetInstance = null;
            var sequence = "";

            switch (option)
            {
                case AlphabetsTypes.Protein:
                    alphabetInstance = ProteinAlphabet.Instance;
                    sequence = utilityObj.xmlUtil.GetTextValue(Constants.ProteinDerivedSequenceNode,
                                Constants.ExpectedDerivedSequence);
                    break;
                case AlphabetsTypes.Rna:
                    alphabetInstance = RnaAlphabet.Instance;
                    sequence = utilityObj.xmlUtil.GetTextValue(Constants.RnaDerivedSequenceNode,
                                Constants.ExpectedDerivedSequence);
                    break;
                case AlphabetsTypes.Dna:
                    alphabetInstance = DnaAlphabet.Instance;
                    sequence = utilityObj.xmlUtil.GetTextValue(Constants.DnaDerivedSequenceNode,
                                Constants.ExpectedDerivedSequence);
                    break;
            }
                        
            Assert.IsTrue(alphabetInstance.ValidateSequence(Encoding.UTF8.GetBytes(sequence), 0, 4));
            
            ApplicationLog.WriteLine(string.Concat(@"Alphabets BVT: Validation of 
                                Validate Sequence method for ", option, " completed successfully."));
        }

        /// <summary>
        /// Validate ValidateSequence method.
        /// Input Data : Valid Dna/Rna/Protein Sequences.
        /// Output Data : Validate Public properties for all Alphabet instances.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        void ValidatePublicProperties(AlphabetsTypes option)
        {
            IAlphabet alphabetInstance = null;
            var count = 0;
            bool hasGaps = true, hasAmbiguity = true, hasTermination = true, isComplementSupported = true;
            var name = "";

            switch (option)
            {
                case AlphabetsTypes.Protein:
                    alphabetInstance = ProteinAlphabet.Instance;
                    count = 24;
                    hasAmbiguity = false;
                    hasGaps = true;
                    hasTermination = true;
                    isComplementSupported = false;
                    name = "Protein";
                    break;
                case AlphabetsTypes.Rna:
                    alphabetInstance = RnaAlphabet.Instance;
                    count = 5;
                    hasAmbiguity = false;
                    hasGaps = true;
                    hasTermination = false;
                    isComplementSupported = true;
                    name = "Rna";
                    break;
                case AlphabetsTypes.Dna:
                    alphabetInstance = DnaAlphabet.Instance;
                    count = 5;
                    hasAmbiguity = false;
                    hasGaps = true;
                    hasTermination = false;
                    isComplementSupported = true;
                    name = "Dna";
                    break;
            }

            Assert.AreEqual(count, alphabetInstance.Count);
            ApplicationLog.WriteLine(string.Concat(@"Alphabets BVT: Validation of 
                                Count property for ", option, " completed successfully."));
            Assert.AreEqual(hasGaps, alphabetInstance.HasGaps);
            ApplicationLog.WriteLine(string.Concat(@"Alphabets BVT: Validation of 
                                HasGaps property for ", option, " completed successfully."));
            Assert.AreEqual(hasAmbiguity, alphabetInstance.HasAmbiguity);
            ApplicationLog.WriteLine(string.Concat(@"Alphabets BVT: Validation of 
                                HasAmbiguity property for ", option, " completed successfully."));
            Assert.AreEqual(hasTermination, alphabetInstance.HasTerminations);
            ApplicationLog.WriteLine(string.Concat(@"Alphabets BVT: Validation of 
                                HasTermination property for ", option, " completed successfully."));
            Assert.AreEqual(isComplementSupported, alphabetInstance.IsComplementSupported);
            ApplicationLog.WriteLine(string.Concat(@"Alphabets BVT: Validation of 
                                IsComplementSupported property for ", option, " completed successfully."));
            Assert.AreEqual(name, alphabetInstance.Name);
            ApplicationLog.WriteLine(string.Concat(@"Alphabets BVT: Validation of 
                                Name property for ", option, " completed successfully."));
        }

        /// <summary>
        /// Validate ValidateSequence method.
        /// Input Data : Valid Dna/Rna/Protein Sequences.
        /// Output Data : Validate Public properties for all Alphabet instances.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        void ValidateGetSymbolValueMap(AlphabetsTypes option)
        {
            IAlphabet alphabetInstance = null;
            byte[] queryReference = null;
            byte inputByte1 = 0, inputByte2 = 0, outputByte1 = 0, outputByte2 = 0;

            switch (option)
            {
                case AlphabetsTypes.Protein:
                    alphabetInstance = ProteinAlphabet.Instance;
                    inputByte1 = (byte)'w';
                    outputByte1 = (byte)'W';
                    inputByte2 = (byte)'e';
                    outputByte2 = (byte)'E';
                    break;
                case AlphabetsTypes.Rna:
                    alphabetInstance = RnaAlphabet.Instance;
                    inputByte1 = (byte)'a';
                    outputByte1 = (byte)'A';
                    inputByte2 = (byte)'u';
                    outputByte2 = (byte)'U';
                    break;
                case AlphabetsTypes.Dna:
                    alphabetInstance = DnaAlphabet.Instance;
                    inputByte1 = (byte)'a';
                    outputByte1 = (byte)'A';
                    inputByte2 = (byte)'t';
                    outputByte2 = (byte)'T';
                    break;
            }

            byte output = 0;
            queryReference = alphabetInstance.GetSymbolValueMap();
            output = queryReference[inputByte1];
            Assert.AreEqual(outputByte1, output);
            output = queryReference[inputByte2];
            Assert.AreEqual(outputByte2, output);
            ApplicationLog.WriteLine(string.Concat(@"Alphabets BVT: Validation of 
                                GetSymbolValueMap method for ", option, " completed successfully."));
        }

        #endregion Supporting Method
    }
}
