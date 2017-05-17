using System.Collections.Generic;
using System.Linq;
using OmniXaml.ReworkPhases;

namespace OmniXaml
{
    public static class InflatedNodeExtensions
    {
        public static IEnumerable<ConstructionNode> GetAllChildren(this ConstructionNode node)
        {
            return node.Children.Concat(node.Assignments.SelectMany(assignment => assignment.Values));
        }

        public static bool ContainsFailedConversion(this InflatedNode node)
        {
            return node.IsPendingCreate ||
                   node.Assignments.SelectMany(assignment => assignment.Values).Any(n => n.ContainsFailedConversion());
        }

        public static InflatedNode WithAssignments(this InflatedNode node, IEnumerable<InflatedMemberAssignment> assignment)
        {
            foreach (var ass in assignment)
            {
                node.Assignments.Add(ass);
            }

            return node;
        }

        public static InflatedNode WithChildren(this InflatedNode node, IEnumerable<InflatedNode> children)
        {
            foreach (var ass in children)
            {
                node.Children.Add(ass);
            }

            return node;
        }

        public static InflatedMemberAssignment WithValues(this InflatedMemberAssignment assignment, IEnumerable<InflatedNode> nodes)
        {
            foreach (var inflatedNode in nodes)
            {
                assignment.Values.Add(inflatedNode);
            }

            return assignment;
        }
    }
}