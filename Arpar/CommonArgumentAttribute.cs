using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arpar
{
    /// <summary>
    /// Attribute to describe common arguments used on class describing arguments.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CommonArgumentAttribute : System.Attribute
    {
        /// <summary>
        /// Initializes attribute with description. Also sets IsMandatory to true.
        /// </summary>
        /// <param name="description">Description (name) of common argument.</param>
        public CommonArgumentAttribute(string description)
        {
            IsMandatory = true;
            Description = description;
        }

        /// <summary>
        /// Description of argument - used for generating documentation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// If this argument is mandatory (used only in generation of documentation).
        /// </summary>
        public bool IsMandatory { get; set; }
    }
}
