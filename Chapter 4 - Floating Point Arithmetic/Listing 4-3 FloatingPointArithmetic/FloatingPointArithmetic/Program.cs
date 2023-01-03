using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloatingPointArithmetic
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("FLOATING POINT ARITHMETIC " +
                "by Marius Iulian MIHAILESCU " +
                "and Stefania Loredana NITA \n");
            
            Console.WriteLine("\t\tDifferent ways of computing 1/105.");
            Console.WriteLine("\t\tMultiply the output with 105 and subtract 1");
            Console.WriteLine("\t\tWe will get an error.");

            double d = 1 / 105.0;
            float s = 1 / 105.0F;

            Console.WriteLine("\t\t\tUsing double: {0} * " +
                "105 - 1 = {1} < 0!", d, d * 105.0 - 1.0);
            Console.WriteLine("\t\t\tUsing single: {0} * " +
                "105 - 1 = {1} > 0!", s, s * 105.0 - 1.0);
            Console.WriteLine();


            
            Console.WriteLine("\t\tComputing a chaos-based " +
                "value for cryptography purpose.");
            float chaotic_value = 4.99F * 17;            
            Console.WriteLine("\t\t\tThe chaotic value is " +
                "{0}.", chaotic_value);
            Console.WriteLine();



            Console.WriteLine("\t\tAnother example of chaotic " +
                "value for which we need the integer part.");
            int another_chaotic_value = (int)(100 * (1 - 0.1F));
            Console.WriteLine("\t\t\tAnother chaotic value is {0}.", 
                another_chaotic_value);
            Console.WriteLine();



            Console.WriteLine("\t\tFor cryptography is " +
                "important to have an implementation " +
                "for IEEE-754");
            double[] double_values = new double[] { 0, -1 / 
                Double.PositiveInfinity, 1, -1,
                //Math.PI,
                //Math.Exp(20),
                //Math.Exp(-20),
                //Double.PositiveInfinity,
                //Double.NegativeInfinity,
                //Double.NaN,
                //Double.Epsilon,
                // -Double.Epsilon,
                //10 / Double.MaxValue 
            };

            for (int i = 0; i < double_values.Length; i++)
            {
                Console.WriteLine("\t\t\tIEEE-754 Value Type({0}) = {1}",
                    double_values[i], 
                    FloatingPoint.Class(double_values[i]));

                Console.WriteLine("\t\t\t{0,19:E8}{1,19:E8}{2,19}{3,19}",
                    FloatingPoint.ComputerNextValue(double_values[i], 
                        Double.PositiveInfinity) - double_values[i],
                    
                    FloatingPoint.ComputerNextValue(double_values[i], 
                    Double.NegativeInfinity) - double_values[i],
                    
                    FloatingPoint.ComputingLogB(double_values[i]),
                    FloatingPoint.ReuturnSignificantMantissa(double_values[i]));
            }            
            Console.ReadLine();
        }
    }
}
