using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arpar;

namespace TestApplication
{
    class BadSettings2
    {
        [ChoicesArgument("val", ArgumentType.Short, Choices = new String[]{"ALPHA", "BETA", "GAMA"})]
        public string value = "ALPHA";
    }
}
