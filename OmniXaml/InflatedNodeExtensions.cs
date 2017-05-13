using System.Collections.Generic;
using System.Linq;
using OmniXaml.ReworkPhases;

namespace OmniXaml
{
    public static class InflatedNodeExtensions
    {
        public static IEnumerable<InflatedNode> GetAllChildren(this InflatedNode node)
        {
            return node.Children.Concat(node.Assignments.SelectMany(assignment => assignment.Children));
        }
    }
}