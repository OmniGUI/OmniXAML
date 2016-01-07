namespace SampleOmniXAML
{
    using System;
    using OmniXaml;
    using OmniXaml.Services.DotNetFx;

    static class Program
    {
        private static void Main()
        {
            var runtimeTypeContext = RuntimeTypeSource.FromAttributes(Assemblies.AssembliesInAppFolder);
            var loader = new DefaultXamlLoader(runtimeTypeContext);

            var model = loader.FromPath("Model.xaml");
            Console.WriteLine("Loaded model:\n{0}", model);
            Console.ReadLine();
        }
    }
}
