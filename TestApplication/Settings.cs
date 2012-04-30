using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arpar;

namespace TestApplication
{
    class Settings
    {
        [Argument("maxlen", ArgumentType.Long, IsMandatory = true, Description = "Maximum length of greetings text.")]
        [ArgumentAlias("len", ArgumentType.Short)]
        [ArgumentAlias("délka", ArgumentType.Short)]
        public int MaximumLenght = -1;

        public string Greetings;
    }
}
