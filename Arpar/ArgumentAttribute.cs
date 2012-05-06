using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arpar
{
    /// <summary>
    /// Base attribute for altering argument behaviour.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ArgumentAttribute : System.Attribute
    {
        /// <summary>
        /// Argument name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type of agrument.
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
        /// Value requirements.
        /// </summary>
        public ParameterRequirements ValueRequirements { get; set; }

        /// <summary>
        /// Initialize attribute with specified name and sets type to long.
        /// </summary>
        /// <param name="name">Name of argument.</param>
        public ArgumentAttribute(string name)
            : this(name, ArgumentType.Long)
        {
        }

        /// <summary>
        /// Initialize attribute.
        /// </summary>
        /// <param name="name">Name of argument.</param>
        /// <param name="type">Type of argument.</param>
        public ArgumentAttribute(string name, ArgumentType type)
        {
            Name = name;
            Type = type;
        }
    }
}
