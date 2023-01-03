using System;
using System.IO;

class Program
{
    static void Main()
    {
        //** we use the array to store the frequencies
        int[] frequency = new int[(int)char.MaxValue];

        //** look at the content of the text file
        string s = File.ReadAllText("TheText.txt");

        //** go through each of the characters
        foreach (char t in s)
        {
            //** store the frequencies as a table
            frequency[(int)t]++;
        }

        //** write all letters that have been found
        for (int letterPos = 0; letterPos < (int)char.MaxValue; letterPos++)
        {
            if (frequency[letterPos] > 0 && char.IsLetterOrDigit((char)letterPos))
            {
                Console.WriteLine("The Letter: {0} has the Frequency: {1}", (char)letterPos, frequency[letterPos]);
            }
        }
        Console.ReadKey();
    }
}
