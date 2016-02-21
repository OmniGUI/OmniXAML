namespace OmniXaml
{
    using Parsers.Parser;
    using Parsers.ProtoParser;

    public class PhaseParserKit
    {
        private readonly IProtoParser protoParser;
        private readonly IInstructionParser parser;
        private readonly IObjectAssembler objectAssembler;

        public PhaseParserKit(IProtoParser protoParser, IInstructionParser parser, IObjectAssembler objectAssembler)
        {
            this.protoParser = protoParser;
            this.parser = parser;
            this.objectAssembler = objectAssembler;
        }

        public IProtoParser ProtoParser => protoParser;

        public IInstructionParser Parser => parser;

        public IObjectAssembler ObjectAssembler => objectAssembler;
    }
}