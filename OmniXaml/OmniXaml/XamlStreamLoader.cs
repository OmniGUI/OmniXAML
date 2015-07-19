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
        private readonly XamlNodesPullParser pullParser;
        private readonly IObjectAssemblerFactory assemblerFactory;

        public XamlStreamLoader(Func<IObjectAssembler, IConfiguredXamlLoader> loaderFactory, IObjectAssemblerFactory assemblerFactory)
        {
            this.loaderFactory = loaderFactory;
            this.assemblerFactory = assemblerFactory;
        }

        public XamlStreamLoader(SuperProtoParser protoProtoParser, XamlNodesPullParser pullParser, IObjectAssemblerFactory assemblerFactory)
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
            var coreXamlXmlLoader = loaderFactory == null ? new ConfiguredXamlXmlLoader(protoProtoParser, pullParser, objectAssembler) : loaderFactory(objectAssembler);
            return coreXamlXmlLoader.Load(stream);
        }
    }

    public class DefaultXamlStreamLoader: XamlStreamLoader
    {
        public DefaultXamlStreamLoader(WiringContext wiringContext) : base(LoaderFactory(wiringContext), new DefaultObjectAssemblerFactory(wiringContext))
        {
        }

        private static Func<IObjectAssembler, IConfiguredXamlLoader> LoaderFactory(WiringContext wiringContext)
        {
            return assembler => new ConfiguredXamlXmlLoader(new SuperProtoParser(wiringContext), new XamlNodesPullParser(wiringContext), assembler);
        }
    }
}