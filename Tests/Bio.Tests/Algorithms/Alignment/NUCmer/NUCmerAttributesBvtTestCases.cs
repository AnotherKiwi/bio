using System.Collections.Generic;
using Bio.Algorithms.Alignment;
using NUnit.Framework;

namespace Bio.Tests.Algorithms.Alignment.NUCmer
{
    /// <summary>
    /// Test Automation code for Bio Sequences and BVT level validations.
    /// </summary>
    [TestFixture]
    public class NUCmerAttributesBvtTestCases
    {
        /// <summary>
        /// Validate the attributes in NUCmerAttributes.
        /// </summary>
        [Test]
        [Category("Priority0")]
        public void ValidateNUCmerAttributes()
        {
            var nucAttrib = new NUCmerAttributes();
            var attributes = nucAttrib.Attributes;

            Assert.AreEqual(9, attributes.Count);

            // Validating all the NUCmer attributes
            var similarityMatrixObj = attributes["SIMILARITYMATRIX"];
            var gapOpenCostObj = attributes["GAPOPENCOST"];
            var gapExtensionCostObj = attributes["GAPEXTENSIONCOST"];

            var lenOfMumObj = attributes["LENGTHOFMUM"];
            var fixedSepObj = attributes["FIXEDSEPARATION"];
            var maxSepObj = attributes["MAXIMUMSEPARATION"];

            var minScoreObj = attributes["MINIMUMSCORE"];
            var sepFactorObj = attributes["SEPARATIONFACTOR"];
            var breakLengthObj = attributes["BREAKLENGTH"];

            Assert.AreEqual("Similarity Matrix", similarityMatrixObj.Name);
            Assert.AreEqual("Gap Cost", gapOpenCostObj.Name);
            Assert.AreEqual("Gap Extension Cost", gapExtensionCostObj.Name);

            Assert.AreEqual("Length of MUM", lenOfMumObj.Name);
            Assert.AreEqual("Fixed Separation", fixedSepObj.Name);
            Assert.AreEqual("Maximum Separation", maxSepObj.Name);

            Assert.AreEqual("Minimum Score", minScoreObj.Name);
            Assert.AreEqual("Separation Factor", sepFactorObj.Name);
            Assert.AreEqual("Break Length", breakLengthObj.Name);
        }
    }
}

