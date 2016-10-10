namespace OmniXaml
{
    using System;
    using System.Collections.Generic;

    public class ConstructionNode
    {
        public ConstructionNode(Type type)
        {
            InstanceType = type;
        }

        public Type InstanceType { get; set; }
        public IEnumerable<PropertyAssignment> Assignments { get; set; } = new List<PropertyAssignment>();

        public override string ToString()
        {
            return $"[{InstanceType.Name}]";
        }
    }
}
