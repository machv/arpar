using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arpar
{
    public enum TypParametru { Short, Long }

    /// <summary>
    /// Typy parametrů
    /// </summary>
    public enum ParameterRequirements { Mandatory, Optional, Denied }

    /// <summary>
    /// Třída pro jeden parametr
    /// TODO: lépe pojmenovat T -> podívat se do specifikace ms
    /// </summary>
    public class Option
    {
        //public string Name { get; set; }
        public TypParametru Type { get; set; }
        public string Description { get; set; }
        public bool IsMandatory { get; set; }
        public ParameterRequirements Parameter { get; set; }
        public object Value { get; internal set; }
        public List<string> Names { get; set; }
        public Option(string name)
        {
            Names = new List<string>();
            Names.Add(name);

            //throw new ArgumentException
        }

        public void AddAlias(params string[] names)
        {
            Names.Add(names);
        }

        //TODO
        public override bool Equals(object obj)
        {
            //if(obj typeof Option)
            //{
            //}
            return base.Equals(obj);
        }
    }
}
