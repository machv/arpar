using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arpar
{
    /// <summary>
    /// Specialized type of argument attribute used for integer fields with ability to define upper and lower boundaries.
    /// </summary>
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

        /// <summary>
        /// Initializes attribute with unlimited boundaries.
        /// </summary>
        /// <param name="name">Name of argument.</param>
        public BoundedArgumentAttribute(string name)
            : base(name)
        {
            HighBound = int.MaxValue;
            LowBound = int.MinValue;
        }

        /// <summary>
        /// Initializes attribute with unlimited boundaries.
        /// </summary>
        /// <param name="name">Name of argument.</param>
        /// <param name="type">Type of argument name.</param>
        public BoundedArgumentAttribute(string name, ArgumentType type)
            : base(name, type)
        {
            HighBound = int.MaxValue;
            LowBound = int.MinValue;
        }
    }
}
