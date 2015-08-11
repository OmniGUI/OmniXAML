namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Assembler;
    using Parsers.ProtoParser;
    using Parsers.XamlNodes;

    public class XamlStreamLoader : IXamlStreamLoader
    {
        private readonly Func<IObjectAssembler, IConfiguredXamlLoader> loaderFactory;
        private readonly IParser<Stream, IEnumerable<ProtoXamlInstruction>> xamlProtoInstructionXamlProtoInstructionParser;
        private readonly IXamlInstructionParser parser;
        private readonly IObjectAssemblerFactory assemblerFactory;

        public XamlStreamLoader(Func<IObjectAssembler, IConfiguredXamlLoader> loaderFactory, IObjectAssemblerFactory assemblerFactory)
        {
            this.loaderFactory = loaderFactory;
            this.assemblerFactory = assemblerFactory;
        }

        public XamlStreamLoader(IParser<Stream, IEnumerable<ProtoXamlInstruction>> xamlProtoInstructionXamlProtoInstructionParser, IXamlInstructionParser parser, IObjectAssemblerFactory assemblerFactory)
        {
            this.xamlProtoInstructionXamlProtoInstructionParser = xamlProtoInstructionXamlProtoInstructionParser;
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
            var coreXamlXmlLoader = loaderFactory == null ? new ConfiguredXamlXmlLoader(xamlProtoInstructionXamlProtoInstructionParser, parser, objectAssembler) : loaderFactory(objectAssembler);
            return coreXamlXmlLoader.Load(stream);
        }
    }
}