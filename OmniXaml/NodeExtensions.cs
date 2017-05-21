using System.Collections.Generic;
using System.Linq;

namespace OmniXaml
{
    public static class NodeExtensions
    {
        public static IEnumerable<ConstructionNode> GetAllChildren(this ConstructionNode node)
        {
            return node.Children.Concat(node.Assignments.SelectMany(assignment => assignment.Values));
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
    }
}