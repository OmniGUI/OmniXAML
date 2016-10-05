namespace OmniXaml
{
    using System.Collections.Generic;

    public class PropertyAssignment
    {
        public Property Property { get; set; }
        public string SourceValue { get; set; }
        public IEnumerable<ContructionNode> Children { get; set; }
    }
}