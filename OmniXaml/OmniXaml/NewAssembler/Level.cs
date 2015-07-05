namespace OmniXaml.NewAssembler
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using Typing;

    public class Level
    {
        public object Instance { get; set; }
        public XamlType XamlType { get; set; }
        public XamlMember XamlMember { get; set; }
        public bool IsCollectionHolderObject { get; set; }
        public bool IsMemberHostingChildren { get; set; }
        public ICollection Collection { get; set; }
        public bool IsGetObject { get; set; }
        public bool IsProcessingValuesAsCtorArguments { get; set; }
        public Collection<ConstructionArgument> CtorArguments { get; set; }

        protected bool Equals(Level other)
        {
            return Equals(Instance, other.Instance) && Equals(XamlType, other.XamlType) && Equals(XamlMember, other.XamlMember) &&
                   IsCollectionHolderObject == other.IsCollectionHolderObject && IsMemberHostingChildren == other.IsMemberHostingChildren &&
                   IsGetObject == other.IsGetObject && IsProcessingValuesAsCtorArguments == other.IsProcessingValuesAsCtorArguments &&
                   Equals(Collection, other.Collection) && Equals(CtorArguments, other.CtorArguments);
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
                hashCode = (hashCode * 397) ^ IsProcessingValuesAsCtorArguments.GetHashCode();
                hashCode = (hashCode * 397) ^ (Collection != null ? Collection.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CtorArguments != null ? CtorArguments.GetHashCode() : 0);
                return hashCode;
            }
        }
    }

    public class ConstructionArgument
    {
        public string StringValue { get; }

        public ConstructionArgument(string stringValue)
        {
            StringValue = stringValue;
        }

        public ConstructionArgument(string stringValue, string value)
        {
            StringValue = stringValue;
            Value = value;
        }

        public object Value { get; set; }

        protected bool Equals(ConstructionArgument other)
        {
            return string.Equals(StringValue, other.StringValue) && Equals(Value, other.Value);
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
            return Equals((ConstructionArgument) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((StringValue != null ? StringValue.GetHashCode() : 0)*397) ^ (Value != null ? Value.GetHashCode() : 0);
            }
        }
    }
}