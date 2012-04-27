using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arpar
{
    // TODO jak se jmenuje
    public class Options : IIterable
    {
        public List<Option> Items { get; set; }

        public void GenerateDocumentation() { }

        public void Parse() { }

        public Options()
        {
        }

        /// <summary>
        /// TODO: mozna tohle neni vubec potreba
        /// </summary>
        /// <param name="arguments"></param>
        public Options(string arguments)
        {
        }

        public Options(string[] arguments)
        {
        }
    }
}
