namespace OmniXaml.Tests.Rework2
{
    using System.Collections.Generic;
    using System.Linq;
    using ReworkPhases;

    public class InflatedNodeComparer : IEqualityComparer<InflatedNode>
    {
        public bool Equals(InflatedNode x, InflatedNode y)
        {
            var sameType = x.Instance.GetType() == y.Instance.GetType();
            var sameAssignments = x.UnresolvedAssignments.SequenceEqual(y.UnresolvedAssignments, new InflatedMemberAssignmentComparer());
            var sameChildren = x.UnresolvedChildren.SequenceEqual(y.UnresolvedChildren, new InflatedNodeComparer());

            return sameType && sameAssignments && sameChildren;
        }

        public int GetHashCode(InflatedNode obj)
        {
            return obj.GetHashCode();
        }
    }
}