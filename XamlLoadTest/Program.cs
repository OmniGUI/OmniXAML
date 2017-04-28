using System;

namespace XamlLoadTest
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Newtonsoft.Json;
    using OmniXaml.Services;

    class Program
    {
        static void Main(string[] args)
        {
            var instance = LoadXaml(args.First());
            
            ShowResult(instance);
        }

        private static void ShowResult(object instance)
        {
            var json = JsonConvert.SerializeObject(instance, Formatting.Indented);
            Console.WriteLine(json);
        }

        private static object LoadXaml(string file)
        {
            var assemblies = new List<Assembly>() {Assembly.GetEntryAssembly(), typeof(OmniXaml.Tests.Model.Window).GetTypeInfo().Assembly};
            var loader = new XamlLoader(assemblies);
            return loader.Load(File.ReadAllText(file));
        }
    }
}