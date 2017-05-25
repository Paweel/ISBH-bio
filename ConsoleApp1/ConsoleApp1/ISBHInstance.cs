using System;
using System.Collections.Generic;
using System.Text;

namespace BIO
{
    public class ISBHInstance
    {
        public void BuildComplDNAInterval(int length, int temperature)
        {
            DNACode = GenerateDNA(length);
            GenerateSpectrum(temperature);
            DNACode = GenerateComplementary();
            GenerateSpectrum(temperature, false);
            Interval();
        }
        private char[] DNA = new char[] { 'A', 'T', 'C', 'G' };
        private int[] intervals = new int[] {6,3,1,0};
        private String DNACode_i;
        public string DNACode { get => DNACode_i; set => DNACode_i = value; }
        public SortedDictionary<string, UInt32> Spectrum { get; set; }
        public SortedDictionary<string, UInt32> SpectrumInterval { get; set; }

        public string GenerateDNA(int length)
        {
            StringBuilder DNAToBuild = new StringBuilder();
            Random rand = new Random();
            for (int i = 0; i < length; i++)
            {
                DNAToBuild.Append(DNA[rand.Next(3)]);
            }
            return DNAToBuild.ToString();
        }
        public string GenerateComplementary()
        {
            StringBuilder DNAToBuild = new StringBuilder();
            int length = DNACode_i.Length;
            for (int i = 0; i < length; i++)
            {
                DNAToBuild.Append(ComplementaryLetter(DNACode_i[i]));
            }
            return DNAToBuild.ToString();

        }
        private char ComplementaryLetter(char c)
        {
            switch (c)
            {
                case 'A':
                    return 'T';
                case 'T':
                    return 'A';
                case 'G':
                    return 'C';
                case 'C':
                    return 'G';
                default:
                    throw new InvalidOperationException();
            }
        }
        private int Temperature(String code)
        {
            int result = 0;
            foreach(var c in code)
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
        public void GenerateSpectrum(int temperature, Boolean reset = true)
        {
            int start = 0;
            if (reset)
            {
                Spectrum = new SortedDictionary<string, UInt32>();
            }
            string temp;
            for (int i = 0; i < (DNACode_i.Length); i++)
            {
                temp = DNACode_i.Substring(start, i + 1 - start);
                if (Temperature(temp) == temperature)
                {
                    start++; 
                    AddToSpectrum(temp);
                }
                if (Temperature(temp) > temperature)
                {
                    start++;
                    i--;
                }

            }
        }
        private void AddToSpectrum(string s)
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
        private UInt32 ChangeInIntervals(UInt32 v)
        {
            UInt32 i;
            for (i = 0; i < intervals.Length; i++)
            {
                if (v > intervals[i])
                    return i;
            }
            return i; 

        }
        public void Interval()
        {
            SpectrumInterval = new SortedDictionary<string, uint>();
            foreach (var s in Spectrum)
            {
                SpectrumInterval.Add(s.Key, ChangeInIntervals(s.Value));
            }
        }

        public void GenerateNegativeErrorInterval(int amount)
        {
            int size = Spectrum.Count;
            Random rand = new Random();
            List<string> values = new List<string>(Spectrum.Keys);
            HashSet<string> toDelete = new HashSet<string>();
            while (toDelete.Count < amount)
            {
                toDelete.Add(values[rand.Next(size)]);
            }
            foreach (string s in toDelete)
            {
                if (--SpectrumInterval[s] < 1)
                {
                    SpectrumInterval.Remove(s);
                }

            }
        }
        public void GenerateNegativeError(int amount)
        {
            int size = Spectrum.Count;
            Random rand = new Random();
            List<string> values = new List<string>(Spectrum.Keys);
            HashSet<string> toDelete = new HashSet<string>();
            while (toDelete.Count < amount)
            {
                toDelete.Add(values[rand.Next(size)]);
            }
            foreach (string s in toDelete)
            {
                if (--Spectrum[s] < 1)
                {
                    Spectrum.Remove(s);
                }

            }
        }

        public void PrintToTerminal()
        {
            Console.WriteLine(DNACode_i);
        }
    }
}
