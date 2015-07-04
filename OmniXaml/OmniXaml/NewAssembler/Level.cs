namespace OmniXaml.NewAssembler
{
    using System.Collections;
    using Typing;

    public class Level
    {
        public Level()
        {
        }

        private Level(Level level)
        {
            Instance = level.Instance;
            XamlType = level.XamlType;
            XamlMember = level.XamlMember;
            IsCollectionHolderObject = level.IsCollectionHolderObject;
            IsMemberHostingChildren = level.IsMemberHostingChildren;
        }

        public object Instance { get; set; }
        public XamlType XamlType { get; set; }
        public XamlMember XamlMember { get; set; }
        public bool IsCollectionHolderObject { get; set; }
        public bool IsMemberHostingChildren { get; set; }
        public ICollection Collection { get; set; }
        public bool IsGetObject { get; set; }

        public void MaterializeType()
        {
            var instance = XamlType.CreateInstance(null);
            Instance = instance;
        }

        public Level Clone()
        {
            return new Level(this);
        }

        protected bool Equals(Level other)
        {
            return Equals(Instance, other.Instance) && Equals(XamlType, other.XamlType) &&
                   Equals(XamlMember, other.XamlMember) && IsCollectionHolderObject == other.IsCollectionHolderObject &&
                   Equals(Collection, other.Collection) && IsGetObject == other.IsGetObject &&
                   IsMemberHostingChildren == other.IsMemberHostingChildren;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Level) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Instance != null ? Instance.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (XamlType != null ? XamlType.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (XamlMember != null ? XamlMember.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ IsCollectionHolderObject.GetHashCode();
                hashCode = (hashCode*397) ^ (Collection != null ? Collection.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ IsGetObject.GetHashCode();
                hashCode = (hashCode*397) ^ IsMemberHostingChildren.GetHashCode();
                return hashCode;
            }
        }
    }
}