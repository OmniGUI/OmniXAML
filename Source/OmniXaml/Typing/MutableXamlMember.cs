namespace OmniXaml.Typing
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Glass;

    public abstract class MutableXamlMember : XamlMemberBase, IDependency<XamlMember>
    {
        protected MutableXamlMember(string name,
            XamlType declaringType,
            IXamlTypeRepository xamlTypeRepository,
            ITypeFeatureProvider typeFeatureProvider) : base(name)
        {
            FeatureProvider = typeFeatureProvider;
            TypeRepository = xamlTypeRepository;
            DeclaringType = declaringType;
        }

        public IXamlTypeRepository TypeRepository { get; }
        public XamlType DeclaringType { get; }
        public IXamlMemberValuePlugin XamlMemberValueConnector => LookupXamlMemberValueConnector();
        public ITypeFeatureProvider FeatureProvider { get; }

        public abstract MethodInfo Getter { get; }
        public abstract MethodInfo Setter { get; }
        public IEnumerable<XamlMember> Dependencies => LookupDependencies();

        protected virtual IEnumerable<XamlMember> LookupDependencies()
        {
            var underlyingType = DeclaringType.UnderlyingType;
            var metadata = TypeRepository.GetMetadata(underlyingType);
            if (metadata != null)
            {
                var namesOfPropsWeDependOn = metadata.GetMemberDependencies(this.Name);
                return namesOfPropsWeDependOn.Select(s => DeclaringType.GetMember(s));
            }
            else
            {
                return new List<XamlMember>();
            }
        }

        public override string ToString()
        {
            return "XamlMember: " + Name;
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
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((MutableXamlMember)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (DeclaringType != null ? DeclaringType.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (XamlType?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}