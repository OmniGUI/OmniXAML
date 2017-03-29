namespace OmniXaml.ReworkPhases
{
    using System.Collections.Generic;

    public class UnresolvedMemberAssignment : IMemberAssignment<UnresolvedNode>
    {
        public IEnumerable<UnresolvedNode> Children { get; set; }
        public Member Member { get; set; }
    }
}