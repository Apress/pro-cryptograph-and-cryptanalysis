using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifiableComputation
{
    class Program
    {
        static void Main(string[] args)
        {
            //** the following items can be seen as big data items from a database
            //** as being the result of a query 
            List<string> bigdata_items = new List<string>
            {
                "WelcomeToApress!",
                "Studying C# is amazing",
                "Adding extra spice, such as cryptography makes it so challenging!",
                "You can master it with passion and dedication!",
                "Good luck!"
            };

            MerkleTreeImplementation tree = new MerkleTreeImplementation(bigdata_items);

            foreach (string s in tree.BigDataOriginalItems)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine("Hash integrity checking is: {0}", tree.GetRoot());
            Console.ReadKey();
        }
    }
}
