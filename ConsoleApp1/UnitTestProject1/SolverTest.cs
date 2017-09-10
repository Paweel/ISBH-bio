using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BIO;
using System.Reflection;
using System.Collections.Generic;

namespace SolverTest
{
	[TestClass]
	public class SolverTest
	{
		[TestMethod]
		void FindLastTwoOligosTest()
		{
			Solver solver = new Solver();
			solver.generate(32, 26);
			String s = "ATTAATATATAGCGATGAGACTAG";
			MethodInfo methodInfo = typeof(Solver).GetMethod("FindLastTwoOligo", BindingFlags.NonPublic | BindingFlags.Instance);
			object[] parameters = { s };
			List<Tuple<string, int>> list = (List<Tuple<string, int>>)methodInfo.Invoke(solver, parameters);
			Assert.AreEqual("GAGACTAG", list[0].Item1);
			Assert.AreEqual(24, list[0].Item2);
			Assert.AreEqual("TGAGACTAG", list[1].Item1);
			Assert.AreEqual(26, list[1].Item2);

		}
	}
}
