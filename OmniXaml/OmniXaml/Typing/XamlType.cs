namespace OmniXaml.Typing
{
    using System;
    using System.Collections;
    using System.Reflection;
    using Glass;

    public class XamlType
    {
        private readonly IXamlTypeRepository typeRepository;

        public XamlType(Type type, IXamlTypeRepository typeRepository)
        {
            Guard.ThrowIfNull(type, nameof(type));
            Guard.ThrowIfNull(typeRepository, nameof(typeRepository));

            this.typeRepository = typeRepository;
            UnderlyingType = type;
            Name = type.Name;
        }

        public Type UnderlyingType { get; }

        public string Name { get; private set; }

        public XamlType BaseType { get; private set; }

        public bool IsCollection
        {
            get
            {
                var typeInfo = UnderlyingType.GetTypeInfo();
                var isCollection = typeof (ICollection).GetTypeInfo().IsAssignableFrom(typeInfo);
                var isEnumerable = typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(typeInfo);

                return isCollection || isEnumerable;
            }
        }

        public bool IsContainer => IsCollection || IsDictionary;

        public bool IsDictionary
        {
            get
            {
                var typeInfo = UnderlyingType.GetTypeInfo();
                var isDictionary = typeof(IDictionary).GetTypeInfo().IsAssignableFrom(typeInfo);
                return isDictionary;
            }
        }

        protected bool Equals(XamlType other)
        {
            return UnderlyingType == other.UnderlyingType;
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

            return Equals((XamlType)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((UnderlyingType?.GetHashCode() ?? 0) * 397);
            }
        }

        public XamlMember GetMember(string name)
        {
            return LookupMember(name);
        }

        protected virtual XamlMember LookupMember(string name)
        {         
            return XamlMember.Builder.Create(name, this, typeRepository);
        }

        private PropertyInfo GetPropertyInfo(string name)
        {
            return UnderlyingType.GetRuntimeProperty(name);
        }

        public XamlMember GetAttachableMember(string name)
        {
            return XamlMember.Builder.CreateAttached(name, this, typeRepository);
        }

        public override string ToString()
        {
            return "XamlType: " + Name;
        }

        public static class Builder
        {
            public static XamlType Create(Type underlyingType, IXamlTypeRepository mother)
            {
                Guard.ThrowIfNull(underlyingType, nameof(mother));

                return new XamlType(underlyingType, mother);
            }

            public static XamlType CreateForBuiltInType(Type type)
            {
                Guard.ThrowIfNull(type, nameof(type));

                return new XamlType(type, new FrameworkBuiltInTypeRepository());
            }
        }

        public bool CanAssignTo(XamlType type)
        {
            var otherUnderlyingType = type.UnderlyingType.GetTypeInfo();
            return otherUnderlyingType.IsAssignableFrom(UnderlyingType.GetTypeInfo());
        }

        protected virtual XamlMember LookupAttachableMember(string name)
        {
            throw new NotImplementedException();
        }
    }
}