namespace OmniXaml.Visualization
{
    using System.Collections.Generic;
    using System.Linq;
    using Typing;

    public class MemberReverserVisitor : IVisitor
    {
        public void Visit(InstructionNode instructionNode)
        {
            var list = GetMutableMembers(instructionNode).ToList();
            list.Reverse();
            var others = GetOthers(instructionNode);

            var newList = new List<InstructionNode>();

            newList.AddRange(list);
            newList.AddRange(others);

            instructionNode.Children = new Sequence<InstructionNode>(newList);

            foreach (var xamlNode in instructionNode.Children)
            {
                xamlNode.AcceptVisitor(this);
            }
        }

        private static IEnumerable<InstructionNode> GetMutableMembers(InstructionNode instructionNode)
        {
            return instructionNode.Children.Where(node => node.Leading.NodeType == XamlNodeType.StartMember && node.Leading.Member is MutableXamlMember);
        }

        private static IEnumerable<InstructionNode> GetOthers(InstructionNode instructionNode)
        {
            return instructionNode.Children.Where(node => !(node.Leading.NodeType == XamlNodeType.StartMember && node.Leading.Member is MutableXamlMember));
        }
    }
}