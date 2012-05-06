using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arpar
{
    public enum ArgumentType { Short, Long }

    /// <summary>
    /// Typy parametrů
    /// </summary>
    public enum ParameterRequirements { Mandatory, Optional, Denied }

    public enum CommandLineArgumentType { Defined, Splitter, Common }
}
