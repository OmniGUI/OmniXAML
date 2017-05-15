namespace OmniXaml.ReworkPhases
{
    using System.Collections.Generic;
    using Zafiro.Core;

    public class InflatedMemberAssignment : IMemberAssignment<InflatedNode>
    {
        public Member Member { get; set; }
        public IEnumerable<InflatedNode> Values { get; set; } = new List<InflatedNode>();

        public override string ToString()
        {
            return $"{nameof(Member)}: {Member}, {nameof(Values)}: {Values.AsCommaSeparatedList()}";
        }      
    }
}