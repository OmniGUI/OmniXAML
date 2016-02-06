namespace OmniXaml
{
    using System.Collections.Generic;
    using System.Linq;
    using Typing;

    public class MemberDependencyNodeSorter
    {
        private IEnumerator<Instruction> enumerator;

        private bool IsStartOfObject => enumerator.Current.InstructionType == InstructionType.StartObject;

        private bool HasMemberInterdependencies => enumerator.Current.XamlType != null && GetSomeMemberHasDependencies(CurrentXamlType);

        private XamlType CurrentXamlType => enumerator.Current.XamlType;

        private bool IsStartOfObjectWithSomeMemberDependency => IsStartOfObject && HasMemberInterdependencies;

        private bool IsEmptyInstruction => enumerator.Current.Equals(default(Instruction));

        public IEnumerable<Instruction> Sort(IEnumerator<Instruction> enumerator)
        {
            this.enumerator = enumerator;

            while (enumerator.MoveNext())
            {
                do
                {
                    if (IsStartOfObjectWithSomeMemberDependency)
                    {
                        foreach (var instruction in SortNodes()) { yield return instruction; }
                    }

                    if (!IsEmptyInstruction && !HasMemberInterdependencies)
                    {
                        yield return enumerator.Current;
                    }
                } while (IsStartOfObjectWithSomeMemberDependency);
            }
        }

        private IEnumerable<Instruction> SortNodes()
        {
            var subSet = LookaheadBuffer.GetUntilEndOfRoot(enumerator);
            var nodes = new InstructionTreeBuilder().CreateHierarchy(subSet);
            var root = new InstructionNode {Children = new Sequence<InstructionNode>(nodes.ToList())};
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