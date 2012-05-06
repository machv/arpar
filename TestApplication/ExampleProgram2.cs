using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arpar;

namespace TestApplication
{
    class ExampleProgram2
    {
        public static void example(String[] args)
        {
            ExampleSettings2 settings = new ExampleSettings2();
            List<string> commonArgs;
            ArgumentParser parser = new ArgumentParser(settings);

            try
            {
                parser.Parse(args);
                commonArgs = parser.CommonArguments;

                Console.WriteLine("version : {0}", settings.version);
                Console.WriteLine("port : {0}", settings.port);
                Console.WriteLine("optimization : {0}", settings.optimization);
                Console.WriteLine("dump : {0}", settings.dump);
                Console.WriteLine("dumpFile : {0}", settings.dumpFile);
                Console.WriteLine("mode : {0}", settings.mode);

                foreach (string s in commonArgs)
                {
                    Console.WriteLine("common argument : {0}", s);
                }
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
