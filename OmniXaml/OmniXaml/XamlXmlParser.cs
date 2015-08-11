namespace OmniXaml
{
    using System.Collections.Generic;
    using System.IO;
    using Glass;
    using Parsers.ProtoParser;
    using Parsers.XamlNodes;

    public class XamlXmlParser : IXamlParser
    {
        private readonly IObjectAssembler objectAssembler;
        private readonly IProtoParser protoParser;
        private readonly IXamlInstructionParser parser;

        public XamlXmlParser(IProtoParser protoParser, IXamlInstructionParser parser, IObjectAssembler objectAssembler)  
        {
            Guard.ThrowIfNull(objectAssembler, nameof(objectAssembler));

            this.objectAssembler = objectAssembler;        
            this.protoParser = protoParser;
            this.parser = parser;
        }

        public object Parse(Stream stream)
        {
            var xamlProtoNodes = protoParser.Parse(stream);
            var xamlNodes = parser.Parse(xamlProtoNodes);
            return Parse(xamlNodes);
        }

        private object Parse(IEnumerable<XamlInstruction> xamlNodes)
        {
            foreach (var xamlNode in xamlNodes)
            {
                objectAssembler.Process(xamlNode);
            }

            return objectAssembler.Result;
        }
    }
}