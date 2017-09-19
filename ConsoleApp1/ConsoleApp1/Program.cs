using Solver;
using System;

namespace BIO
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
			Solver.Solver solver = new Solver.Solver();
            solver.generate(32, 32);
            solver.solve();
			solver.PrintResults();
            Console.ReadKey();
        }
    }
}