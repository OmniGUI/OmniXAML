namespace OmniXaml
{
    using System.Collections.Generic;
    using System.IO;
    using Glass;
    using Parsers.ProtoParser;
    using Parsers.XamlNodes;

    public class ConfiguredXamlXmlLoader : IConfiguredXamlLoader
    {
        private readonly IObjectAssembler objectAssembler;
        private readonly IParser<Stream, IEnumerable<ProtoXamlInstruction>> protoParser;
        private readonly IXamlInstructionParser parser;

        public ConfiguredXamlXmlLoader(IParser<Stream, IEnumerable<ProtoXamlInstruction>> protoParser, IXamlInstructionParser parser, IObjectAssembler objectAssembler)  
        {
            Guard.ThrowIfNull(objectAssembler, nameof(objectAssembler));

            this.objectAssembler = objectAssembler;        
            this.protoParser = protoParser;
            this.parser = parser;
        }

        public object Load(Stream stream)
        {
            var xamlProtoNodes = protoParser.Parse(stream);
            var xamlNodes = parser.Parse(xamlProtoNodes);
            return Load(xamlNodes);
        }

        private object Load(IEnumerable<XamlInstruction> xamlNodes)
        {
            foreach (var xamlNode in xamlNodes)
            {
                objectAssembler.Process(xamlNode);
            }

            return objectAssembler.Result;
        }
    }
}