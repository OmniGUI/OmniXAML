namespace OmniXaml.ReworkPhases
{
    using System.Collections.Generic;
    using Zafiro.Core;

    public static class InflateNodeExtensions
    {
        public static void AssociateTo(this IEnumerable<InflatedNode> nodes, object parent)
        {
            foreach (var inflatedNode in nodes)
            {
                Collection.UniversalAdd(parent, inflatedNode.Instance);
            }
        }
    }
}