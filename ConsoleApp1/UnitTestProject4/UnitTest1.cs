using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Solver;

namespace BIOTest
{
   [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestGenerateComplementaryDNA()
        {
            ISBHInstance i = new Solver.ISBHInstance();
            var s1 = i.GenerateDNA(14);
            i.DNACode = s1;
            var s2 = i.GenerateComplementary();
            Assert.AreNotEqual(s1, s2);
            i.DNACode = s2;
            var s3 = i.GenerateComplementary();
            Assert.AreEqual(s1, s3);
        }

        [TestMethod]
        public void TestSpectrum()
        {
            ISBHInstance i = new Solver.ISBHInstance();
            var s1 = "AATTCGATGATGG";
            i.DNACode = s1;
            i.GenerateSpectrum(10);
            Assert.IsTrue(i.Spectrum.ContainsKey("ATTC"), "1");
            Assert.IsTrue(i.Spectrum.ContainsKey("TCG"), "2");
            Assert.IsTrue(i.Spectrum.ContainsKey("CGA"), "3");
            Assert.IsTrue(i.Spectrum.ContainsKey("TGG"), "4");

        }

        [TestMethod]
        public void TestSpectrumInterval()
        {
            ISBHInstance i = new Solver.ISBHInstance();
            var s1 = "AATTAATTAATTAATTCGATGATGGGGGGGGGGGGG";
            i.DNACode = s1;
            i.GenerateSpectrum(8);
            i.Interval();
            /* Intervals >7 - 2
            *  <6,4> - 1
            *  <3,1> - 0
            *  0 - 3 <- indicates error
            */
            Assert.AreEqual((UInt32)1, i.SpectrumInterval["AATT"], "1");
            Assert.AreEqual((UInt32)0, i.SpectrumInterval["TTAA"], "2");
            Assert.AreEqual((UInt32)2, i.SpectrumInterval["GG"], "3");
        }

		[TestMethod]
		public void TestSpectrumIntervalCompl()
		{
			ISBHInstance i = new Solver.ISBHInstance();
			var s1 = "ATCTCATCGATGATCATCGATC";
			i.DNACode = s1;
			i.GenerateSpectrum(8);
			i.Interval();
			/* Intervals >7 - 2
            *  <6,4> - 1
            *  <3,1> - 0
            *  0 - 3 <- indicates error
            */
			i.DNACode = i.GenerateComplementary();
			i.GenerateSpectrum(8, false);
			i.Interval();
			Assert.AreEqual((UInt32)1, i.SpectrumInterval["ATC"], "1");
			Assert.AreEqual((UInt32)1, i.SpectrumInterval["TAG"], "2");
			Assert.AreEqual((UInt32)0, i.SpectrumInterval["GAT"], "3");
			Assert.AreEqual((UInt32)0, i.SpectrumInterval["CTA"], "4");

		}

		[TestMethod]
        public void TestNegativeErrorInterval()
        {
            ISBHInstance i = new Solver.ISBHInstance();
            var s1 = i.GenerateDNA(200);
            i.DNACode = s1;
            i.GenerateSpectrum(10);
            i.Interval();
            UInt32 count1 = 0, count2 = 0;
            foreach(var v in i.SpectrumInterval.Values)
            {
                count1 += v; 
            }
            i.GenerateNegativeErrorInterval(2);
            foreach(var v in i.SpectrumInterval.Values)
            {
                count2 += v; 
            }
            Assert.AreEqual(count1 - 2, count2);
        }

    }
}
