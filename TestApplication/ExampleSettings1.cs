using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arpar;

namespace TestApplication
{
    /// <summary>
    /// Example settings to show all possible variants.
    /// </summary>
    class ExampleSettings1
    {
        /// <summary>
        /// Mandatory field text with description and default value "Text to print".
        /// </summary>
        [Argument("text", IsMandatory = true, Description = "Hello world text to print.")]
        public string Greetings = "Text to print";

        /// <summary>
        /// Next mandatory field sec with short type of option and long alias second]
        /// </summary>
        [Argument("second", ArgumentType.Short, IsMandatory = true)]
        [ArgumentAlias("second")]
        public string Second;

        /// <summary>
        /// Bounded mandatory integer argument maxlength with alias len and default value 32.
        /// </summary>
        [BoundedArgument("maxlength", ArgumentType.Long, IsMandatory = true, Description = "Maximum length of text.", LowBound = 1, HighBound = 120)]
        [ArgumentAlias("len", ArgumentType.Short)]
        public int MaximumLenght = 32;

        /// <summary>
        /// Optional argument gender which accepts only two options: "Male", "Female"
        /// </summary>
        [ChoicesArgument("gender", "Male", "Female")]
        public string Gender = "Male";

        /// <summary>
        /// Identical way to write choices for gender.
        /// </summary>
        [ChoicesArgument("gender2", Choices = new string[] { "Male", "Female" })]
        public string Gender2 = "Male";

    }
}
