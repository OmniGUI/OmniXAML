namespace OmniXaml
{
    using System.IO;
    using Assembler;
    using Parsers.ProtoParser.SuperProtoParser;
    using Parsers.XamlNodes;

    public class BoostrappableXamlStreamLoader : IXamlStreamLoader
    {
        private readonly WiringContext wiringContext;
        private readonly SuperProtoParser protoProtoParser;
        private readonly XamlNodesPullParser pullParser;
        private readonly IObjectAssemblerFactory assemblerFactory;

        public BoostrappableXamlStreamLoader(WiringContext wiringContext, SuperProtoParser protoProtoParser, XamlNodesPullParser pullParser, IObjectAssemblerFactory assemblerFactory)
        {
            this.wiringContext = wiringContext;
            this.protoProtoParser = protoProtoParser;
            this.pullParser = pullParser;
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

        private object LoadInternal(Stream stream, IObjectAssembler objectAssembler)
        {
            var coreXamlXmlLoader = new CoreXamlXmlLoader(protoProtoParser, pullParser, objectAssembler);
            return coreXamlXmlLoader.Load(stream);
        }
    }
}