using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arpar
{
    [AttributeUsage(AttributeTargets.Class)]
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
    }
}
