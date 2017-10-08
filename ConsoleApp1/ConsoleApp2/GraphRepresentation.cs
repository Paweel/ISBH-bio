using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace metaheuristic
{
    class GraphRepresentation
    {
		// overlap, interval
		int[,] graph;
		public int[] Interval { get; }
		SortedDictionary<string, int> spectrumInterval;
		int size;
		public List<string> keys { get; }

		public GraphRepresentation(SortedDictionary<string, int> spectrumInterval)
		{
			size = spectrumInterval.Count;
			graph = new int[size, size];
			Interval = new int[size];
			keys = Enumerable.ToList(spectrumInterval.Keys);
			this.spectrumInterval= spectrumInterval;

			for(int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					graph[i, j] = Overlap(keys[i], keys[j]);
					Interval[i] = spectrumInterval[keys[i]];
				}
			}
		}

		/*
		 * return node, value, interval tuple
		 */
		public List<Tuple<int, int, int>> GetNext(int node)
		{
			List<Tuple<int, int, int>> result = new List<Tuple<int, int, int>>();
			for(int i = 0; i < size; i++)
			{
				result.Add(new Tuple<int ,int, int>(i, graph[node, i], Interval[i]));
			}
			result.Sort((x, y) => x.Item2.CompareTo(y.Item2));
			return result;
		}

		public int Overlap(String s1, String s2)
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
			return min; //???
		}
	}
}
