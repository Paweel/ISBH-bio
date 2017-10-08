using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CommonDNAOperations;

namespace metaheuristic
{
    class Generator
    {
		GraphRepresentation graph;
		//actual visit count
		int[] nodes;
		int size;
		int dnaSize;
		int minNode;
		List<int> result;
		string firstOligo;
		static Random random = new Random();

		public Generator(SortedDictionary<string, int> spectrumInterval, int dnaSize, string firstOligo)
		{
			size = spectrumInterval.Count;
			graph = new GraphRepresentation(spectrumInterval);
			nodes = new int[size];
			this.dnaSize = dnaSize;
			minNode = graph.Interval.Sum();
			this.firstOligo = firstOligo;
		}

		//answers as nodes list
		public List<int> Generate()
		{

			//clear
			result = new List<int>();
			nodes = new int[size];
			minNode = graph.Interval.Sum();
			//
			int actual = graph.keys.FindIndex(s => s == firstOligo);
			Add(actual);
			var nextNodes = graph.GetNext(actual);
			
			int listSize = nextNodes.Count;
			int choosen = random.Next(0, Math.Min(5, listSize));
			int first = choosen;

			while (result.Count < dnaSize * 1.2)
			{
				Tuple<int, int, int> nextNode = nextNodes[choosen];
				if (nodes[nextNode.Item1] + 1 > IntToMax(nextNode.Item3))
				{
					choosen = (choosen + 1) % listSize;
					//if (first == choosen) 
						//revert
					continue;
				}
				
				actual = nextNode.Item1;
				Add(actual);
				nextNodes = graph.GetNext(actual);
				listSize = nextNodes.Count;
				choosen = random.Next(0, Math.Min(5, listSize));
				first = choosen;
			}
			return result;
		}

		private void Add(int node)
		{
			nodes[node]++;
			result.Add(node);
		}

		public DNA ToDna(List<int> list)
		{
			StringBuilder code = new StringBuilder(graph.keys[list[0]]);
			for(int i = 1; i < list.Count; i++)
			{
				Common.Merge(code, graph.keys[list[i]]);
			}
			return new DNA(code);
		}
		//bool minconstraint(tuple<int, int, int> nextnode)
		//{
		//	if (nodes[nextnode.item1] + 1 <= inttomin(nextnode.item3))
		//		minnode--;
		//}
	}
}
