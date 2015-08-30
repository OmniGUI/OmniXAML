namespace OmniXaml.Visualization
{
    using System.Collections.Generic;

    public static class NodeVisualizer
    {
        public static IEnumerable<VisualizationTag> ToTags(IEnumerable<XamlInstruction> xamlNodes)
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

        private static bool LowersLevel(XamlInstruction current)
        {
            return current.InstructionType.ToString().Contains("End");
        }

        private static bool RaisesLevel(XamlInstruction current)
        {
            return current.InstructionType.ToString().Contains("Start") || current.InstructionType == XamlInstructionType.GetObject;
        }

        public static VisualizationNode ToTree(IEnumerable<XamlInstruction> xamlNodes)
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