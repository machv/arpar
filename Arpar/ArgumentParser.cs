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

        public List<String> CommonArguments { get; protected set; }



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

                        // Check duplicity of argument across all registered arguments
                        if (ArgumentsByName.ContainsKey(attr.Name))
                        {
                            throw new DuplicateArgumentException(string.Format("Argument name {0} is already registered.", attr.Name));
                        }

                        // Also duplicate name of this argument into Names List for uniform manipulation.
                        ArgumentAliasAttribute aliasAttribute = new ArgumentAliasAttribute(attr.Name, attr.Type);
                        string prefixedName = GetPrefixedArgumentName(aliasAttribute);

                        // But before inserting check duplicity within current attribute (because names of current attribute are
                        //  registered in global ArgumentsByName after all names of currently processed attribute are known)
                        if (arg.Names.ContainsKey(prefixedName))
                        {
                            throw new DuplicateArgumentException(string.Format("This argument has already registered name {0}.", attr.Name));
                        }

                        arg.Names.Add(prefixedName, aliasAttribute);
                    }

                    if (attribute is ArgumentAliasAttribute)
                    {
                        ArgumentAliasAttribute alias = attribute as ArgumentAliasAttribute;

                        // Check duplicity of argument across all registered arguments
                        if (ArgumentsByName.ContainsKey(alias.Name))
                        {
                            throw new DuplicateArgumentException(string.Format("Argument name {0} is already registered.", alias.Name));
                        }

                        arg.Names.Add(GetPrefixedArgumentName(alias.Name, alias.Type), alias);
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

        //TODO: udělat to přes streamy
        public void GenerateDocumentation()
        {
            foreach (Argument argument in arguments)
            {
                string prefix = GetArgumentPrefix(argument.Attribute.Type);
                Console.WriteLine("  " + prefix + argument.Attribute.Name + ":\t" + argument.Attribute.Description);

                if (argument.Attribute is ChoicesArgumentAttribute)
                {
                    ChoicesArgumentAttribute choicesAttribute = argument.Attribute as ChoicesArgumentAttribute;

                    if (choicesAttribute.Choices != null)
                    {
                        Console.WriteLine("\tList of values for this argument:");
                        foreach (string value in choicesAttribute.Choices)
                        {
                            Console.WriteLine("\t\t" + value); // Muze byt udelano i na jedne radce oddelene carkami (stredniky)
                        }
                    }
                }
            }

        }

        protected string GetPrefixedArgumentName(ArgumentAliasAttribute aliasAttribute)
        {
            return GetPrefixedArgumentName(aliasAttribute.Name, aliasAttribute.Type);
        }

        /// <summary>
        /// Generates fully prefixed name of argument.
        /// </summary>
        /// <param name="name">Plain argument name</param>
        /// <param name="type">Type of argument</param>
        /// <returns></returns>
        protected string GetPrefixedArgumentName(string name, ArgumentType type)
        {
            return GetArgumentPrefix(type) + name;
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
                CommandLineArgumentType argumentType = DetermineArgumentType(CurrentArg);

                switch (argumentType)
                {
                    case CommandLineArgumentType.Defined:
                        bool nextArgumentProcessed = TryLoadValueMoveIndex(CurrentArg, ConsoleArgs, index);

                        if (nextArgumentProcessed)
                            index++;
                        break;
                    case CommandLineArgumentType.Common:
                        CommonArguments.Add(CurrentArg);
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

        private bool AllMandatoryArgumentsSatisfied()
        {
            foreach (Argument arg in arguments)
            {
                if (arg.Attribute.IsMandatory && !arg.IsSatisfied)
                    return false;
            }

            return true;
        }

        private bool TryLoadValueMoveIndex(string arg, string[] ConsoleArgs, int index) // TODO: pokud bude nutno rozlisovat - a -- jako prefix, musi se sem pridat parametr s typem nacteneho argumentu
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

            if(argument.IsSatisfied)
            {
                throw new ArgumentException("Argument " + arg + " has been specified several times");
            }

            bool isValue = NextArgumentIsValue(ConsoleArgs, index, argument.Type) || value != null;
            ParameterRequirements valueRequirements = argument.Attribute.ValueRequirements;

            if (valueRequirements == ParameterRequirements.Denied)
            {
                if(value != null)
                {
                    throw new ArgumentException("Argument " + arg + " has denied value specification");
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
                throw new ArgumentException("Value for argument " + arg + " is Mandatory and has been omitted");
            }

            return false;
        }

        private CommandLineArgumentType DetermineArgumentType(String arg)
        {
            if (arg == null)
                throw new ArgumentNullException("Argument in function DetermineArgumentType has not to be null.");

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

        private bool NextArgumentIsValue(string[] args, int index, Type type)
        {
            index++;

            if (index >= args.Length)
                return false;

            string arg = args[index];

            if (type == typeof(int))
            {
                int outValue;
                return int.TryParse(arg, out outValue);
            }
            /*else if (type == typeof(string)) // TODO: rozhodnout se, co je spravne, jestli jako hodnotu pro string brat cokoli, nebo jen to co nezacina pomlckami
            {
                return true;
            }*/


            CommandLineArgumentType ArgType = DetermineArgumentType(arg);

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
                if (argument.Attribute is ChoicesArgumentAttribute) //Fixed choices argument
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

        private static bool IsInAttributeBoundary(BoundedArgumentAttribute boundedAttribute, int intValue)
        {
            return intValue >= boundedAttribute.LowBound && intValue <= boundedAttribute.HighBound;
        }

    }
}
// TODO: sjednotit velka pismena na zacatku promennych. Nevim jak to ma byt :-(