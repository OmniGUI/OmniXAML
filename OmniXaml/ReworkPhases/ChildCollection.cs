using System.Collections.ObjectModel;

namespace OmniXaml.ReworkPhases
{
    public class ChildCollection : Collection<InflatedNode>
    {
        public InflatedNode Owner { get; }

        public ChildCollection(InflatedNode owner)
        {
            Owner = owner;
        }

        protected override void InsertItem(int index, InflatedNode item)
        {
            item.ParentCollection = item;
            base.InsertItem(index, item);
        }
    }
}