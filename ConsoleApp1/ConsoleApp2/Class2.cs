using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Linq;
using System.IO;
using CsvHelper;

namespace metaheuristic
{
    class Class2
    {
		public static void Test()
		{
			TextWriter SWfile = new StreamWriter("result.csv");
			var SWcsv = new CsvWriter(SWfile);
			List<TimeSpan> listSW = new List<TimeSpan>();
			List<double> listSWI = new List<double>();
			for (int count = 100; count <= 300; count += 50)
			{
				for (int errors = 2; errors <= 6; errors+=2)
				{
					for (int i = 0; i < 20; i++)
					{
						Tuple<TimeSpan, double> SW = TestRun(count, (int)((errors*0.05)*count), 400);
						listSW.Add(SW.Item1);
						listSWI.Add(SW.Item2);
					}
					double doubleAverageTicks = listSW.Average(timeSpan => timeSpan.Ticks);
					long longAverageTicks = Convert.ToInt64(doubleAverageTicks);
					TimeSpan SWw = new TimeSpan(longAverageTicks);
					Console.WriteLine("Avg. Time = {0}", SWw);
					Console.WriteLine("Avg. Best = {0}", listSWI.Average());
					Console.WriteLine("Count = {0}", count);
					Console.WriteLine("Errors = {0}", errors * 0.05);
					Tuple<TimeSpan, double, int, double> SWtuple = new Tuple<TimeSpan, double, int, double>(SWw, listSWI.Average(), count, (errors*0.05));
					SWcsv.WriteRecord(SWtuple);
					SWcsv.NextRecord();
					//SWcsv.Flush();
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
			double result = -1;
			sw.Start();
			result = TestMeta(length, errors, pop, iterations);
			sw.Stop();
			return new Tuple<TimeSpan, double>(sw.Elapsed, result);
		}

		static double TestMeta(int length = 100, int errors = 3, int pop = 800, int iterations = 2000)
		{
			NewMeta metaheuristic = new NewMeta(length, 32, errors);
			//Console.WriteLine("xD");
			var list = metaheuristic.GenPop(pop);
			NodeRepresentation best = metaheuristic.GetBest();
			int old_best = best.Evaluate(metaheuristic.graph, length);
			int old_i = 0;
			int contest = 0;
			double mutation = 0.2;
			bool end = false;
			for (int i = 0; i < iterations; i++)
			{
				metaheuristic.Mutate(mutation);
				metaheuristic.CrossOver(0.7);
				if (contest-- <= 0)
				{
					metaheuristic.Contest();
					mutation = 0.2;
					metaheuristic.MutationAmount = length / 10;
				}
					
				//Console.WriteLine(i);
				if (i > 100 + old_i)
				{
					old_i = i;
					best = metaheuristic.GetBest();
					int new_best = best.Evaluate(metaheuristic.graph, length);
					if (old_best == new_best)
					{
						contest = 5;
						if (!end)
						{
							end = true;
							mutation = 0.7;
							metaheuristic.MutationAmount = length / 5;
						}
						else
							break;
					}
					old_best = new_best;
				}
			}
			best = metaheuristic.GetBest();
			//Console.WriteLine(metaheuristic.isbh.DNACode);
			//Console.WriteLine("");
			//Console.WriteLine(best.ToDna(metaheuristic.graph).code.ToString());
			//Console.WriteLine(best.Evaluate(metaheuristic.graph, length).ToString());
			//Console.WriteLine(LevenshteinDistance.Compute(best.ToDna(metaheuristic.graph).code.ToString(), metaheuristic.isbh.DNACode));
			//Console.WriteLine(LevenshteinDistance.Compute(CommonDNAOperations.Complementary(best.ToDna(metaheuristic.graph).code.ToString()), metaheuristic.isbh.DNACode));
			SimMetricsMetricUtilities.SmithWatermanGotoh xd = new SimMetricsMetricUtilities.SmithWatermanGotoh();
			return xd.GetSimilarity(CommonDNAOperations.Complementary(best.ToDna(metaheuristic.graph).code.ToString()), metaheuristic.isbh.DNACode);
			//return LevenshteinDistance.Compute(CommonDNAOperations.Complementary(best.ToDna(metaheuristic.graph).code.ToString()), metaheuristic.isbh.DNACode);
		}
    }
}
