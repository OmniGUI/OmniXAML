namespace OmniXaml.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Typing;
    using Visualization;

    public class Skimmer
    {
        public IEnumerable<XamlNode> FilterInput(IEnumerator<XamlNode> enumerator)
        {
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.NodeType == XamlNodeType.StartObject)
                {
                    var hasDeps = GetSomeMemberHasDependencies(enumerator.Current.XamlType);
                    if (hasDeps)
                    {
                        foreach (var xamlNode in SortNodes(enumerator)) yield return xamlNode;
                    }

                }

                yield return enumerator.Current;

            }
        }

        private IEnumerable<XamlNode> SortNodes(IEnumerator<XamlNode> enumerator)
        {
            var subSet = Enumerable.ToList<XamlNode>(LookAhead(enumerator));
            var nodes = new NodeHierarchizer().CreateHierarchy(subSet);
            var root = new HierarchizedXamlNode { Children = new Sequence<HierarchizedXamlNode>(nodes.ToList()) };
            root.AcceptVisitor(new DependencySortingVisitor());

            foreach (var xamlNode in root.Children.SelectMany(node => node.Dump()))
            {
                yield return xamlNode;
            }
        }

        public static IEnumerable<XamlNode> LookAhead(IEnumerator<XamlNode> enumerator)
        {
            var count = 0;
            var yielded = 0;
            var isEndOfOffendingBlock = false;

            if (enumerator.Current.Equals(default(XamlNode)))
            {
                yield break;
            }

            do
            {


                yield return enumerator.Current;
                yielded++;

                if (enumerator.Current.NodeType == XamlNodeType.StartObject)
                {
                    count++;
                }
                else if (enumerator.Current.NodeType == XamlNodeType.EndObject)
                {
                    count--;
                }

                if (count == 0)
                {
                    isEndOfOffendingBlock = true;
                }

                enumerator.MoveNext();

            } while (!isEndOfOffendingBlock);
        }

        private static bool GetSomeMemberHasDependencies(XamlType xamlType)
        {
            var allMembers = xamlType.GetAllMembers().OfType<MutableXamlMember>();
            return allMembers.Any(member => member.Dependencies.Any());
        }
    }
}