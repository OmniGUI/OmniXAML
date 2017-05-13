using System;
using System.Collections.Generic;

namespace OmniXaml.ReworkPhases
{
    public class InflatedNode : IInstanceHolder
    {
        public bool ConversionFailed { get; set; }
        public string SourceValue { get; set; }
        public Type InstanceType { get; set; }
        public object Instance { get; set; }
        public List<InflatedMemberAssignment> Assignments { get; set; } = new List<InflatedMemberAssignment>();
        public IEnumerable<InflatedNode> Children { get; set; } = new List<InflatedNode>();
        public string Name { get; set; }
        public InflatedNode Parent { get; set; }

        protected bool Equals(InflatedNode other)
        {
            return Equals(Instance, other.Instance) && ConversionFailed == other.ConversionFailed &&
                   string.Equals(SourceValue, other.SourceValue) && Equals(InstanceType, other.InstanceType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((InflatedNode) obj);
        }


        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Instance != null ? Instance.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ ConversionFailed.GetHashCode();
                hashCode = (hashCode * 397) ^ (SourceValue != null ? SourceValue.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (InstanceType != null ? InstanceType.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}