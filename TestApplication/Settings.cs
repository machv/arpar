using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arpar;

namespace TestApplication
{
    class Settings
    {
        [Argument("maxlen", ArgumentType.Long, IsMandatory = true, Description = "Maximum length of greetings text.", HighBound = 120)]
        [ArgumentAlias("len", ArgumentType.Short)]
        [ArgumentAlias("délka", ArgumentType.Short)]
        public int MaximumLenght = -1;

        [Argument("text", ArgumentType.Long, IsMandatory = true, Description = "Hello world text to print.")]
        public string Greetings = "BAF";
    }
}
