using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arpar;

namespace TestApplication
{
    class ExampleProgram1
    {
        internal static void examples(string[] args)
        {
            ExampleSettings1 settings = new ExampleSettings1();
            ArgumentParser parser = new ArgumentParser(settings);

            try
            {
                parser.Parse(args);

                Console.WriteLine("Text to print : {0}", settings.TextToPrint);
                Console.WriteLine("Second text: {0}", settings.SecondText);
                Console.WriteLine("Gender : {0}", settings.Gender);
                Console.WriteLine("Gender2 : {0}", settings.Gender2);
                Console.WriteLine("Maximum length: {0}", settings.MaximumLenght);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error occured during parsing arguments:");
                Console.WriteLine("\t" + e.Message);
                Console.WriteLine();
                parser.GenerateDocumentation();
            }
        }
    }
}
