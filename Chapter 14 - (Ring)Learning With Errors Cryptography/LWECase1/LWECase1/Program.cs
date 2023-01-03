using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LWECase1
{
    class Program
    {
        public static int[] public_key = new int[200];
        public static int[] values = new int[] { 5, 8, 12, 16, 2, 6, 11, 3, 7, 10 };
        public static int s = 5;
        public static int e = 12;
        public static int message = 1;
        public static int val = 0;
        public static int sum = 0;
        public static int remainder = 0;

        static void Main(string[] args)
        {
            Random randValue = new Random();
            int[] res = new int[200];
            int k = 0;

            for (int x=0; x<values.Length; x++)
            {
                public_key[k] = values[x] * s + e;
                k++;
            }

            for(int i=0; i< public_key.Length; i++)
            {
                res[i] = randValue.Next(public_key[i], public_key.Length / 2);
            }

            for(int j=0; j<res.Length; j++)
            {
                sum += res[j];
            }

            Console.WriteLine("The message to be send: {0}", message);
            Console.WriteLine("The random values:");
            PrintValues(values);
            Console.WriteLine("The public key is: ");
            PrintValues(public_key);
            Console.WriteLine("The selected values are:");
            PrintValues(res);

            //** compute the sum
            if (message == 1)
                sum += 1;

            Console.WriteLine("The sum is: {0}", sum);
            
            Console.WriteLine("The encrypted message is: {0}", sum);

            remainder = sum % s;

            if(remainder % 2 == 0)            
                Console.WriteLine("The message received is 0");
            else
                Console.WriteLine("The message received is 1");

            Console.ReadKey();
        }

        public static void PrintValues(Object[] myArr)
        {
            foreach (Object i in myArr)
            {
                Console.Write("\t{0}", i);
            }
            Console.WriteLine();
        }

        public static void PrintValues(int[] myArr)
        {
            foreach (int i in myArr)
            {
                Console.Write("\t{0}", i);
            }
            Console.WriteLine();
        }
    }
}
