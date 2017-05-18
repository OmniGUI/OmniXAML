using System;
using System.Text;
using System.Xml;
using ExtendedXmlSerializer.Configuration;
using OmniXaml.Services;

namespace XamlLoadTest
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Newtonsoft.Json;

    partial class Program
    {
        static void Main(string[] args)
        {
            var instance = LoadXaml(args.First());
            
            ShowResult(instance);
            Console.ReadLine();
        }

        private static void ShowResult(object instance)
        {
            Console.WriteLine($"AS JSON:\n{WriteAsJson(instance)}\n");
            Console.WriteLine($"AS XML:\n{WriteAsXml(instance)}");
        }

        private static string WriteAsXml(object instance)
        {
            var sb = new StringBuilder();
            var cc = new ConfigurationContainer();
            var xmlWriterSettings = new XmlWriterSettings
            {
                Indent = true,
            };

            using (var xmlWriter = XmlWriter.Create(new StringWriter(sb), xmlWriterSettings))
            {
                cc
                    .Create()
                    .Serialize(xmlWriter, instance);
            }

            return sb.ToString();            
        }

        private static string WriteAsJson(object instance)
        {
            return JsonConvert.SerializeObject(instance, Formatting.Indented);
        }

        private static object LoadXaml(string file)
        {
            var assemblies = new List<Assembly>() {Assembly.GetEntryAssembly(), typeof(OmniXaml.Tests.Model.Window).GetTypeInfo().Assembly};
            var loader = new ExtendedXamlLoader(assemblies);
            return loader.Load(File.ReadAllText(file));
        }
    }
}