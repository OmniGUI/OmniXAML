namespace OmniXaml.Visualization
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class NodeHierarchizer
    {
        public IEnumerable<HierarchizedXamlNode> CreateHierarchy(IEnumerable<XamlInstruction> xamlNodes)
        {
            var stream = xamlNodes.GetEnumerator();
            stream.MoveNext();
            return HierarchizedXamlNodes(stream);
        }

        private Sequence<HierarchizedXamlNode> HierarchizedXamlNodes(IEnumerator<XamlInstruction> stream)
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

        private static bool IsTrailing(XamlInstruction xamlInstruction)
        {
            IEnumerator<XamlInstruction> stream;
            return xamlInstruction.NodeType == XamlNodeType.EndMember || xamlInstruction.NodeType == XamlNodeType.EndObject;
        }

        private bool IsLeading(XamlInstruction current)
        {
            return current.NodeType == XamlNodeType.StartMember || current.NodeType == XamlNodeType.StartObject;
        }
    }
}