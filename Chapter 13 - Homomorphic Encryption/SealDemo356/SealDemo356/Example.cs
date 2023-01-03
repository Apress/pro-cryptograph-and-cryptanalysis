using Microsoft.Research.SEAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace SealDemo356
{
    class Example
    {
        public static void EasyExample()
        {
            EncryptionParameters parameters = new EncryptionParameters(SchemeType.BFV);

            parameters.PolyModulusDegree = 4096;
            parameters.CoeffModulus = CoeffModulus.BFVDefault(4096);
            parameters.PlainModulus = new Modulus(1024);

            SEALContext SEALctx = new SEALContext(parameters);

            KeyGenerator keyGenerator = new KeyGenerator(SEALctx);
            PublicKey pK = keyGenerator.PublicKey;
            SecretKey sK = keyGenerator.SecretKey;
            Encryptor encrypt = new Encryptor(SEALctx, pK);
            Evaluator evaluate = new Evaluator(SEALctx);
            Decryptor decrypt = new Decryptor(SEALctx, sK);

            Console.WriteLine("Evaluation of 3(x^2 + 2)(x + 1)^2");
            Console.WriteLine();
            int value = 3;
            Plaintext plainValue = new Plaintext(value.ToString());
            Console.WriteLine($"The value = {value} is expressed as a plaintext polynomial 0x{plainValue}.");

            Console.WriteLine();
            Ciphertext encryptedValue = new Ciphertext();
            encrypt.Encrypt(plainValue, encryptedValue);

            Console.WriteLine($"- the size of the freshly encrypted value is: {encryptedValue.Size}");
            Console.WriteLine("- the initial noise budget of the encrypted value: {0} bits",
                decrypt.InvariantNoiseBudget(encryptedValue));

            Plaintext decryptedValue = new Plaintext();
            Console.Write("- the decryption of encrypted value: ");
            decrypt.Decrypt(encryptedValue, decryptedValue);
            Console.WriteLine($"0x{decryptedValue}");

            /*
            Compute (x^2 + 2).
            */
            Console.WriteLine("Compute squareValuePlusTwo (x^2+2).");
            Ciphertext squareValuePlusTwo = new Ciphertext();
            evaluate.Square(encryptedValue, squareValuePlusTwo);
            Plaintext plainTextTwo = new Plaintext("2");
            evaluate.AddPlainInplace(squareValuePlusTwo, plainTextTwo);


            Console.WriteLine($"- the size of squareValuePlusTwo: {squareValuePlusTwo.Size}");
            Console.WriteLine("- the noise budget in squareValuePlusTwo: {0} bits",
                decrypt.InvariantNoiseBudget(squareValuePlusTwo));

            Plaintext decryptedResult = new Plaintext();
            Console.Write("- the decryption of squareValuePlusTwo: ");
            decrypt.Decrypt(squareValuePlusTwo, decryptedResult);
            Console.WriteLine($"0x{decryptedResult}");

            /*
            Compute (x + 1)^2.
            */
            Console.WriteLine("Compute valuePlusOneSquare ((x+1)^2).");
            Plaintext plainTextOne = new Plaintext("1");
            Ciphertext valuePlusOneSquare = new Ciphertext();
            evaluate.AddPlain(encryptedValue, plainTextOne, valuePlusOneSquare);
            evaluate.SquareInplace(valuePlusOneSquare);
            Console.WriteLine($"- the size of valuePlusOneSquare: {valuePlusOneSquare.Size}");
            Console.WriteLine("- the noise budget in valuePlusOneSquare: {0} bits",
                decrypt.InvariantNoiseBudget(valuePlusOneSquare));
            Console.Write("- decryption of valuePlusOneSquare: ");
            decrypt.Decrypt(valuePlusOneSquare, decryptedResult);
            Console.WriteLine($"0x{decryptedResult}");

            /*
            Multiply (x^2 + 2) * (x + 1)^2 * 3.
            */

            Console.WriteLine("Compute encryptedOutcome 3(x^2 + 2)(x + 1)^2 .");
            Ciphertext encryptedOutcome = new Ciphertext();
            Plaintext plainTextThree = new Plaintext("3");
            evaluate.MultiplyPlainInplace(squareValuePlusTwo, plainTextThree);
            evaluate.Multiply(squareValuePlusTwo, valuePlusOneSquare, encryptedOutcome);
            Console.WriteLine($"- size of encryptedOutcome: {encryptedOutcome.Size}");
            Console.WriteLine("- the noise budget in encryptedOutcome: {0} bits",
                decrypt.InvariantNoiseBudget(encryptedOutcome));
            decrypt.Decrypt(encryptedOutcome, decryptedResult);
            Console.WriteLine("- decryption of 3(x^2+2)(x+1)^2 = 0x{0}",
                decryptedResult);
        }
    }
}
