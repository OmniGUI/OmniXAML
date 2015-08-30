namespace OmniXaml
{
    using System.Collections.Generic;
    using System.Linq;
    using Typing;
    using Visualization;

    public class MemberDependencyNodeSorter
    {
        public IEnumerable<XamlInstruction> Sort(IEnumerator<XamlInstruction> enumerator)
        {
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.InstructionType == XamlInstructionType.StartObject)
                {
                    var hasMembersWithDependencies = GetSomeMemberHasDependencies(enumerator.Current.XamlType);
                    if (hasMembersWithDependencies)
                    {
                        foreach (var instruction in SortNodes(enumerator)) yield return instruction;
                    }

                }

                yield return enumerator.Current;
            }
        }

        private IEnumerable<XamlInstruction> SortNodes(IEnumerator<XamlInstruction> enumerator)
        {
            var subSet = LookaheadBuffer.GetUntilEndOfRoot(enumerator);
            var nodes = new InstructionTreeBuilder().CreateHierarchy(subSet);
            var root = new InstructionNode { Children = new Sequence<InstructionNode>(nodes.ToList()) };
            root.AcceptVisitor(new DependencySortingVisitor());

            foreach (var instruction in root.Children.SelectMany(node => node.Dump()))
            {
                yield return instruction;
            }
        }

        private static bool GetSomeMemberHasDependencies(XamlType xamlType)
        {
            var allMembers = xamlType.GetAllMembers().OfType<MutableXamlMember>();
            return allMembers.Any(member => member.Dependencies.Any());
        }
    }
}