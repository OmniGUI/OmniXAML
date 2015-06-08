namespace OmniXaml.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
    using Assembler;
    using Attributes;
    using Typing;

    public class XamlXmlLoaderBuilder
    {
        private IEnumerable<XamlNamespace> namespaceRegistrations = new Collection<XamlNamespace>();
        private readonly IEnumerable<Assembly> lookupAssemblies = new Collection<Assembly>();
        private IEnumerable<PrefixRegistration> prefixes = new Collection<PrefixRegistration>();
        private IEnumerable<ContentPropertyDefinition> contentPropertyDefinitionDefinitions = new Collection<ContentPropertyDefinition>();

        public IEnumerable<XamlNamespace> NamespaceRegistrations => namespaceRegistrations;

        public IEnumerable<ContentPropertyDefinition> ContentPropertyDefinitions => contentPropertyDefinitionDefinitions;

        public XamlXmlLoader Build()
        {
            var wiringContextBuilder = new WiringContextBuilder();

            wiringContextBuilder.WithContentPropertiesFromAssemblies(lookupAssemblies);
            wiringContextBuilder.WithNsPrefixes(prefixes);

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

        internal XamlXmlLoaderBuilder WithNamespaces(IEnumerable<XamlNamespace> namespaces)
        {
            namespaceRegistrations = namespaces;
            return this;
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

    }

    public static class ContentProperties
    {
        public static IEnumerable<ContentPropertyDefinition> DefinedInAssemblies(IEnumerable<Assembly> assemblies)
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
        private readonly Type ownerType;
        private readonly string name;

        public ContentPropertyDefinition(Type ownerType, string name)
        {
            this.ownerType = ownerType;
            this.name = name;
        }

        public Type OwnerType => ownerType;

        public string Name => name;
    }
}