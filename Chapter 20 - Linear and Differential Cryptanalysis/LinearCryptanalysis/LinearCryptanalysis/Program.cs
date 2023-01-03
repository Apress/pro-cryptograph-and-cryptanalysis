using System;

namespace LinearCryptanalysis
{
    class ExampleOfLinearCryptanalysis
    {
        #region Variables
        public static int[,] approximation_table = new int[16,16];
		public static int[] known_plaintext = new int[500];
        public static int[] known_ciphertext = new int[500];
        public static int number_known = 0;

        public static int[] theSBox = new int [16] {9, 11, 12, 4, 10, 1, 2, 6, 13, 7, 3, 8, 15, 14, 0, 5};
        public static int[] revtheSBox = {14, 5, 6, 10, 3, 15, 7, 9, 11, 0, 4, 1, 2, 8, 13, 12};
        #endregion

        //** the function will round the sbox values accordingly
        //** based on the value inputed and the sub key
        public static int RoundingFunction(int theInputValue, int theSubKey)
        {
            int index_position = theInputValue ^ theSubKey;
            return theSBox[index_position];
        }

        //** generatiing the keys
        //** and generating the known pairs
        public static void FillingTheKnowledgedOnces()
        {            
            Random randomNumber = new Random();
            int theSubKey_1 = randomNumber.Next() % 16;
            int theSubKey_2 = randomNumber.Next() % 16;

            Console.WriteLine("Generating the data: Key1 = {0}, Key2 = {1}\n", theSubKey_1, theSubKey_2);

            for (int h = 0; h < number_known; h++)
            {
                known_plaintext[h] = randomNumber.Next() % 16;
                known_ciphertext[h] = RoundingFunction(RoundingFunction(known_plaintext[h], theSubKey_1), theSubKey_2);
            }

            Console.WriteLine("Generating the data: Generating {0} Known Pairs\n\n", number_known);
        }

        //** show the the linear approximation
        //** note that the parameters
        //** a and b starts from 1
        public static void DisplayTheApproximation()
        {
            Console.WriteLine("Generate the linear approximation: \n");

            for (int a = 1; a < 16; a++)
            {
                for (int b = 1; b < 16; b++)
                {
                    if (approximation_table[a, b] == 14)
                        Console.WriteLine("{0} : {1} -> {2}\n", approximation_table[a, b], a, b);
                }
            }
            Console.WriteLine("\n");
        }

        public static int ApplyingTheMask(int v, int m)
        {
            //** v - is the value
            //** m - is the mask
            int internal_value = v & m;
            int total_amount = 0;

            while (internal_value > 0)
            {
                int temporary = internal_value % 2;
                internal_value /= 2;

                if (temporary == 1)
                    total_amount = total_amount ^ 1;
            }
            return total_amount;
        }

        //** the function will validate and 
        //** test the keys accordingly
        public static void ValidationAndTestingKeys(int key_1, int key_2)
        {
            for (int h = 0; h < number_known; h++)
            {
                if (RoundingFunction(RoundingFunction(known_plaintext[h], key_1), key_2) != known_ciphertext[h])
                    break;
            }
            Console.WriteLine("* ");
        }

        public static void FindingTheApproximation()
        {           
            Random randomNumber = new Random();

            //** The output the mask
            for (int a = 1; a < 16; a++)
            {
                //** The input mask
                for (int b = 1; b < 16; b++)
                {
                    //** the input
                    for (int c = 0; c < 16; c++)
                    {
                        if (ApplyingTheMask(c, b) == ApplyingTheMask(theSBox[c], a))
                        {
                            approximation_table[b, a]++;
                        }
                    }
                }
            }
        }          

