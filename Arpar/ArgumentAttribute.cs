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
        public ParameterRequirements ValueRequirements { get; set; }

        protected ArgumentAttribute()
        {
        }

        public ArgumentAttribute(string name)
            : this(name, ArgumentType.Long)
        {
        }

        public ArgumentAttribute(string name, ArgumentType type)
        {
            Name = name;
            Type = type;
        }
    }
}
