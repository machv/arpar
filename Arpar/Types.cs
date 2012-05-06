using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arpar
{
    /// <summary>
    /// Enumeration for type of argument's name (eg. short name).
    /// </summary>
    public enum ArgumentType
    {
        /// <summary>
        /// Short name of argument.
        /// </summary>
        Short,

        /// <summary>
        /// Long name of argument.
        /// </summary>
        Long
    }

    /// <summary>
    /// Requirements for argument's value.
    /// </summary>
    public enum ParameterRequirements
    {
        /// <summary>
        /// Mandatory value - Value has to be specified.
        /// </summary>
        Mandatory,

        /// <summary>
        /// Optional value - value can be passed to argument  but is not required.
        /// </summary>
        Optional,

        /// <summary>
        /// Denied value - argument does not have to have value.
        /// </summary>
        Denied
    }

    /// <summary>
    /// Type of the argument from command line.
    /// </summary>
    public enum CommandLineArgumentType
    {
        /// <summary>
        /// Argument in form with short or long prefix.
        /// Example: -v --version
        /// </summary>
        Defined,

        /// <summary>
        /// Delimiter of common arguments.
        /// Example: --
        /// </summary>
        Splitter,

        /// <summary>
        /// Common argument.
        /// Is not prefixed by "-" neither "--" and is not equal to "--".
        /// </summary>
        Common
    }
}
