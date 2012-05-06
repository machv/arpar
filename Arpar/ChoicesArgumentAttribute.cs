using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arpar
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ChoicesArgumentAttribute : ArgumentAttribute
    {
        public string[] Choices { get; set; }


        public ChoicesArgumentAttribute(string name)
            : base(name)
        {
        }

        public ChoicesArgumentAttribute(string name, ArgumentType type)
            : base(name, type)
        {
        }

        public ChoicesArgumentAttribute(string name, params string[] choices)
            : base(name)
        {
            Choices = choices;
        }

        public ChoicesArgumentAttribute(string name, ArgumentType type, params string[] choices)
            : base(name, type)
        {
            Choices = choices;
        }
    }
}
