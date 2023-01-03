using System;

namespace RSACryptography
{
    class Program
    {
        public static void Main(string[] args)
        {
            // encryption and decryption a password example
            var password = "P@sswrd123";

            Console.WriteLine("\n Original Password Text: " + password);            

            var textToBeEncrypted = CryptographyHelper.Encrypt(password);
            Console.WriteLine("\n Encrypted Password Text: " + textToBeEncrypted);            

            var textToBeDecrypted = CryptographyHelper.Decrypt(textToBeEncrypted);
            Console.WriteLine("\n Decrypted Password Text: " + textToBeDecrypted);

            //** encryption and decryption for database connection string
            var connectionString = "Data Source=USER-Test\\SQLEXPRESS;Initial Catalog=OrderProcessing;Integrated Security=True";

            Console.WriteLine("\n Original Connection String Text: " + connectionString);
            textToBeEncrypted = CryptographyHelper.Encrypt(connectionString);
            Console.WriteLine("\n Encrypted Connection String Text: " + textToBeEncrypted);
            textToBeDecrypted = CryptographyHelper.Decrypt(textToBeEncrypted);
            Console.WriteLine("\n Decrypted Connection String Text: " + textToBeDecrypted);

            //** encryption and decryption of a very long query result from DB
            var longTextForEncryption = "Literally, Blockchain is a chain of blocks which could be simply assumed as an immutable data structure. Immutability is one of the most prominent features of a blockchain, which leads us to build trust in completely unreliable environments.";
            Console.WriteLine("The encryption of the query is: {0}", CryptographyHelper.Encrypt(longTextForEncryption));

            Console.ReadKey();
        }
    }
}
