namespace OmniXaml.Typing
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Glass;
    using TypeConversion;

    public class XamlType
    {
        public XamlType(Type type, IXamlTypeRepository typeRepository, ITypeFactory typeTypeFactory, ITypeFeatureProvider featureProvider)
        {
            Guard.ThrowIfNull(type, nameof(type));
            Guard.ThrowIfNull(typeRepository, nameof(typeRepository));
            Guard.ThrowIfNull(featureProvider, nameof(featureProvider));

            TypeRepository = typeRepository;
            TypeFactory = typeTypeFactory;
            FeatureProvider = featureProvider;
            UnderlyingType = type;
            Name = type.Name;
        }

        private XamlType(Type type)
        {
            UnderlyingType = type;
        }

        public Type UnderlyingType { get; }
        public string Name { get; }

        public bool IsCollection
        {
            get
            {
                var typeInfo = UnderlyingType.GetTypeInfo();
                var isCollection = typeof (ICollection).GetTypeInfo().IsAssignableFrom(typeInfo);
                var isEnumerable = typeof (IEnumerable).GetTypeInfo().IsAssignableFrom(typeInfo);

                return UnderlyingType != typeof (string) && (isCollection || isEnumerable);
            }
        }

        public bool IsContainer => IsCollection || IsDictionary;

        public bool IsDictionary
        {
            get
            {
                var typeInfo = UnderlyingType.GetTypeInfo();
                var isDictionary = typeof (IDictionary).GetTypeInfo().IsAssignableFrom(typeInfo);
                return isDictionary;
            }
        }

        public bool NeedsConstructionParameters => UnderlyingType.GetTypeInfo().DeclaredConstructors.All(info => info.GetParameters().Any());
        // ReSharper disable once MemberCanBeProtected.Global
        public IXamlTypeRepository TypeRepository { get; }
        // ReSharper disable once MemberCanBeProtected.Global
        public ITypeFactory TypeFactory { get; }
        // ReSharper disable once MemberCanBeProtected.Global
        public ITypeFeatureProvider FeatureProvider { get; }

        public XamlMember ContentProperty
        {
            get
            {
                var propertyName = FeatureProvider.GetContentPropertyName(UnderlyingType);

                if (propertyName == null)
                {
                    return null;
                }

                var member = TypeRepository.GetXamlType(UnderlyingType).GetMember(propertyName);
                return member;
            }
        }

        public ITypeConverter TypeConverter => FeatureProvider.GetTypeConverter(UnderlyingType);

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

            return Equals((XamlType) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((UnderlyingType?.GetHashCode() ?? 0)*397);
            }
        }

        public XamlMember GetMember(string name)
        {
            return LookupMember(name);
        }

        protected virtual XamlMember LookupMember(string name)
        {
            return new XamlMember(name, this, TypeRepository, FeatureProvider);
        }

        public AttachableXamlMember GetAttachableMember(string name)
        {
            return LookupAttachableMember(name);
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

        public object CreateInstance(object[] parameters)
        {
            return TypeFactory.Create(UnderlyingType, parameters);
        }

        public static XamlType Create(Type underlyingType,
            IXamlTypeRepository xamlTypeRepository,
            ITypeFactory typeFactory,
            ITypeFeatureProvider featureProvider)
        {
            Guard.ThrowIfNull(underlyingType, nameof(xamlTypeRepository));

            return new XamlType(underlyingType, xamlTypeRepository, typeFactory, featureProvider);
        }

        public static XamlType CreateForBuiltInType(Type type)
        {
            Guard.ThrowIfNull(type, nameof(type));

            return new XamlType(type);
        }

        protected virtual AttachableXamlMember LookupAttachableMember(string name)
        {
            var getter = UnderlyingType.GetTypeInfo().GetDeclaredMethod("Get" + name);
            var setter = UnderlyingType.GetTypeInfo().GetDeclaredMethod("Set" + name);
            return TypeRepository.GetAttachableMember(name, getter, setter);            
        }

        public IEnumerable<XamlMemberBase> GetAllMembers()
        {
            var properties = UnderlyingType.GetRuntimeProperties().Where(IsValidMember).ToList();
            return properties.Select(props => GetMember(props.Name));
        }

        private static bool IsValidMember(PropertyInfo info)
        {
            var isIndexer = info.GetIndexParameters().Any();
            var hasValidSetter = info.SetMethod != null && info.SetMethod.IsPublic;
            var hasValidGetter = info.GetMethod != null && info.GetMethod.IsPublic;
            return hasValidGetter && hasValidSetter && !isIndexer;
        }

        public bool IsNameScope => LookupIsNamescope();

        protected virtual bool LookupIsNamescope()
        {
            return this.UnderlyingType is INameScope;
        }
    }
}