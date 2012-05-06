using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arpar
{
    /// <summary>
    /// Attribute for adding aliases to existing argument attribute. May be defined multiple times to same field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class ArgumentAliasAttribute : System.Attribute
    {
        /// <summary>
        /// Name for argument.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type of name.
        /// </summary>
        public ArgumentType Type { get; set; }

        /// <summary>
        /// Initializes attribute with name and long type of name.
        /// </summary>
        /// <param name="name">Name for alias.</param>
        public ArgumentAliasAttribute(string name)
        {
            Name = name;
            Type = ArgumentType.Long;
        }

        /// <summary>
        /// Initializes attribute.
        /// </summary>
        /// <param name="name">Name for alias.</param>
        /// <param name="type">Type of name.</param>
        public ArgumentAliasAttribute(string name, ArgumentType type)
        {
            Name = name;
            Type = type;
        }
    }
}
