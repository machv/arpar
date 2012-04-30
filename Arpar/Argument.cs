using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arpar
{
    /// <summary>
    /// Saves information loaded from reflection.
    /// </summary>
    public class Argument
    {
        public ArgumentAttribute Attribute;
        public List<ArgumentAliasAttribute> Aliases = new List<ArgumentAliasAttribute>();
    }
}
