using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arpar
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class BoundedArgumentAttribute : ArgumentAttribute
    {
        /// <summary>
        /// Low bound for integer value.
        /// </summary>
        public int LowBound { get; set; } // toto neni treba u vsech parametru, co to mit az v nejake tride, ktera bude dedit od ArgumentAttribute?

        /// <summary>
        /// High bound for integer value.
        /// </summary>
        public int HighBound { get; set; }

        /// <summary>
        /// Private constructor for setting default values of properties because attributes cannot use nullable types.
        /// </summary>
        BoundedArgumentAttribute()
        {
            HighBound = int.MaxValue;
            LowBound = int.MinValue;
        }

        public BoundedArgumentAttribute(string name)
            : base(name)
        {
        }

        public BoundedArgumentAttribute(string name, ArgumentType type)
            : base(name, type)
        {
        }
    }
}
