namespace OmniXaml.Tests
{
    using System.Collections.Generic;

    public static class NodeVisualizer
    {
        public static IEnumerable<Tag> Convert(IEnumerable<XamlNode> xamlNodes)
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
            return current.NodeType.ToString().Contains("End") ;
        }

        private static bool RaisesLevel(XamlNode current)
        {
            return current.NodeType.ToString().Contains("Start") || current.NodeType == XamlNodeType.GetObject;
        }        
    }
}