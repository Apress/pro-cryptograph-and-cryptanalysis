using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosSystemCryptography
{
    class GenerateChaosValues
    {
        private object[] items = new object[94];
        private char last_direction;
        private readonly string generator_ID;

        //** the property will return the index of the item for a specific value as character
        public int ItemIndex(char inputValue)
        {
            int locationElement;
            locationElement = Array.IndexOf(items, inputValue);
            return locationElement;
        }

        //** the property will return the index of the item for a specific value as integer
        public object ItemIndex(int inputValue)
        {
            object locationElement;
            locationElement = items.GetValue(inputValue);
            return locationElement;
        }

        //** constructor
        public GenerateChaosValues(string generator_name = null)
        {
            generator_ID = generator_name;
            for (int i = 32; i <= 125; i++)
            {
                items[i - 32] = (char)i;
            }
            
        }

        public void GeneratorRotation(int rotation, char chosen_direction) 
        {
            object[] rotation_done = new object[items.Length];
            int length = items.Length;
            int rotation_location = (rotation % length);

            //** the torus will have a right rotation
            if (chosen_direction == 'R')
            {
                for (int in_direction = 0; in_direction < length; in_direction++)
                {
                    if (rotation_location + in_direction >= length)
                    {
                        int suplimentary_rotation = rotation_location - (length - in_direction);
                        rotation_done[suplimentary_rotation] = items[in_direction];
                    }
                    else
                    {
                        rotation_done[in_direction + rotation_location] = items[in_direction];
                    }
                }
                last_direction = 'R';
            }
            else
            {
                for (int in_direction = 0; in_direction < length; in_direction++)
                {
                    if (rotation_location + in_direction >= length)
                    {
                        int suplimentary_rotation = rotation_location - (length - in_direction);
                        rotation_done[in_direction] = items[suplimentary_rotation];
                    }
                    else
                    {
                        rotation_done[in_direction] = items[in_direction + rotation_location];
                    }
                }
                last_direction = 'L';
            }
            Array.Copy(rotation_done, items, rotation_done.Length);

        }
        public void GeneratorRotation(int rotation)
        {
            object[] rotation_done = new object[items.Length];
            int length = items.Length;
            int rotation_location = (rotation % length);

            for (int in_direction = 0; in_direction < length; in_direction++)
            {
                if (rotation_location + in_direction >= length)
                {
                    int suplimentary_rotation = rotation_location - (length - in_direction);
                    rotation_done[suplimentary_rotation] = items[in_direction];
                }
                else
                {
                    rotation_done[in_direction + rotation_location] = items[in_direction];
                }
            }
            last_direction = 'R';
            Array.Copy(rotation_done, items, rotation_done.Length);
        }
        public void PrintInConsole(int rotated = 0)
        {            
            Console.Write($"{generator_ID} rotated {rotated} {((last_direction == 'L') ? "left" : "right")}: ");
            for (int i = 0; i < items.Length; i++)
            {
                Console.Write($"{items[i]}, ");
            }
            Console.WriteLine("");           
        }
    }
}
