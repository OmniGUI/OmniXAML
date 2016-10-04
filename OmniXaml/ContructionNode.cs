namespace OmniXaml
{
    using System;
    using System.Collections.Generic;

    public class ContructionNode
    {
        public Type InstanceType { get; set; }
        public IEnumerable<PropertyAssignment> Assignments { get; set; }
    }
}
