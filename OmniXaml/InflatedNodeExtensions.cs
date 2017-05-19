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

        public static ConstructionNode WithAssignments(this ConstructionNode node, IEnumerable<MemberAssignment> assignment)
        {
            foreach (var ass in assignment)
            {
                node.Assignments.Add(ass);
            }

            return node;
        }

        public static ConstructionNode WithChildren(this ConstructionNode node, IEnumerable<ConstructionNode> children)
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