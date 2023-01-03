using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org;

namespace BCCase
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Generate private/public key pair");
            Console.WriteLine("================================\n");

            Console.WriteLine("CASE 1 - 128 Bytes\n");
            GeneratePKeys(128);
            Console.WriteLine("*************************************************************");

            Console.WriteLine("\n\n\nCASE 2 - 256 Bytes\n");
            GeneratePKeys(256);

        }

        public static void GeneratePKeys(int intSize)
        {
            //Generating p-128 keys 128 specifies strength
            var keyPair = GenerateKeys(intSize);
            TextWriter textWriter = new StringWriter();
            Org.BouncyCastle.OpenSsl.PemWriter pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(textWriter);
            pemWriter.WriteObject(keyPair.Private);
            pemWriter.Writer.Flush();
            string privateKey = textWriter.ToString();
            
            Console.WriteLine("\tThe private key is:");
            Console.WriteLine("\t\t\t {0}", privateKey.ToString());

            ECPrivateKeyParameters privateKeyParam = (ECPrivateKeyParameters)keyPair.Private;

            Console.WriteLine("\tD parameter: {0}", privateKeyParam.D.ToString());

            textWriter = new StringWriter();
            pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(textWriter);
            pemWriter.WriteObject(keyPair.Public);
            pemWriter.Writer.Flush();


            ECPublicKeyParameters publicKeyParam = (ECPublicKeyParameters)keyPair.Public;

            string publickey = textWriter.ToString();
            
            Console.WriteLine("\nThe public key is:");
            Console.WriteLine("\t\t\t{0}", publickey.ToString());

            Console.WriteLine("\nX parameter: {0}", publicKeyParam.Q.XCoord.ToBigInteger().ToString());
            Console.WriteLine("Y parameter: {0}", publicKeyParam.Q.YCoord.ToBigInteger().ToString());   
        }

        public static AsymmetricCipherKeyPair GenerateKeys(int keySize)
        {
            //using ECDSA algorithm for the key generation
            var gen = new Org.BouncyCastle.Crypto.Generators.EllipticCurveKeyGenerator("ECDSA");

            //Creating Random
            var secureRandom = new SecureRandom();

            //Parameters creation using the random and keysize
            var keyGenParam = new KeyGenerationParameters(secureRandom, keySize);

            //Initializing generation algorithm with the Parameters            
            gen.Init(keyGenParam);

            //Generation of Key Pair
            return gen.GenerateKeyPair();
        }
    }
}
