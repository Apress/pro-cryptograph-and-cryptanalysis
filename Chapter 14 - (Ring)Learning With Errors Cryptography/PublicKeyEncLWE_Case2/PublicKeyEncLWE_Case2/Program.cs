using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicKeyEncLWE_Case2
{
    class Program
    {
        public static int[] A = new int[30];
        public static int[] B = new int[30];
        public static int[] e = new int[30];
        public static int s = 20;
        public static int message = 1;
        public static int q = 15;
        public static int nvals = 20;


        static void Main(string[] args)
        {
            Random randomSample = new Random();
            IEnumerable<int> q_values = Enumerable.Range(0, q);
            IEnumerable<int> n_values = Enumerable.Range(nvals - 1, nvals / 4);
            double u = 0;
            double v = 0;
            int sample = 0;

            foreach (int q_value in q_values)
            {
                for (int i = 0; i < q; i++)
                {
                    A[i] = randomSample.Next(q_value, nvals);
                }
            }
            for (int x = 0; x < A.Length; x++)
            {
                e[x] = randomSample.Next(1, 4);
                B[x] = (A[x] * s + e[x]) % 2;                
            }

            Console.WriteLine("PARAMETERS SECTION\n");
            Console.WriteLine("\tMessage to be send: {0}", message);
            Console.WriteLine("\tThe public key (A):");
            PrintValues(A);
            Console.WriteLine("\tThe public key (B):");
            PrintValues(B);
            Console.WriteLine("\tThe errors (e) are: ");
            PrintValues(e);
            Console.WriteLine("\tThe secret key is: {0}", s);
            Console.WriteLine("\tPrime number is: {0}", q);


            foreach(int n_value in n_values)
            {
                sample = randomSample.Next(nvals - 1, n_value);
                Console.WriteLine("The sample is {0}", sample);
            }

            IEnumerable<int> samples = Enumerable.Range(0, sample);
            string errors = string.Empty;

            for (int x = 0; x < samples.Count(); x++)
            {
                errors = "[" + A[x] + ", " + B[x] + ", ], end = " + u + A[x];
            }

            Console.WriteLine(errors);

            double flooring = q / 2;

            v += Math.Floor(flooring) * message;

            u = v % q;
            v = u % q;

            Console.WriteLine("u = {0}", u);
            Console.WriteLine("v = {0}", v);

            double res = (v - s * u) % q;
            Console.WriteLine("The result is: {0}", res);
            if (res > q / 2)
                Console.WriteLine("Message is 1");
            else
                Console.WriteLine("Message is 0");

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
