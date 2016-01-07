namespace OmniXaml
{
    using System.Collections.Generic;
    using System.Linq;
    using Typing;

    public class MemberDependencyNodeSorter
    {
        public IEnumerable<Instruction> Sort(IEnumerator<Instruction> enumerator)
        {
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.InstructionType == InstructionType.StartObject)
                {
                    var hasMembersWithDependencies = GetSomeMemberHasDependencies(enumerator.Current.XamlType);
                    if (hasMembersWithDependencies)
                    {
                        foreach (var instruction in SortNodes(enumerator)) yield return instruction;
                    }

                }

                if (!IsEmptyInstruction(enumerator))
                {
                    yield return enumerator.Current;
                }
            }
        }

        private static bool IsEmptyInstruction(IEnumerator<Instruction> enumerator)
        {
            return enumerator.Current.Equals(default(Instruction));
        }

        private IEnumerable<Instruction> SortNodes(IEnumerator<Instruction> enumerator)
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
            var allMembers = xamlType.GetAllMembers().OfType<MutableMember>();
            return allMembers.Any(member => member.Dependencies.Any());
        }
    }
}