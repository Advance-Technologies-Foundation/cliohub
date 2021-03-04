using OpenCommand;
using System;

namespace ConsoleArgsTransition
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var module = new CommandLineModuleProxy();
                module.Execute();
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
