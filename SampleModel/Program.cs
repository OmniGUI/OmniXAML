namespace SampleModel
{
    using System;
    using System.IO;
    using System.Reflection;
    using OmniXaml.Services;

    public class Program
    {
        public static void Main(string[] args)
        {
            var xaml = File.ReadAllText("Model.xml");
            var result = new XamlLoader(new[] {Assembly.GetEntryAssembly()}).Load(xaml);
            var rocky = result.NamescopeAnnotator.Find("Rocky", result.Instance);
            Console.WriteLine($"Rocky was found! Here it is: {rocky}");
        }
    }
}
