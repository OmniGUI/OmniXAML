namespace OmniXaml.Tests.Rework2
{
    using System.Collections.Generic;
    using System.Linq;
    using ReworkPhases;

    public class InflatedMemberAssignmentComparer : IEqualityComparer<InflatedMemberAssignment>
    {
        public bool Equals(InflatedMemberAssignment x, InflatedMemberAssignment y)
        {
            var sameChildren = x.Children.SequenceEqual(y.Children);
            var sameMember = Equals(x.Member, y.Member);
            return sameChildren && sameMember;
        }

        public int GetHashCode(InflatedMemberAssignment obj)
        {
            return obj.GetHashCode();
        }
    }
}