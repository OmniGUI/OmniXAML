namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Catalogs;
    using TypeConversion;

    public class WiringContextBuilder
    {
        private IContentPropertyProvider contentPropertyProvider;
        private ITypeConverterProvider converterProvider;

        private readonly TypeContextBuilder typingCoreBuilder = new TypeContextBuilder();
        private IEnumerable<Assembly> assembliesForNamespaces;

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
            typingCoreBuilder.WithXamlNs(xamlNs, assembly, clrNs);
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

            if (assembliesForNamespaces!=null)
            {
                typingCore.AddCatalog(new AttributeBasedClrMappingCatalog(assembliesForNamespaces));
            }

            return new WiringContext(typingCore, contentPropertyProvider, converterProvider);
        }

        public WiringContextBuilder WithNamespacesProvidedByAttributes(IEnumerable<Assembly> assembliesForNamespaces)
        {
            this.assembliesForNamespaces = assembliesForNamespaces;
            return this;
        }
    }
}