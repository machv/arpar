using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Arpar
{
    /// <summary>
    /// Saves information loaded from reflection.
    /// </summary>
    public class Argument
    {
        public ArgumentAttribute Attribute;
        //public List<ArgumentAliasAttribute> Names = new List<ArgumentAliasAttribute>();
        public Type Type;
        public FieldInfo Info;
        public bool IsSatisfied;

        public Dictionary<string, ArgumentAliasAttribute> Names = new Dictionary<string, ArgumentAliasAttribute>();
    }
}
