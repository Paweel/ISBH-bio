using Solver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace metaheuristic
{
    class Program
    {
		
		static void Main(string[] args)
        {
			int length = 120;
			NewMeta metaheuristic = new NewMeta(length, 32);
			Console.WriteLine("aaaa");
			var list = metaheuristic.GenPop(800);

			for(int i = 0; i < 2000; i++)
			{
				metaheuristic.Mutate(0.2);
				metaheuristic.CrossOver(0.7);
				metaheuristic.Contest();
				Console.WriteLine(i);
			}
			NodeRepresentation best = metaheuristic.GetBest();
			Console.WriteLine(metaheuristic.isbh.DNACode);
			Console.WriteLine("");
			Console.WriteLine(best.ToDna(metaheuristic.graph).code.ToString());
			Console.WriteLine(best.Evaluate(metaheuristic.graph, length).ToString());
			Console.ReadKey();
		}

	}
}
