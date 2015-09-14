namespace OmniXaml.Typing
{
    using System;
    using System.Reflection;
    using Glass;

    public class XamlTypeRepository : IXamlTypeRepository
    {
        private readonly IXamlNamespaceRegistry xamlNamespaceRegistry;
        private readonly ITypeFactory typeTypeFactory;
        private readonly ITypeFeatureProvider featureProvider;

        public XamlTypeRepository(IXamlNamespaceRegistry xamlNamespaceRegistry, ITypeFactory typeTypeFactory, ITypeFeatureProvider featureProvider)
        {
            Guard.ThrowIfNull(xamlNamespaceRegistry, nameof(xamlNamespaceRegistry));
            Guard.ThrowIfNull(typeTypeFactory, nameof(typeTypeFactory));
            Guard.ThrowIfNull(featureProvider, nameof(featureProvider));

            this.xamlNamespaceRegistry = xamlNamespaceRegistry;
            this.typeTypeFactory = typeTypeFactory;
            this.featureProvider = featureProvider;
        }

        public ITypeFeatureProvider FeatureProvider => featureProvider;

        public ITypeFactory TypeFactory => typeTypeFactory;

        public virtual XamlType GetXamlType(Type type)
        {
            Guard.ThrowIfNull(type, nameof(type));

            return XamlType.Create(type, this, TypeFactory, featureProvider);
        }

        public XamlType GetByQualifiedName(string qualifiedName)
        {
            var tuple = qualifiedName.Dicotomize(':');

            var prefix = tuple.Item2 == null ? string.Empty : tuple.Item1;
            var typeName = tuple.Item2 ?? tuple.Item1;

            return GetByPrefix(prefix, typeName);
        }

        public XamlType GetByPrefix(string prefix, string typeName)
        {
            var ns = xamlNamespaceRegistry.GetNamespaceByPrefix(prefix);

            if (ns == null)
            {
                throw new XamlParseException($"Cannot find a namespace with the prefix \"{prefix}\"");
            }

            var type = ns.Get(typeName);

            if (type == null)
            {
                throw new XamlParseException($"The type \"{{{prefix}:{typeName}}} cannot be found\"");
            }

            return GetXamlType(type);                       
        }

        public XamlType GetWithFullAddress(XamlTypeName xamlTypeName)
        {
            var ns = xamlNamespaceRegistry.GetNamespace(xamlTypeName.Namespace);

            if (ns == null)
            {
                throw new TypeNotFoundException($"Error trying to resolve a XamlType: Cannot find the namespace '{xamlTypeName.Namespace}'");
            }

            var correspondingType = ns.Get(xamlTypeName.Name);

            if (correspondingType != null)
            {
                return GetXamlType(correspondingType);
            }

            throw new TypeNotFoundException($"Error trying to resolve a XamlType: The type {xamlTypeName.Name} has not been found into the namespace '{xamlTypeName.Namespace}'");
        }

        public XamlMember GetMember(PropertyInfo propertyInfo)
        {
            var owner = GetXamlType(propertyInfo.DeclaringType);
            return new XamlMember(propertyInfo.Name, owner, this, featureProvider);
        }

        public AttachableXamlMember GetAttachableMember(string name, MethodInfo getter, MethodInfo setter)
        {
            return new AttachableXamlMember(name, getter, setter, this, featureProvider);
        }
    }
}