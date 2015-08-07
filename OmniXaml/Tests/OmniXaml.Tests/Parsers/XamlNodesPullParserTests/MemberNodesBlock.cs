namespace OmniXaml.Tests.Parsers.XamlNodesPullParserTests
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Glass;
    using Typing;

    public class MemberNodesBlock : IDependency<MemberNodesBlock>
    {
        private readonly Queue<XamlNode> nodes = new Queue<XamlNode>();
        private readonly MutableXamlMember member;

        public MemberNodesBlock(XamlNode headingNode)
        {
            member = (MutableXamlMember)headingNode.Member;
        }

        public void AddNode(XamlNode node)
        {
            nodes.Enqueue(node);
        }

        public IEnumerable<MemberNodesBlock> Dependencies { get; set; }

        public Queue<XamlNode> Nodes => nodes;

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