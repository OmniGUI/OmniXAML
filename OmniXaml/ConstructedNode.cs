namespace OmniXaml
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class ConstructedNode
    {
        public ConstructedNode Parent { get; set; }
        public object GeneratedInstance { get; set; }
        public ICollection<ConstructedNode> Children { get; } = new Collection<ConstructedNode>();
    }
}