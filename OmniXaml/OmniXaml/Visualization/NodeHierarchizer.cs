namespace OmniXaml.Visualization
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class NodeHierarchizer
    {
        public IEnumerable<HierarchizedXamlNode> CreateHierarchy(IEnumerable<XamlNode> xamlNodes)
        {
            var stream = xamlNodes.GetEnumerator();
            stream.MoveNext();
            return HierarchizedXamlNodes(stream);
        }

        private Sequence<HierarchizedXamlNode> HierarchizedXamlNodes(IEnumerator<XamlNode> stream)
        {            
            var nodes = new Sequence<HierarchizedXamlNode>();

            while (IsLeading(stream.Current))
            {
                var currentNode = new HierarchizedXamlNode();
                currentNode.Leading = stream.Current;
                var continueWorking = true;
                while (stream.MoveNext() && continueWorking)
                {
                    if (IsLeading(stream.Current))
                    {
                        currentNode.Children = HierarchizedXamlNodes(stream);
                    }

                    var xamlNode = stream.Current;

                    if (IsTrailing(xamlNode))
                    {
                        continueWorking = false;
                        currentNode.Trailing = stream.Current;
                    }
                    else
                    {
                        currentNode.Body.Add(stream.Current);
                    }
                }                

                nodes.Add(currentNode);
            }

            return nodes;
        }

        private static bool IsTrailing(XamlNode xamlNode)
        {
            IEnumerator<XamlNode> stream;
            return xamlNode.NodeType == XamlNodeType.EndMember || xamlNode.NodeType == XamlNodeType.EndObject;
        }

        private bool IsLeading(XamlNode current)
        {
            return current.NodeType == XamlNodeType.StartMember || current.NodeType == XamlNodeType.StartObject;
        }
    }
}