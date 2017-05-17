using System;
using System.Collections.Generic;

namespace OmniXaml.ReworkPhases
{
    public class InflatedNode : IInstanceHolder
    {
        public InflatedMemberAssignment ParentAssignment { get; set; }
        public InflatedNode ParentCollection { get; set; }
        public bool IsPendingCreate { get; set; }
        public string SourceValue { get; set; }
        public Type InstanceType { get; set; }
        public object Instance { get; set; }
        public AssignmentCollection Assignments { get; }
        public ChildCollection Children { get; } 
        public string Name { get; set; }
        public InflatedNode Parent => ParentAssignment?.Parent ?? ParentCollection;

        public InflatedNode()
        {
            Assignments = new AssignmentCollection(this);
            Children = new ChildCollection(this);
        }

        protected bool Equals(InflatedNode other)
        {
            return Equals(Instance, other.Instance) && IsPendingCreate == other.IsPendingCreate &&
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
                hashCode = (hashCode * 397) ^ IsPendingCreate.GetHashCode();
                hashCode = (hashCode * 397) ^ (SourceValue != null ? SourceValue.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (InstanceType != null ? InstanceType.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}