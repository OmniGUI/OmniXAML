namespace OmniXaml.TypeLocation
{
    using System.Reflection;

    public class ConfiguredAssembly
    {
        private readonly Assembly assembly;

        public ConfiguredAssembly(Assembly assembly)
        {
            this.assembly = assembly;
        }

        public ConfiguredAssemblyWithNamespaces WithNamespaces(params string[] strings)
        {
            return new ConfiguredAssemblyWithNamespaces(assembly, strings);
        }
    }
}