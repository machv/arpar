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
        public int LowBound { get; set; }

        /// <summary>
        /// High bound for integer value.
        /// </summary>
        public int HighBound { get; set; }

        public BoundedArgumentAttribute(string name)
            : base(name)
        {
            HighBound = int.MaxValue;
            LowBound = int.MinValue;
        }

        public BoundedArgumentAttribute(string name, ArgumentType type)
            : base(name, type)
        {
            HighBound = int.MaxValue;
            LowBound = int.MinValue;
        }
    }
}
