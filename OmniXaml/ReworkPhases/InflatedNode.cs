namespace OmniXaml.ReworkPhases
{
    using System;
    using System.Collections.Generic;

    public class InflatedNode : IInstanceHolder
    {
        public HashSet<InflatedMemberAssignment> UnresolvedAssignments { get; set; } = new HashSet<InflatedMemberAssignment>();
        public object Instance { get; set; }
        public bool IsConversionFailed { get; set; }
        public string SourceValue { get; set; }
        public Type InstanceType { get; set; }

        protected bool Equals(InflatedNode other)
        {
            return UnresolvedAssignments.SetEquals(other.UnresolvedAssignments) && Equals(Instance, other.Instance) &&
                   IsConversionFailed == other.IsConversionFailed && SourceValue == other.SourceValue && 
                   InstanceType == other.InstanceType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((InflatedNode) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode =  UnresolvedAssignments != null ? UnresolvedAssignments.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Instance != null ? Instance.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsConversionFailed.GetHashCode();
                hashCode = (hashCode * 397) ^ InstanceType.GetHashCode();
                hashCode = (hashCode * 397) ^ (SourceValue != null ? SourceValue.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}