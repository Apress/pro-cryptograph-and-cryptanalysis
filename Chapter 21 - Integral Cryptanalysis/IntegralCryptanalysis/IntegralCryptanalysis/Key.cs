using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace IntegralCryptanalysis
{
    public class Key
    {
        //** permutation table (initial)
        //** used to permutate the key
        private byte[] initial_permutation_table_1 = {
            06, 30, 13, 07, 05, 35, 15, 14,
            12, 18, 03, 38, 09, 10, 22, 25,
            16, 04, 21, 08, 39, 37, 36, 02,
            24, 11, 28, 27, 29, 23, 33, 01,
            32, 17, 31, 00, 26, 34, 20, 19
        };

        /// <summary>
        /// The representation of the key as a byte array
        /// </summary>
        public byte[] KeyBytes
        {
            get; set;
        }

        /// <summary>
        /// Encryption and decryption key for a text
        /// </summary>
        /// <param name="keyAsAString">The key to use for this instance</param>
        public Key(string keyAsAString)
        {
            int k = 0, key_length = keyAsAString.Length;

            //** expansion of the key to a maximum of 40 bytes
            while (keyAsAString.Length < 40)
                keyAsAString += keyAsAString[k++];

            KeyBytes = System.Text.Encoding.UTF8.GetBytes(keyAsAString);

            //** permutation of the key bytes using
            //** initial_permutation_table_1 
            for (k = 0; k < KeyBytes.Length; k++)
                KeyBytes[k] = KeyBytes[initial_permutation_table_1[k]];

            Debug.WriteLine("The post permutation key is: "+ System.Text.Encoding.UTF8.GetString(KeyBytes));
        }

        /// <summary>
        /// Generate the keys that are used within the round function
        /// </summary>
        /// <returns>A list with the keys that are of 64-bit. The format is ulong.</returns>
        public List<ulong> ReturnRoundKeys()
        {
            //** Rounds is defined as 64-bit 
            //** keys found in the Key string
            int count_of_round = KeyBytes.Length / 8;
            List<ulong> round_keys = new List<ulong>();

            for (int k = 0; k < count_of_round; k++)
            {
                byte[] round_key_bytes = new byte[8];
                ulong round_key = 0;

                Array.Copy(KeyBytes, k * 8, round_key_bytes, 0, 8);
                Array.Reverse(round_key_bytes);

                round_key = BitConverter.ToUInt64(round_key_bytes, 0);

                round_keys.Add(round_key);
            }
            return round_keys;
        }
    }
}
