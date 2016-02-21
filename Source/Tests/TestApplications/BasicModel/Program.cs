namespace BasicModel
{
    using System;
    using Model;
    using OmniXaml;
    using OmniXaml.ObjectAssembler;
    using OmniXaml.Services.DotNetFx;

    internal static class Program
    {
        private static void Main()
        {
            var runtimeTypeSource = RuntimeTypeSource.FromAttributes(Assemblies.AssembliesInAppFolder);
            var loader = new DefaultLoader(runtimeTypeSource);

            var model = (Zoo)loader.FromPath("Model.xaml", new Settings { InstanceLifeCycleListener = new MyListener() });
            var byName = model.Find("Rocky");

            Console.WriteLine("Loaded model:\n{0}", model);
            Console.WriteLine($"Searching an animal by name in this namescope (Zoo instance): \n\tRocky => {byName}");
            Console.ReadLine();
        }
    }

    internal class MyListener : IInstanceLifeCycleListener
    {
        public void OnBegin(object instance)
        {
        }

        public void OnAfterProperties(object instance)
        {
        }

        public void OnAssociatedToParent(object instance)
        {
        }

        public void OnEnd(object instance)
        {
        }
    }
}
