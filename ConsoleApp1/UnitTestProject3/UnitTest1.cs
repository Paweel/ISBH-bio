using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Solver;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

namespace SolverTest
{
	[TestClass]
	public class SolverTest
	{
		[TestMethod]
		public void FindLastTwoOligosTest()
		{
			Solver.Solver solver = new Solver.Solver();
			solver.generate(32, 26);
			StringBuilder s = new StringBuilder("ATTAATATATAGCGATGAGACTAG");
			MethodInfo methodInfo = typeof(Solver.Solver).GetMethod("FindLastTwoOligos", BindingFlags.NonPublic | BindingFlags.Instance);
			object[] parameters = { s };
			List<Tuple<string, int>> list = (List<Tuple<string, int>>)methodInfo.Invoke(solver, parameters);
			Assert.AreEqual("GAGACTAG", list[0].Item1);
			Assert.AreEqual(24, list[0].Item2);
			Assert.AreEqual("TGAGACTAG", list[1].Item1);
			Assert.AreEqual(26, list[1].Item2);
		}

		[TestMethod]
		public void TestSolve()
		{
			for(int i = 0; i < 10; i++)
			{
				Solver.Solver solver = new Solver.Solver();
				solver.generate(32, 26);
				solver.solve();
				Assert.IsTrue(solver.bagOfResults.Contains(solver.orginalDNA), "\n" + solver.orginalDNA + "\n" + print(solver.bagOfResults));
			}
		}

		private string print(List<String> b)
		{
			StringBuilder s = new StringBuilder();
			foreach(var v in b)
			{
				s.Append(v);
				s.Append('\n');
			}
			return s.ToString();
		}
	}
}
