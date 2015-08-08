namespace OmniXaml
{
    using System.Collections.Generic;
    using System.Linq;
    using Typing;
    using Visualization;

    public class MemberDependencyNodeSorter
    {
        public IEnumerable<XamlNode> Sort(IEnumerator<XamlNode> enumerator)
        {
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.NodeType == XamlNodeType.StartObject)
                {
                    var hasMembersWithDependencies = GetSomeMemberHasDependencies(enumerator.Current.XamlType);
                    if (hasMembersWithDependencies)
                    {
                        foreach (var xamlNode in SortNodes(enumerator)) yield return xamlNode;
                    }

                }

                yield return enumerator.Current;
            }
        }

        private IEnumerable<XamlNode> SortNodes(IEnumerator<XamlNode> enumerator)
        {
            var subSet = LookaheadBuffer.GetUntilEndOfRoot(enumerator);
            var nodes = new NodeHierarchizer().CreateHierarchy(subSet);
            var root = new HierarchizedXamlNode { Children = new Sequence<HierarchizedXamlNode>(nodes.ToList()) };
            root.AcceptVisitor(new DependencySortingVisitor());

            foreach (var xamlNode in root.Children.SelectMany(node => node.Dump()))
            {
                yield return xamlNode;
            }
        }

        private static bool GetSomeMemberHasDependencies(XamlType xamlType)
        {
            var allMembers = xamlType.GetAllMembers().OfType<MutableXamlMember>();
            return allMembers.Any(member => member.Dependencies.Any());
        }
    }
}