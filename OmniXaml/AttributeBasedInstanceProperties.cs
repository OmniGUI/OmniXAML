namespace OmniXaml
{
    using System;
    using System.Collections.Generic;

    internal class AttributeBasedInstanceProperties
    {
        public string Name { get; set; }
        public IEnumerable<MemberAssignment> Assignments { get; set; }
        public string Key { get; set; }
        public Type InstantiateAs { get; set; }
    }
}