namespace OmniXaml
{
    using System;
    using System.IO;
    using Assembler;
    using Parsers.ProtoParser.SuperProtoParser;
    using Parsers.XamlNodes;

    public class BootstrappableXamlStreamLoader : IXamlStreamLoader
    {
        private readonly Func<IObjectAssembler, ICoreXamlLoader> loaderFactory;
        private readonly SuperProtoParser protoProtoParser;
        private readonly XamlNodesPullParser pullParser;
        private readonly IObjectAssemblerFactory assemblerFactory;

        public BootstrappableXamlStreamLoader(Func<IObjectAssembler, ICoreXamlLoader> loaderFactory, IObjectAssemblerFactory assemblerFactory)
        {
            this.loaderFactory = loaderFactory;
            this.assemblerFactory = assemblerFactory;
        }

        public BootstrappableXamlStreamLoader(SuperProtoParser protoProtoParser, XamlNodesPullParser pullParser, IObjectAssemblerFactory assemblerFactory)
        {
            this.protoProtoParser = protoProtoParser;
            this.pullParser = pullParser;
            this.assemblerFactory = assemblerFactory;
        }

        public object Load(Stream stream)
        {
            return LoadInternal(stream, assemblerFactory.CreateAssembler());
        }

        public object Load(Stream stream, object rootInstance)
        {
            var settings = new ObjectAssemblerSettings {RootInstance = rootInstance};
            return LoadInternal(stream, assemblerFactory.CreateAssembler(settings));
        }

        private object LoadInternal(Stream stream, IObjectAssembler objectAssembler)
        {
            var coreXamlXmlLoader = loaderFactory == null ? new CoreXamlXmlLoader(protoProtoParser, pullParser, objectAssembler) : loaderFactory(objectAssembler);
            return coreXamlXmlLoader.Load(stream);
        }
    }
}