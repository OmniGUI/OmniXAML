namespace OmniXaml.NewAssembler
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using Typing;

    public class Level
    {
        public object Instance { get; set; }
        public XamlType XamlType { get; set; }
        public XamlMemberBase XamlMember { get; set; }
        public bool IsCollectionHolderObject { get; set; }
        public bool IsMemberHostingChildren { get; set; }
        public ICollection Collection { get; set; }
        public bool IsGetObject { get; set; }
        public Collection<ConstructionArgument> CtorArguments { get; set; }
        public bool WasAssociatedRightAfterCreation { get; set; }

        protected bool Equals(Level other)
        {
            return Equals(Instance, other.Instance) && Equals(XamlType, other.XamlType) && Equals(XamlMember, other.XamlMember) &&
                   IsCollectionHolderObject == other.IsCollectionHolderObject && IsMemberHostingChildren == other.IsMemberHostingChildren &&
                   IsGetObject == other.IsGetObject && Equals(Collection, other.Collection) && Equals(CtorArguments, other.CtorArguments);
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
            if (obj.GetType() != this.GetType())
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
                hashCode = (hashCode * 397) ^ (XamlMember != null ? XamlMember.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsCollectionHolderObject.GetHashCode();
                hashCode = (hashCode * 397) ^ IsMemberHostingChildren.GetHashCode();
                hashCode = (hashCode * 397) ^ IsGetObject.GetHashCode();                
                hashCode = (hashCode * 397) ^ (Collection != null ? Collection.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CtorArguments != null ? CtorArguments.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}