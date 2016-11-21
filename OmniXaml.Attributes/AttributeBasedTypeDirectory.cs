namespace OmniXaml.DefaultLoader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Tests.Namespaces;
    using TypeLocation;

    public class AttributeBasedTypeDirectory : ITypeDirectory
    {
        private readonly ITypeDirectory inner;

        public AttributeBasedTypeDirectory(IList<Assembly> assemblies)
        {
            inner = new TypeDirectory(GetNamespaces(assemblies));
        }

        private IEnumerable<XamlNamespace> GetNamespaces(IEnumerable<Assembly> assemblies)
        {
            var xamlNamespaces = from a in assemblies
                let attributes = a.GetCustomAttributes<XmlnsDefinitionAttribute>()
                from byNamespace in attributes.GroupBy(arg => arg.XmlNamespace)
                let name = byNamespace.Key
                let clrNamespaces = byNamespace.Select(arg => arg.ClrNamespace)
                select XamlNamespace.Map(name)
                    .With(
                        Route.Assembly(a)
                            .WithNamespaces(clrNamespaces.ToArray()));

            return xamlNamespaces;
        }

        public Type GetTypeByFullAddress(Address address)
        {
            return inner.GetTypeByFullAddress(address);
        }
    }
}