using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arpar;

namespace TestApplication
{
    class GoodSettings1
    {
        [Argument("a", ArgumentType.Short, IsMandatory = false, ValueRequirements = ParameterRequirements.Denied)]
        public bool a = false;

        [Argument("b", ArgumentType.Short, IsMandatory = false, ValueRequirements = ParameterRequirements.Denied)]
        public bool b = false;
    }
}
