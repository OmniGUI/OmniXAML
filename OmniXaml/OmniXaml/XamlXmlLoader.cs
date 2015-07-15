namespace OmniXaml
{
    using System.IO;
    using Assembler;
    using NewAssembler;

    public class XamlXmlLoader : IXamlLoader
    {
        private readonly WiringContext wiringContext;

        public XamlXmlLoader(WiringContext wiringContext)
        {
            this.wiringContext = wiringContext;
        }

        public object Load(Stream stream)
        {
            var objectAsssembler = new SuperObjectAssembler(wiringContext);
            var coreXamlXmlLoader = new CoreXamlXmlLoader(objectAsssembler, wiringContext);
            return coreXamlXmlLoader.Load(stream);
        }

        public object Load(Stream stream, object rootInstance)
        {
            var objectAsssembler = new SuperObjectAssembler(wiringContext, new ObjectAssemblerSettings { RootInstance = rootInstance });
            var coreXamlXmlLoader = new CoreXamlXmlLoader(objectAsssembler, wiringContext);
            return coreXamlXmlLoader.Load(stream);
        }
    }
}