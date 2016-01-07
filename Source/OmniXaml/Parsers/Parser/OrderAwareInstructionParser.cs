namespace OmniXaml.Parsers.Parser
{
    using System.Collections.Generic;

    public class OrderAwareInstructionParser : IInstructionParser
    {
        private readonly IInstructionParser parser;

        public OrderAwareInstructionParser(IInstructionParser parser)
        {
            this.parser = parser;
        }

        public IEnumerable<Instruction> Parse(IEnumerable<ProtoInstruction> protoNodes)
        {
            var nodeSorter = new MemberDependencyNodeSorter();
            var originalNodes = parser.Parse(protoNodes);
            var enumerator = originalNodes.GetEnumerator();
            return nodeSorter.Sort(enumerator);
        }
    }
}