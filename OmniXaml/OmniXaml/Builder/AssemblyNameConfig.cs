namespace OmniXaml.Builder
{
    using System.Collections.Generic;

    public class AssemblyNameConfig
    {
        private readonly string name;

        public AssemblyNameConfig(string name)
        {
            this.name = name;
        }

        public XamlNamespace With(IEnumerable<ConfiguredAssemblyWithNamespaces> assemblyAndClrs)
        {
            return new XamlNamespace(name, new AddressPack(assemblyAndClrs));
        }
    }
}