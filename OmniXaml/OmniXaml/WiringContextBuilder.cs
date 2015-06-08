namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Builder;
    using Catalogs;
    using TypeConversion;
    using Typing;

    public class WiringContextBuilder
    {
        private IContentPropertyProvider contentPropertyProvider;
        private ITypeConverterProvider converterProvider;

        private readonly TypeContextBuilder typingCoreBuilder = new TypeContextBuilder();

        public WiringContextBuilder()
        {            
            converterProvider = new TypeConverterProvider();
            contentPropertyProvider = new ContentPropertyProvider();            
        }

        public WiringContextBuilder AddNsForThisType(string prefix, string xamlNs, Type referenceType)
        {
            typingCoreBuilder.AddNsForThisType(prefix, xamlNs, referenceType);
            return this;
        }

        public WiringContextBuilder WithXamlNs(string xamlNs, Assembly assembly, string clrNs)
        {
            return this;
        }

        public WiringContextBuilder WithNsPrefix(string prefix, string ns)
        {
            typingCoreBuilder.WithNsPrefix(prefix, ns);
            return this;
        }


        public WiringContextBuilder WithContentPropertiesFromAssemblies(IEnumerable<Assembly> assemblies)
        {
            contentPropertyProvider.AddCatalog(new AttributeBasedContentPropertyCatalog(assemblies));
            return this;
        }

        public WiringContextBuilder WithContentPropertyProvider(IContentPropertyProvider provider)
        {
            contentPropertyProvider = provider;
            return this;
        }

        public WiringContextBuilder WithConverterProvider(ITypeConverterProvider provider)
        {
            converterProvider = provider;
            return this;
        }

        public WiringContext Build()
        {
            var typingCore = typingCoreBuilder.Build();
            
            return new WiringContext(typingCore, contentPropertyProvider, converterProvider);
        }

        public WiringContextBuilder WithContentProperty(ContentPropertyDefinition contentPropertyDefinition)
        {
            contentPropertyProvider.Add(contentPropertyDefinition);
            return this;
        }

        public WiringContextBuilder WithNamespaces(IEnumerable<XamlNamespace> namespaceRegistrations)
        {
            typingCoreBuilder.WithNamespaces(namespaceRegistrations);
            return this;
        }

        public WiringContextBuilder WithNsPrefixes(IEnumerable<PrefixRegistration> prefixRegistrations)
        {
            typingCoreBuilder.WithNsPrefixes(prefixRegistrations);
            return this;
        }

        public WiringContextBuilder WithContentProperties(IEnumerable<ContentPropertyDefinition> contentProperties)
        {
            foreach (var contentPropertyDefinition in contentProperties)
            {
                contentPropertyProvider.Add(contentPropertyDefinition);
            }
            return this;
        }
    }
}