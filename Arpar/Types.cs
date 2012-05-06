using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arpar
{
    /// <summary>
    /// Enumeration for type of argument name.
    /// </summary>
    public enum ArgumentType { Short, Long }

    /// <summary>
    /// Requirements for argument's value.
    /// </summary>
    public enum ParameterRequirements { Mandatory, Optional, Denied }

    //TODO: dokumentace prosím
    public enum CommandLineArgumentType { Defined, Splitter, Common }
}
