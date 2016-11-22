namespace OmniXaml.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Attributes;
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
            var routes = from a in assemblies
                let attributes = a.GetCustomAttributes<XmlnsDefinitionAttribute>()
                from byNamespace in attributes.GroupBy(arg => arg.XmlNamespace)
                let name = byNamespace.Key
                let clrNamespaces = byNamespace.Select(arg => arg.ClrNamespace)
                let configuredAssemblyWithNamespaces = Route.Assembly(a).WithNamespaces(clrNamespaces.ToArray())
                select new {Ns = name, configuredAssemblyWithNamespaces};

            var nss = from route in routes
                group route by route.Ns
                into g
                let ns = g.Select(arg => arg.configuredAssemblyWithNamespaces).ToArray()  
                select XamlNamespace.Map(g.Key).With(ns);
                                  
            return nss;
        }

        public Type GetTypeByFullAddress(Address address)
        {
            return inner.GetTypeByFullAddress(address);
        }
    }
}