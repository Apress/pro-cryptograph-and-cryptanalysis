using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VerifiableComputation
{
    class MerkleTreeImplementation
    {
        private List<string> bigDataItems;
        private List<string> bigDataOriginalItems;

        public List<string> BigDataOriginalItems
        {
            get => bigDataOriginalItems;
            private set
            {
                bigDataOriginalItems = value;
                bigDataItems = value;
            }
        }

        public MerkleTreeImplementation(List<string> BigData_OriginalItems)
        {
            BigDataOriginalItems = BigData_OriginalItems;
            CreateTree();
        }

        public string GetRoot()
        {
            return bigDataItems[0];
        }

        private void CreateTree()
        {
            var data_items = bigDataItems;
            var temporary_data_items = new List<string>();
            
            //** using 2 element go and parse the list for items
            for (int i = 0; i < data_items.Count; i += 2)
            {
                //** Take the left element
                string left_element = data_items[i];

                //** the element from right is empty
                string right_element = String.Empty;

                //** once we have the proper item we will need to replace the empty string from above with the proper one
                if (i + 1 != data_items.Count)
                    right_element = data_items[i + 1];

                //** compute the hash for the left value
                string leftHash = HashTheBigData(left_element);

                //** if we we have the item from right as being empty we will hash it
                string rightHash = String.Empty;
                if (right_element != String.Empty)
                    rightHash = HashTheBigData(right_element);
                                
                //** if we have the hash for right empty, we will add the sum of the left with right into temporary_items
                if (right_element != String.Empty)
                    temporary_data_items.Add(leftHash + rightHash);
                //** contrary, we will add the left hash only
                else
                    temporary_data_items.Add(leftHash);
            }

            //** if the size of the list is different from 1
            if (data_items.Count != 1)
            {
                //** once we are here we will replace replace bigDataItems with temporary_data_items
                bigDataItems = temporary_data_items;
                //** call again the function
                CreateTree();
            }
            else                
                //** once we get 1 item then we can say that we have the root for the tree.
                //** we will save it at bigDataItems 
                bigDataItems = temporary_data_items;
        }
        private string HashTheBigData(string bigData)
        {
            using (var sha256 = SHA256.Create())
            {
                //** use some big data volume   
                byte[] hasshed_bytes_of_bigdata = sha256.ComputeHash(Encoding.UTF8.GetBytes(bigData));

                //** take the hash value and work with it accordingly
                string current_hash_bigdata_value = BitConverter.ToString(hasshed_bytes_of_bigdata).Replace("-", "").ToLower();
                return current_hash_bigdata_value;
            }
        }
    }
}
