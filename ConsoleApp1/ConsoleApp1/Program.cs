using CsvHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace BIO
{
	class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
			Test();
        }

		public static void Test()
		{
			TextWriter SWfile = new StreamWriter("result-dokladny-xd-xd.csv");
			var SWcsv = new CsvWriter(SWfile);
			List<TimeSpan> listSW = new List<TimeSpan>();
			List<double> listSWI = new List<double>();
			for (int count = 20; count <= 36; count += 4)
			{
				for (int errors = 9; errors <= 9; errors += 3)
				{
					for (int i = 0; i < 10; i++)
					{
						Tuple<TimeSpan, double> SW = TestRun(count, errors, 400);
						listSW.Add(SW.Item1);
						listSWI.Add(SW.Item2);
					}
					double doubleAverageTicks = listSW.Average(timeSpan => timeSpan.Ticks);
					long longAverageTicks = Convert.ToInt64(doubleAverageTicks);
					TimeSpan SWw = new TimeSpan(longAverageTicks);
					Console.WriteLine("Avg. Time = {0}", SWw);
					Console.WriteLine("Avg. Best = {0}", listSWI.Average());
					Console.WriteLine("Count = {0}", count);
					Console.WriteLine("Errors = {0}", errors);
					Tuple<TimeSpan, double, int, double> SWtuple = new Tuple<TimeSpan, double, int, double>(SWw, listSWI.Average(), count, (errors * 0.05));
					SWcsv.WriteRecord(SWtuple);
					SWcsv.NextRecord();
					listSW.Clear();
					listSWI.Clear();
				}
			}
			SWfile.Flush();
			SWfile.Close();
		}

		static Tuple<TimeSpan, double> TestRun(int length = 100, int errors = 3, int pop = 100, int iterations = 2000)
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();
			Solver.Solver solver = new Solver.Solver();
			solver.generate(length, 32, errors);
			solver.solve();
			if (solver.bagOfResults.FindIndex(s => s == solver.orginalDNA) == -1)
			{
				Console.WriteLine("XD nie pykło");
			}
					
			sw.Stop();
			return new Tuple<TimeSpan, double>(sw.Elapsed, 1);
		}
	}
}