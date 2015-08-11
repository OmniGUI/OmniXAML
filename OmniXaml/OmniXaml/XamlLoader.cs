namespace OmniXaml
{
    using System;
    using System.IO;
    using Assembler;
    using Parsers.ProtoParser;
    using Parsers.XamlNodes;

    public class XamlLoader : IXamlLoader
    {
        private readonly Func<IObjectAssembler, IXamlParser> loaderFactory;
        private readonly IProtoParser protoInstructionParser;
        private readonly IXamlInstructionParser parser;
        private readonly IObjectAssemblerFactory assemblerFactory;

        public XamlLoader(Func<IObjectAssembler, IXamlParser> loaderFactory, IObjectAssemblerFactory assemblerFactory)
        {
            this.loaderFactory = loaderFactory;
            this.assemblerFactory = assemblerFactory;
        }

        public XamlLoader(IProtoParser protoInstructionParser, IXamlInstructionParser parser, IObjectAssemblerFactory assemblerFactory)
        {
            this.protoInstructionParser = protoInstructionParser;
            this.parser = parser;
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
            var coreXamlXmlLoader = loaderFactory == null
                ? new XamlXmlParser(protoInstructionParser, parser, objectAssembler)
                : loaderFactory(objectAssembler);

            return coreXamlXmlLoader.Parse(stream);
        }
    }
}