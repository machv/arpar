using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arpar
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]

    public class ArgumentAliasAttribute : System.Attribute
    {
        public string Name { get; set; }
        public ArgumentType Type { get; set; }
        public ArgumentAliasAttribute(string name, ArgumentType type)
        {
            Name = name;
            Type = type;
        }
    }
}
