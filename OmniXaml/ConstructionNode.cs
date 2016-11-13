namespace OmniXaml
{
    using System;
    using System.Collections;
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
        public IEnumerable<MemberAssignment> Assignments { get; set; } = new Collection<MemberAssignment>();
        public IEnumerable<string> InjectableArguments { get; set; } = new Collection<string>();
        public IEnumerable<ConstructionNode> Children { get; set; } = new Collection<ConstructionNode>();
        public object Key { get; set; }

        public override string ToString()
        {
            return $"[{InstanceType.Name}]";
        }

        protected bool Equals(ConstructionNode other)
        {
            return InstanceType == other.InstanceType && string.Equals(Name, other.Name) && Enumerable.SequenceEqual(Assignments, other.Assignments) &&
                   Enumerable.SequenceEqual(InjectableArguments, other.InjectableArguments) && Enumerable.SequenceEqual(Children, other.Children) && Equals(Key, other.Key);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals((ConstructionNode) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = InstanceType != null ? InstanceType.GetHashCode() : 0;
                hashCode = (hashCode*397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Assignments != null ? Assignments.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (InjectableArguments != null ? InjectableArguments.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Children != null ? Children.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Key != null ? Key.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}