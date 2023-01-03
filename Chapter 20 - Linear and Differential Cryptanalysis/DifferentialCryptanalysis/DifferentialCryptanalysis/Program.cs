using System;

namespace DifferentialCryptanalysis
{
    class ExampleOfDifferentialCryptanalysis
    {
        //** variables
        public static int[] Known_P0 = new int[10000];
        public static int[] Known_P1 = new int[10000];
        public static int[] Known_C0 = new int[10000];
        public static int[] Known_C1 = new int[10000];
        public static int Good_P0, Good_P1, Good_C0, Good_C1;
        public static int numbers_pairs;
        public static int[] characters_data0 = new int[16];
        public static int characters_data_max = 0;
        public static int[,] characters = new int[32, 32];


        public static int[] theSBOX = new int[16] { 3, 14, 1, 10, 4, 9, 5, 6, 8, 11, 15, 2, 13, 12, 0, 7 }; 
        public static int[] sbox_reviwed = { 14, 2, 11, 0, 4, 6, 7, 15, 8, 5, 3, 9, 13, 12, 1, 10 };

        public static int round_function(int theInputData, int theKey)
        {
            return theSBOX[theKey ^ theInputData];
        }

        public static int encryption(int theInputData, int k_0, int k_1)
        {
            int x_0 = round_function(theInputData, k_0);
            return x_0 ^ k_1;
        }

        public static void find_differences()
        {
            Console.WriteLine("\nGenerating a differential table structure for XOR:\n");

            Random rnd = new Random();
            
            int x, y;

            for (x = 0; x < 16; x++)
            {
                for (y = 0; y < 16; y++)
                {
                    characters[x ^ y, theSBOX[x] ^ theSBOX[y]] = rnd.Next(-1, 1);                   
                }
            }

            for (x = 0; x < 16; x++)
            {
                for (y = 0; y < 16; y++)
                {
                    characters[x^y, theSBOX[x] ^ theSBOX[y]]++;
                    
                }
            }

            for (x = 0; x < 16; x++)
            {
                for (y = 0; y < 16; y++)
                    Console.Write("{0}", characters[x, y] + " ");
                Console.WriteLine("\n");
            }

            Console.WriteLine("\nShow the possible differentials:\n");

            for (x = 0; x < 16; x++)
                for (y = 0; y < 16; y++)
                    if (characters[x, y] == 6)
                        Console.WriteLine("\t\t6/16: {0} to {1}\n", x, y);
        }

        public static void genCharData(int input_differences, int output_differences)
        {
            Console.WriteLine("\nValues represented as possible intermediate based on differntial has been generated: ({0} to {1}):\n", input_differences, output_differences);

            characters_data_max = 0;
            int p;

            for (p = 0; p < 16; p++)
            {
                int theComputation = p ^ input_differences;

                if ((theSBOX[p] ^ theSBOX[theComputation]) == output_differences)
                {
                    Console.WriteLine("\t\tThe certain values choosen are:   {0} + {1} to  {2} + {3}\n", p, theComputation, theSBOX[p], theSBOX[theComputation]);
                    characters_data0[characters_data_max] = p;
                    characters_data_max++;
                }
            }
        }

        public static void genPairs(int input_differences)
        {
            Random randomNumber = new Random();

            Console.WriteLine("\nGenerating {0} known pairs with input differential of {1}.\n", numbers_pairs, input_differences);

            //** generate randomly subkey
            int Real_K0 = randomNumber.Next() % 16;

            //** generate randomly subkey
            int Real_K1 = randomNumber.Next() % 16;

            Console.WriteLine("\t\tThe K0 Real Value is = {0}\n", Real_K0);
            Console.WriteLine("\t\tThe K1 Real Value is = {0}\n", Real_K1);

            int b;

            //** Generate plaintexts pairs using different 
            //** XORs based on the differences 
            //** that are provided as input
            for (b = 0; b < numbers_pairs; b++)
            {
                Known_P0[b] = randomNumber.Next() % 16;
                Known_P1[b] = Known_P0[b] ^ input_differences;
                Known_C0[b] = encryption(Known_P0[b], Real_K0, Real_K1);
                Known_C1[b] = encryption(Known_P1[b], Real_K0, Real_K1);
            }
        }

        public static void findGoodPair(int output_differences)
        {
            Console.WriteLine("\nSearching for good pair:\n");

            int c;
            for (c = 0; c < numbers_pairs; c++)
                if ((Known_C0[c] ^ Known_C1[c]) == output_differences)
                {
                    Good_C0 = Known_C0[c];
                    Good_C1 = Known_C1[c];
                    Good_P0 = Known_P0[c];
                    Good_P1 = Known_P1[c];
                    Console.WriteLine("\t\tA good pair has been found: (P0 = {0}, P1 = {1}) to (C0 = {2}, C1 = {3})\n", Good_P0, Good_P1, Good_C0, Good_C1);
                    return;
                }
            Console.WriteLine("There is no pair proper found!\n");
        }

        public static int testKey(int Test_Key_0, int Test_Key_1)
        {
            int c;
            int someCrappyValue = 0;
            for (c = 0; c < numbers_pairs; c++)
            {
                if ((encryption(Known_P0[c], Test_Key_0, Test_Key_1) != Known_C0[c]) || (encryption(Known_P1[c], Test_Key_0, Test_Key_1) != Known_C1[c]))
                {
                    someCrappyValue = 1;
                    break;
                }
            }

            if (someCrappyValue == 0)
                return 1;
            else
                return 0;
        }

        public static void crack()
        {
            Console.WriteLine("\nUsing brute force to reduce the keyspace:\n");

            for (int g = 0; g < characters_data_max; g++)
            {
                int Test_K0 = characters_data0[g] ^ Good_P0;

                int Test_K1 = theSBOX[characters_data0[g]] ^ Good_C0;

                if (testKey(Test_K0, Test_K1) == 1)
                    Console.WriteLine("\t\tThe Key is! ({0}, {1})\n", Test_K0, Test_K1);
                else
                    Console.WriteLine("\t\t({0}, {1})\n", Test_K0, Test_K1);
            }
        }

        static void Main(String[] args)
        {
            Console.WriteLine("DIFFERENTIAL CRYPTANALYSIS\n");
            Random randomPerRunning = new Random();
            //** generating random values per each running
            randomPerRunning.Next();

            //** identify proper differentials 
            //** within the SBoxes
            find_differences();

            //** defining a numerical 
            //** value for known pairs
            numbers_pairs = 8;

            //** identify data inputs that will help 
            //** to lead us to specific characteristic
            genCharData(4, 7);

            //** randomly, generate pairs of 
            //** chosen-plaintext
            genPairs(4);

            //** based and using the characteristic,
            //** we will choose a known pair
            findGoodPair(7);

            //** use characteristic_data0 and within 
            //** the proper pair we will find it 
            crack();            

            while (true) { }
            Console.ReadKey();
        }
    }
}