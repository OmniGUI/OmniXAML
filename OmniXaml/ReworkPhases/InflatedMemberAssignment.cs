namespace OmniXaml.ReworkPhases
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Zafiro.Core;

    public class InflatedMemberAssignment : IMemberAssignment<InflatedNode>
    {
        public Member Member { get; set; }
        public IEnumerable<InflatedNode> Children { get; set; } = new List<InflatedNode>();

        public override string ToString()
        {
            return $"{nameof(Member)}: {Member}, {nameof(Children)}: {Children.AsCommaSeparatedList()}";
        }      
    }
}