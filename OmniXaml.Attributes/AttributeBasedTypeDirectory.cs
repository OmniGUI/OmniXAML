namespace OmniXaml.DefaultLoader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using TypeLocation;

    public class AttributeBasedTypeDirectory : ITypeDirectory
    {
        private readonly TypeDirectory inner;

        public AttributeBasedTypeDirectory(IList<Assembly> assemblies)
        {
            inner = new TypeDirectory();
            RegisterAssemblies(assemblies);
        }

        private void RegisterAssemblies(IList<Assembly> assemblies)
        {
            RegisterPrefixes(assemblies);
            RegisterNamespaces(assemblies);
        }

        private void RegisterNamespaces(IEnumerable<Assembly> assemblies)
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

            foreach (var ns in xamlNamespaces)
            {
                inner.AddNamespace(ns);
            }
        }

        private void RegisterPrefixes(IEnumerable<Assembly> assemblies)
        {
            var prefixRegistrarions = from ass in assemblies
                from attr in ass.GetCustomAttributes<XmlnsPrefixAttribute>()
                select new PrefixRegistration(attr.Prefix, attr.XmlNamespace);

            foreach (var registration in prefixRegistrarions)
            {
                inner.RegisterPrefix(registration);
            }
        }

        public IEnumerable<PrefixRegistration> RegisteredPrefixes => inner.RegisteredPrefixes;


        public Namespace GetNamespaceByPrefix(string prefix)
        {
            return inner.GetNamespaceByPrefix(prefix);
        }

        public void RegisterPrefix(PrefixRegistration prefixRegistration)
        {
            inner.RegisterPrefix(prefixRegistration);
        }

        public XamlNamespace GetXamlNamespace(string namespaceName)
        {
            return inner.GetXamlNamespace(namespaceName);
        }

        public Namespace GetNamespace(string name)
        {
            return inner.GetNamespace(name);
        }

        
        public void AddNamespace(XamlNamespace xamlNamespace)
        {
            inner.AddNamespace(xamlNamespace);
        }
        
        public Type GetTypeByPrefix(string prefix, string typeName)
        {
            return inner.GetTypeByPrefix(prefix, typeName);
        }

        public Type GetTypeByFullAddres(Address address)
        {
            return inner.GetTypeByFullAddres(address);
        }

        public Type GetByPrefixedName(string prefixedName)
        {
            return inner.GetByPrefixedName(prefixedName);
        }
    }
}