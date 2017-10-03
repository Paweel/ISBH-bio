using Solver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace metaheuristic
{
    class Metaheuristic
    {

		private static Random random = new Random();
		private ISBHInstance isbh = new ISBHInstance();
		string orginalDNA;
		string first;
		int length;
		int temperature;
		List<DNA> population;
		SortedDictionary<string, int> SpectrumLong;
		SortedDictionary<string, int> SpectrumShort;
		public void Generate(int length, int temp)
		{
			this.temperature = temp;
			this.length = length;
			isbh.BuildComplDNAInterval(length, temp);
			isbh.GenerateNegativeErrorInterval(3);
			orginalDNA = isbh.DNACode;
			first = isbh.first;
			SpectrumLong = isbh.SpectrumInterval;
			isbh.BuildComplDNAInterval(length, temp - 2, true);
			isbh.GenerateNegativeErrorInterval(3);
			if (first == "")
				first = CommonDNAOperations.Complementary(isbh.first);
			SpectrumShort = isbh.SpectrumInterval;
		}

		public List<DNA> GenPop(int amount)
		{
			population = new List<DNA>();
			for (int i = 0; i < amount; i++)
			{
				population.Add(quickGen(SpectrumLong));
			}
			return population;
		}

		public void Mutate(Double percent)
		{
			foreach(var dna in population)
			{
				if (random.NextDouble() < percent)
					dna.Mutate(random);
			}
		}

		public void CrossOver(Double percent)
		{
			foreach (var dna in population)
			{
				if (random.NextDouble() < percent)
				{
					population.Add(new DNA(population[random.Next(0, population.Count)]));
					dna.Crossover(population[random.Next(0, population.Count)], random);
				}
			}
		}

		public void Contest()
		{
			int[] array = new int[4];
			List<DNA> newPop = new List<DNA>();
			Shuffle(population);
			for(int i = 0; i < population.Count; i+=4)
			{
				int min = int.MaxValue;
				int minPosition = 0;
				for(int j = 0; j < 4; j ++)
				{
					if (i * 4 + j > population.Count)
						break;
					array[j] = population[i * 4 + j].Evaluate(temperature, length, SpectrumLong);
					if (array[j] < min)
					{
						min = array[j];
						minPosition = i * 4 + j;
					}
					newPop.Add(population[minPosition]);
				}
			}
			population = newPop;
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

		DNA quickGen(SortedDictionary<string, int> spectrum)
		{
			var oligos = RandomList(spectrum);
			StringBuilder code = new StringBuilder(isbh.first);
			oligos.Remove(isbh.first);
			foreach (var s in oligos)
			{
				Merge(code, s);
			}
			DNA dna = new DNA(code);
			return dna;
			
		}


		public void Merge(StringBuilder code1, String code2)
		{
			int position = Overlap(code1, code2);

			if (position == -1)
				code1.Append(code2);
			if (position >= 0)
			{
				if (position + code2.Length > code1.Length)
				{
					code1.Length = position;
					code1.Append(code2);
				}
			}
		}

		public int Overlap(StringBuilder s1, String s2)
		{
			Boolean success = true;
			int min = Math.Min(s1.Length, s2.Length);
			for (int i = 0; i < min; i++)
			{
				success = true;
				for (int j = 0; j < min - i; j++)
				{
					if (s1[j + i] != s2[j])
					{
						success = false;
						break;
					}
				}
				if (success)
					return i;
			}
			return -1;
		}

		public List<TKey> RandomList<TKey, TValue>(IDictionary<TKey, TValue> dict)
		{
			Random rand = new Random();
			List<TKey> keys = Enumerable.ToList(dict.Keys);
			Shuffle(keys);
			return keys;
		}
	}
}
