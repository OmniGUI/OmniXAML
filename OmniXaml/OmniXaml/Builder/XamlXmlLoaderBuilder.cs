namespace OmniXaml.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Assembler;
    using Attributes;
    using Typing;

    public class XamlXmlLoaderBuilder
    {
        private IEnumerable<XamlNamespace> namespaceRegistrations = new List<XamlNamespace>();
        private IEnumerable<Assembly> lookupAssemblies = new List<Assembly>();
        private List<PrefixRegistration> prefixes = new List<PrefixRegistration>();
        private IEnumerable<Assembly> assembliesForNamespaces;
        private IEnumerable<ContentPropertyDefinition> contentContentProperties;

        public IEnumerable<XamlNamespace> NamespaceRegistrations => namespaceRegistrations;

        public IEnumerable<ContentPropertyDefinition> ContentProperties => contentContentProperties;

        public XamlXmlLoader Build()
        {
            var wiringContextBuilder = new WiringContextBuilder();

            wiringContextBuilder.WithContentPropertiesFromAssemblies(lookupAssemblies);

            foreach (var prefixRegistration in prefixes)
            {
                wiringContextBuilder.WithNsPrefix(prefixRegistration.Prefix, prefixRegistration.Ns);
            }

            if (assembliesForNamespaces != null)
            {
                wiringContextBuilder.WithNamespacesProvidedByAttributes(assembliesForNamespaces);
            }
            else
            {
                foreach (var mapping in namespaceRegistrations)
                {
                    foreach (var address in mapping.Addresses)
                    {
                        wiringContextBuilder.WithXamlNs(mapping.Name, address.Assembly, address.Namespace);
                    }
                }
            }

            var wiringContext = wiringContextBuilder.Build();
            var assembler = new ObjectAssembler(wiringContext);
            return new XamlXmlLoader(assembler, wiringContext);
        }

        internal XamlXmlLoaderBuilder WithContentProperties(IEnumerable<Assembly> lookupAssemblies)
        {
            this.lookupAssemblies = lookupAssemblies;
            return this;
        }

        internal XamlXmlLoaderBuilder WithNamespaces(IEnumerable<XamlNamespace> namespaceRegistration)
        {
            this.namespaceRegistrations = namespaceRegistration;
            return this;
        }

        internal XamlXmlLoaderBuilder WithNamespacesProvidedByAttributes(IEnumerable<Assembly> assembliesForNamespaces)
        {
            this.assembliesForNamespaces = assembliesForNamespaces;
            return this;
        }

        public XamlXmlLoaderBuilder WithNsPrefixes(List<PrefixRegistration> prefixRegistrations)
        {
            this.prefixes = prefixRegistrations;
            return this;
        }

        public void WithContentProperties(IEnumerable<ContentPropertyDefinition> definedInAssemblies)
        {

            this.contentContentProperties = definedInAssemblies;
        }

    }

    public static class ContentProperties
    {
        public static IEnumerable<ContentPropertyDefinition> DefinedInAssemblies(Assembly[] assemblies)
        {
            return from assembly in assemblies
                   let allTypes = assembly.DefinedTypes
                   from typeInfo in allTypes
                   let attribute = typeInfo.GetCustomAttribute<ContentPropertyAttribute>()
                   where attribute != null
                   select new ContentPropertyDefinition(typeInfo.AsType(), attribute.Name);

        }
    }

    public class ContentPropertyDefinition
    {
        private readonly Type type;
        private readonly string name;

        public ContentPropertyDefinition(Type type, string name)
        {
            this.type = type;
            this.name = name;
        }

        public Type Type => type;

        public string Name => name;
    }
}