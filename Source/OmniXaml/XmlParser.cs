namespace OmniXaml
{
    using System.Collections.Generic;
    using Glass.Core;
    using Parsers.Parser;
    using Parsers.ProtoParser;

    public class XmlParser : IParser
    {
        private readonly IObjectAssembler objectAssembler;
        private readonly IProtoParser protoParser;
        private readonly IInstructionParser parser;

        public XmlParser(PhaseParserKit phaseParserKit)  
        {
            Guard.ThrowIfNull(phaseParserKit, nameof(phaseParserKit));

            objectAssembler = phaseParserKit.ObjectAssembler;        
            protoParser = phaseParserKit.ProtoParser;
            parser = phaseParserKit.Parser;
        }

        public object Parse(IXmlReader stream)
        {
            var xamlProtoNodes = protoParser.Parse(stream);
            var xamlNodes = parser.Parse(xamlProtoNodes);
            return Parse(xamlNodes);
        }

        private object Parse(IEnumerable<Instruction> xamlNodes)
        {
            foreach (var instruction in xamlNodes) { objectAssembler.Process(instruction); }

            return objectAssembler.Result;
        }
    }
}