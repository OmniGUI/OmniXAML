namespace OmniXaml
{
    using System.IO;
    using Assembler;

    public class XamlStreamLoader : IXamlStreamLoader
    {
        private readonly WiringContext wiringContext;
        private readonly IObjectAssemblerFactory assemblerFactory;

        public XamlStreamLoader(WiringContext wiringContext, IObjectAssemblerFactory assemblerFactory)
        {
            this.wiringContext = wiringContext;
            this.assemblerFactory = assemblerFactory;
        }

        public object Load(Stream stream)
        {
            return LoadInternal(stream, assemblerFactory.GetAssembler(null));
        }

        public object Load(Stream stream, object rootInstance)
        {
            return LoadInternal(stream, assemblerFactory.GetAssembler(new ObjectAssemblerSettings {RootInstance = rootInstance}));
        }

        private object LoadInternal(Stream stream, IObjectAssembler objectAsssembler)
        {
            var coreXamlXmlLoader = new CoreXamlXmlLoader(objectAsssembler, wiringContext);
            return coreXamlXmlLoader.Load(stream);
        }
    }
}