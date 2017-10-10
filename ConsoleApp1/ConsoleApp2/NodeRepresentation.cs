using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace metaheuristic
{
    class NodeRepresentation
    {
		List<int> code;
		GraphRepresentation graph;
		static Random random = new Random();
		public NodeRepresentation(List<int> code, GraphRepresentation graph)
		{
			this.code = new List<int>(code);
			this.graph = graph;
		}

		public NodeRepresentation Mutate(int amount)
		{
			NodeRepresentation temp = new NodeRepresentation(code, graph);
			
			for (int i = 0; i < amount; i++)
			{
				int choice = random.Next(0, 2);
				int position = random.Next(0, temp.code.Count - 1);
				switch (choice)
				{
					//delete
					case 0:
						temp.code.Remove(position);
						break;
					//add
					case 1:
						int node = random.Next(0, graph.Keys.Count);
						temp.code.Insert(position, node);
						break;
					//
					case 2:
						int position2 = random.Next(0, temp.code.Count);
						int c = temp.code[position];
						temp.code[position] = temp.code[position2];
						temp.code[position2] = c;
						break;
				}
			}
			return temp;
		}

		public NodeRepresentation CrossOver(NodeRepresentation node2)
		{
			List<int> newCode = new List<int>();
			int index = 0;
			int amount = random.Next(1, node2.code.Count / 2);
			List<int> temp = node2.code;
			bool cont = true;
			while(cont)
			{
				if (index + amount > temp.Count)
				{
					amount = temp.Count - index;
					if (amount < 1)
						break;
					cont = false;
				}
					
				newCode.InsertRange(index, temp.GetRange(index, amount));
				index += amount;
				amount = random.Next(1, temp.Count / 2);
				if (temp == node2.code)
					temp = code;
				else
					temp = node2.code;
			}

			return new NodeRepresentation(newCode, graph);
		}

		private int CodeLength()
		{
			return CodeLength(graph, code);
		}
		public static int CodeLength(GraphRepresentation graph, List<int> code)
		{
			int sum = graph.Keys[code[0]].Length;
			for(int i = 0; i < code.Count - 1; i++)
			{
				sum += graph.graph[code[i], code[i + 1]];
			}
			return sum;
		}
		public int Evaluate(GraphRepresentation graph, int dnaLength)
		{
			int points = Math.Abs(CodeLength() - dnaLength);
			int[] eval = new int[graph.size];
			foreach(int i in code)
			{
				//count quantity of each node
				eval[i]++;
			}

			for(int i = 0; i < code.Count - 1; i++)
			{
				points += graph.HiddenNodesArray[code[i], code[i + 1]].penalty;
				foreach(var v in graph.HiddenNodesArray[code[i], code[i + 1]].nodes)
				{
					eval[v]++;
				}
			}

			for(int i = 0; i < eval.Length; i++)
			{
				//too less
				if (graph.Interval[i].min > eval[i])
					points += graph.Interval[i].min - eval[i];
				//too many
				else if (graph.Interval[i].max < eval[i])
					points += eval[i] - graph.Interval[i].max;
			}
			return points;
		}

		public DNA ToDna(GraphRepresentation graph)
		{
			StringBuilder codeS = new StringBuilder(graph.Keys[code[0]]);
			for (int i = 1; i < code.Count; i++)
			{
				Common.Merge(codeS, graph.Keys[code[i]]);
			}
			return new DNA(codeS);
		}

		public void Merge(StringBuilder code1, StringBuilder code2)
		{
			int position = Common.Overlap(code1, code2);

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

	}
}
