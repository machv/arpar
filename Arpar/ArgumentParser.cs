using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Arpar
{
    /// <summary>
    /// Main class for handling command line arguments.
    /// </summary>
    public class ArgumentParser
    {
        #region Prefixes and splitters

        private static string shortOptionPrefix = "-";
        /// <summary>
        /// Prefix for short type of argument.
        /// </summary>
        public static string ShortOptionPrefix
        {
            get
            {
                return shortOptionPrefix;
            }
            set
            {
                shortOptionPrefix = value;
            }
        }

        private static string longOptionPrefix = "--";
        /// <summary>
        /// Prefix for long type of argument.
        /// </summary>
        public static string LongOptionPrefix
        {
            get
            {
                return longOptionPrefix;
            }
            set
            {
                longOptionPrefix = value;
            }
        }

        private static string splitter = "--";
        /// <summary>
        /// Splitter defining where arguments ends.
        /// </summary>
        public static string Splitter
        {
            get
            {
                return splitter;
            }
            set
            {
                splitter = value;
            }
        }
        
        private static char valueDelimiter = '=';
        /// <summary>
        /// Delimiter of value for short type of argument
        /// </summary>
        public static char ValueDelimiter
        {
            get
            {
                return valueDelimiter;
            }
            set
            {
                valueDelimiter = value;
            }
        }
        
        #endregion

        /// <summary>
        /// Pointer to user object with definition of arguments.
        /// </summary>
        private object ObjectToFill;

        /// <summary>
        /// Dictionary for searching for Argument by its name (may be used in Parser and when testing duplicity)
        /// </summary>
        private Dictionary<string, Argument> ArgumentsByName = new Dictionary<string, Argument>();

        /// <summary>
        /// Array of arguments accepted in format from main() function.
        /// </summary>
        public string[] ConsoleArgs { get; set; }

        /// <summary>
        /// List of all recognized arguments from user's definition object.
        /// </summary>
        private List<Argument> arguments = new List<Argument>();

        //TODO: zdokumentovat
        public List<String> CommonArguments { get; protected set; }


        /// <summary>
        /// Initialize and parse accepted arguments from definition object.
        /// </summary>
        /// <param name="sett">Object containing definition of all accepted arguments.</param>
        public ArgumentParser(object sett)
        {
            ObjectToFill = sett;

            Type type = ObjectToFill.GetType();

            FieldInfo[] infos = type.GetFields();

            // From reflection read all fields and if are anotated by our attributes, use them in arguments.
            foreach (FieldInfo info in infos)
            {
                // Only public fields are allowed to defining arguments (because we need to be able to write value back);
                if (!info.IsPublic)
                {
                    continue;
                }

                Argument arg = new Argument();

                object[] attributes = info.GetCustomAttributes(false);
                foreach (object attribute in attributes)
                {
                    if (attribute is ArgumentAttribute)
                    {
                        ArgumentAttribute attr = attribute as ArgumentAttribute;

                        arg.Attribute = attr;

                        // Check constraits for boundary argument type
                        if (attr is BoundedArgumentAttribute)
                        {
                            // type
                            if (info.FieldType != typeof(int))
                            {
                                throw new ArgumentException("Bounded argument can be used only with field of type int.");
                            }

                            // default value
                            BoundedArgumentAttribute boundedAttribute = attr as BoundedArgumentAttribute;
                            int intValue = (int)info.GetValue(ObjectToFill);

                            // 0 is default value if no value specified, so we have to skip this value in boundary check
                            if (intValue != 0 && !IsInAttributeBoundary(boundedAttribute, intValue))
                            {
                                throw new ArgumentOutOfRangeException(attr.Name, string.Format("Default value of argument {0} does not meet specified boundaries.", arg.Attribute.Name));
                            }
                        }

                        // Check constraints for choices argument type
                        if (attr is ChoicesArgumentAttribute)
                        {
                            if (info.FieldType != typeof(string))
                            {
                                throw new ArgumentException("Choices argument can be used only with field of type string.");
                            }

                            ChoicesArgumentAttribute choicesAttribute = attr as ChoicesArgumentAttribute;
                            string currentValue = info.GetValue(ObjectToFill) as string;

                            if (currentValue != null && !choicesAttribute.Choices.Contains(currentValue))
                            {
                                throw new ArgumentOutOfRangeException(attr.Name, string.Format("Default value {0} of argument {1} is not in allowed choices.", currentValue, arg.Attribute.Name));
                            }
                        }

                        // Also duplicate name of this argument into Names List for uniform manipulation.
                        ArgumentAliasAttribute aliasAttribute = new ArgumentAliasAttribute(attr.Name, attr.Type);
                        string prefixedName = GetPrefixedArgumentName(aliasAttribute);

                        CheckArgumentNameDuplicity(arg, aliasAttribute, prefixedName);

                        arg.Names.Add(prefixedName, aliasAttribute);
                    }

                    if (attribute is ArgumentAliasAttribute)
                    {
                        ArgumentAliasAttribute aliasAttribute = attribute as ArgumentAliasAttribute;
                        string prefixedName = GetPrefixedArgumentName(aliasAttribute);

                        CheckArgumentNameDuplicity(arg, aliasAttribute, prefixedName);

                        arg.Names.Add(prefixedName, aliasAttribute);
                    }
                }

                // If we matched all required fields than we can chceck validity and insert this argument into parsed arguments List 
                //   and include in names Dictionary)
                if (arg.Attribute != null)
                {
                    arg.Type = info.FieldType;
                    arg.Info = info;
                    arguments.Add(arg);

                    foreach (KeyValuePair<string, ArgumentAliasAttribute> alias in arg.Names)
                    {
                        ArgumentsByName.Add(alias.Key, arg);
                    }
                }
            }
        }

        /// <summary>
        /// Tests if name of argument is already inserted in our definitions. If yes, then fire corresponding exception.
        /// </summary>
        /// <param name="arg">Argument to check for duplicity.</param>
        /// <param name="aliasAttribute">Attribute containing name and type to be checked.</param>
        /// <param name="prefixedName">Parsed name containing already right prefix.</param>
        private void CheckArgumentNameDuplicity(Argument arg, ArgumentAliasAttribute aliasAttribute, string prefixedName)
        {
            // Check duplicity of argument across all registered arguments
            if (ArgumentsByName.ContainsKey(aliasAttribute.Name))
            {
                throw new DuplicateArgumentException(string.Format("Argument name {0} is already registered.", aliasAttribute.Name));
            }

            // But before inserting check duplicity within current attribute (because names of current attribute are
            //  registered in global ArgumentsByName after all names of currently processed attribute are known)
            if (arg.Names.ContainsKey(prefixedName))
            {
                throw new DuplicateArgumentException(string.Format("This argument has already registered name {0}.", aliasAttribute.Name));
            }
        }

        /// <summary>
        /// Generates documentation block for accepted arguments and writes it to standard output.
        /// </summary>
        public void GenerateDocumentation()
        {
            GenerateDocumentation(Console.Out);
        }

        /// <summary>
        /// Generates documentation block for accepted arguments and writes it text writer output.
        /// </summary>
        public void GenerateDocumentation(System.IO.TextWriter output)
        {
            foreach (Argument argument in arguments)
            {
                string prefix = GetArgumentPrefix(argument.Attribute.Type);
                output.WriteLine("  " + prefix + argument.Attribute.Name + ":\t" + argument.Attribute.Description);

                if (argument.Attribute is ChoicesArgumentAttribute)
                {
                    ChoicesArgumentAttribute choicesAttribute = argument.Attribute as ChoicesArgumentAttribute;

                    if (choicesAttribute.Choices != null)
                    {
                        output.WriteLine("\tList of values for this argument:");
                        foreach (string value in choicesAttribute.Choices)
                        {
                            output.WriteLine("\t\t" + value); // Muze byt udelano i na jedne radce oddelene carkami (stredniky)
                        }
                    }
                }
            }
        }

        /// <summary>
        /// According to argument's type returns full name with prefix.
        /// </summary>
        /// <param name="aliasAttribute">Alias attribute containing name and argument type.</param>
        /// <returns>Name with corresponding prefix.</returns>
        private string GetPrefixedArgumentName(ArgumentAliasAttribute aliasAttribute)
        {
            return GetPrefixedArgumentName(aliasAttribute.Name, aliasAttribute.Type);
        }

        /// <summary>
        /// Generates fully prefixed name of argument.
        /// </summary>
        /// <param name="name">Raw argument name.</param>
        /// <param name="type">Type of argument.</param>
        /// <returns>Name with corresponding prefix.</returns>
        private string GetPrefixedArgumentName(string name, ArgumentType type)
        {
            return GetArgumentPrefix(type) + name;
        }

        /// <summary>
        /// Returns string prefix corresponding to given argument type.
        /// </summary>
        /// <param name="type">Type of the argument.</param>
        /// <returns>Prefix for given argument type.</returns>
        private string GetArgumentPrefix(ArgumentType type)
        {
            switch (type)
            {
                case ArgumentType.Long:
                    return LongOptionPrefix;
                case ArgumentType.Short:
                    return ShortOptionPrefix;
            }

            return string.Empty;
        }

        /// <summary>
        /// Reads arguments, parse them and write parsed values to definition object passed to constructor.
        /// </summary>
        /// <param name="args">String containing the arguments.</param>
        public void Parse(string args)
        {
            ConsoleArgs = args.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            Parse(ConsoleArgs);
        }

        /// <summary>
        /// Reads arguments, parse them and write parsed values to definition object passed to constructor.
        /// </summary>
        /// <param name="args">Arguments in string array as passed to main() function.</param>
        /// <exception cref="ArgumentException">May be thrown during parsing of int or bool value if incorrect value is passed.</exception>
        public void Parse(string[] args) 
        {
            ConsoleArgs = args;

            CommonArguments = new List<string>();

            if (ConsoleArgs == null)
            {
                throw new InvalidOperationException("Missing parameters to parse.");
            }

            for (int index = 0; index < ConsoleArgs.Length; index++)
            {
                string currentArg = ConsoleArgs[index];
                CommandLineArgumentType argumentType = DetermineArgumentType(currentArg);

                switch (argumentType)
                {
                    case CommandLineArgumentType.Defined:
                        bool nextArgumentProcessed = TryLoadValueMoveIndex(currentArg, ConsoleArgs, index);

                        if (nextArgumentProcessed)
                        {
                            index++;
                        }
                        break;
                    case CommandLineArgumentType.Common:
                        CommonArguments.Add(currentArg);
                        break;
                    case CommandLineArgumentType.Splitter:
                        CopyRest(ConsoleArgs, CommonArguments, index + 1);
                        index = ConsoleArgs.Length;
                        break;
                }
            }

            if (!AllMandatoryArgumentsSatisfied())
            {
                throw new ArgumentException("All mandatory atributes has not been satisfied");
            }

        }

        /// <summary>
        /// Checks whether all arguments specified as mandatory has been set.
        /// </summary>
        /// <returns>True is all mandatory atributes has been set. False otherwise.</returns>
        private bool AllMandatoryArgumentsSatisfied()
        {
            foreach (Argument arg in arguments)
            {
                if (arg.Attribute.IsMandatory && !arg.IsSatisfied)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks whether the value for the argument can be defined. If it can then tries to load it at first
        /// from the same argument separated by '='. If it does not succeed tries to load the value from next
        /// argument. Arguments with denied value has to be bools and they are set to true when they are processed.
        /// </summary>
        /// <param name="arg">Processed argument. Can contain itself its value separated by '='.</param>
        /// <param name="ConsoleArgs">All arguments which are processed. The value of currently processed argument
        /// can be in the next argument.</param>
        /// <param name="index">Index of currently processed argument.</param>
        /// <returns>True if the value has been extracted from the next argument. False otherwise.</returns>
        private bool TryLoadValueMoveIndex(string arg, string[] ConsoleArgs, int index)
        {
            Argument argument;
            string value = null;

            if (ArgumentContainsValue(arg))
            {
                value = GetValueFromArgument(arg);
                arg = TrimValueFromArgument(arg);
            }

            if (ArgumentsByName.ContainsKey(arg))
            {
                argument = ArgumentsByName[arg];
            }
            else
            {
                throw new ArgumentException(string.Format("Argument {0} is not supported", arg));
            }

            if (argument.IsSatisfied)
            {
                throw new ArgumentException(string.Format("Argument {0} has been specified several times", arg));
            }

            bool isValue = NextArgumentIsValue(ConsoleArgs, index, argument.Type) || value != null;
            ParameterRequirements valueRequirements = argument.Attribute.ValueRequirements;

            if (valueRequirements == ParameterRequirements.Denied)
            {
                if (value != null)
                {
                    throw new ArgumentException(string.Format("Argument {0} has denied value specification", arg));
                }

                argument.Info.SetValue(ObjectToFill, true);
                argument.IsSatisfied = true;
            }
            else if (isValue)
            {
                if (value == null)
                {
                    LoadValue(argument, ConsoleArgs[index + 1]);
                    argument.IsSatisfied = true;
                    return true;
                }
                else
                {
                    LoadValue(argument, value);
                    argument.IsSatisfied = true;
                }
            }
            else if (valueRequirements == ParameterRequirements.Mandatory)
            {
                throw new ArgumentException(string.Format("Value for argument {0} is Mandatory and has been omitted", arg));
            }

            return false;
        }

        /// <summary>
        /// Determines whether the argument is defined by programer of the application, or it is common argument
        /// or it is separator of rest of common atributes.
        /// </summary>
        /// <param name="arg">The argument of which the type is wanted.</param>
        /// <returns>Type of the given argument.</returns>
        private CommandLineArgumentType DetermineArgumentType(String arg)
        {
            if (arg == null)
            {
                throw new ArgumentNullException("Argument in function DetermineArgumentType has not to be null.");
            }

            if (arg.Equals(Splitter))
            {
                return CommandLineArgumentType.Splitter;
            }
            else if (arg.StartsWith(LongOptionPrefix) || arg.StartsWith(ShortOptionPrefix))
            {
                return CommandLineArgumentType.Defined;
            }
            else
            {
                return CommandLineArgumentType.Common;
            }
        }

        /// <summary>
        /// Returns argument name without defined value in its deklaration.
        /// Example: From -a=2 returns -a.
        /// </summary>
        /// <param name="arg">Argument with possibly defined value.</param>
        /// <returns>Argument name without value deklaration.</returns>
        private string TrimValueFromArgument(string arg)
        {
            int index = arg.IndexOf(ValueDelimiter);
            string argument = arg.Remove(index);

            return argument;

        }

        /// <summary>
        /// Copy rest of the items from source field into destionation list starting
        /// with the item at position start.
        /// </summary>
        /// <param name="source">Source of items to be copied.</param>
        /// <param name="destination">Destination for copied items.</param>
        /// <param name="start">Position from which the copiing will be started.</param>
        private void CopyRest(string[] source, List<string> destination, int start)
        {
            for (int index = start; index < source.Length; index++)
            {
                destination.Add(source[index]);
            }
        }

        /// <summary>
        /// Determine whether the next argument could be a value of previous argument.
        /// </summary>
        /// <param name="args">Arguments which are processed.</param>
        /// <param name="index">Index of the currently processed argument.</param>
        /// <param name="type">Type of the currently processed argument.</param>
        /// <returns>True if the next argument could be value of currently processed argument. False otherwise.</returns>
        private bool NextArgumentIsValue(string[] args, int index, Type type)
        {
            index++;

            if (index >= args.Length)
            {
                return false;
            }

            string argument = args[index];

            if (type == typeof(int))
            {
                int outValue;
                return int.TryParse(argument, out outValue);
            }

            CommandLineArgumentType argumentType = DetermineArgumentType(argument);

            if (argumentType == CommandLineArgumentType.Common)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines wherter the argument contains its value.
        /// Exampla: -a=2.
        /// </summary>
        /// <param name="argument">Argument to be checked.</param>
        /// <returns>True if the argument contins its value. False otherwise.</returns>
        private bool ArgumentContainsValue(string argument)
        {
            int index = argument.IndexOf(ValueDelimiter);

            // the index has not to be the last position of the string, therefor the -1.
            if (index > 0 && index < argument.Length - 1)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Extract the value from the argument and return it in the form of the string.
        /// </summary>
        /// <param name="argument">Argument from which the value should be extracted.</param>
        /// <returns>String form of the extracted value.</returns>
        private string GetValueFromArgument(string argument)
        {
            int index = argument.IndexOf(ValueDelimiter);

            // Wew have to remove the '=' character, therefor the +1.
            string value = argument.Remove(0, index + 1);

            return value;
        }

        /// <summary>
        /// Loads value into variable representing the given argument.
        /// </summary>
        /// <param name="argument">The argument representing the variable.</param>
        /// <param name="value">Value of the variable in string form.</param>
        private void LoadValue(Argument argument, string value)
        {
            if (argument.Type == typeof(string))
            {
                if (argument.Attribute is ChoicesArgumentAttribute)
                {
                    ChoicesArgumentAttribute choicesAttribute = argument.Attribute as ChoicesArgumentAttribute;

                    if (choicesAttribute.Choices == null)
                    {
                        argument.Info.SetValue(ObjectToFill, value);
                    }
                    else if (choicesAttribute.Choices.Contains(value))
                    {
                        argument.Info.SetValue(ObjectToFill, value);
                    }
                    else
                    {
                        throw new ArgumentException(string.Format("Value {0} is not in the list of supported values.", value));
                    }
                }
                else //Regular string value
                {
                    argument.Info.SetValue(ObjectToFill, value);
                }
            }
            else if (argument.Type == typeof(int))
            {
                int intValue = int.Parse(value);

                if (argument.Attribute is BoundedArgumentAttribute)
                {
                    BoundedArgumentAttribute boundedAttribute = argument.Attribute as BoundedArgumentAttribute;

                    if (IsInAttributeBoundary(boundedAttribute, intValue))
                    {
                        argument.Info.SetValue(ObjectToFill, intValue);
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }
            }
            else if (argument.Type == typeof(bool))
            {
                bool boolValue = bool.Parse(value);
                argument.Info.SetValue(ObjectToFill, boolValue);
            }
            else
            {
                throw new ArgumentException(string.Format("Argument of type {0} is not supported.", argument.Type.ToString()));
            }
        }

        /// <summary>
        /// Tests if value is inside required boundaries defined in attribute.
        /// </summary>
        /// <param name="boundedAttribute">Attribute containing boundaries to be checked.</param>
        /// <param name="intValue">Value to be checked if is inside boundaries.</param>
        /// <returns>True if inside boundaries.</returns>
        private static bool IsInAttributeBoundary(BoundedArgumentAttribute boundedAttribute, int intValue)
        {
            return intValue >= boundedAttribute.LowBound && intValue <= boundedAttribute.HighBound;
        }
    }
}