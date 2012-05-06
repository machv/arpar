using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arpar;

namespace TestApplication
{
    class ExampleSettings2
    {
        /// <summary>
        /// Simple argument with denied value specification and defined short and long variant.
        /// </summary>
        [Argument("version", ArgumentType.Long, ValueRequirements=ParameterRequirements.Denied, Description="Outputs version of the program and exits the application.")]
        [ArgumentAlias("v", ArgumentType.Short)]
        public bool version;

        /// <summary>
        /// Argument with mandatory unbounded int value.
        /// </summary>
        [Argument("port", ArgumentType.Long, ValueRequirements=ParameterRequirements.Mandatory, Description="Number of the port.")]
        [ArgumentAlias("p", ArgumentType.Short)]
        public int port;

        /// <summary>
        /// Mandatory argument with bounded int value. Value is optional and default is 1.
        /// </summary>
        [BoundedArgument("optimization", ArgumentType.Long, IsMandatory=true, ValueRequirements=ParameterRequirements.Optional, LowBound=0, HighBound=5,
            Description="Degree of the optimization of the program. The range is [0, 5]. Default value is 1.")]
        [ArgumentAlias("o", ArgumentType.Short)]
        public int optimization = 1;

        /// <summary>
        /// Bool specification with mandatory value.
        /// </summary>
        [Argument("dump", ArgumentType.Long, ValueRequirements=ParameterRequirements.Mandatory, Description="Defines whether the coredump should be provided.")]
        [ArgumentAlias("d", ArgumentType.Short)]
        public bool dump = false;

        /// <summary>
        /// String argument with no specified list of values.
        /// </summary>
        [Argument("dumpFile", ArgumentType.Long, ValueRequirements=ParameterRequirements.Mandatory, Description="Specification of the file for the coredump.")]
        public string dumpFile;

        /// <summary>
        /// String argument with specified list of values.
        /// </summary>
        [ChoicesArgument("mode", ArgumentType.Long, Choices= new string[]{"client", "server"}, ValueRequirements=ParameterRequirements.Mandatory, Description="Mode of the application")]
        [ArgumentAlias("m", ArgumentType.Short)]
        public string mode = "client";

        // TODO: description for common arguments inputFile outputFile
    }
}
