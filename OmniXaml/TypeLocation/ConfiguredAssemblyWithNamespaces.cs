namespace OmniXaml.TypeLocation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class ConfiguredAssemblyWithNamespaces
    {
        public Assembly Assembly { get; }
        public IEnumerable<string> Namespaces { get; }

        public ConfiguredAssemblyWithNamespaces(Assembly assembly, IEnumerable<string> namespaces)
        {
            this.Assembly = assembly;
            this.Namespaces = namespaces;
        }

        public Type Get(string typeName)
        {
            foreach (var ns in Namespaces)
            {
                var fullName = ns + '.' + typeName;
                var firstOrDefault = Assembly.DefinedTypes.FirstOrDefault(info => info.FullName == fullName);

                if (firstOrDefault != null)
                {
                    return firstOrDefault.AsType();
                }
            }

            return null;
        }
    }
}