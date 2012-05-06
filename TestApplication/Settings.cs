using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arpar;

namespace TestApplication
{
    class Settings
    {
        [BoundedArgument("maxlen", ArgumentType.Long, IsMandatory = true, Description = "Maximum length of greetings text.", HighBound = 120)]
        [ArgumentAlias("len", ArgumentType.Short)]
        [ArgumentAlias("délka", ArgumentType.Short)]
        public int MaximumLenght = -1;

        [Argument("text", ArgumentType.Long, IsMandatory = true, Description = "Hello world text to print.")]
        public string Greetings = "BAF";

        [ChoicesArgument("bs", ArgumentType.Short, IsMandatory = true, Description = "Bounded string.", Choices = new string[] { "ALPHA", "BETA", "GAMA" })]
        public string BoundedString = "ALPHA";

        [ChoicesArgument("aa", ArgumentType.Long, "Auto", "Kolo", "Pěšky")]
        public string Method = "Auto";

        [ChoicesArgument("gender", "Male", "Female")]
        public string Gender = "Male";

        [ChoicesArgument("gender2", Choices = new string[] { "Male", "Female" })]
        public string Gender2 = "Male";

    }
}
