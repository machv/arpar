using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arpar
{
    /// <summary>
    /// Specialized attribute to define fixed enumeration of available choices for field of type string.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ChoicesArgumentAttribute : ArgumentAttribute
    {
        /// <summary>
        /// Available choices for value.
        /// </summary>
        public string[] Choices { get; set; }

        /// <summary>
        /// Initializes attribute with argument's name.
        /// </summary>
        /// <param name="name">Name of argument.</param>
        public ChoicesArgumentAttribute(string name)
            : base(name)
        {
        }

        /// <summary>
        /// Initializes attribute with argument's name and type.
        /// </summary>
        /// <param name="name">Name of argument.</param>
        /// <param name="type">Type of argument name.</param>
        public ChoicesArgumentAttribute(string name, ArgumentType type)
            : base(name, type)
        {
        }

        /// <summary>
        /// Initializes attribute with argument's name and list of available choices.
        /// </summary>
        /// <param name="name">Argument name.</param>
        /// <param name="choices">Choices for value.</param>
        public ChoicesArgumentAttribute(string name, params string[] choices)
            : base(name)
        {
            Choices = choices;
        }

        /// <summary>
        /// Initializes attribute with argument's name, type and list of available choices.
        /// </summary>
        /// <param name="name">Argument name.</param>
        /// <param name="type">Type of argument name.</param>
        /// <param name="choices">Choices for value.</param>
        public ChoicesArgumentAttribute(string name, ArgumentType type, params string[] choices)
            : base(name, type)
        {
            Choices = choices;
        }
    }
}
