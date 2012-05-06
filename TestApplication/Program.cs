using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arpar;


namespace TestApplication
{
    class Program
    {
        static void Main(string[] args) // TODO: zapracovat na hlaskach v error messages
        {
            Console.WriteLine("Settings\n");

            Settings sett = new Settings();
            ArgumentParser arpar = new ArgumentParser(sett);

            Console.WriteLine(sett.Greetings);
            Console.WriteLine(sett.BoundedString);

            arpar.GenerateDocumentation();
            arpar.Parse("--text nazdar -bs=GAMA");

            Console.WriteLine(sett.Greetings);
            Console.WriteLine(sett.BoundedString);

            
            Console.WriteLine("\n\nBadSettings1\n");

            BadSettings1 bs1 = new BadSettings1();
            ArgumentParser bs1ap = new ArgumentParser(bs1);

            try
            {
                bs1ap.Parse("-val -1");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                bs1ap.Parse("-val=6");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                bs1ap.Parse("-val a");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("\n\nBadSettings2\n");

            BadSettings2 bs2 = new BadSettings2();
            ArgumentParser bs2ap = new ArgumentParser(bs2);

            try
            {
                bs2ap.Parse("-val=DELTA");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("\n\nGoodSettings1\n");

            GoodSettings1 gs1 = new GoodSettings1();
            ArgumentParser gs1ap = new ArgumentParser(gs1);

            gs1ap.Parse("-a b c d -- -e -f g"); // U parametru, ktery ma denied hodnotu, vynutit typ bool

            Console.WriteLine("a: " + gs1.a);
            Console.WriteLine("b: " + gs1.b);
            Console.WriteLine("Common arguments:");

            foreach (string s in gs1ap.CommonArguments)
            {
                Console.WriteLine(s);
            }


            Console.ReadLine();
        }
    }
}
