namespace OmniXaml.Typing
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Glass.Core;
    using TypeConversion;

    public abstract class MutableMember : MemberBase, IDependency<Member>
    {
        protected MutableMember(string name,
            XamlType declaringType,
            ITypeRepository typeRepository,
            ITypeFeatureProvider typeFeatureProvider) : base(name)
        {
            FeatureProvider = typeFeatureProvider;
            TypeRepository = typeRepository;
            DeclaringType = declaringType;
        }

        public ITypeRepository TypeRepository { get; }
        public XamlType DeclaringType { get; }
        public IMemberValuePlugin MemberValuePlugin => LookupXamlMemberValueConnector();
        public ITypeFeatureProvider FeatureProvider { get; }

        public abstract MethodInfo Getter { get; }
        public abstract MethodInfo Setter { get; }
        public IEnumerable<Member> Dependencies => LookupDependencies();

        protected virtual IEnumerable<Member> LookupDependencies()
        {
            var metadata = FeatureProvider.GetMetadata(DeclaringType.UnderlyingType);
            if (metadata != null)
            {
                var namesOfPropsWeDependOn = metadata.GetMemberDependencies(this.Name);
                return namesOfPropsWeDependOn.Select(s => DeclaringType.GetMember(s));
            }
            else
            {
                return new List<Member>();
            }
        }

        public override string ToString()
        {
            return "Member: " + Name;
        }

        protected virtual IMemberValuePlugin LookupXamlMemberValueConnector()
        {
            return new MemberValuePlugin(this);
        }

        public void SetValue(object instance, object value, IValueContext pipeline)
        {
            MemberValuePlugin.SetValue(instance, value, pipeline);
        }

        public object GetValue(object instance)
        {
            return MemberValuePlugin.GetValue(instance);
        }

        protected bool Equals(MutableMember other)
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
            return Equals((MutableMember)obj);
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