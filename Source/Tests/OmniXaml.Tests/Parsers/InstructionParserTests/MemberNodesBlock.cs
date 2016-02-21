namespace OmniXaml.Tests.Parsers.InstructionParserTests
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Glass;
    using Typing;

    public class MemberNodesBlock : IDependency<MemberNodesBlock>
    {
        private readonly Queue<Instruction> nodes = new Queue<Instruction>();
        private readonly MutableMember member;

        public MemberNodesBlock(Instruction headingInstruction)
        {
            member = (MutableMember)headingInstruction.Member;
        }

        public void AddNode(Instruction instruction)
        {
            nodes.Enqueue(instruction);
        }

        public IEnumerable<MemberNodesBlock> Dependencies { get; set; }

        public Queue<Instruction> Nodes => nodes;

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