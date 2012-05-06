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
        /// 
        /// </summary>
        Mandatory,

        /// <summary>
        /// 
        /// </summary>
        Optional,

        /// <summary>
        /// 
        /// </summary>
        Denied
    }

    //TODO: dokumentace prosím
    /// <summary>
    /// 
    /// </summary>
    public enum CommandLineArgumentType
    {
        /// <summary>
        /// 
        /// </summary>
        Defined,

        /// <summary>
        /// 
        /// </summary>
        Splitter,

        /// <summary>
        /// 
        /// </summary>
        Common
    }
}
