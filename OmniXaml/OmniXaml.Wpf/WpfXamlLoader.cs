namespace OmniXaml.Wpf
{
    using System;
    using System.IO;
    using Assembler;

    public class WpfLoader : ILoader
    {
        private readonly WiringContext wiringContext;

        public WpfLoader()
        {
            wiringContext = WpfWiringContextFactory.Create();
        }

        public object Load(Stream stream)
        {
            var objectAssembler = new WpfObjectAssembler(wiringContext);
            var xamlXmlLoader = new CoreXamlXmlLoader(objectAssembler, wiringContext);
            return xamlXmlLoader.Load(stream);
        }

        public object Load(Stream stream, object rootInstance)
        {
            var objectAssembler = new WpfObjectAssembler(wiringContext, new ObjectAssemblerSettings() { RootInstance = rootInstance });
            var xamlXmlLoader = new CoreXamlXmlLoader(objectAssembler, wiringContext);
            return xamlXmlLoader.Load(stream);
        }
    }
}