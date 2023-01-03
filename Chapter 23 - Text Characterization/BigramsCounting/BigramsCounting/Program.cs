using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigramsCounting
{
    class Program
    {
        static void Main(string[] args)
        {
            String text = "Welcome to Apress! This book is about cryptography and C#.";
            String bigramPattern = "to";
            Console.WriteLine("Full text: " + text);
            Console.WriteLine("Bigram: " + bigramPattern + "\n");
            Console.WriteLine("The number of occurrences of \"" + bigramPattern + "\" in \"" + text + "\" is: " + countFrequenciesBigrams(bigramPattern, text).ToString());

        }

        static int countFrequenciesBigrams(String bigramPattern, String text)
        {
            int bigramPatternLength = bigramPattern.Length;
            int textLength = text.Length;
            int occurrences = 0;

            for (int idx = 0; idx <= textLength - bigramPatternLength; idx++)
            {
                int jIdx;
                for (jIdx = 0; jIdx < bigramPatternLength; jIdx++)
                {
                    if (text[idx + jIdx] != bigramPattern[jIdx])
                    {
                        break;
                    }
                }

                if (jIdx == bigramPatternLength)
                {
                    occurrences++;
                    jIdx = 0;
                }
            }
            return occurrences;
        }
    }
}
