using ATF.OpenCommand;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace OpenCommand
{
    public class CommandLineModuleProxy : IExecutor
    {
        string operationVerb = "operation";

        string operation;

        IExecutor executor;

        private Dictionary<string, string> _args = new Dictionary<string, string>();

        public CommandLineModuleProxy()
        {
            ParseArgumentsValue();
            InitExecutor();
            SetPropertiesValue();
        }

        private void InitExecutor()
        {


            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            var pluginAssembliesPathes = Directory.GetFiles(appDir, "*.dll", SearchOption.AllDirectories);
            foreach (var assemblyPath in pluginAssembliesPathes)
            {
                FindCommandFromAssembly(Assembly.LoadFrom(assemblyPath));
            }
            if (executor is null)
            {
                throw new Exception($"Command <{operation.ToUpper()}> was not recognized");
            }
        }

        private void FindCommandFromAssembly(Assembly assembly)
        {
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                var commands = type.GetCustomAttributes<Command>();
                foreach (var command in commands)
                {
                    if (command.Name == operation)
                    {
                        executor = (IExecutor)Activator.CreateInstance(type);
                    }
                }
            }
        }

        public void Execute()
        {
            executor.Execute();
        }

        private void ParseArgumentsValue()
        {
            var args = Environment.GetCommandLineArgs();
            foreach (var arg in args)
            {
                var options = arg.Split('=');
                if (options.Length == 2)
                {
                    if (options[0] == operationVerb)
                    {
                        operation = options[1];
                    }
                    else
                    {
                        var key = options[0].Trim('-').ToLowerInvariant();
                        var value = options[1];
                        Console.WriteLine($"Recognized property <{key}> with value <{value}>");
                        _args[key] = value;
                    }
                }
            }
        }

        public static object TryParse(string inValue, Type type)
        {
            TypeConverter converter =
                TypeDescriptor.GetConverter(type);
            var value = converter.ConvertFromString(null,
                CultureInfo.InvariantCulture, inValue);
            return value;
        }

        private void SetPropertiesValue()
        {
            PropertyInfo[] props = executor.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object[] attrs = prop.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    if (attr is Options)
                    {
                        var stringValue = _args[prop.Name.ToLowerInvariant()];
                        prop.SetValue(executor, TryParse(stringValue, prop.PropertyType));
                    }
                }
            }
        }
    }
}