using System.Collections.Generic;
using System.Linq;
using OmniXaml.ReworkPhases;

namespace OmniXaml
{
    public static class InflatedNodeExtensions
    {
        public static IEnumerable<InflatedNode> GetAllChildren(this InflatedNode node)
        {
            return node.Children.Concat(node.Assignments.SelectMany(assignment => assignment.Values));
        }

        public static bool ContainsFailedConversion(this InflatedNode node)
        {
            return node.ConversionFailed ||
                   node.Assignments.SelectMany(assignment => assignment.Values).Any(n => n.ContainsFailedConversion());
        }
    }
}