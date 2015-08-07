namespace OmniXaml.Visualization
{
    using System.Collections.Generic;
    using System.Linq;
    using Glass;
    using Typing;

    public class DependencySortingVisitor : IVisitor
    {
        public void Visit(HierarchizedXamlNode hierarchizedXamlNode)
        {
            var mutable = GetMutableMembers(hierarchizedXamlNode).ToList();
            var sorted = ShortByDependencies(mutable);
            var others = GetOthers(hierarchizedXamlNode);

            var newList = new List<HierarchizedXamlNode>();

            newList.AddRange(sorted);
            newList.AddRange(others);

            hierarchizedXamlNode.Children = new Sequence<HierarchizedXamlNode>(newList);

            foreach (var xamlNode in hierarchizedXamlNode.Children)
            {
                xamlNode.AcceptVisitor(this);
            }
        }

        private IEnumerable<HierarchizedXamlNode> ShortByDependencies(List<HierarchizedXamlNode> list)
        {
            if (!list.Any())
            {
                return new List<HierarchizedXamlNode>();
            }

            var members = list.Select(node => (MutableXamlMember) node.Leading.Member);
            var xamlNode = members.First();
            var sortedList = xamlNode.Sort();

            var sortedNodes = SortNodesAccordingTo(list, sortedList);
            return sortedNodes;
        }

        private static IEnumerable<HierarchizedXamlNode> SortNodesAccordingTo(List<HierarchizedXamlNode> list, IEnumerable<MutableXamlMember> sortedList)
        {
            var sorted = new List<HierarchizedXamlNode>();

            foreach (var mutableXamlMember in sortedList)
            {
                sorted.Add(list.First(node => mutableXamlMember.Equals((MutableXamlMember) node.Leading.Member)));
            }

            return sorted;
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