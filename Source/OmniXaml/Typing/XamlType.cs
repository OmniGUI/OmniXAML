namespace OmniXaml.Typing
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Glass.Core;
    using TypeConversion;

    public class XamlType
    {
        public XamlType(Type type, ITypeRepository typeRepository, ITypeFactory typeTypeFactory, ITypeFeatureProvider featureProvider)
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
                var isCollection = typeof(ICollection).GetTypeInfo().IsAssignableFrom(typeInfo);
                var isEnumerable = typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(typeInfo);

                return UnderlyingType != typeof(string) && (isCollection || isEnumerable);
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

        public bool NeedsConstructionParameters => UnderlyingType.GetTypeInfo().DeclaredConstructors.All(info => info.GetParameters().Any());
        // ReSharper disable once MemberCanBeProtected.Global
        public ITypeRepository TypeRepository { get; }
        // ReSharper disable once MemberCanBeProtected.Global
        public ITypeFactory TypeFactory { get; }
        // ReSharper disable once MemberCanBeProtected.Global
        public ITypeFeatureProvider FeatureProvider { get; }

        public Member ContentProperty
        {
            get
            {
                var propertyName = FeatureProvider.GetContentPropertyName(UnderlyingType);

                if (propertyName == null)
                {
                    return null;
                }

                var member = TypeRepository.GetByType(UnderlyingType).GetMember(propertyName);
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

            return Equals((XamlType)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((UnderlyingType?.GetHashCode() ?? 0) * 397);
            }
        }

        public Member GetMember(string name)
        {
            return LookupMember(name);
        }

        protected virtual Member LookupMember(string name)
        {
            return new Member(name, this, TypeRepository, FeatureProvider);
        }

        public AttachableMember GetAttachableMember(string name)
        {
            return LookupAttachableMember(name);
        }

        public override string ToString()
        {
            return "XamlType: " + Name;
        }

        public object CreateInstance(object[] parameters)
        {
            return TypeFactory.Create(UnderlyingType, parameters);
        }

        public static XamlType Create(Type underlyingType,
            ITypeRepository typeRepository,
            ITypeFactory typeFactory,
            ITypeFeatureProvider featureProvider)
        {
            Guard.ThrowIfNull(underlyingType, nameof(typeRepository));

            return new XamlType(underlyingType, typeRepository, typeFactory, featureProvider);
        }

        public static XamlType CreateForBuiltInType(Type type)
        {
            Guard.ThrowIfNull(type, nameof(type));

            return new XamlType(type);
        }

        protected virtual AttachableMember LookupAttachableMember(string name)
        {
            var getter = UnderlyingType.GetTypeInfo().GetDeclaredMethod("Get" + name);
            var setter = UnderlyingType.GetTypeInfo().GetDeclaredMethod("Set" + name);
            return TypeRepository.GetAttachableMember(name, getter, setter);
        }

        public IEnumerable<MemberBase> GetAllMembers()
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

        public virtual INameScope GetNamescope(object instance)
        {
            return instance as INameScope;
        }

        public Member RuntimeNamePropertyMember
        {
            get
            {
                if (TypeRepository == null)
                {
                    return null;
                }

                var metadata = FeatureProvider.GetMetadata(UnderlyingType);

                var runtimeNameProperty = metadata?.RuntimePropertyName;

                if (runtimeNameProperty == null)
                {
                    return null;
                }

                var propInfo = UnderlyingType.GetRuntimeProperty(runtimeNameProperty);
                if (propInfo == null)
                {
                    return null;
                }

                return GetMember(runtimeNameProperty);
            }
        }

        public virtual void BeforeInstanceSetup(object instance)
        {
        }

        public virtual void AfterInstanceSetup(object instance)
        {
        }

        public virtual void AfterAssociationToParent(object instance)
        {
        }
    }
}