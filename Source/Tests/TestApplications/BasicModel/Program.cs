using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleOmniXAML
{
    using OmniXaml;
    using OmniXaml.Services.DotNetFx;

    static class Program
    {
        private static void Main(string[] args)
        {
            var wiringContext = WiringContext.FromAttributes(Assemblies.AssembliesInAppFolder);
            var loader = new DefaultXamlLoader(wiringContext.TypeContext);

            var model = loader.FromPath("Model.xaml");
            Console.WriteLine("Loaded model:\n{0}", model);
            Console.ReadLine();
        }
    }
}
