namespace OmniXaml
{
    using System.Collections.Generic;
    using System.Linq;
    using Glass.Core;
    using Typing;
    using Visualization;

    public class DependencySortingVisitor : IVisitor
    {
        public void Visit(InstructionNode instructionNode)
        {
            var directives = GetDirectives(instructionNode);
            var sortable = GetSortableMembers(instructionNode);
            var others = GetOthers(instructionNode);

            var rearrangedNodes = new List<InstructionNode>();

            var sorted = ShortByDependencies(sortable.ToList());

            rearrangedNodes.AddRange(directives);
            rearrangedNodes.AddRange(others);
            rearrangedNodes.AddRange(sorted);

            instructionNode.Children = new Sequence<InstructionNode>(rearrangedNodes);

            foreach (var node in instructionNode.Children)
            {
                node.AcceptVisitor(this);
            }
        }

        private IEnumerable<InstructionNode> GetDirectives(InstructionNode instructionNode)
        {
            return instructionNode.Children.Where(node => IsDirective(node));
        }

        private static bool IsDirective(InstructionNode node)
        {
            return node.Leading.InstructionType == InstructionType.StartMember && node.Leading.Member.IsDirective;
        }

        private static IEnumerable<InstructionNode> ShortByDependencies(List<InstructionNode> list)
        {
            if (!list.Any())
            {
                return new List<InstructionNode>();
            }

            var members = list.Select(node => (MutableMember) node.Leading.Member).ToList();
            var sortedList = members.SortDependencies();
            var finalDeps = sortedList.Where(sr => members.Contains(sr));

            var sortedNodes = SortNodesAccordingTo(list, finalDeps);
            return sortedNodes;
        }

        private static IEnumerable<InstructionNode> SortNodesAccordingTo(List<InstructionNode> list, IEnumerable<MutableMember> sortedList)
        {
            var sorted = new List<InstructionNode>();

            foreach (var xamlMember in sortedList)
            {
                sorted.Add(list.First(node => xamlMember.Equals((MutableMember) node.Leading.Member)));
            }

            return sorted;
        }

        private static IEnumerable<InstructionNode> GetSortableMembers(InstructionNode instructionNode)
        {
            return instructionNode.Children.Where(IsSortable);
        }

        private static bool IsSortable(InstructionNode node)
        {
            return node.Leading.InstructionType == InstructionType.StartMember && node.Leading.Member is MutableMember && !IsDirective(node);
        }

        private static IEnumerable<InstructionNode> GetOthers(InstructionNode instructionNode)
        {
            return instructionNode.Children.Where(IsOther);
        }

        private static bool IsOther(InstructionNode node)
        {
            return !IsDirective(node) && !IsSortable(node);
        }
    }
}