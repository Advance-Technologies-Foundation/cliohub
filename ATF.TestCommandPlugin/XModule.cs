using ATF.OpenCommand;
using System;

namespace ATF.TestCommand
{

    [Command(Name = "test", Help = "Test recognized string, int and bool parameters")]
    public class XModule : IExecutor
    {

        [Options]
        public string StringValue { get; set; }

        [Options]
        public int IntValue { get; set; }

        [Options]
        public bool BooleanValue { get; set; }

        public void Execute()
        {
            Console.WriteLine($"Command parameters:");
            Console.WriteLine($"string: {StringValue}");
            Console.WriteLine($"int: {IntValue}");
            Console.WriteLine($"bool: {BooleanValue}");
        }
    }

}