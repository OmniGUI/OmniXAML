namespace OmniXaml
{
    using System;
    using System.IO;
    using Assembler;
    using Parsers.ProtoParser.SuperProtoParser;
    using Parsers.XamlNodes;

    public class XamlStreamLoader : IXamlStreamLoader
    {
        private readonly Func<IObjectAssembler, IConfiguredXamlLoader> loaderFactory;
        private readonly SuperProtoParser protoProtoParser;
        private readonly IXamlNodesPullParser pullParser;
        private readonly IObjectAssemblerFactory assemblerFactory;

        public XamlStreamLoader(Func<IObjectAssembler, IConfiguredXamlLoader> loaderFactory, IObjectAssemblerFactory assemblerFactory)
        {
            this.loaderFactory = loaderFactory;
            this.assemblerFactory = assemblerFactory;
        }

        public XamlStreamLoader(SuperProtoParser protoProtoParser, IXamlNodesPullParser pullParser, IObjectAssemblerFactory assemblerFactory)
        {
            this.protoProtoParser = protoProtoParser;
            this.pullParser = pullParser;
            this.assemblerFactory = assemblerFactory;
        }

        public object Load(Stream stream)
        {
            return LoadInternal(stream, assemblerFactory.CreateAssembler(new ObjectAssemblerSettings()));
        }

        public object Load(Stream stream, object rootInstance)
        {
            var settings = new ObjectAssemblerSettings {RootInstance = rootInstance};
            return LoadInternal(stream, assemblerFactory.CreateAssembler(settings));
        }

        private object LoadInternal(Stream stream, IObjectAssembler objectAssembler)
        {
            var coreXamlXmlLoader = loaderFactory == null ? new ConfiguredXamlXmlLoader(protoProtoParser, pullParser, objectAssembler) : loaderFactory(objectAssembler);
            return coreXamlXmlLoader.Load(stream);
        }
    }
}