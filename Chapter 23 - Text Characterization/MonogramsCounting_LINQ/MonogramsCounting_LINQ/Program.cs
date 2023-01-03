using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogramsCounting_LINQ
{
    class Program
    {
        static void Main()
        {
            var frequencies = from c in File.ReadAllText("TheText.txt")                            
                              group c by c into groupCharactersFrequencies
                              select groupCharactersFrequencies;              

            foreach (var c in frequencies)
                Console.WriteLine($"The character: {c.Key} has the frequency: {c.Count()} times");

            Console.ReadKey();
        }
    }
}
