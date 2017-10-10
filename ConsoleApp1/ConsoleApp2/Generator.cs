using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CommonDNAOperations;

namespace metaheuristic
{
    class Generator
    {

		public GraphRepresentation graph { get; }
		//actual visit count
		int[] nodes;
		int size;
		int dnaSize;
		int minNode;
		List<int> result;
		string firstOligo;
		static Random random = new Random();

		public Generator(SortedDictionary<string, int> spectrumInterval, int dnaSize, string firstOligo, int temp)
		{
			
			graph = new GraphRepresentation(spectrumInterval, temp);
			size = graph.size;
			nodes = new int[size];
			this.dnaSize = dnaSize;
			//minNode = graph.Interval.Sum();
			this.firstOligo = firstOligo;
		}

		//answers as nodes list
		public List<int> Generate(double percentSize = 1)
		{

			//clear
			result = new List<int>();
			nodes = new int[size];
			//minNode = graph.Interval.Sum();
			//
			int actual = graph.Keys.FindIndex(s => s == firstOligo);
			Add(actual);
			var nextNodes = graph.GetNext(actual);
			
			int listSize = nextNodes.Count;
			int choosen = random.Next(0, Math.Min(5, listSize));
			int first = choosen;

			while (NodeRepresentation.CodeLength(graph, result) < dnaSize * percentSize)
			{
				Tuple<int, int, LookupRow> nextNode = nextNodes[choosen];
				if (nodes[nextNode.Item1] + 1 > nextNode.Item3.max)
				{
					choosen = (choosen + 1) % listSize;
					//if (first == choosen) 
						//revert
					continue;
				}
				if (nextNode.Item1 == actual)
				{
					//cannot went to same node
					choosen = (choosen + 1) % listSize;
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
			nodes[graph.Interval[node].complementary]++;
			result.Add(node);
		}

		public DNA ToDna(List<int> list)
		{
			StringBuilder code = new StringBuilder(graph.Keys[list[0]]);
			for(int i = 1; i < list.Count; i++)
			{
				Common.Merge(code, graph.Keys[list[i]]);
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
