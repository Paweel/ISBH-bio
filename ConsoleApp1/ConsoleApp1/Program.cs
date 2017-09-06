using BIO;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Solver solver = new Solver();
            solver.generate(20, 32);
            solver.solve();
			solver.PrintResults();
            Console.ReadKey();
        }
    }
}