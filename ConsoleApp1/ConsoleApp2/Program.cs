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
			
			Metaheuristic metaheuristic = new Metaheuristic();
			Console.WriteLine("aaaa");
			metaheuristic.Generate(80, 32);
			var list = metaheuristic.GenPop(50);

			for(int i = 0; i < 100; i++)
			{
				metaheuristic.Mutate(0.4);
				metaheuristic.CrossOver(0.7);
				metaheuristic.Contest();
				Console.WriteLine(i);
			}
			DNA best = metaheuristic.GetBest();
			Console.WriteLine(metaheuristic.isbh.DNACode);
			Console.WriteLine("");
			Console.WriteLine(best.code.ToString());
			Console.WriteLine(best.Evaluate(32,80, metaheuristic.SpectrumLong).ToString());
			Console.ReadKey();
		}

	}
}
