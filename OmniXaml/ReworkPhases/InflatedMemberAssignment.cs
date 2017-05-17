using Zafiro.Core;

namespace OmniXaml.ReworkPhases
{
    public class InflatedMemberAssignment : IMemberAssignment
    {
        public InflatedMemberAssignment()
        {
            Values = new ValueCollection(this);
        }

        public InflatedNode Parent { get; set; }
        public Member Member { get; set; }
        public ValueCollection Values { get; }

        public override string ToString()
        {
            return $"{nameof(Member)}: {Member}, {nameof(Values)}: {Values.AsCommaSeparatedList()}";
        }      
    }
}