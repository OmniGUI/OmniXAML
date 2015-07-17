namespace OmniXaml
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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
        private IEnumerable<TypeConverterRegistration> converterRegistrations = new Collection<TypeConverterRegistration>();

        public WiringContextBuilder()
        {                        
            contentPropertyProvider = new ContentPropertyProvider();            
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

        public virtual WiringContext Build()
        {
            var typingCore = typingCoreBuilder.Build();

            if (converterProvider == null)
            {
                converterProvider = new TypeConverterProvider();
                foreach (var converterRegistration in converterRegistrations)
                {
                    converterProvider.RegisterConverter(converterRegistration);
                }
            }
            
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

        public WiringContextBuilder WithConverters(IEnumerable<TypeConverterRegistration> converterRegistrations)
        {
            this.converterRegistrations = converterRegistrations;
            return this;
        }

        public WiringContextBuilder WithTypeFactory(ITypeFactory typeFactory)
        {
            typingCoreBuilder.WithTypeFactory(typeFactory);
            return this;
        }
    }
}