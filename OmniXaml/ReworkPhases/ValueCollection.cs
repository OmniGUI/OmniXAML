using System.Collections.ObjectModel;

namespace OmniXaml.ReworkPhases
{
    public class ValueCollection : Collection<InflatedNode>
    {
        public InflatedMemberAssignment Assignment { get; }

        public ValueCollection(InflatedMemberAssignment assignment)
        {
            Assignment = assignment;
        }
        protected override void InsertItem(int index, InflatedNode item)
        {
            item.ParentAssignment = Assignment;
            base.InsertItem(index, item);
        }
    }
}