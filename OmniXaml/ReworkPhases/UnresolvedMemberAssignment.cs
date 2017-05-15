namespace OmniXaml.ReworkPhases
{
    using System.Collections.Generic;

    public class UnresolvedMemberAssignment : IMemberAssignment<InflatedNode>
    {
        public IEnumerable<InflatedNode> Values { get; set; }
        public Member Member { get; set; }
    }
}