using System;
using System.Collections.Generic;
using System.Text;
using static CommonDNAOperations;

namespace Solver
{
    public class Solver
    {
        public string orginalDNA;

        //Given data:
        //intervals that say how many times oligo where detected
        private int[] intervals = new int[] { 6, 3, 1, 0 };
        //spectrum from higer temperature
        public SortedDictionary<string, int> SpectrumLong { get; set; }
        //spectrum from lower temperature
        public SortedDictionary<string, int> SpectrumShort { get; set; }
        //first oligo
        public string first;
        //higher temperature in ISBH second temp is always lower by 2
        public int higherT;
        //length of dna sequence
        public int givenDNALength;

        public char[] nucleo = new char[] {'A', 'T', 'C', 'G'};

		public List<String> bagOfResults;
        private int longestOligo;
        private int shortestOligo;
        //count oligo quantity in answer
        public SortedDictionary<string, int> SpectrumCounter { get; set; }
		private int minOligoToAddS = 0;
		private int minOligoToAddL = 0;
		ISBHInstance isbh = new ISBHInstance();

        public void generate(int length, int temp, int errors)
        {
            higherT = temp;
            this.givenDNALength = length;
            isbh.BuildComplDNAInterval(length, temp);
			isbh.GenerateNegativeErrorInterval(errors);
			orginalDNA = isbh.DNACode;
            first = isbh.first;
            SpectrumLong = isbh.SpectrumInterval;
            isbh.BuildComplDNAInterval(length, temp - 2, true);
			isbh.GenerateNegativeErrorInterval(errors);
			if (first == "")
				first = Complementary(isbh.first);
            SpectrumShort = isbh.SpectrumInterval;

            longestOligo = higherT / 2;
            shortestOligo = (higherT - 2) / 4;
			bagOfResults = new List<string>();
			SpectrumCounter = new SortedDictionary<string, int>();

			setMinOligoToAdd();
        }

		private void setMinOligoToAdd() {
			foreach (var oligo in SpectrumLong.Keys)
				minOligoToAddL += intervalToMinOccurence(SpectrumLong, oligo);
			foreach (var oligo in SpectrumShort.Keys)
				minOligoToAddS += intervalToMinOccurence(SpectrumShort, oligo);
		}
        public string solve()
        {
            StringBuilder dnaResult = new StringBuilder(first);
			addLastTwoOligo(new StringBuilder(first.Substring(0, first.Length - 1)));
			while(true)
			{
				if (dnaResult.Length < first.Length)
					break; //failure or end of all space to search
				if (!addLastTwoOligo(dnaResult))
				{
					//try luck in next
					Revert(dnaResult);
					continue;
				}
				if (LimitsBroken(dnaResult))
				{
					Revert(dnaResult);
					continue;
				}
				if (dnaResult.Length >= givenDNALength)
				{
					bagOfResults.Add(dnaResult.ToString());
					Revert(dnaResult);
					continue;
				}
				dnaResult.Append(nucleo[0]);
			}
            //Console.WriteLine("match: " + bagOfResults.Contains(orginalDNA));
			//Console.WriteLine("first:    " + first);
			//Console.WriteLine("original: " + orginalDNA);
			return dnaResult.ToString();
        }

		private void Revert(StringBuilder dnaResult)
		{
			do
			{
				if (dnaResult.Length < first.Length)
					break;
				RemoveOligo(dnaResult);
			}
			while (!Next(dnaResult));
		}

		private Boolean LimitsBroken(StringBuilder dnaResult)
		{
			if (minOligoToAddS + 2 * dnaResult.Length > 2 * givenDNALength)
				return true;
			if (minOligoToAddL + 2 * dnaResult.Length > 2 * givenDNALength)
				return true;
			return false;
		}

        private int charToTemp(char c)
        {
            switch(c)
            {
                case 'A':
                    return 2;
                case 'T':
                    return 2;
                case 'G':
                    return 4;
                case 'C':
                    return 4;
            }
            throw new InvalidOperationException();
        }

		private List<Tuple<String, int>> FindLastTwoOligos(StringBuilder DNA)
		{
			List<Tuple<String, int>> result = new List<Tuple<String, int>>();
			String oligo;
			for (int i = shortestOligo; i <= longestOligo; i++)
			{
				if (i > DNA.Length)
					break;
				oligo = DNA.ToString(DNA.Length - i, i);
				int t = Temperature(oligo);
				if (t == (higherT - 2) || (t == higherT))
				{
					result.Add(new Tuple<string, int>(oligo, t));
				}
			}
			return result;
		}

		/**
         * If false it will not add anything to SpectrumCounter
         */
		private Boolean addLastTwoOligo(StringBuilder DNA)
        {
			var oligos = FindLastTwoOligos(DNA);
			Boolean error = false;
			int i;
			for(i = 0; i < oligos.Count; i++)
			{
				if (!incCountSpectrum(oligos[i].Item1, oligos[i].Item2))
				{
					error = true;
				}
			}
			return !error;
        }

		private void RemoveOligo(StringBuilder DNA)
		{
			foreach(var tuple in FindLastTwoOligos(DNA))
			{
				decCountSpectrum(tuple.Item1, tuple.Item2);
				decCountSpectrum(Complementary(tuple.Item1), tuple.Item2);
			}
		}

        /**
         *  false on removing oligo from spectrum
         */
        private Boolean decCountSpectrum(string oligo, int temp)
        {
            SortedDictionary<string, int> Spectrum;
            if (temp == higherT)
				Spectrum = SpectrumLong;
            else
				Spectrum = SpectrumShort;

			if (SpectrumCounter[oligo] <= intervalToMinOccurence(Spectrum, oligo))
				addToMinOligo(1, temp);
			if (--SpectrumCounter[oligo] <= 0)
            {				
				SpectrumCounter.Remove(oligo);
				return false;
            }
			return true;
        }

        /**
         * return false if oligo quantity exceed maximum interval
         */
        private Boolean incCountSpectrum(string oligo, int temp)
        {
			Boolean result = true;
			String complOligo = Complementary(oligo);
            SortedDictionary<string, int> Spectrum;
            if (temp == higherT)
				Spectrum = SpectrumLong;
            else
				Spectrum = SpectrumShort;
                
            if (SpectrumCounter.ContainsKey(oligo) && (SpectrumCounter[oligo] > intervalToMaxOccurence(Spectrum, oligo))) // more than allowed in interval without error!!!
                result = false;
			if (SpectrumCounter.ContainsKey(complOligo) && (SpectrumCounter[complOligo] > intervalToMaxOccurence(Spectrum, complOligo))) // more than allowed in interval without error! (Spectrum.ContainsKey(oligo) <- one is always allowed from negative error
				result = false;
			
			AddToSpectrum(SpectrumCounter, oligo);
			AddToSpectrum(SpectrumCounter, complOligo);

			if (SpectrumCounter[oligo] <= intervalToMinOccurence(Spectrum, oligo))
				addToMinOligo(-1, temp);
			if (SpectrumCounter[complOligo] <= intervalToMinOccurence(Spectrum, complOligo))
				addToMinOligo(-1, temp);
			return result;
        }

		private void addToMinOligo(int num, int temp)
		{
			if (temp == higherT)
				minOligoToAddL += num;
			else if (temp == higherT - 2)
				minOligoToAddS += num;
		}
		public void PrintResults()
		{
			Console.WriteLine("all results:");
			foreach (var s in bagOfResults)
			{
				Console.WriteLine("");
				Console.WriteLine(s);
			}
		}
    }
}
