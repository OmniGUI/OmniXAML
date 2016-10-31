namespace SampleModel
{
    using System.IO;
    using System.Reflection;
    using OmniXaml.Attributes;

    public class Program
    {
        public static void Main(string[] args)
        {
            var xaml = File.ReadAllText("Model.xml");
            var result = XamlLoader.FromAttributes(Assembly.GetEntryAssembly()).Load(xaml);
            var rocky = result.NamescopeAnnotator.Find("Rocky", result.Instance);            
        }
    }
}
