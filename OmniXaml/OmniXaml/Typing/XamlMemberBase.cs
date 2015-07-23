namespace OmniXaml.Typing
{
    public abstract class XamlMemberBase
    {
        public string Name { get; }
        public XamlType XamlType { get; protected set; }
        public abstract bool IsDirective { get;  }
        public abstract bool IsAttachable { get; }

        protected XamlMemberBase(string name)
        {
            this.Name = name;
        }

        protected bool Equals(XamlMemberBase other)
        {
            return string.Equals(Name, other.Name) && XamlType.Equals(other.XamlType);
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
            return Equals((XamlMemberBase) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Name.GetHashCode()*397) ^ XamlType.GetHashCode();
            }
        }
    }
}