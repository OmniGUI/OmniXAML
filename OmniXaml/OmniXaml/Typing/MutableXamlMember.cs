namespace OmniXaml.Typing
{
    using System.Reflection;

    public abstract class MutableXamlMember : XamlMemberBase
    {
        private readonly ITypeFeatureProvider typeFeatureProvider;
        public IXamlTypeRepository TypeRepository { get; }

        protected MutableXamlMember(string name,
            XamlType declaringType,
            IXamlTypeRepository xamlTypeRepository,
            ITypeFeatureProvider typeFeatureProvider) : base(name)
        {
            this.typeFeatureProvider = typeFeatureProvider;
            TypeRepository = xamlTypeRepository;
            DeclaringType = declaringType;            
        }

        public XamlType DeclaringType { get; }
        
        public IXamlMemberValuePlugin XamlMemberValueConnector => LookupXamlMemberValueConnector();

        public ITypeFeatureProvider FeatureProvider => typeFeatureProvider;

        public abstract MethodInfo Getter { get; }
        public abstract MethodInfo Setter { get; }

        public override string ToString()
        {
            return IsDirective ? "XamlDirective:" : "XamlMember: " + Name;
        }
       
        protected virtual IXamlMemberValuePlugin LookupXamlMemberValueConnector()
        {
            return new MemberValuePlugin(this);
        }

        public void SetValue(object instance, object value)
        {
            XamlMemberValueConnector.SetValue(instance, value);
        }

        public object GetValue(object instance)
        {
            return XamlMemberValueConnector.GetValue(instance);
        }

        protected bool Equals(MutableXamlMember other)
        {
            return base.Equals(other) && Equals(DeclaringType, other.DeclaringType) && Equals(XamlType, other.XamlType);
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
            return Equals((MutableXamlMember) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode*397) ^ (DeclaringType != null ? DeclaringType.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (XamlType?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}