using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arpar
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CommonArgumentAttribute : System.Attribute
    {
        public CommonArgumentAttribute(string description)
        {
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
