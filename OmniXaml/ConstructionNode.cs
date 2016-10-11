namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class ConstructionNode
    {
        public ConstructionNode(Type type)
        {
            InstanceType = type;
        }

        public Type InstanceType { get; set; }
        public IEnumerable<PropertyAssignment> Assignments { get; set; } = new Collection<PropertyAssignment>();
        public IEnumerable<string> InjectableArguments { get; set; } = new Collection<string>();

        public override string ToString()
        {
            return $"[{InstanceType.Name}]";
        }
    }
}
