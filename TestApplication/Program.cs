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
            Settings sett = new Settings();
            ArgumentParser arpar = new ArgumentParser(sett);

            arpar.GenerateDocumentation();

            arpar.Parse(args);
        }
    }
}
