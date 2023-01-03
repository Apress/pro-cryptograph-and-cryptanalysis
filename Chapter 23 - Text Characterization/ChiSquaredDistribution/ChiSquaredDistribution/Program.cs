using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiSquaredDistribution
{
    class Program
    {
        static void Main(string[] args)
        {
            int number_of_experiments = 10000;
            int number_of_stars_distribution = 100;

            Random theGenerator = new Random();
            double theDistribution = 6.0;            

            int[] probability = new int[10];
            

            for (int counter = 0; counter < number_of_experiments; ++counter)
            {
                double no = theGenerator.NextDouble() + theDistribution;
                if ((no >= 0.0) && (no < 10.0))
                    ++probability[(int)no];
            }

            Console.WriteLine("The Chi-Squared Distribution(6.0):");

            for (int index = 0; index < 10; ++index)
            {
                Console.WriteLine("index {0} ", index, " -- {1}: ",  (index + 1), ":");
                Console.WriteLine("{0}", probability[index] * number_of_stars_distribution / number_of_experiments);
            }

            Console.ReadKey();
        }
    }
}
