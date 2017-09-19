using System;
using System.Collections.Generic;
using System.Text;

public class CommonDNAOperations
{
	public static readonly char[] DNA = { 'A', 'T', 'C', 'G' };
	public static readonly Tuple<int, int>[] intervals = { new Tuple<int, int>(1, 3), new Tuple<int, int>(4, 6), new Tuple<int, int>(7 , int.MaxValue) };

	public static char ComplementaryLetter(char c)
	{
		switch (c)
		{
			case 'A':
				return 'T';
			case 'T':
				return 'A';
			case 'C':
				return 'G';
			case 'G':
				return 'C';
			default:
				throw new InvalidOperationException();
		}
	}
	public static SortedDictionary<String, int> GenerateSpectrum(String DNACode, int temperature)
	{
		SortedDictionary<String, int> spectrum = new SortedDictionary<String, int>();
		int start = 0;
		string temp;

		for (int i = 0; i < (DNACode.Length); i++)
		{
			temp = DNACode.Substring(start, i + 1 - start);
			if (Temperature(temp) == temperature)
			{
				start++;
				AddToSpectrum(spectrum, temp);
			}
			if (Temperature(temp) > temperature)
			{
				start++;
				i--;
			}
		}
		return spectrum;
	}
	public static String Complementary(String s) {
		StringBuilder result = new StringBuilder();
		foreach (var c in s)
		{
			result.Append(ComplementaryLetter(c));
		}
		return result.ToString();
	}

	public static int Temperature(String code)
	{
		int result = 0;
		foreach (var c in code)
		{
			switch (c)
			{
				case 'A':
					result += 2;
					break;
				case 'T':
					result += 2;
					break;
				case 'C':
					result += 4;
					break;
				case 'G':
					result += 4;
					break;
				default:
					throw new InvalidOperationException();
			}
		}
		return result;
	}

	public static int Temperature(char[] s, int start, int length)
	{
		int result = 0;
		for(int i = start; i < start + length; i++)
		{
			switch (s[i])
			{
				case 'A':
					result += 2;
					break;
				case 'T':
					result += 2;
					break;
				case 'C':
					result += 4;
					break;
				case 'G':
					result += 4;
					break;
				default:
					throw new InvalidOperationException();
			}
		}
		return result;
	}

	public static void AddToSpectrum(SortedDictionary<string, int> Spectrum, string s)
	{
		if (Spectrum.ContainsKey(s))
		{

			Spectrum[s]++;
		}
		else
		{
			Spectrum.Add(s, 1);
		}
	}
	/**
	 * 
	 */
	public static int intervalToMinOccurence(SortedDictionary<string, int> Spectrum, string s)
	{
		if (Spectrum.ContainsKey(s))
			return intervals[Spectrum[s]].Item1;
		return 0;
	}
	
	/**
	 * give max occurence to given oligo
	 * ??? not sure
	 */
	public static int intervalToMaxOccurence(SortedDictionary<string, int> Spectrum, string s)
	{
		int inter;
		if (Spectrum.ContainsKey(s))
			inter = Spectrum[s];
		else
			return intervals[0].Item2;
		if (inter + 1 < intervals.Length)
			return intervals[inter + 1].Item2;
		return intervals[intervals.Length - 1].Item2;
		
	}
	public static int numToInterval(int num)
	{
		int i;
		for(i = 0; i < intervals.Length; i++)
		{
			if (!(num > intervals[i].Item2))
				return i;
		}
		return i;
	}

	public static Boolean Next(StringBuilder s)
	{
		switch (s[s.Length - 1])
		{
			case 'A':
				s[s.Length - 1] = 'T';
				return true;
			case 'T':
				s[s.Length - 1] = 'C';
				return true;
			case 'C':
				s[s.Length - 1] = 'G';
				return true;
			case 'G':
				s.Length--;
				return false;
		}
		throw new InvalidOperationException();
	}
}
