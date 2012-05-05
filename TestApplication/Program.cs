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

            Console.WriteLine(sett.Greetings);

            arpar.GenerateDocumentation();

            //arpar.Parse(args);
            arpar.Parse("--text nazdar");

            Console.WriteLine(sett.Greetings);

            Console.ReadLine();
        }
    }
}
