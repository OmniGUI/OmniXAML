namespace OmniXaml.Builder
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reflection;
    using Assembler;
    using Typing;

    public class XamlXmlLoaderBuilder
    {
        private IEnumerable<XamlNamespace> namespaceRegistrations = new Collection<XamlNamespace>();
        private readonly IEnumerable<Assembly> lookupAssemblies = new Collection<Assembly>();
        private IEnumerable<PrefixRegistration> prefixes = new Collection<PrefixRegistration>();
        private IEnumerable<ContentPropertyDefinition> contentPropertyDefinitionDefinitions = new Collection<ContentPropertyDefinition>();
        private IEnumerable<TypeConverterRegistration> converterRegistrations = new Collection<TypeConverterRegistration>();

        public IEnumerable<XamlNamespace> NamespaceRegistrations => namespaceRegistrations;

        public IEnumerable<ContentPropertyDefinition> ContentPropertyDefinitions => contentPropertyDefinitionDefinitions;

        public XamlXmlLoader Build()
        {
            var wiringContextBuilder = new WiringContextBuilder();

            wiringContextBuilder
                .WithContentPropertiesFromAssemblies(lookupAssemblies)
                .WithNsPrefixes(prefixes)
                .WithConverters(Converters.FromAssemblies(lookupAssemblies));


            RegisterNamespaces(wiringContextBuilder);
            RegisterContentProperties(wiringContextBuilder);

            var wiringContext = wiringContextBuilder.Build();
            var assembler = new ObjectAssembler(wiringContext);
            return new XamlXmlLoader(assembler, wiringContext);
        }

        private void RegisterContentProperties(WiringContextBuilder wiringContextBuilder)
        {
            foreach (var contentPropertyDefinition in ContentPropertyDefinitions)
            {
                wiringContextBuilder.WithContentProperty(contentPropertyDefinition);
            }
        }

        private void RegisterNamespaces(WiringContextBuilder wiringContextBuilder)
        {
            wiringContextBuilder.WithNamespaces(NamespaceRegistrations);
        }

        public XamlXmlLoaderBuilder WithNsPrefixes(List<PrefixRegistration> prefixRegistrations)
        {
            prefixes = prefixRegistrations;
            return this;
        }

        public void WithContentProperties(IEnumerable<ContentPropertyDefinition> definitions)
        {
            contentPropertyDefinitionDefinitions = definitions;
        }

        public XamlXmlLoaderBuilder WithNamespaces(IEnumerable<XamlNamespace> xamlNamespaces)
        {
            namespaceRegistrations = xamlNamespaces;
            return this;
        }

        public XamlXmlLoaderBuilder WithConverters(IEnumerable<TypeConverterRegistration> converterRegistrations)
        {
            this.converterRegistrations = converterRegistrations;
            return this;
        }
    }
}