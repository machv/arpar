using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arpar
{
    [AttributeUsage(AttributeTargets.All)]
    public class ArgumentAttribute : System.Attribute
    {
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
        public object Value { get; internal set; }
        //public List<string> Names { get; set; }

        public string Name { get; set; }


        public ArgumentAttribute(string name, ArgumentType type)
        {
            Name = name;
            Type = type;
        }

        public ArgumentAttribute(string name)
        {
            Name = name;
            Type = ArgumentType.Long;
        }


    }
}
