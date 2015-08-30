namespace OmniXaml
{
    using System.Collections.Generic;
    using System.Linq;
    using Glass;
    using Typing;
    using Visualization;

    public class DependencySortingVisitor : IVisitor
    {
        public void Visit(InstructionNode instructionNode)
        {
            var mutable = GetMutableMembers(instructionNode).ToList();
            var sorted = ShortByDependencies(mutable);
            var others = GetOthers(instructionNode);

            var newList = new List<InstructionNode>();

            newList.AddRange(sorted);
            newList.AddRange(others);

            instructionNode.Children = new Sequence<InstructionNode>(newList);

            foreach (var node in instructionNode.Children)
            {
                node.AcceptVisitor(this);
            }
        }

        private IEnumerable<InstructionNode> ShortByDependencies(List<InstructionNode> list)
        {
            if (!list.Any())
            {
                return new List<InstructionNode>();
            }

            var members = list.Select(node => (MutableXamlMember) node.Leading.Member);
            var xamlNode = members.First();
            var sortedList = xamlNode.Sort();

            var finalDeps = sortedList.Where(sr => members.Contains(sr));

            var sortedNodes = SortNodesAccordingTo(list, finalDeps);
            return sortedNodes;
        }

        private static IEnumerable<InstructionNode> SortNodesAccordingTo(List<InstructionNode> list, IEnumerable<MutableXamlMember> sortedList)
        {
            var sorted = new List<InstructionNode>();

            foreach (var xamlMember in sortedList)
            {
                sorted.Add(list.First(node => xamlMember.Equals((MutableXamlMember) node.Leading.Member)));
            }

            return sorted;
        }

        private static IEnumerable<InstructionNode> GetMutableMembers(InstructionNode instructionNode)
        {
            return instructionNode.Children.Where(node => node.Leading.InstructionType == XamlInstructionType.StartMember && node.Leading.Member is MutableXamlMember);
        }

        private static IEnumerable<InstructionNode> GetOthers(InstructionNode instructionNode)
        {
            return instructionNode.Children.Where(node => !(node.Leading.InstructionType == XamlInstructionType.StartMember && node.Leading.Member is MutableXamlMember));
        }
    }
}