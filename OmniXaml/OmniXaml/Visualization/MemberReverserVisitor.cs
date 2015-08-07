namespace OmniXaml.Visualization
{
    using System.Collections.Generic;
    using System.Linq;
    using Typing;

    public class MemberReverserVisitor : IVisitor
    {
        public void Visit(HierarchizedXamlNode hierarchizedXamlNode)
        {
            var list = GetMutableMembers(hierarchizedXamlNode).ToList();
            list.Reverse();
            var others = GetOthers(hierarchizedXamlNode);

            var newList = new List<HierarchizedXamlNode>();

            newList.AddRange(list);
            newList.AddRange(others);

            hierarchizedXamlNode.Children = new Sequence<HierarchizedXamlNode>(newList);

            foreach (var xamlNode in hierarchizedXamlNode.Children)
            {
                xamlNode.AcceptVisitor(this);
            }
        }

        private static IEnumerable<HierarchizedXamlNode> GetMutableMembers(HierarchizedXamlNode hierarchizedXamlNode)
        {
            return hierarchizedXamlNode.Children.Where(node => node.Leading.NodeType == XamlNodeType.StartMember && node.Leading.Member is MutableXamlMember);
        }

        private static IEnumerable<HierarchizedXamlNode> GetOthers(HierarchizedXamlNode hierarchizedXamlNode)
        {
            return hierarchizedXamlNode.Children.Where(node => !(node.Leading.NodeType == XamlNodeType.StartMember && node.Leading.Member is MutableXamlMember));
        }
    }
}