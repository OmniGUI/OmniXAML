namespace OmniXaml
{
    using Parsers.ProtoParser;
    using Parsers.XamlInstructions;

    public class PhaseParserKit
    {
        private readonly IProtoParser protoParser;
        private readonly IXamlInstructionParser parser;
        private readonly IObjectAssembler objectAssembler;

        public PhaseParserKit(IProtoParser protoParser, IXamlInstructionParser parser, IObjectAssembler objectAssembler)
        {
            this.protoParser = protoParser;
            this.parser = parser;
            this.objectAssembler = objectAssembler;
        }

        public IProtoParser ProtoParser => protoParser;

        public IXamlInstructionParser Parser => parser;

        public IObjectAssembler ObjectAssembler => objectAssembler;
    }
}