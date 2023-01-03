using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloatingPointArithmetic
{
    public enum IEEEStandardization
    {        
        //** the value is a signaling NaN - not a number
        Signaling_Not_a_Number,        

        //** the value is represented by a quiet 
        //** NaN - not a number and non-signaling
        Quiet_Not_a_Number,

        //** the value represents a positive infinity        
        Value_Positive_Infinity,
        
        //** the value represents a negative infinity
        Value_Negative_Infinity,
        
        //** The value represents a normal and positive number
        Normalization_Positive_Normalized,
        
        //** The value represents a normal and negative number        
        Normalization_Negative_Normalized,
        
        //** A denormalized positive number        
        Denormalization_Positive_Denormalized,
        
        //** The value is a denormalized negative number
        Denormalization_Negative_Denormalized,
        
        //** The value represents a positive zero        
        Value_Positive_Zero,

        //** the value represents a negative zero
        Value_Negative_Zero
    }
}
