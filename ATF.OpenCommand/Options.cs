using System;

namespace ATF.OpenCommand
{
    [AttributeUsage(AttributeTargets.Property)]
    public class Options: Attribute
    {

    }


    [AttributeUsage(AttributeTargets.Class)]
    public class Command : Attribute
    { 
        public string Name { get; set; }

        public string Help { get; set; }

        public string Example { get; set; }
    }

    public interface IExecutor {
        void Execute();

    }

}
