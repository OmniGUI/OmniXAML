namespace OmniXaml.ObjectAssembler
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using Typing;

    public class Level
    {
        public object Instance { get; set; }
        public XamlType XamlType { get; set; }
        public MemberBase Member { get; set; }
        public ICollection Collection { get; set; }
        public bool IsGetObject { get; set; }
        public Collection<ConstructionArgument> CtorArguments { get; set; }
        public bool WasInstanceAssignedRightAfterBeingCreated { get; set; }
        public InstanceProperties InstanceProperties { get; } = new InstanceProperties();
        public bool IsEmpty { get; set; }

        protected bool Equals(Level other)
        {
            return Equals(Instance, other.Instance) && Equals(XamlType, other.XamlType) && Equals(Member, other.Member) &&
                   IsGetObject == other.IsGetObject && Equals(Collection, other.Collection) &&
                   Equals(CtorArguments, other.CtorArguments);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((Level)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Instance != null ? Instance.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (XamlType != null ? XamlType.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Member != null ? Member.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsGetObject.GetHashCode();
                hashCode = (hashCode * 397) ^ (Collection != null ? Collection.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CtorArguments != null ? CtorArguments.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}