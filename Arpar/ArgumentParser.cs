﻿using System;
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

                        // Also duplicate name of this argument into Names List for uniform manipulation.
                        arg.Names.Add(new ArgumentAliasAttribute(attr.Name, attr.Type));
                    }

                    if (attribute is ArgumentAliasAttribute)
                    {
                        ArgumentAliasAttribute alias = attribute as ArgumentAliasAttribute;

                        //todo: kontrolovat, jestli uz tenhle alias neni pridan (jak pro tenhle argument, tak pro jiny)
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
                Console.WriteLine("  " + prefix + argument.Attribute.Name + ": " + argument.Attribute.Description);
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
        public void Parse(string[] args)
        {
            ConsoleArgs = args;

            if (ConsoleArgs == null)
            {
                throw new InvalidOperationException("Missing parameters to parse.");
            }

            //TODO: do parse

            int i = 0;
            foreach (string arg in ConsoleArgs)
            {
                foreach (Argument argument in arguments)
                {
                    if ((GetArgumentPrefix(argument.Attribute.Type) + argument.Attribute.Name) == arg)
                    {
                        if (argument.Type == typeof(string))
                        {
                            argument.Info.SetValue(ObjectToFill, ConsoleArgs[i + 1]);
                        }
                    }
                }
                i++;
            }
        }

    }
}
