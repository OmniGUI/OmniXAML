namespace OmniXaml.Builder
{
    using System.Collections.Generic;
    using System.Reflection;

    public class ConfiguredAssembly
    {
        private readonly Assembly assembly;

        public ConfiguredAssembly(Assembly assembly)
        {
            this.assembly = assembly;
        }

        public ConfiguredAssemblyWithNamespaces WithNamespaces(IEnumerable<string> strings)
        {
            return new ConfiguredAssemblyWithNamespaces(assembly, strings);
        }
    }
}