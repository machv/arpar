using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arpar;

namespace TestApplication
{
    class BadSettings1
    {
        [BoundedArgument("val", ArgumentType.Short, LowBound=0, HighBound=5)]
        public int value = 0;
    }
}
