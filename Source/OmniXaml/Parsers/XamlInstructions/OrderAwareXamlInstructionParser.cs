namespace OmniXaml.Parsers.XamlInstructions
{
    using System.Collections.Generic;

    public class OrderAwareXamlInstructionParser : IXamlInstructionParser
    {
        private readonly IXamlInstructionParser parser;

        public OrderAwareXamlInstructionParser(IXamlInstructionParser parser)
        {
            this.parser = parser;
        }

        public IEnumerable<XamlInstruction> Parse(IEnumerable<ProtoXamlInstruction> protoNodes)
        {
            var nodeSorter = new MemberDependencyNodeSorter();
            var originalNodes = parser.Parse(protoNodes);
            var enumerator = originalNodes.GetEnumerator();
            return nodeSorter.Sort(enumerator);
        }
    }
}