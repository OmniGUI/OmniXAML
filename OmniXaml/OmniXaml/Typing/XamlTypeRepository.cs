namespace OmniXaml.Typing
{
    using System;
    using System.Reflection;
    using Glass;

    public class XamlTypeRepository : IXamlTypeRepository
    {
        private readonly IXamlNamespaceRegistry xamlNamespaceRegistry;

        public XamlTypeRepository(IXamlNamespaceRegistry xamlNamespaceRegistry)
        {
            this.xamlNamespaceRegistry = xamlNamespaceRegistry;
        }

        public XamlType Get(Type type)
        {
            Guard.ThrowIfNull(type, nameof(type));

            return XamlType.Builder.Create(type, this);
        }

        public XamlType GetByPrefix(string prefix, string typeName)
        {
            var ns = xamlNamespaceRegistry.GetNamespaceByPrefix(prefix);
            var type = ns.Get(typeName);

            if (type == null)
            {
                throw new TypeNotFoundException($"The type \"{{{prefix}:{typeName}}} cannot be found\"");
            }

            return Get(type);                       
        }

        public XamlType GetWithFullAddress(XamlTypeName xamlTypeName)
        {
            var ns = xamlNamespaceRegistry.GetNamespace(xamlTypeName.Namespace);

            if (ns == null)
            {
                return XamlType.Builder.CreateUnreachable(xamlTypeName);
            }

            var correspondingType = ns.Get(xamlTypeName.Name);

            if (correspondingType != null)
            {
                return XamlType.Builder.Create(correspondingType, this);
            }

            return XamlType.Builder.CreateUnreachable(xamlTypeName);
        }

        public XamlMember GetMember(PropertyInfo propertyInfo)
        {
            var owner = Get(propertyInfo.DeclaringType);
            return new XamlMember(propertyInfo.Name, owner, this, false);
        }
    }
}