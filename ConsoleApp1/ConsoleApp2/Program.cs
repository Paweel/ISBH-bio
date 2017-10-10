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
			
			NewMeta metaheuristic = new NewMeta(80, 32);
			Console.WriteLine("aaaa");
			var list = metaheuristic.GenPop(400);

			for(int i = 0; i < 20000; i++)
			{
				metaheuristic.Mutate(1.0);
				//metaheuristic.CrossOver(0.7);
				metaheuristic.Contest();
				Console.WriteLine(i);
			}
			NodeRepresentation best = metaheuristic.GetBest();
			Console.WriteLine(metaheuristic.isbh.DNACode);
			Console.WriteLine("");
			Console.WriteLine(best.ToDna(metaheuristic.graph).code.ToString());
			Console.WriteLine(best.Evaluate(metaheuristic.graph, 80).ToString());
			Console.ReadKey();
		}

	}
}
