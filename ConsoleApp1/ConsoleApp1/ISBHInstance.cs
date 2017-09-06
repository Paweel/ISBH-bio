using System;
using System.Collections.Generic;
using System.Text;
using static CommonDNAOperations;

namespace BIO
{
    public class ISBHInstance
    {
        public void BuildComplDNAInterval(int length, int temperature, Boolean keepOld = false)
        {
            if (!keepOld)
                DNACode = GenerateDNA(length);
            GenerateSpectrum(temperature);
            DNACode = GenerateComplementary();
            GenerateSpectrum(temperature, false);
            Interval();
        }
        
        private String DNACode_i;
        public string DNACode { get => DNACode_i; set => DNACode_i = value; }
        public SortedDictionary<string, UInt32> Spectrum { get; set; }
        public SortedDictionary<string, UInt32> SpectrumInterval { get; set; }
        public string first = "";

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
        
        public void GenerateSpectrum(int temperature, Boolean reset = true)
        {
            int start = 0;
            if (reset)
            {
                Spectrum = new SortedDictionary<string, UInt32>();
                first = "";
            }
            string temp;


            for (int i = 0; i < (DNACode_i.Length); i++)
            {
                temp = DNACode_i.Substring(0, i + 1);
                if (Temperature(temp) == temperature)
                {
                    first = temp;
                    break;
                }
            }

            for (int i = 0; i < (DNACode_i.Length); i++)
            {
                temp = DNACode_i.Substring(start, i + 1 - start);
                if (Temperature(temp) == temperature)
                {
                    start++; 
                    AddToSpectrum(Spectrum, temp);
                }
                if (Temperature(temp) > temperature)
                {
                    start++;
                    i--;
                }
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
                if (--SpectrumInterval[s] < 1) //???
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
