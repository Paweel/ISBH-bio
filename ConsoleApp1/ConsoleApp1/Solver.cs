using System;
using System.Collections.Generic;
using System.Text;
using static CommonDNAOperations;

namespace BIO
{
    class Solver
    {
        public string orginalDNA;

        //Given data:
        //intervals that say how many times oligo where detected
        private int[] intervals = new int[] { 6, 3, 1, 0 };
        //spectrum from higer temperature
        public SortedDictionary<string, UInt32> SpectrumLong { get; set; }
        //spectrum from lower temperature
        public SortedDictionary<string, UInt32> SpectrumShort { get; set; }
        //first oligo
        public string first;
        //higher temperature in ISBH second temp is always lower by 2
        public int higherT;
        //length of dna sequence
        public int givenDNALength;

        public char[] nucleo = new char[] {'A', 'T', 'C', 'G'};

		private List<String> bagOfResults;
        private int longestOligo;
        private int shortestOligo;
        //count oligo quantity in answer
        public SortedDictionary<string, UInt32> SpectrumCounter { get; set; }
		private int minOligoToAddS = 0;
		private int minOligoToAddL = 0;
		ISBHInstance isbh = new ISBHInstance();

        public void generate(int length, int temp)
        {
            higherT = temp;
            this.givenDNALength = length;
            isbh.BuildComplDNAInterval(length, temp);
            orginalDNA = isbh.DNACode;
            first = isbh.first;
            SpectrumLong = isbh.SpectrumInterval;
            isbh.BuildComplDNAInterval(length, temp - 2, true);
            SpectrumShort = isbh.SpectrumInterval;

            longestOligo = higherT / 2;
            shortestOligo = (higherT - 2) / 4;
			bagOfResults = new List<string>();
			SpectrumCounter = new SortedDictionary<string, uint>();

			setMinOligoToAdd();
        }

		private void setMinOligoToAdd() {
			foreach (var oligo in SpectrumLong.Keys)
				minOligoToAddL += minOccurence(SpectrumLong, oligo);
			foreach (var oligo in SpectrumShort.Keys)
				minOligoToAddS += minOccurence(SpectrumShort, oligo);
		}
        public string solve()
        {
            StringBuilder DNA = new StringBuilder(first);
            solveR(DNA);
            Console.WriteLine("match: " + DNA.ToString().Equals(orginalDNA));
            Console.WriteLine("original: " + orginalDNA);
            return DNA.ToString();
        }
        private int Temperature(String oligo)
        {
            int result = 0;
            foreach (var c in oligo)
            {
                switch (c)
                {
                    case 'A':
                        result += 2;
                        break;
                    case 'T':
                        result += 2;
                        break;
                    case 'G':
                        result += 4;
                        break;
                    case 'C':
                        result += 4;
                        break;
                    default:
                        throw new InvalidOperationException();
                }

            }
            return result;
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

        /**
         * If false it will not add anything to SpectrumCounter
         */
        private Boolean addLastTwoOligo(StringBuilder DNA)
        {
            String oligo;
            String shorterOligo = null;
            for(int i = shortestOligo; i <= longestOligo; i++) {
				if (i >= DNA.Length)
					break;
                oligo = DNA.ToString(DNA.Length - i, i);
                int t = Temperature(oligo);
                if (t == (higherT - 2))
                {
                    if (!incCountSpectrum(oligo, t))
                        return false;
                    shorterOligo = oligo;
                }
                else if (t == higherT)
                {
                    if (!incCountSpectrum(oligo, t)) {
						if (shorterOligo != null)
						{
							decCountSpectrum(shorterOligo, higherT - 2);
							decCountSpectrum(Complementary(shorterOligo), higherT - 2);
						}
						return false;
                    }
                    break;
                }
                else if (t > higherT)
                {
                    break;
                }   
            }
            return true;
        }

		private void RemoveOligo(StringBuilder DNA)
		{
			String oligo;
			for (int i = shortestOligo; i <= longestOligo; i++)
			{
				if (i >= DNA.Length)
					break;
				oligo = DNA.ToString(DNA.Length - i, i);
				int t = Temperature(oligo);
				if (t == (higherT - 2))
				{
					decCountSpectrum(oligo, t);
					decCountSpectrum(Complementary(oligo), t);
				}
				else if (t == higherT)
				{	
					decCountSpectrum(oligo, higherT - 2);
					decCountSpectrum(Complementary(oligo), higherT - 2);
					break;
				}
				else if (t > higherT)
				{
					break;
				}
			}
		}

        private Boolean solveR(StringBuilder DNA)
        {
            foreach(var c in nucleo)
			{
				//minOligo count two complementary chains so we need to multiply
				if (minOligoToAddS + 2 * DNA.Length > 2 * givenDNALength)
					return false;
				if (minOligoToAddL + 2 * DNA.Length > 2 * givenDNALength)
					return false;

				DNA.Append(c);
                if (!addLastTwoOligo(DNA))
				{
					//revert
                    DNA.Remove(DNA.Length - 1, 1);
                    continue;
                }
				//check end condition
				if (DNA.Length == givenDNALength)
				{
					bagOfResults.Add(DNA.ToString());
					RemoveOligo(DNA);
					DNA.Remove(DNA.Length - 1, 1);
					return true;
				}
				solveR(DNA);
				//revert
				RemoveOligo(DNA);
				DNA.Remove(DNA.Length - 1, 1);
			}
			
			return false;
        }

        /**
         *  false on removing oligo from spectrum
         */
        private Boolean decCountSpectrum(string oligo, int temp)
        {
            SortedDictionary<string, UInt32> Spectrum;
            if (temp == higherT)
				Spectrum = SpectrumLong;
            else
				Spectrum = SpectrumShort;

			if (SpectrumCounter[oligo] <= minOccurence(Spectrum, oligo))
				addToMinOligo(1, temp);
			if (--SpectrumCounter[oligo] <= 0)
            {				
				SpectrumCounter.Remove(oligo);
				return false;
            }
			return true;
        }

        /**
         * return false if oligo quantity exceed maximum interval + 1
         * in this case it don't add that oligo to SpectrumCounter
         */
        private Boolean incCountSpectrum(string oligo, int temp)
        {
			String complOligo = Complementary(oligo);
            SortedDictionary<string, UInt32> Spectrum;
            if (temp == higherT)
				Spectrum = SpectrumLong;
            else
				Spectrum = SpectrumShort;
                
            if ((SpectrumCounter.ContainsKey(oligo) && (SpectrumCounter[oligo]) > maxOccurence(Spectrum, oligo))) // more than allowed in interval without error!!!
                return false;
			if ((SpectrumCounter.ContainsKey(complOligo) && (SpectrumCounter[complOligo]) > maxOccurence(Spectrum, complOligo))) // more than allowed in interval without error! (Spectrum.ContainsKey(oligo) <- one is always allowed from negative error
				return false;
			
			AddToSpectrum(SpectrumCounter, oligo);
			AddToSpectrum(SpectrumCounter, complOligo);

			if (SpectrumCounter[oligo] <= minOccurence(Spectrum, oligo))
				addToMinOligo(-1, temp);
			if (SpectrumCounter[complOligo] <= minOccurence(Spectrum, complOligo))
				addToMinOligo(-1, temp);
			return true;
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
