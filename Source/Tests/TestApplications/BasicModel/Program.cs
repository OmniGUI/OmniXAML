namespace SampleOmniXAML
{
    using System;
    using Model;
    using OmniXaml;
    using OmniXaml.Services.DotNetFx;

    static class Program
    {
        private static void Main()
        {
            var runtimeTypeSource = RuntimeTypeSource.FromAttributes(Assemblies.AssembliesInAppFolder);
            var loader = new DefaultLoader(runtimeTypeSource);

            var model = (Zoo) loader.FromPath("Model.xaml");
            var byName = model.Find("Rocky");

            Console.WriteLine("Loaded model:\n{0}", model);
            Console.WriteLine($"Searching an animal by name in this namescope (Zoo instance): \n\tRocky => {byName}");
            Console.ReadLine();
        }
    }
}
