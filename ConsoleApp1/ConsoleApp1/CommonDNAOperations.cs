using System;
using System.Collections.Generic;
using System.Text;

public class CommonDNAOperations
{
	public static readonly char[] DNA = { 'A', 'T', 'C', 'G' };
	public static readonly int[] intervals = { 6, 3, 0 };
	//That was probably bad idea ^ candidate to refactor
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

	public static void AddToSpectrum(SortedDictionary<string, UInt32> Spectrum, string s)
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
	public static int minOccurence(SortedDictionary<string, UInt32> Spectrum, string s)
	{
		if (Spectrum.ContainsKey(s))
			return intervals[Spectrum[s]] + 1;
		return 0;
	}
	
	/**
	 * give max occurence to given oligo
	 * ??? not sure
	 */
	public static int maxOccurence(SortedDictionary<string, UInt32> Spectrum, string s)
	{
		if (Spectrum.ContainsKey(s))
			if (Spectrum[s] - 1 > 0)
				return intervals[Spectrum[s] - 1];
			else
				return int.MaxValue;
		return intervals[2];
	}
}
