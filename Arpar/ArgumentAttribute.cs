using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arpar
{
    /// <summary>
    /// Attribute for altering argument behaviour.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ArgumentAttribute : System.Attribute
    {
        /// <summary>
        /// Argument name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type of agrument (Long/Short.)
        /// </summary>
        public ArgumentType Type { get; set; }

        /// <summary>
        /// Description of argument - used for generating documentation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// If argument is mandatory.
        /// </summary>
        public bool IsMandatory { get; set; }

        /// <summary>
        /// Požadavky na hodnotu parametru
        /// </summary>
        public ParameterRequirements ParameterRequirements { get; set; }

        /// <summary>
        /// Low bound for integer value.
        /// </summary>
        public int LowBound { get; set; }

        /// <summary>
        /// High bound for integer value.
        /// </summary>
        public int HighBound { get; set; }

        /// <summary>
        /// Private constructor for setting default values of properties because attributes cannot use nullable types.
        /// </summary>
        ArgumentAttribute()
        {
            HighBound = int.MaxValue;
            LowBound = int.MinValue;
        }

        public ArgumentAttribute(string name)
            : this(name, ArgumentType.Long)
        {
        }

        public ArgumentAttribute(string name, ArgumentType type)
            : this()
        {
            Name = name;
            Type = type;
        }
    }
}
