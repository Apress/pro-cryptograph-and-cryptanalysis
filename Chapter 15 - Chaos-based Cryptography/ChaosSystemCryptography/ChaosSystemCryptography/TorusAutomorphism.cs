using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosSystemCryptography
{
    class TorusAutomorphism
    {
        private object CharacterEncryption(char input_characters, GenerateChaosValues[] iterations, int generators, bool logging)
        {
            int spotted_difference;
            spotted_difference = iterations[(generators % 2 == 0) ? 0 : 1].ItemIndex(input_characters);
            foreach (GenerateChaosValues iteration_generator in iterations)
            {
                iteration_generator.GeneratorRotation(spotted_difference, 'L');
                if (logging == true) { iteration_generator.PrintInConsole(spotted_difference + generators); }
            }
            return iterations[2].ItemIndex(0);
        }

        public object[] Encryption(string input_locations, GenerateChaosValues[] generators_locations, bool logging)
        {
            object[] finalOutputObject = new object[input_locations.Length];
            for (int i = 0; i < input_locations.Length; i++)
            {
                finalOutputObject[i] = CharacterEncryption(input_locations[i], generators_locations, i, logging);
            }
            return finalOutputObject;
        }

        private object CharacterDecryption(char input_characterst, GenerateChaosValues[] iterations, int generators, bool logging)
        {
            int spotted_difference;
            spotted_difference = iterations[2].ItemIndex(input_characterst);
            foreach (GenerateChaosValues iterWheel in iterations)
            {
                iterWheel.GeneratorRotation(spotted_difference, 'L');             
                if (logging == true) { iterWheel.PrintInConsole(spotted_difference + generators); }
            }
            return iterations[(generators % 2 == 0) ? 0 : 1].ItemIndex(0);
        }
        
        public object[] Decryption(string encryption_locations, GenerateChaosValues[] generators_locations, bool logging)
        {
            object[] finalOutputObject = new object[encryption_locations.Length];
            for (int i = encryption_locations.Length - 1; i >= 0; i--)
            {
                finalOutputObject[i] = CharacterDecryption(encryption_locations[i], generators_locations, i, logging);
            }
            return finalOutputObject;
        }
    }
}
