using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arpar;


namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Options o = new Options();



            //o.Parse(args);

            Option<string> opt = new Option<string> { Type = Arpar.TypParametru.Long, IsMandatory = true };
            //opt.setConstraints(...)
        }
    }
}
