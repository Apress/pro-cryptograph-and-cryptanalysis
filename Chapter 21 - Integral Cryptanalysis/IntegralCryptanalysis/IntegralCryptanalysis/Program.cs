using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegralCryptanalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            string theKeyword = "", data_input = "", data_output = "", data_file = "";

            FeistelNetwork feist_network;
            StreamReader file_stream_reader;
            StreamWriter file_stream_writer;

            //** create a help text for the user
            const string helperData =
            @"Building Integral Cryptanalysis Attack 
                Usage:
				private [-option] keyword input_file output_file
				Options:
					-enc Encrypt the file passed as input using the keyword
					-dec Decrypt the file passed as input using the keyword";

            //** show in the console the helper 
            //** if we have less than four arguments
            if (args.Length < 4)
            {
                Console.WriteLine(helperData);
                return;
            }
            else if (args[1].Length < 10 ||args[1].Length > 40)
            {
                //** Output usage if the password 
                //** is too short/long
                Console.Write("The length of the password is invalid.The password should have between 10 - 40 characters.\n" + helperData);
                return;
            }

            theKeyword = args[1];
            data_input = args[2];
            data_output = args[3];

            //** environment input/output configuration
            feist_network = new FeistelNetwork(theKeyword);
            file_stream_reader = new StreamReader(data_input);
            file_stream_writer = new StreamWriter(data_output);

            //** Read the data from the input file
            data_file = file_stream_reader.ReadToEnd();
            file_stream_reader.Close();

            if (args[0] == "-enc")
            {
                //** do the encryption based 
                //** on the argument provided
                string ciphertext = feist_network.EncryptionOp(data_file);
                file_stream_writer.Write(ciphertext);
                Console.WriteLine("The file has been encrypted with success.The file has been saved to: " + data_output);
            }
            else if (args[0] == "-dec")
            {
                //** do the decryption based on the argument
                string thePlaintext = feist_network.DecryptionOp(data_file);
                file_stream_writer.Write(thePlaintext);
                Console.WriteLine("The file has been decrypted with success.The file has been saved to: " + data_output);
            }
            else
            {
                //** invalid option selected
                Console.Write("The selected option is invalid. Please, choose another option.\n"  + helperData);
            }

            file_stream_writer.Close();

            Console.ReadKey();
        }

    }
}
