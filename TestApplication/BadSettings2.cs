using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arpar;

namespace TestApplication
{
    class BadSettings2
    {
        [Argument("val", ArgumentType.Short, ListOfString = new String[]{"ALPHA", "BETA", "GAMA"})]
        public string value = "ALPHA";
    }
}
