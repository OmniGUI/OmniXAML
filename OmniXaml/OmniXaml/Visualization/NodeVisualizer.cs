namespace OmniXaml.Visualization
{
    using System.Collections.Generic;

    public static class NodeVisualizer
    {
        public static IEnumerable<VisualizationTag> ToTags(IEnumerable<XamlNode> xamlNodes)
        {
            var enumerator = xamlNodes.GetEnumerator();
            var level = 0;

            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;

                if (LowersLevel(current))
                {
                    level--;
                }

                yield return new VisualizationTag(current.ToString(), level);

                if (RaisesLevel(current))
                {
                    level++;
                }

            }
        }

        private static bool LowersLevel(XamlNode current)
        {
            return current.NodeType.ToString().Contains("End");
        }

        private static bool RaisesLevel(XamlNode current)
        {
            return current.NodeType.ToString().Contains("Start") || current.NodeType == XamlNodeType.GetObject;
        }

        public static VisualizationNode ToTree(IEnumerable<XamlNode> xamlNodes)
        {
            var enumerator = xamlNodes.GetEnumerator();

            var stack = new Stack<VisualizationNode>();
            stack.Push(new VisualizationNode("Root"));

            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;

                if (LowersLevel(current))
                {
                    stack.Pop();
                }
                else
                {
                    var item = new VisualizationNode(current);
                    stack.Peek().Children.Add(item);

                    if (RaisesLevel(current))
                    {
                        stack.Push(item);
                    }
                }
            }

            return stack.Peek();
        }
    }
}