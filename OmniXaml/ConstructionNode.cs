namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class ConstructionNode
    {
        public ConstructionNode()
        {            
        }

        public ConstructionNode(Type type)
        {
            InstanceType = type;
        }

        public Type InstanceType { get; set; }
        public string Name { get; set; }
        public IEnumerable<MemberAssignment> Assignments { get; set; } = new Collection<MemberAssignment>();
        public IEnumerable<string> PositionalParameters { get; set; } = new Collection<string>();
        public IEnumerable<ConstructionNode> Children { get; set; } = new Collection<ConstructionNode>();
        public string Key { get; set; }
        public Type InstantiateAs { get; set; }
        public Type ActualInstanceType => InstantiateAs ?? InstanceType;
        public string SourceValue { get; set; }
        public object Instance { get; set; }
        public bool IsCreated { get; set; }
        public ConstructionNode Parent { get; set; }

        protected bool Equals(ConstructionNode other)
        {
            return InstanceType == other.InstanceType && string.Equals(Name, other.Name) && Enumerable.SequenceEqual(Assignments, other.Assignments) &&
                   Enumerable.SequenceEqual(PositionalParameters, other.PositionalParameters) && Enumerable.SequenceEqual(Children, other.Children) && Equals(Key, other.Key) && InstantiateAs == other.InstantiateAs;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (!(obj is ConstructionNode))
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
                hashCode = (hashCode*397) ^ (PositionalParameters != null ? PositionalParameters.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Children != null ? Children.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Key != null ? Key.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (InstantiateAs != null ? InstantiateAs.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static IEnumerable<ConstructionNode> ForString(string str)
        {
            yield return new ConstructionNode(typeof(string)) {SourceValue = str};
        }
    }
}