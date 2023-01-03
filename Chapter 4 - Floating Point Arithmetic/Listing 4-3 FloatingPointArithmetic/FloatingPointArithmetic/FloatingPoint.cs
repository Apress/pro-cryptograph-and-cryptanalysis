using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloatingPointArithmetic
{
    public sealed class FloatingPoint
    {
        #region variable and instances
            //** the constructor of the class
            private FloatingPoint() { }

            //** the value for conversion is 2^60
            //** 2^60 = 1,152,921,504,606,846,976 (decimal base)
            //** 2^60 = 10 00 00 00 00 00 00 00 (hex bytes)
            //** 8 * 2^60 = 2 * 1,152,921,504,606,846,976 = 
            //**          = 2,305,843,009,213,693,952
            //** we will use "unchecked" for supressing overflow-checking
            //** for integral-type arithmetic operations and conversions
            private static readonly double UnfavorableNegativeValue 
                = BitConverter.Int64BitsToDouble(unchecked(8 * 0x1000000000000000));

            //** constants        
            private const Double minimum_double = 4.94065645841247e-324;

            //** 0x7FFFFFFFFFFFFFFF = 9,223,372,036,854,775,807 (decimal)
            private const long mask_sign_value = -1 - 0x7FFFFFFFFFFFFFFF;
            private const long clear_mask_sign = 0x7FFFFFFFFFFFFFFF;

            private const long signficand_mask = 0xFFFFFFFFFFFFF;
            private const long clearing_significand_mask = mask_sign_value | significand_exponent_mask;

            private const long significand_exponent_mask = 0x7FF0000000000000;
            private const long clearing_exponent_mask = mask_sign_value | signficand_mask;

            private const int deviation = 1023;
        private const int significant_bits = 52;
        #endregion

        #region Methods for getting parts of a double's binary representation.
        
            //** the method return the significand of double value
            public static long ReuturnSignificantMantissa(double value)
            {
                return BitConverter.DoubleToInt64Bits(value) 
                            & signficand_mask;
            }
               
            //** the method will return the signicand 
            //** for a floating-point number
            public static double ReturnSignficantForFloatingPoint(double value)
            {
                if (Double.IsNaN(value)) return value;

                if (Double.IsInfinity(value)) return value;

                //** computing the exponent using the deviation
                int exponentValue = ComputeDeviationExponent(value);
                long significand = ReuturnSignificantMantissa(value);
            
                //** number 0 and denormalization 
                //** values has to be treated separetely
                if (exponentValue == 0)
                {
                    //** if the significand is equal 
                    //** with we will return 0
                    if (significand == 0)
                        return 0.0;

                    //** otherwise we will shit the significand to the left 
                    //** until significand will be 53 bits long
                    while (significand < signficand_mask)
                        significand <<= 1;                        
                        //** truncate the leading bit
                        significand &= signficand_mask;
                }
                return BitConverter.Int64BitsToDouble
                    ((deviation << 52) + significand);
            }

        
            //** The function will return a non-deviation 
            //** exponent for a floating-point value.
            //** The non-deviation is computed through
            //** substracting the deviation from deviated exponent.
            public static int NonDeviationExponent(double value)
            {
                return (int)((BitConverter.DoubleToInt64Bits(value) 
                    & significand_exponent_mask) >> 52) - deviation;
            }
       
            //** The function will return the 
            //** deviation exponnent for a 
            //** floating-point value.
            //** The returned value is obtained 
            //** and computed directly from
            //** and within binary representation of "value"
            public static int ComputeDeviationExponent(double value)
            {
                return (int)((BitConverter.DoubleToInt64Bits(value)
                    & significand_exponent_mask) >> 52);
            }
        
            //** The function returns the bit sign
            //** of a value. The bit sign is obtained 
            //** directly from the binary representation
            //** of the value
            public static int SignBit(double value)
            {
                return ((BitConverter.DoubleToInt64Bits(value) 
                    & mask_sign_value) != 0) ? 1 : 0;
            }
        #endregion

        #region Below contains the implementation of the IEEE-754         

            //** The class represents the implementation
            //** of IEEE-754 floating-point 
            //** References:
            //** https://www.geeksforgeeks.org/ieee-standard-754-floating-point-numbers/
            public static IEEEStandardization Class
                (double value)
            {
                long bits_value_representation = 
                    BitConverter.DoubleToInt64Bits(value);

                bool positive_value = (bits_value_representation >= 0);

                bits_value_representation = 
                    bits_value_representation & clear_mask_sign;

                //** case when we have a overflow
                //** for Not-A-Number
                if (bits_value_representation 
                    >= 0x7ff0000000000000) 
                {
                    //** this is case of infinity
                    if ((bits_value_representation 
                        & signficand_mask) == 0) 
                    {
                        if (positive_value)
                            return IEEEStandardization.
                                Value_Positive_Infinity;
                        else
                            return IEEEStandardization.
                                Value_Negative_Infinity;
                    }
                    else
                    {
                        if ((bits_value_representation 
                            & signficand_mask) 
                            < 0x8000000000000)
                            return IEEEStandardization.
                                Quiet_Not_a_Number;
                        else
                            return IEEEStandardization.
                                Signaling_Not_a_Number;
                    }
                }
                //** this is happening when we have
                //** 0 or a denormalization value
                else if (bits_value_representation 
                    < 0x0010000000000000)
                {
                    if (bits_value_representation == 0)
                    {
                        if (positive_value)
                            return IEEEStandardization.
                                Value_Positive_Zero;
                        else
                            return IEEEStandardization.
                                Value_Negative_Zero;
                    }
                    else
                    {
                        if (positive_value)
                            return IEEEStandardization.
                                Denormalization_Positive_Denormalized;
                        else
                            return IEEEStandardization.
                                Denormalization_Negative_Denormalized;
                    }
                }
                else
                {
                    if (positive_value)
                        return IEEEStandardization.
                            Normalization_Positive_Normalized;
                    else
                        return IEEEStandardization.
                            Normalization_Negative_Normalized;
                }
            }

            //** The function copy the 
            //** sign of the number.
            //** theSizeOfTheValue parameter 
            //**        the number for 
            //**        which we copy the sign
            //** theValueOfTheSign parameter
            //**        the value of the number
            //**        for which we do the copy
            public static double CopyProcedureForSign
                    (double theSizeOfTheValue, 
                     double theValueOfTheSign)
            {
                //** we do a bit manipulation
                //** do the copying process for
                //* the first bit for theSizeOfTheValue 
                //** and theValueOfTheSign
                return BitConverter.Int64BitsToDouble(
                    (BitConverter.DoubleToInt64Bits
                        (theSizeOfTheValue) & 
                            clear_mask_sign)
                    | (BitConverter.DoubleToInt64Bits
                        (theValueOfTheSign) & 
                            mask_sign_value));
            }     

            //** a boolean function to 
            //** check if the value is 
            //** finite or not
            public bool CheckIfIsFinite(double value)
            {
            //** Verify the part represented by the exponent.
            //** if all the bits are 1, then we 
            //** are dealing with a infinity (not-a-number).
                long bits = BitConverter.
                    DoubleToInt64Bits(value);
                return ((bits & significand_exponent_mask) 
                    == significand_exponent_mask);
            }      

            //** The function will return the
            //** non-biased exponent for a value.            
            public static double ComputingLogB(double value)
            {
                //** Let's deal with the 
                //** important situations.
                if (double.IsNaN(value))
                    return value;
                if (double.IsInfinity(value))
                    return double.PositiveInfinity;
                if (value == 0)
                    return double.NegativeInfinity;

                int exponentDeviationValue = 
                    ComputeDeviationExponent(value);

                //** if we dealing with a denormalization value
                //** we need to take the right action
                if (exponentDeviationValue == 0)
                {
                    exponentDeviationValue = -1074;

                    //** compute the signficand with no sign
                    long bits = BitConverter.
                        DoubleToInt64Bits(value) & clear_mask_sign;

                    //** we move on if we finish dealing with situations 
                    //** when bits = 0
                    do
                    {
                        bits >>= 1;
                        exponentDeviationValue++;
                    }
                    while (bits > 0);
                    return exponentDeviationValue;
                }

                //** exponentDeviationValue was significand,
                //** proceed with subtraction the deviation
                //** to obtain and compute the non-deviation exponent
                return exponentDeviationValue - deviation;
            }   

            //** Compute the floating-point 
            //** number for the next number.
            //** 'from' parameter
            //**        - represent the starting point
            //** 'to' parameter
            //**        - represent a value that shows 
            //**          the direction in which we will 
            //**          move in order to identity 
            //**          the next value
            public static double 
                ComputerNextValue(double from, double to)
            {
                //** If 'to' is equal with from
                //** there is no direction to move in,
                //** so we will compute and get 'from' value
                if (from == to)
                    return from;

                //** if not-a-number occur will 
                //** be returned by themselves
                if (double.IsNaN(from))
                    return from;
                if (double.IsNaN(to))
                    return to;

                //** an infinity will be an infinity all time
                if (double.IsInfinity(from))
                    return from;

                //** deal with 0 situation
                if (from == 0)
                    return (to > 0) ? 
                        minimum_double : -minimum_double;           

                //** For the rest of the 
                //** situation we are dealing.
                //** With incrementation or 
                //** decrementation the bits value.
                //** Values for transitions to infinity,
                //** denormalized values, and to zero are
                //** managed in this way.
                long bits_value = BitConverter.DoubleToInt64Bits(from);

                // A xor here avoids nesting conditionals. We have to increment if
                // fromValue lies between 0 and toValue.

                //** XOR operation will help us to 
                //** not taken into consideration
                //** conditionals. 
                if ((from > 0) ^ (from > to))
                    bits_value++;
                else
                    bits_value--;
                return BitConverter.
                    Int64BitsToDouble(bits_value);
            }      

            //** the function compute and return
            //** a value that is powered with 2
            public static double Scalb(double number, 
                                       int exponent)
            {
                // Treat special cases first.
                if (number == 0 || 
                            double.IsInfinity(number) || 
                            double.IsNaN(number))
                    return number;

                if (exponent == 0)
                    return number;

                int computedExponentValue = ComputeDeviationExponent(number);
                long significand = ReuturnSignificantMantissa(number);
                long getting_sign = ((number > 0) ? 0 : mask_sign_value);

                //** check if 'number' is denormalized
                if (computedExponentValue == 0)
                {
                    if (exponent < 0)
                    {
                        //** an exponent that is negative 
                        //** we will shift the significand 
                        //** -exponent bits to the right
                        significand >>= -exponent;
                        return BitConverter.
                            Int64BitsToDouble(getting_sign | significand);
                    }
                    else
                    {                    
                        //** a number that is positive is 
                        //** necessary to be shifted on left
                        //** and this will be done until a 
                        //** normalized number is obtained
                        while (significand <= signficand_mask 
                            && exponent > 0)
                        {
                            significand <<= 1;
                            exponent--;
                        }

                        if (significand > signficand_mask)
                            exponent++;                   

                        //** test if we have a overflow
                        if (exponent > 2 * deviation)
                            return (number > 0) ? 
                                double.PositiveInfinity 
                                : double.NegativeInfinity;

                        //** the number represents the 
                        //** significand exponent for the result
                        return BitConverter.Int64BitsToDouble(getting_sign 
                            | ((long)exponent << 52) | 
                                (significand & signficand_mask));
                    }
                }

                //** Once we are reaching here, 
                //** we are aware that 'exoponent' 
                //** is normalized.
                //** Proceeding with scaling. 'exponent'
                //** will be the significand exponent for the result
                computedExponentValue = 
                    computedExponentValue + exponent;

                //** verify if we have 0 or denormalization
                if (computedExponentValue < 0)
                {
                    significand = ((1L << 52) + 
                        significand) >> (1 - 
                            computedExponentValue);

                    return BitConverter.
                        Int64BitsToDouble(getting_sign | significand);
                }

                //** Veirfy if we have an overflow
                if (computedExponentValue > 
                    2 * deviation)
                    return (number > 0) ? 
                        double.PositiveInfinity : 
                        double.NegativeInfinity;

                //** If we're here, the result is normalized.
                long bits = getting_sign | 
                    ((long)computedExponentValue << 52) | significand;

                return BitConverter.Int64BitsToDouble(bits);
            }
        
            //** the function computes a value 
            //** wich will point out if the two
            //** values are unordered
            public static bool Unordered(double value1, double value2)
            {
                return double.IsNaN(value1) || double.IsNaN(value2);
            }
        #endregion

        #region Methods for conversion bit with single-precision
            public static unsafe int ConversionSingleToInt32Bits(float val)
            {
                return *((int*)&val);
            }

            public static unsafe float ConversionInt32BitsToSingle(int val)
            {
                return *((float*)&val);
            }
        #endregion
    }
}