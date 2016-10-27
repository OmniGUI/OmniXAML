namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class ConstructionNode
    {
        public ConstructionNode(Type type)
        {
            InstanceType = type;
        }

        public Type InstanceType { get; set; }
        public string Name { get; set; }
        public IEnumerable<PropertyAssignment> Assignments { get; set; } = new Collection<PropertyAssignment>();
        public IEnumerable<string> InjectableArguments { get; set; } = new Collection<string>();

        public override string ToString()
        {
            return $"[{InstanceType.Name}]";
        }

        protected bool Equals(ConstructionNode other)
        {
            return Equals(InstanceType, other.InstanceType) 
                && Enumerable.SequenceEqual(Assignments, other.Assignments) && 
                Enumerable.SequenceEqual(InjectableArguments, other.InjectableArguments);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((ConstructionNode) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (InstanceType != null ? InstanceType.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Assignments != null ? Assignments.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (InjectableArguments != null ? InjectableArguments.GetHashCode() : 0);
                return hashCode;
            }
        }
    }

    public class ConstructionNode<T> : ConstructionNode
    {
        public ConstructionNode() : base(typeof(T))
        {
        }
    }
}
