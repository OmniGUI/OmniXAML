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
            var sameAssignments = x.Assigments.SequenceEqual(y.Assigments, new InflatedMemberAssignmentComparer());
            var sameChildren = x.Children.SequenceEqual(y.Children, new InflatedNodeComparer());
            var sameSourceValue = x.SourceValue == y.SourceValue;
            var sameIsResolved = x.IsResolved == y.IsResolved;

            return sameType && sameAssignments && sameChildren && sameSourceValue && sameIsResolved;
        }

        public int GetHashCode(InflatedNode obj)
        {
            return obj.GetHashCode();
        }
    }
}