namespace OmniXaml
{
    using System.IO;
    using Assembler;

    public class XamlLoader : ILoader
    {
        private readonly WiringContext wiringContext;
        private readonly IObjectAssemblerFactory assemblerFactory;

        public XamlLoader(WiringContext wiringContext, IObjectAssemblerFactory assemblerFactory)
        {
            this.wiringContext = wiringContext;
            this.assemblerFactory = assemblerFactory;
        }
        
        public object Load(Stream stream)
        {
            var objectAsssembler = assemblerFactory.GetAssembler(null);
            var coreXamlXmlLoader = new CoreXamlXmlLoader(objectAsssembler, wiringContext);
            return coreXamlXmlLoader.Load(stream);
        }

        public object Load(Stream stream, object rootInstance)
        {
            var objectAsssembler = assemblerFactory.GetAssembler(new ObjectAssemblerSettings { RootInstance = rootInstance });
            var coreXamlXmlLoader = new CoreXamlXmlLoader(objectAsssembler, wiringContext);
            return coreXamlXmlLoader.Load(stream);
        }
    }
}