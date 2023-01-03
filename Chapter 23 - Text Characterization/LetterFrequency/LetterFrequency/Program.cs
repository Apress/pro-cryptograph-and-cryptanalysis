using System;
using System.Linq;

namespace LetterFrequency
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "";
            VerifyFreq();

            Console.Write("The number of letters: ");
            input = Console.ReadLine();

            Compute(input);
        }

        //** Store the letter frequencies.
        //** For more details and the values 
        //** stored below, see the link:
        //** http://en.wikipedia.org/wiki/Letter_frequency
        private static float[] wikiFrequencies =
        {
            8.167f, 1.492f, 2.782f, 4.253f, 12.702f,
            2.228f, 2.015f, 6.094f, 6.966f, 0.153f,
            0.772f, 4.025f, 2.406f, 6.749f, 7.507f,
            1.929f, 0.095f, 5.987f, 6.327f, 9.056f,
            2.758f, 0.978f, 2.360f, 0.150f, 1.974f,
            0.074f
        };

        //** create a instance of a number 
        //** generator using Random class
        private static Random randomNumber = new Random();

        //** compute the ASCII value of letter A
        private static int int_AsciiA = (int)'A';

        //** verify that the frequencies are adding up to 100
        private static void VerifyFreq()
        {
            //** compute the difference to E
            float totalAmount = wikiFrequencies.Sum();
            float differenceComputation = 100f - totalAmount;
            wikiFrequencies[(int)'E' - int_AsciiA] += differenceComputation;
        }

        //** based on the frequencies
        //** generate randomly the letters
        private static void Compute(string txtNumLetters)
        {
            //** monitor and track each letter 
            //** that has been generated
            int[] countGeneratedLetters = new int[26];

            //** randomly generate the letters
            int theNumberOfLetters = int.Parse(txtNumLetters);
            string result = "";
            for (int k = 0; k < theNumberOfLetters; k++)
            {
                //** randomly generate a number 
                //** between 0 and 100
                double randomlyNumber = 100.0 *
                randomNumber.NextDouble();

                //** select the letter that 
                //** this will represents
                for (int numberOfLetter = 0; ; numberOfLetter++)
                {
                    //** extract the frequency of the 
                    //** letter from the number
                    randomlyNumber -= wikiFrequencies[numberOfLetter];

                    //** if the randomly number is 
                    //** less and equal than 0
                    //** it means that we have the letter
                    if ((randomlyNumber <= 0) || (numberOfLetter == 25))
                    {
                        char character = (char)(int_AsciiA + numberOfLetter);
                        result += character.ToString() + ' ';

                        countGeneratedLetters[numberOfLetter]++;
                        break;
                    }
                }
            }
            
            Console.WriteLine(result + "\n");

            //** show the frequencies            
            for (int i = 0; i < countGeneratedLetters.Length; i++)
            {
                char ch = (char)(int_AsciiA + i);
                float frequency = (float)countGeneratedLetters[i] / theNumberOfLetters * 100;
                string str = string.Format("{0}\t{1,6}\t{2,6}\t{3,6}", ch.ToString(), frequency.ToString("0.000"), wikiFrequencies[i].ToString("0.000"), (frequency - wikiFrequencies[i]).ToString("0.000"));
                Console.WriteLine(str);
            }
        }

    }
}

