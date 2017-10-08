using System;
using System.Collections.Generic;
using System.Text;
using BIO;


namespace metaheuristic
{
    class DNA
    {
		public StringBuilder code;

		public DNA(DNA dna)
		{
			this.code = new StringBuilder(dna.code.ToString());
		}

		public DNA(StringBuilder stringBuilder)
		{
			this.code = stringBuilder;
		}

		public StringBuilder Mutate(Random random, int amount = 1)
		{
			StringBuilder temp = new StringBuilder(code.ToString());
			int choice = random.Next(0,2);
			for(int i = 0; i < amount; i++)
			{
				int position = random.Next(0, temp.Length - 1);
				switch (choice)
				{
					//delete
					case 0:
						temp.Remove(position, 1);
						break;
					//add
					case 1:
						int nucleo = random.Next(0, 3);
						temp.Insert(position, CommonDNAOperations.DNA[nucleo]);
						break;
					//
					case 2:
						int position2 = random.Next(0, temp.Length);
						char c = temp[position];
						temp[position] = temp[position2];
						temp[position2] = c;
						break;
				}
			}
			return temp;
		}	

		public StringBuilder Crossover(DNA DNA2, Random random)
		{
			StringBuilder result = new StringBuilder(DNA2.code.ToString());
			StringBuilder temp = new StringBuilder(code.ToString());

			int dividePoint = random.Next(1, 9);
			result.Length = (int)(result.Length * (dividePoint / 10.0));
			temp.Length = (int)(temp.Length * ((10 - dividePoint) / 10.0));
			Merge(result, temp);

			return result;
		}

		public void Merge(StringBuilder code1, StringBuilder code2)
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

		public int Overlap(StringBuilder s1, StringBuilder s2)
		{
			Boolean success = true;
			int min = Math.Min(s1.Length, s2.Length);
			for(int i = 0; i < min; i++)
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

		public int Evaluate(int temperature, int length, SortedDictionary<String, int> spectrumInterval)
		{
			int points = Math.Abs(length - code.Length);
			SortedDictionary<String, int> spectrum = CommonDNAOperations.GenerateSpectrum(code.ToString(), temperature);
			SortedDictionary<String, int> spectrumCompl = CommonDNAOperations.GenerateSpectrum(CommonDNAOperations.Complementary(code.ToString()), temperature);
			SortedDictionary<String, int> interval = CommonDNAOperations.Interval(CommonDNAOperations.MergeSpectrum(spectrum, spectrumCompl));

			points += CommonDNAOperations.SpectrumDiffInPlace(interval, spectrumInterval);
			return points;
		}
    }



}
