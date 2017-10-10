using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace metaheuristic
{
    class GraphRepresentation
    {
		// overlap, interval
		public int[,] graph { get; }
		// complementary min max
		public List<LookupRow> Interval { get; }
		public HiddenNodes[,] HiddenNodesArray { get; set; }
		SortedDictionary<string, int> spectrumInterval;
		public int size { get; }
		public List<string> Keys { get; }

		public GraphRepresentation(GraphRepresentation graph)
		{
			this.graph = graph.graph;
			Interval = graph.Interval;
			spectrumInterval = graph.spectrumInterval;
			size = graph.size;
			Keys = graph.Keys;
			HiddenNodesArray = graph.HiddenNodesArray;

		}

		public GraphRepresentation(SortedDictionary<string, int> spectrumInterval, int temperature)
		{
			size = spectrumInterval.Count;

			Interval = new List<LookupRow>((int)(size * 1.5));
			Keys = Enumerable.ToList(spectrumInterval.Keys);
			this.spectrumInterval = spectrumInterval;

			for (int i = 0; i < size; i++)
			{
				Interval.Add(new LookupRow());
				Interval[i].min = CommonDNAOperations.IntToMin(spectrumInterval[Keys[i]]);
				Interval[i].max = CommonDNAOperations.IntToMax(spectrumInterval[Keys[i]]);
			}
			//complementary
			for (int i = 0; i < size; i++)
			{
				string complOligo = CommonDNAOperations.Complementary(Keys[i]);
				int compl = Keys.FindIndex(s => s == complOligo);
				if (compl == -1)
				{
					//not found
					Keys.Add(complOligo);
					LookupRow lookup = new LookupRow
					{
						complementary = i,
						min = 0,
						max = CommonDNAOperations.IntToMax(0)
					};
					Interval.Add(lookup);
					Interval[i].complementary = Interval.Count - 1;
				}
				else
				{
					Interval[i].complementary = compl;
				}
			}
			size = Interval.Count;
			graph = new int[size, size];

			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					graph[i, j] = Overlap(Keys[i], Keys[j]);
				}
			}

			HiddenNodes(temperature);
		}

		public void HiddenNodes(int temperature)
		{
			HiddenNodesArray = new HiddenNodes[size, size];

			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					HiddenNodes node = new HiddenNodes();
					StringBuilder temp = new StringBuilder(Keys[i]);
					Common.Merge(temp, Keys[j]);
					var spectrum = CommonDNAOperations.GenerateSpectrum(temp.ToString(), temperature);
					foreach(var v in spectrum)
					{
						int index = Keys.FindIndex(s => s == v.Key);
						if (index == -1)
							node.penalty++;
						else
							node.nodes.Add(index);
					}
					HiddenNodesArray[i, j] = node;
				}
			}
		}

		/*
		 * return node, value, interval tuple
		 */
		public List<Tuple<int, int, LookupRow>> GetNext(int node)
		{
			List<Tuple<int, int, LookupRow>> result = new List<Tuple<int, int, LookupRow>>();
			for(int i = 0; i < size; i++)
			{
				result.Add(new Tuple<int ,int, LookupRow>(i, graph[node, i], Interval[i]));
			}
			result.Sort((x, y) => x.Item2.CompareTo(y.Item2));
			return result;
		}
		
		public int Overlap(String s1, String s2)
		{
			if (s1 == s2)
				return 10000;
			Boolean success = true;
			int min = Math.Min(s1.Length, s2.Length);
			int start = Math.Max(s1.Length - s2.Length, 0);
			for (int i = start; i < s1.Length; i++)
			{
				success = true;
				for (int j = 0; j < s1.Length - i; j++)
				{
					if (s1[j + i] != s2[j])
					{
						success = false;
						break;
					}
				}
				if (success)
					return s2.Length - (s1.Length - i);
			}
			return s2.Length; //???
		}
	}
}
