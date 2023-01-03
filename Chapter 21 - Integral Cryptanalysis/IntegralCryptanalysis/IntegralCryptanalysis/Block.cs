using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegralCryptanalysis
{
    public class Block
    {
        /// <summary>
        /// Represents the data that are held by the block
        /// </summary>
        public byte[] DataStructure { get; set; }

        /// <summary>
        /// Represents the left half of the data block
        /// </summary>
        public byte[] LeftHalf
        {
            get
            {
                return DataStructure.Take(DataStructure.Length / 2).ToArray();
            }
            set
            {
                Array.Copy(value, DataStructure, DataStructure.Length / 2);
            }
        }

        /// <summary>
        /// Represents the right half of the data block
        /// </summary>
        public byte[] RightHalf
        {
            get
            {
                return DataStructure.Skip(DataStructure.Length / 2).ToArray();
            }
            set
            {
                Array.Copy(value, 0, DataStructure, DataStructure.Length / 2, DataStructure.Length / 2);
            }
        }

        /// <summary>
        /// Get and return as BitArray the left half of the block data
        /// </summary>
        public BitArray TheLeftBitsOfBlock
        {
            get
            {
                return new BitArray(LeftHalf);
            }
        }

        /// <summary>
        /// Get and return as BitArray the right half of the block data
        /// </summary>
        public BitArray RightBitsOfBlock
        {
            get
            {
                return new BitArray(RightHalf);
            }
        }

        /// <summary>
        /// Representation of the size in bytes of the Block
        /// </summary>
        public int BlockSize
        {
            get
            {
                return DataStructure.Length;
            }
        }

        /// <summary>
        /// The representation of a data block. Constructor 
        /// </summary>
        /// <param name="size_of_the_block">The size value (in bytes) of the block</param>
        public Block(int size_of_the_block)
        {
            DataStructure = new byte[size_of_the_block];
        }

        /// <summary>
        /// The representation of a data block. Constructor
        /// </summary>
        /// <param name="the_data_block">the data content stored by the block</param>
        public Block(byte[] the_data_block)
        {
            DataStructure = the_data_block;
        }

        /// <summary>
        /// Swaps the halves (left and right) of the block
        /// </summary>
        public void SwapHalfes()
        {
            byte[] temporary = LeftHalf;
            LeftHalf = RightHalf;
            RightHalf = temporary;
        }

        /// <summary>
        /// Converts the Block to a UTF-8 string
        /// </summary>
        /// <returns>String representation of this block</returns>
        public override string ToString()
        {
            return System.Text.Encoding.UTF8.GetString(DataStructure);
        }
    }

}
