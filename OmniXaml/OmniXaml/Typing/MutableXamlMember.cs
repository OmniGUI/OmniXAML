namespace OmniXaml.Typing
{
    using System.Linq;
    using System.Reflection;

    public abstract class MutableXamlMember : XamlMemberBase
    {
        private readonly ITypeFeatureProvider typeFeatureProvider;
        public IXamlTypeRepository TypeRepository { get; }
        public ITypeFactory TypeFactory { get; }

        protected MutableXamlMember(string name, XamlType owner, IXamlTypeRepository xamlTypeRepository, ITypeFactory typeFactory, ITypeFeatureProvider featureProvider) : base(name)
        {
            typeFeatureProvider = featureProvider;
            TypeRepository = xamlTypeRepository;
            TypeFactory = typeFactory;

            DeclaringType = owner;
            XamlType = LookupType();
        }
        
        public XamlType DeclaringType { get; }
        
        public IXamlMemberValuePlugin XamlMemberValueConnector => LookupXamlMemberValueConnector();

        public ITypeFeatureProvider FeatureProvider => typeFeatureProvider;

        protected abstract XamlType LookupType();             

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