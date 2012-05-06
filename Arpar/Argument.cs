using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Arpar
{
    /// <summary>
    /// Used for saving information loaded from reflection about object containing argument definitions.
    /// </summary>
    class Argument
    {
        /// <summary>
        /// Pointer to attribute object associated with argument.
        /// </summary>
        public ArgumentAttribute Attribute { get; set; }

        /// <summary>
        /// Type of argument used in argument definition.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Field info from reflection can be used to set value of object during parsing passed arguments to application.
        /// </summary>
        public FieldInfo Info { get; set; }

        /// <summary>
        /// If true then this argument was already satisfied during parsing of passed parameters. It is used to check duplicity in setting argument's value.
        /// </summary>
        public bool IsSatisfied { get; set; }

        /// <summary>
        /// Dictionary of all associated names with this argument (aliases together with primary name).
        /// </summary>
        public Dictionary<string, ArgumentAliasAttribute> Names = new Dictionary<string, ArgumentAliasAttribute>();
    }
}
