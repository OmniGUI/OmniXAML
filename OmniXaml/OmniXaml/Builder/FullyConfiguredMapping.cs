namespace OmniXaml.Builder
{
    using System.Collections.Generic;

    public class FullyConfiguredMapping
    {
        public AssemblyConfiguration AssemblyConfiguration { get; }
        public string XamlNamespace { get; }
        public IEnumerable<string> Namespaces => AssemblyConfiguration.ClrNamespaceConfiguration.Namespaces;

        public FullyConfiguredMapping(AssemblyConfiguration configuredAssemblyConfiguration, string xamlNamespace)
        {
            AssemblyConfiguration = configuredAssemblyConfiguration;
            XamlNamespace = xamlNamespace;
        }
    }
}