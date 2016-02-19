namespace OmniXaml.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class ConfiguredAssemblyWithNamespaces
    {
        private readonly Assembly assembly;
        private readonly IEnumerable<string> strings;

        public ConfiguredAssemblyWithNamespaces(Assembly assembly, IEnumerable<string> strings)
        {
            this.assembly = assembly;
            this.strings = strings;
        }

        public Type Get(string typeName)
        {
            foreach (var ns in strings)
            {
                var fullName = ns + '.' + typeName;
                var firstOrDefault = assembly.DefinedTypes.FirstOrDefault(info => info.FullName == fullName);

                if (firstOrDefault != null)
                {
                    return firstOrDefault.AsType();
                }
            }

            return null;
        }
    }
}