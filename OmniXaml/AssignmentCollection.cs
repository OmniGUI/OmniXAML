using System.Collections.ObjectModel;
using OmniXaml.ReworkPhases;

namespace OmniXaml
{
    public class AssignmentCollection : Collection<InflatedMemberAssignment>
    {
        public InflatedNode Node { get; }

        public AssignmentCollection(InflatedNode node) 
        {
            Node = node;
        }

        protected override void InsertItem(int index, InflatedMemberAssignment item)
        {
            item.Parent = Node;
            base.InsertItem(index, item);
        }
    }
}