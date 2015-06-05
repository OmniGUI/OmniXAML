namespace OmniXaml.Builder
{
    using System.Collections.Generic;
    using System.Reflection;
    using Assembler;
    using Typing;

    public class XamlXmlLoaderBuilder
    {
        private List<FullyConfiguredMapping> namespaceRegistration = new List<FullyConfiguredMapping>();
        private IEnumerable<Assembly> lookupAssemblies = new List<Assembly>();
        private List<PrefixRegistration> prefixes = new List<PrefixRegistration>();
        private IEnumerable<Assembly> assembliesForNamespaces;

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
                foreach (var mapping in namespaceRegistration)
                {
                    foreach (var address in mapping.Addresses)
                    {
                        wiringContextBuilder.WithXamlNs(mapping.XamlNamespace, address.Assembly, address.Namespace);
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

        internal XamlXmlLoaderBuilder WithNamespaces(List<FullyConfiguredMapping> namespaceRegistration)
        {
            this.namespaceRegistration = namespaceRegistration;
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
    }
}