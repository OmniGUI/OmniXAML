namespace OmniXaml.TypeLocation
{
    public class AssemblyNameConfig
    {
        private readonly string name;

        public AssemblyNameConfig(string name)
        {
            this.name = name;
        }

        public XamlNamespace With(params ConfiguredAssemblyWithNamespaces[] assemblyAndClrs)
        {
            return new XamlNamespace(name, new AddressPack(assemblyAndClrs));
        }
    }
}