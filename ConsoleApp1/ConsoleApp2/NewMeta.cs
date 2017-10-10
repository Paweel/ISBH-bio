using Solver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace metaheuristic
{
	class NewMeta
	{
		private static Random random = new Random();
		public ISBHInstance isbh = new ISBHInstance();
		string orginalDNA;
		string first;
		int length;

		public int MutationAmount {get; set;}
		public double CrossOverSize { get; set; }
		public double CrossOverPercent { get; set; }
		public double MutationPercent { get; set; }
		public Generator generator { get; }
		public GraphRepresentation graph { get; }
		int temperature;
		List<NodeRepresentation> population;
		public SortedDictionary<string, int> SpectrumLong;
		SortedDictionary<string, int> SpectrumShort;

		public NewMeta(int length, int temp, int errors = 3)
		{
			Generate(length, temp, errors);
			generator = new Generator(SpectrumLong, length, first, temp);
			graph = generator.graph;
			CrossOverSize = 0.5;
			MutationAmount = length / 10;
		}
		private void Generate(int length, int temp, int errors = 3)
		{
			this.temperature = temp;
			this.length = length;
			isbh.BuildComplDNAInterval(length, temp);
			isbh.GenerateNegativeErrorInterval(errors);
			orginalDNA = isbh.DNACode;
			first = isbh.first;
			SpectrumLong = isbh.SpectrumInterval;
			isbh.BuildComplDNAInterval(length, temp - 2, true);
			isbh.GenerateNegativeErrorInterval(errors);
			if (first == "")
				first = CommonDNAOperations.Complementary(isbh.first);
			SpectrumShort = isbh.SpectrumInterval;
		}

		public List<NodeRepresentation> GenPop(int amount)
		{	
			population = new List<NodeRepresentation>();
			for (int i = 0; i < amount; i++)
			{
				population.Add(new NodeRepresentation(generator.Generate(random.NextDouble() * 0.2 + 0.9), graph));
			}
			return population;
		}

		public void Mutate(Double percent)
		{
			List<NodeRepresentation> list = new List<NodeRepresentation>();
			foreach (var dna in population)
			{
				if (random.NextDouble() < percent)
					list.Add(dna.Mutate(MutationAmount));
			}
			population.AddRange(list);
		}

		public void CrossOver(double percent)
		{
			List<NodeRepresentation> list = new List<NodeRepresentation>();
			foreach (var dna in population)
			{
				if (random.NextDouble() < percent)
				{
					list.Add(dna.CrossOver(population[random.Next(0, population.Count)], CrossOverSize));
				}
			}
			population.AddRange(list);
		}
		
		public void Contest()
		{
			int[] array = new int[4];
			List<NodeRepresentation> newPop = new List<NodeRepresentation>();
			if (population.Count <= 400)
				return;
			Shuffle(population);
			for (int i = 0; i < population.Count; i += 4)
			{
				int min = int.MaxValue;
				int minPosition = 0;
				for (int j = 0; j < 4; j++)
				{
					if (i + j >= population.Count)
						break;
					array[j] = population[i + j].Evaluate(graph, length);
					if (array[j] < min)
					{
						min = array[j];
						minPosition = i + j;
					}
				}
				newPop.Add(population[minPosition]);
			}
			population = newPop;

			if (population.Count >= 800)
				Contest();
		}
		public static void Shuffle<T>(IList<T> list)
		{
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = random.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		public List<TKey> RandomList<TKey, TValue>(IDictionary<TKey, TValue> dict)
		{
			List<TKey> keys = Enumerable.ToList(dict.Keys);
			Shuffle(keys);
			return keys;
		}

		public NodeRepresentation GetBest()
		{
			NodeRepresentation best = null;
			int min = int.MaxValue;
			int temp;
			foreach (var v in population)
			{
				temp = v.Evaluate(graph, length);
				if (temp < min)
				{
					best = v;
					min = temp;
				}
			}
			return best;
		}
	}
}

