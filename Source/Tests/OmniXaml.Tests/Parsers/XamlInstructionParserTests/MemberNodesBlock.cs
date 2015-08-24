namespace OmniXaml.Tests.Parsers.XamlInstructionParserTests
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Glass;
    using Typing;

    public class MemberNodesBlock : IDependency<MemberNodesBlock>
    {
        private readonly Queue<XamlInstruction> nodes = new Queue<XamlInstruction>();
        private readonly MutableXamlMember member;

        public MemberNodesBlock(XamlInstruction headingInstruction)
        {
            member = (MutableXamlMember)headingInstruction.Member;
        }

        public void AddNode(XamlInstruction instruction)
        {
            nodes.Enqueue(instruction);
        }

        public IEnumerable<MemberNodesBlock> Dependencies { get; set; }

        public Queue<XamlInstruction> Nodes => nodes;

        public void Link(Collection<MemberNodesBlock> queues)
        {
            var deps = new Collection<MemberNodesBlock>();

            var myDeps = member.Dependencies;
            foreach (var xamlMember in myDeps)
            {
                var located = queues.First(block => block.member.Equals(xamlMember));
                deps.Add(located);
            }

            Dependencies = deps;
        }
    }
}