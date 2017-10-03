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

			for(int i = 0; i < 30; i++)
			{
				metaheuristic.Mutate(0.2);
				metaheuristic.CrossOver(0.5);
				metaheuristic.Contest();
				Console.WriteLine(i);
			}
        }

	}
}
