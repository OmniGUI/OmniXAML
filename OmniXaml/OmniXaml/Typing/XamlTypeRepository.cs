namespace OmniXaml.Typing
{
    using System;

    public class XamlTypeRepository : IXamlTypeRepository
    {
        private readonly IXamlNamespaceRegistry xamlNamespaceRegistry;

        public XamlTypeRepository(IXamlNamespaceRegistry xamlNamespaceRegistry)
        {
            this.xamlNamespaceRegistry = xamlNamespaceRegistry;
        }

        public XamlType Get(Type type)
        {            
            return XamlType.Builder.Create(type, this);
        }

        public XamlType GetByPrefix(string prefix, string typeName)
        {
            var ns = xamlNamespaceRegistry.GetNamespaceForPrefix(prefix);
            return GetWithFullAddress(new XamlTypeName(ns, typeName));
        }

        public XamlType GetWithFullAddress(XamlTypeName xamlTypeName)
        {
            var xamlNamespace = xamlNamespaceRegistry.GetXamlNamespace(xamlTypeName.Namespace);

            if (xamlNamespace == null)
            {
                return XamlType.Builder.CreateUnreachable(xamlTypeName);
            }

            var correspondingType = xamlNamespace.Get(xamlTypeName.Name);

            if (correspondingType != null)
            {
                return XamlType.Builder.Create(correspondingType, this);
            }

            return XamlType.Builder.CreateUnreachable(xamlTypeName);
        }
    }
}