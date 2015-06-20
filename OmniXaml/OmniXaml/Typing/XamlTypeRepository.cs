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

        public virtual XamlType GetXamlType(Type type)
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
            return new XamlMember(propertyInfo.Name, owner, this, false);
        }

        public XamlMember GetAttachableMember(string name, MethodInfo getter, MethodInfo setter)
        {
            throw new NotImplementedException();
        }
    }
}