        public static void Main(String[] args)
        {            
            int[] key_score = new int[16];
            int[] theProperKeys = new int[16];
            int stateProgress = 0;            
            int maximum_score = 0;            
            int guessing_key_1, guessing_key_2;
            int x, y;

            Random randomNumber = new Random();

            Console.WriteLine("Linear Cryptanalysis Simulation Program\n");
            Console.WriteLine("=======================================\n\n");

            randomNumber.Next();

            FindingTheApproximation();
            DisplayTheApproximation();

            int approximationAsInput = 11;
            int approximationAsOutput = 11;

            number_known = 16;
            FillingTheKnowledgedOnces();


            Console.WriteLine("Cryptanalysis Linear Attack - PHASE1. \n\t\t Based on linear approximation = {0} -> {1}\n", approximationAsInput, approximationAsOutput);			


            for (x = 0; x < 16; x++)
            {
                key_score[x] = 0;
                
                for (y = 0; y < number_known; y++)
                {
                    stateProgress++;

                    //** Find Bi by guessing at K1
                    int middle_round = RoundingFunction(known_plaintext[y], x);

                    if ((ApplyingTheMask(middle_round, approximationAsInput) == ApplyingTheMask(known_ciphertext[y], approximationAsOutput)))
                        key_score[x]++;
                    else
                        key_score[x]--;
                }
            }

            for (x = 0; x < 16; x++)
            {
                int theScore = key_score[x] * key_score[x];
                if (theScore > maximum_score)
                    maximum_score = theScore;
            }

            for (y = 0; y < 16; y++)
                theProperKeys[y] = -1;

            y = 0;

            for (x = 0; x < 16; x++)
                if ((key_score[x] * key_score[x]) == maximum_score)
                {
                    theProperKeys[y] = x;
                    Console.WriteLine("Cryptanalysis Linear Attack - PHASE 2. \n\t\t The possible for Key 1 = {0}\n", theProperKeys[x]);
                    y++;
                }

            for (y = 0; y < 16; y++)
            {
                if (theProperKeys[y] != -1)
                {
                    int testing_key_1 = RoundingFunction(known_plaintext[0], theProperKeys[y]) ^ revtheSBox[known_ciphertext[0]];
                                        
                    int g;
                    int wrong = 0;
                    for (g = 0; g < number_known; g++)
                    {
                        stateProgress += 2;
                        int testOut = RoundingFunction(RoundingFunction(known_plaintext[g], theProperKeys[y]), testing_key_1);

                        if (testOut != known_ciphertext[g])
                            wrong = 1;
                    }
                    if (wrong == 0)
                    {
                        Console.WriteLine("Cryptanalayis Linear Attack - PHASE 3.\n");
                        Console.WriteLine("\t\tI have found the keys! Key1 = {0}, Key2 = {1}\n", theProperKeys[y], testing_key_1);


                        guessing_key_1 = theProperKeys[y];
                        guessing_key_2 = testing_key_1;
                        Console.WriteLine("Cryptanalysis Linear Attack - PHASE 4.\n");
                        Console.WriteLine("\t\tThe number of computation until the key has been found = 0\n", stateProgress);						

                        }
                }
            }

            Console.WriteLine("Cryptanalyis Linear Attack - PHASE 5.\n");
            Console.WriteLine("The number of computation = {0}\n\n", stateProgress);

            stateProgress = 0;

            for (y = 0; y < 16; y++)
            {
                for (x = 0; x < 16; x++)
                {
                    int t;
                    int wrong = 0;
                    for (t = 0; t < number_known; t++)
                    {
                        stateProgress += 2;
                        int testOut = RoundingFunction(RoundingFunction(known_plaintext[t], y), x);

                        if (testOut != known_ciphertext[t])
                            wrong = 1;
                    }
                    if (wrong == 0)
                    {
                        Console.WriteLine("Brute Force - PHASE 1.\n");
                        Console.WriteLine("\t\tI managed to find the keys! \n\t\t Key1 = {0} \n\t\t Key2 = {1}\n", y, x);
    

                        Console.WriteLine("Brute Force - PHASE 2\n");
                        Console.WriteLine("\t\tThe number of computations until the key was dound = {0}\n", stateProgress);
                    }
                }
            }

            Console.WriteLine("Brute Force - PHASE 3.\n");
            Console.WriteLine("Computations total_amount = {0}\n", stateProgress);

            while (true) { }
        }
    }
}
