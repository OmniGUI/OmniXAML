namespace OmniXaml.ReworkPhases
{
    using System;

    public class UnresolvedNode : IInstanceHolder
    {
        public string SourceValue { get; set; }
        public Type InstanceType { get; set; }
        public object Instance { get; set; }
    }
}