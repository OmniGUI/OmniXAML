using Zafiro.Core;

namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class ConstructionNode : IChild<ConstructionNode>
    {
        public ConstructionNode()
        {            
            Assignments = new ParentLinkedCollection<MemberAssignment, ConstructionNode>(this);
            Children = new ParentLinkedCollection<ConstructionNode, ConstructionNode>(this);
        }

        public ConstructionNode(Type type) : this()
        {
            InstanceType = type;
        }

        public Type InstanceType { get; set; }
        public string Name { get; set; }
        public ParentLinkedCollection<MemberAssignment, ConstructionNode> Assignments { get; }
        public IEnumerable<string> PositionalParameters { get; set; } = new Collection<string>();
        public ParentLinkedCollection<ConstructionNode, ConstructionNode> Children { get; }
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
    }

    public class ConstructionNode<T> : ConstructionNode
    {

        public ConstructionNode()
        {
            this.InstanceType = typeof(T);
        }
    }

    public class ConstructionNode<TBaseType, TSubtype> : ConstructionNode where TSubtype : TBaseType
    {

        public ConstructionNode()
        {
            this.InstanceType = typeof(TBaseType);
            this.InstantiateAs = typeof(TSubtype);
        }
    }
}