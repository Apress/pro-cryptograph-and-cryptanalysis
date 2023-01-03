using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace ECDiffiedHellmanCng
{
    class Program
    {
        //** Alice User
        class UserA
        {
            //** represents the public key of alice
            public static byte[] pk_alice;               

            public static void Main(string[] args)
            {
                string message_send_by_alice = "Hello Bob, Welcome to CryptoWorld!";

                using (ECDiffieHellmanCng user_alice = new ECDiffieHellmanCng())
                {
                    user_alice.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
                    user_alice.HashAlgorithm = CngAlgorithm.Sha256;
                    pk_alice = user_alice.PublicKey.ToByteArray();

                    //** we send to Bob
                    UserB user_bob = new UserB();
                    CngKey k = CngKey.Import(user_bob.pk_bob, CngKeyBlobFormat.EccPublicBlob);

                    byte[] alice_key = user_alice.DeriveKeyMaterial(CngKey.Import
                                      (user_bob.pk_bob, 
                                       CngKeyBlobFormat.EccPublicBlob));

                    byte[] encryptionOfMessage = null;
                    byte[] initialize_vector = null;
                    
                    //** sending the message
                    SendMessage(alice_key,
                                message_send_by_alice,
                                out encryptionOfMessage,
                                out initialize_vector);

                    Console.WriteLine("\n\nALICE Side");
                    Console.WriteLine("==========\n");
                    Console.WriteLine("\tAlice sends: {0}", message_send_by_alice);
                    Console.WriteLine("\tAlice public key: {0}", PrintByteArray(pk_alice).ToString());
                    Console.WriteLine("\tThe initialization vector (IV): {0} ", PrintByteArray(initialize_vector).ToString());
                    Console.WriteLine("\tLength of the message: {0} ", message_send_by_alice.Length.ToString());


                    //** receiving message
                    user_bob.ReceivingMessage(encryptionOfMessage, initialize_vector);
                }
            }

            //** the function will help us to convert a byte to string.
            public static StringBuilder PrintByteArray(byte[] bytes)
            {
                var string_builder = new StringBuilder("new byte[] { ");
                foreach (var theByte in bytes)
                {
                    string_builder.Append(theByte + " ");
                }
                string_builder.Append("}");
                return string_builder;
            }

            private static void SendMessage(byte[] key, 
                                            string theSecretMessage,
                                            out byte[] encryption_message, 
                                            out byte[] initialize_vector)
            {
                //** we will use AES cryptography algorithm for encryption
                using (Aes aes_crypto_alg = new AesCryptoServiceProvider())
                {
                    aes_crypto_alg.Key = key;
                    initialize_vector = aes_crypto_alg.IV;

                    //** we encrypt the message using AES
                    using (MemoryStream encrypted_text = new MemoryStream())
                    using (CryptoStream crypto_stream = new CryptoStream(encrypted_text, 
                                                              aes_crypto_alg.CreateEncryptor(),
                                                              CryptoStreamMode.Write))
                    {
                        byte[] clear_text = Encoding.UTF8.GetBytes(theSecretMessage);
                        crypto_stream.Write(clear_text, 0, clear_text.Length);
                        crypto_stream.Close();
                        encryption_message = encrypted_text.ToArray();
                    }

                    Console.WriteLine("\n\n(Encrypted) Message sended from Alice -> Bob");
                    Console.WriteLine("================================================\n");
                    Console.WriteLine("\tSecret message is: {0}", theSecretMessage.ToString());
                    Console.WriteLine("\tEncryptedMessage is: {0}", PrintByteArray(encryption_message).ToString());
                    Console.WriteLine("\tInitialize vector is: {0}", PrintByteArray(initialize_vector).ToString());
                }
            }
        }

        //** User Bob
        public class UserB
        {
            //** the public key of bon
            public byte[] pk_bob;
            private byte[] bob_key;

            public UserB()
            {
                using (ECDiffieHellmanCng user_bob = new ECDiffieHellmanCng())
                {
                    user_bob.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
                    user_bob.HashAlgorithm = CngAlgorithm.Sha256;
                    pk_bob = user_bob.PublicKey.ToByteArray();
                    bob_key = user_bob.DeriveKeyMaterial(CngKey.Import
                                (UserA.pk_alice, 
                                 CngKeyBlobFormat.EccPublicBlob));
                }
            }

            public void ReceivingMessage(byte[] message_encrypted, 
                                         byte[] initialize_vector)
            {
                using (Aes aes = new AesCryptoServiceProvider())
                {
                    aes.Key = bob_key;
                    aes.IV = initialize_vector;

                    //** let's decrypt the message
                    using (MemoryStream plaintext = new MemoryStream())
                    {
                        using (CryptoStream crypto_stream = new CryptoStream(plaintext, 
                                                   aes.CreateDecryptor(), 
                                                   CryptoStreamMode.Write))
                        {
                            crypto_stream.Write(message_encrypted, 
                                                0, 
                                                message_encrypted.Length);

                            crypto_stream.Close();
                            string message = Encoding.UTF8.GetString(plaintext.ToArray());

                            Console.WriteLine("\n\nBOB Side");
                            Console.WriteLine("============\n");
                            Console.WriteLine("The plaintext message is: {0}", message);
                            Console.WriteLine("The length of the message received " +
                                "by Bob is : {0}", message.Length.ToString());
                        }
                    }
                }
            }
        }
    }
}
