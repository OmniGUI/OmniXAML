namespace OmniXaml.Typing
{
    using System;
    using System.Collections;
    using System.Reflection;
    using Glass;

    public class XamlType
    {
        private readonly IXamlTypeRepository typeRepository;
        private readonly ITypeFactory typeFactory;

        public XamlType(Type type, IXamlTypeRepository typeRepository, ITypeFactory typeFactory)
        {
            Guard.ThrowIfNull(type, nameof(type));
            Guard.ThrowIfNull(typeRepository, nameof(typeRepository));

            this.typeRepository = typeRepository;
            this.typeFactory = typeFactory;
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

                return UnderlyingType != typeof(string) && (isCollection || isEnumerable) ;
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
            return new XamlMember(name, this, typeRepository, typeFactory, false);
        }

        private PropertyInfo GetPropertyInfo(string name)
        {
            return UnderlyingType.GetRuntimeProperty(name);
        }

        public XamlMember GetAttachableMember(string name)
        {
            return new XamlMember(name, this, typeRepository, typeFactory, true);
        }

        public override string ToString()
        {
            return "XamlType: " + Name;
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

        public object CreateInstance(object[] parameters)
        {
            return typeFactory.Create(UnderlyingType, parameters);
        }

        public static XamlType Create(Type underlyingType, IXamlTypeRepository xamlTypeRepository, ITypeFactory typeFactory)
        {
            Guard.ThrowIfNull(underlyingType, nameof(xamlTypeRepository));

            return new XamlType(underlyingType, xamlTypeRepository, typeFactory);
        }

        public static XamlType CreateForBuiltInType(Type type)
        {
            Guard.ThrowIfNull(type, nameof(type));

            return new XamlType(type, new FrameworkBuiltInTypeRepository(), null);
        }
    }
}