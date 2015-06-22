namespace OmniXaml.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Classes;

    public static class NodeVisualizer
    {
        public static IEnumerable<Tag> ToTags(IEnumerable<XamlNode> xamlNodes)
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

                yield return new Tag(current.ToString(), level);

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

        public static Node ToTree(IEnumerable<XamlNode> xamlNodes)
        {
            var enumerator = xamlNodes.GetEnumerator();

            var stack = new Stack<Node>();
            stack.Push(new Node("Root"));

            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;

                if (LowersLevel(current))
                {
                    stack.Pop();
                }
                else
                {
                    var item = new Node(current);
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

    public class Node
    {
        public XamlNode XamlNode { get; }

        public Node(string name) : this(new XamlNode(XamlNodeType.None))
        {

        }

        public Node(XamlNode xamlNode)
        {
            this.XamlNode = xamlNode;           
            this.Children = new Collection<Node>();
        }

        public ICollection<Node> Children { get; set; }
    }

    public enum NodeType
    {
        Root,
        NamespaceDeclaration,
        GetObject,
        Object,
        Member,
        Value,
    }
}