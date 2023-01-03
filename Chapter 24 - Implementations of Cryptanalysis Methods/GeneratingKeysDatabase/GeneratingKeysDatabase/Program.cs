using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratingKeysDatabase
{
    class Program
    {
        public static string size = Console.ReadLine();
        public static int values_based_on_length = Convert.ToInt32(size);
        public char first_character = 'a';
        public char last_character = 'z';
        public int string_length = values_based_on_length;

        static void Main(string[] args)
        {            
            var writting_password = new Program();            
            writting_password.WrittingPasswordsAndKeys(" ");            
            Console.ReadLine();
        }

        //** automatically generates the passwords and create a file
        private void WrittingPasswordsAndKeys(string cryptographic_passwords)
        {
            //** location and file name that contains the passwords
            string file = "passwords_database.txt";

            //** add on each row a new password
            File.AppendAllText(file, Environment.NewLine + cryptographic_passwords);

            //** display it on the console
            Console.WriteLine(cryptographic_passwords);

            //** don't do anything if the length of the passwords is equal with the length of the string
            //** and continue with generating the passwords and keys
            if (cryptographic_passwords.Length == string_length)
            {
                return;
            }
            for (char c = first_character; c <= last_character; c++)
            {
                WrittingPasswordsAndKeys(cryptographic_passwords + c);
            }

        }
    }
}
