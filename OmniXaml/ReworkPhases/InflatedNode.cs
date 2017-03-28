namespace OmniXaml.ReworkPhases
{
    using System.Collections.Generic;

    public class InflatedNode
    {
        public IEnumerable<InflatedNode> Children { get; set; } = new List<InflatedNode>();
        public IEnumerable<InflatedMemberAssignment> Assigments { get; set; } = new List<InflatedMemberAssignment>();
        public object Instance { get; set; }
        public string SourceValue { get; set; }
        public bool IsResolved { get; set; }
    }
}