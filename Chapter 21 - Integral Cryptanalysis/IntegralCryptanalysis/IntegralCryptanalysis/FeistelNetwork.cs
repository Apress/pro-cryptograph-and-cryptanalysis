using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegralCryptanalysis
{
    public class FeistelNetwork
    {
        //** represents the size in bytes 
        //** for each of the block
        public int BlockSize
        {
            get;
            private set;
        }

        //** the password code for 
        //** encryption and decryption
        public string PasswordCode
        {
            get;
            set;
        }

        /// <summary>
        /// The basic constructor of Feistel Class.
        /// </summary>
        /// <param name="password_code">represents the 
        /// password code</param>
        public FeistelNetwork(string password_code)
        {
            this.PasswordCode = password_code;
            this.BlockSize = 16;
        }

        /// <summary>
        /// Constructs a new instance of the Feist class, with a custom blocksize
        /// </summary>
        /// <param name="password_code">Passcode used in this instance</param>
        /// <param name="the_block_size">Size of the blocks to use in this instance</param>
        public FeistelNetwork(string password_code, int the_block_size) : this(password_code)
        {
            this.BlockSize = the_block_size;
        }

        /// <summary>
        /// Encryption operation of the clear text using the password code
        /// </summary>
        /// <param name="clearText">The string to encrypt</param>
        /// <returns>The encrypted text.</returns>
        public string EncryptionOp(string clearText)
        {
            return DoCiphering(clearText, true);
        }

        /// <summary>
        /// Decryption operation of the encrypted text using the password code
        /// </summary>
        /// <param name="clearText">The string to decrypt</param>
        /// <returns>The decrypted text.</returns>
        public string DecryptionOp(string clearText)
        {
            return DoCiphering(clearText, false);
        }

        /// <summary>
        /// Do a Feistel encryption on the text
        /// </summary>
        /// <param name="sourceText">The clear text or encrypted text to encrypt/decrypt</param>
        /// <param name="isClearText">Decide if the given text represents (true) or not (false) a plaintext string</param>
        /// <returns>A string of plain or ciphered 
        /// text</returns>
        private string DoCiphering(string sourceText, bool isClearText)
        {
            int pointer_block = 0;
            string cipher_text_posting = "";
            List<ulong> the_round_keys = new Key(PasswordCode).ReturnRoundKeys();

            //** Do a padding operation to 
            //** the string using '\0'.
            //** The output will 
            //** be a multiple of <blocksize>
            while (sourceText.Length % BlockSize != 0)
                sourceText += new char();

            //** in case of decryption, reverse
            //** the encryption keys 
            if (!isClearText)
                the_round_keys.Reverse();

            byte[] the_text_bytes = Encoding.UTF8.GetBytes(sourceText);

            //** do iteration through the text
            //** moving with <blocksize> bytes per iteration
            while (pointer_block < the_text_bytes.Length)
            {
                byte[] the_block_as_bytes = new byte[BlockSize];
                Array.Copy(the_text_bytes, pointer_block, the_block_as_bytes, 0, BlockSize);

                Block text_as_block = new Block(the_block_as_bytes);

                //** if we have a ciphertext, 
                //** swap it in halves
                if (!isClearText)
                    text_as_block.SwapHalfes();

                //** each round keys will be 
                //** applied to the text
                foreach (ulong the_round_key in the_round_keys)
                    text_as_block = RoundOnTheBlock(text_as_block, the_round_key);

                //** build the output by appending it
                if (!isClearText) text_as_block.SwapHalfes();
                cipher_text_posting += text_as_block.ToString();

                pointer_block += BlockSize;
            }
            return cipher_text_posting.Trim('\0');
        }

        /// <summary>
        /// Do a single round encryption on the block
        /// </summary>
        /// <param name="theBlock">The block that will be encrypted or decrypted</param>
        /// <param name="theRoundKey">The round key which will be applied as the round function</param>
        /// <returns>The next block in the round sequence</returns>
        private Block RoundOnTheBlock(Block block, ulong theRoundKey)
        {
            ulong theRoundFunction = 0;

            Block roundBlock = new Block(block.BlockSize);

            BitArray keyBits = new BitArray(BitConverter.GetBytes(theRoundKey)), funcBits = block.RightBitsOfBlock.Xor(keyBits);

            roundBlock.LeftHalf = block.RightHalf;

            //** do the proper casting AND round 
            //** the function bits to an int
            //** set R(i+1) as L(i) XOR f
            theRoundFunction = ToInteger64(funcBits);
            roundBlock.RightHalf = BitConverter.GetBytes(ToInteger64(block.TheLeftBitsOfBlock) ^ theRoundFunction);

            return roundBlock;
        }

        /// <summary>
        /// Helper method used for conversion of BitArray to have an integer representation
        /// </summary>
        /// <param name="theArray">BitArray that will be converted</param>
        /// <returns>A value of 64-bit integer of the array</returns>
        private ulong ToInteger64(BitArray theArray)
        {
            byte[] array_as_byte = new byte[8];
            theArray.CopyTo(array_as_byte, 0);
            return BitConverter.ToUInt64(array_as_byte, 0);
        }
    }

}
