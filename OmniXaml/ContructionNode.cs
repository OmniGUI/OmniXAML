namespace OmniXaml
{
    using System;
    using System.Collections.Generic;

    public class ContructionNode
    {
        public ContructionNode(Type type)
        {
            InstanceType = type;
        }

        public Type InstanceType { get; set; }
        public IEnumerable<PropertyAssignment> Assignments { get; set; } = new List<PropertyAssignment>();
    }
}
