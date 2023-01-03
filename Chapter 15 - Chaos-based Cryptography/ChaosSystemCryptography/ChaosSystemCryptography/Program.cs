using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosSystemCryptography
{
    class Program
    {
        public static void Main(string[] args)
        {
            bool logging = true;
            string key = "$6G";
            string input_value = string.Empty;
            Console.Write("What to you want to do? Encryption (e) or Decryption (d): ");
            string option = (Console.ReadLine() == "encrypt") ? "e" : "d";

            int zero_location = Convert.ToInt32(key[0]);
            int one_location = Convert.ToInt32(key[1]);
            int two_location = Convert.ToInt32(key[2]);

            TorusAutomorphism torus_automorphism = new TorusAutomorphism();

            GenerateChaosValues generator0 = new GenerateChaosValues((logging == true) ? "generator0" : null);
            GenerateChaosValues generator1 = new GenerateChaosValues((logging == true) ? "generator1" : null);
            GenerateChaosValues generator2 = new GenerateChaosValues((logging == true) ? "generator2" : null);
            GenerateChaosValues[] generators = new GenerateChaosValues[] { generator0, generator1, generator2 };

            generator0.GeneratorRotation(zero_location);
            generator1.GeneratorRotation(one_location);
            generator2.GeneratorRotation(two_location);

            if (option == "e")
            {
                Console.Write("Enter the text for encryption: ");
                input_value = Console.ReadLine();

                if (logging == true)
                {
                    generator0.PrintInConsole(zero_location);
                    generator1.PrintInConsole(one_location);
                    generator2.PrintInConsole(two_location);
                }

                Console.WriteLine("");
                Console.WriteLine($"The input message: {input_value}");

                object[] finalValue = torus_automorphism.Encryption(input_value, generators, logging);

                Console.WriteLine("");
                Console.Write("\nThe output message: ");
                for (int j = 0; j < finalValue.Length; j++) { Console.Write($"{finalValue[j]}"); }
            }
            else if (option == "d")
            {
                Console.Write("What is the ciphertext for decryption: ");
                string ciphertext_input = Console.ReadLine();
                if (logging == true)
                {
                    generator0.PrintInConsole(zero_location);
                    generator1.PrintInConsole(one_location);
                    generator2.PrintInConsole(two_location);
                }
                Console.WriteLine($"\nEncryption for input string: {ciphertext_input}");
                object[] finalDecrypted = torus_automorphism.Decryption(ciphertext_input, generators, logging);
                Console.Write("\nThe decrypted text is: ");
                for (int j = 0; j < finalDecrypted.Length; j++) { Console.Write(finalDecrypted[j]); }
            }
            Console.WriteLine("\n Press any key to exit...");
            Console.ReadKey();
        }
    }
}
