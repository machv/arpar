using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Arpar
{
    /// <summary>
    /// Main class for manipulating with command line arguments.
    /// </summary>
    public class ArgumentParser
    {
        protected string ShortOptionPrefix = "-";
        protected string LongOptionPrefix = "--";
        protected string Splitter = "--";
        protected char ValueDelimiter = '=';
        protected object ObjectToFill;

        /// <summary>
        /// Dictionary for searching for Argument by its name (may be used in Parser and when testing duplicity)
        /// </summary>
        protected Dictionary<string, Argument> ArgumentsByName = new Dictionary<string, Argument>();

        /// <summary>
        /// Array of arguments accepted in format from main() function.
        /// </summary>
        public string[] ConsoleArgs { get; set; }
        protected List<Argument> arguments = new List<Argument>();

        protected List<String> CommonArguments { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sett">Object with accepted arguments.</param>
        public ArgumentParser(object sett)
        {
            ObjectToFill = sett;

            Type type = ObjectToFill.GetType();

            FieldInfo[] infos = type.GetFields();

            foreach (FieldInfo info in infos)
            {
                Argument arg = new Argument();

                object[] attributes = info.GetCustomAttributes(false);
                foreach (object attribute in attributes)
                {
                    if (attribute is ArgumentAttribute)
                    {
                        ArgumentAttribute attr = attribute as ArgumentAttribute;

                        arg.Attribute = attr;

                        // Check duplicity of argument across all registered arguments
                        if (ArgumentsByName.ContainsKey(attr.Name))
                        {
                            throw new DuplicateArgumentException("Argument name " + attr.Name + " is already registered.");
                        }

                        // Also duplicate name of this argument into Names List for uniform manipulation.
                        arg.Names.Add(new ArgumentAliasAttribute(attr.Name, attr.Type));
                    }

                    if (attribute is ArgumentAliasAttribute)
                    {
                        ArgumentAliasAttribute alias = attribute as ArgumentAliasAttribute;

                        // Check duplicity of argument across all registered arguments
                        if (ArgumentsByName.ContainsKey(alias.Name))
                        {
                            throw new DuplicateArgumentException("Argument name " + alias.Name + " is already registered.");
                        }

                        arg.Names.Add(alias);
                    }
                }

                // If we matched all required fields than we can insert this into parsed arguments List and include in names Dictionary)
                if (arg.Attribute != null)
                {
                    arg.Type = info.FieldType;
                    arg.Info = info;
                    arguments.Add(arg);

                    foreach (ArgumentAliasAttribute alias in arg.Names)
                    {
                        ArgumentsByName.Add(alias.Name, arg);
                    }
                }
            }
        }

        public void GenerateDocumentation()
        {
            foreach (Argument argument in arguments)
            {
                string prefix = GetArgumentPrefix(argument.Attribute.Type);
                Console.WriteLine("  " + prefix + argument.Attribute.Name + ":\t" + argument.Attribute.Description);

                if (argument.Attribute.ListOfString != null)
                {
                    Console.WriteLine("\tList of values for this argument:");
                    foreach(string value in argument.Attribute.ListOfString)
                    {
                        Console.WriteLine("\t\t" + value); // Muze byt udelano i na jedne radce oddelene carkami (stredniky)
                    }
                }
            }

        }

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

        public void Parse(string args)
        {
            ConsoleArgs = args.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            Parse(ConsoleArgs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public void Parse(string[] args) // TODO: parser muze vyhodit vyjimku pri parsovani intu a boolu. Je to treba zdokumentovat.
        {
            ConsoleArgs = args;

            CommonArguments = new List<string>();

            if (ConsoleArgs == null)
            {
                throw new InvalidOperationException("Missing parameters to parse.");
            }

            for (int index = 0; index < ConsoleArgs.Length; index++)
            {
                string CurrentArg = ConsoleArgs[index];
                string TrimmedArg = null;
                CommandLineArgumentType argumentType = DetermineArgumentType(CurrentArg);

                switch (argumentType)
                {
                    case CommandLineArgumentType.Short:
                        TrimmedArg = TrimArgumentPrefix(CurrentArg, ArgumentType.Short);
                        break;
                    case CommandLineArgumentType.Long:
                        TrimmedArg = TrimArgumentPrefix(CurrentArg, ArgumentType.Long);
                        break;
                    case CommandLineArgumentType.Common:
                        CommonArguments.Add(CurrentArg);
                        continue;
                    case CommandLineArgumentType.Splitter:
                        CopyRest(ConsoleArgs, CommonArguments, index + 1);
                        index = ConsoleArgs.Length;
                        continue;
                }

                bool nextArgumentProcessed = TryLoadValueMoveIndex(TrimmedArg, ConsoleArgs, index);

                if (nextArgumentProcessed)
                    index++;

            }

        }

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
                throw new ArgumentException("Argument " + arg + " is not supported");
            }

            bool isValue = NextArgumentIsValue(ConsoleArgs, index) || value != null;
            ParameterRequirements valueRequirements = argument.Attribute.ParameterRequirements;

            if (isValue && valueRequirements != ParameterRequirements.Denied)
            {
                if (value == null)
                {
                    LoadValue(argument, ConsoleArgs[index+1]);
                    return true;
                }
                else
                {
                    LoadValue(argument, value);
                }
            }
            else if (!isValue && valueRequirements == ParameterRequirements.Mandatory)
            {
                throw new ArgumentException("Value for argument " + arg + " is Mandatory and has been omitted");
            }

            return false;
        }

        private CommandLineArgumentType DetermineArgumentType(String arg)
        {
            if (arg == null)
                throw new ArgumentNullException("Argument in function DetermineArgumentType has not to be null.");

            if(arg.Equals(Splitter))
            {
                return CommandLineArgumentType.Splitter;
            }
            else if (arg.StartsWith(LongOptionPrefix))
            {
                return CommandLineArgumentType.Long;
            }
            else if (arg.StartsWith(ShortOptionPrefix))
            {
                return CommandLineArgumentType.Short;
            }
            else
            {
                return CommandLineArgumentType.Common;
            }
        }

        private string TrimArgumentPrefix(string arg, ArgumentType type)
        {
            string TrimmedArg = null;

            switch (type)
            {
                case ArgumentType.Short:
                    TrimmedArg = arg.Remove(0, ShortOptionPrefix.Length);
                    break;
                case ArgumentType.Long:
                    TrimmedArg = arg.Remove(0, LongOptionPrefix.Length);
                    break;
            }

            return TrimmedArg;
        }

        private string TrimValueFromArgument(string arg)
        {
            int index = arg.IndexOf(ValueDelimiter);

            string argument = arg.Remove(index);

            return argument;

        }

        private void CopyRest(string[] source, List<string> destination, int start)
        {
            for (int index = start; index < source.Length; index++)
            {
                destination.Add(source[index]);
            }
        }

        private bool NextArgumentIsValue(string[] args, int index)
        {
            index++;

            if (index >= args.Length)
                return false;

            CommandLineArgumentType ArgType = DetermineArgumentType(args[index]);

            if (ArgType == CommandLineArgumentType.Common)
                return true;

            return false;
        }
        
        private bool ArgumentContainsValue(string arg)
        {
            int index = arg.IndexOf(ValueDelimiter);

            // the index has not to be the last position of the string, therefor the -1.
            if (index > 0 && index < arg.Length - 1)
            {
                return true;
            }

            return false;
        }

        private string GetValueFromArgument(string arg)
        {
            int index = arg.IndexOf(ValueDelimiter);

            // Wew have to remove the '=' character, therefor the +1.
            string value = arg.Remove(0, index + 1);

            return value;
        }

        private void LoadValue(Argument argument, string value)
        {
            if (argument.Type == typeof(string))
            {
                if (argument.Attribute.ListOfString == null)
                {
                    argument.Info.SetValue(ObjectToFill, value);
                }
                else if (argument.Attribute.ListOfString.Contains(value))
                {
                    argument.Info.SetValue(ObjectToFill, value);
                }
                else
                {
                    throw new ArgumentException("Value " + value + " is not in the list of supported values");
                }

            }
            else if (argument.Type == typeof(int))
            {
                int intValue = int.Parse(value);

                if (intValue >= argument.Attribute.LowBound && intValue <= argument.Attribute.HighBound)
                {
                    argument.Info.SetValue(ObjectToFill, intValue);
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            else if (argument.Type == typeof(bool))
            {
                bool boolValue = bool.Parse(value);
                argument.Info.SetValue(ObjectToFill, boolValue);
            }
            else
            {
                throw new ArgumentException("Argument of type " + argument.Type.ToString() + " is not supported");
            }
        }

    }
}
// TODO: sjednotit velka pismena na zacatku promennych. Nevim jak to ma byt :-(