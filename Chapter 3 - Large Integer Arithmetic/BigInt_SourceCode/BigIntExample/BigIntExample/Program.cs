using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace BigIntExample
{
    class Program
    {
        static void Main(string[] args)
        {
            //** compute bigger factorials
            Console.Write("Enter a factorial to be computed (>100) :  ");
            int factorial = Convert.ToInt32(Console.ReadLine());
            ComputeBigFactorial(factorial);
            
            Console.WriteLine(Environment.NewLine);

            //** compute sum of the first 
            //** even number to get Fibonacci Series
            Console.WriteLine("Enter a number for the " +
                "first even set (>= 100 000) : ");
            int evenNumberFib = Convert.ToInt32(Console.ReadLine());           
            SumOfFirstOneLacEvenFibonacciSeries(evenNumberFib);
            Console.WriteLine(Environment.NewLine);

            //** computing greatest common divisor
            Console.WriteLine("GCD = " + 
                    BigInteger.GreatestCommonDivisor(12, 24));

            //** comparing purpose
            BigInteger value1 = 10;
            BigInteger value2 = 100;

            switch (BigInteger.Compare(value1, value2))
            {
                case -1:
                    Console.WriteLine("{0} < {1}", value1, value2);
                    break;
                case 0:
                    Console.WriteLine("{0} = {1}", value1, value2);
                    break;
                case 1:
                    Console.WriteLine("{0} > {1}", value1, value2);
                    break;
            }   

            //** parsing
            Console.WriteLine(BigInteger.Parse("10000000"));

            //** obtaining negation
            Console.WriteLine(BigInteger.Negate(-100));

            //** returning the sign
            Console.WriteLine(BigInteger.Negate(-1).Sign);

            //** returning conversion (int to BigInterger)
            int i = 100;
            BigInteger bI = (BigInteger)i;
            Console.WriteLine(bI);

            //** returning conversion (BigInteger to int)
            BigInteger BI = 200;
            int j = (int)BI;
            Console.WriteLine(j);
            Console.Read();
        }

        //** computing the factorials
        private static void ComputeBigFactorial(int factorial)
        {
            BigInteger number = factorial;
            BigInteger fact = 1; 
            for (; number-- > 0; ) fact *= number+1;
            Console.WriteLine(fact);
        }

        
        //** computing the first even fibonacci series
        private static void SumOfFirstOneLacEvenFibonacciSeries(
                        int evenNumberFib)
        {
            int limit = evenNumberFib;

            BigInteger value1 = 1;
            BigInteger value2 = 2;
            BigInteger theSum = 0;
            BigInteger even_sum = value1 + value2;      
            
            for (int i = 2; i < limit; i++)
            {
                theSum = value1 + value2;
                if (theSum % 2 == 0) even_sum += theSum;
                value1 = value2;
                value2 = theSum;
            }

            Console.WriteLine(even_sum);

        }
    }
}
