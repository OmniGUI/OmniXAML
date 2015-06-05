namespace OmniXaml.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Typing;

    public class FullyConfiguredMapping
    {
        public string XamlNamespace { get; private set; }

        public FullyConfiguredMapping(AssemblyConfiguration configuredAssemblyConfiguration, string xamlNamespace)
        {
            if (configuredAssemblyConfiguration == null)
            {
                throw new ArgumentNullException(nameof(configuredAssemblyConfiguration));
            }

            var clrNamespaceAddresses = configuredAssemblyConfiguration.ClrNamespaceConfiguration.Namespaces
                .Select(ns => new ClrNamespaceAddress(configuredAssemblyConfiguration.Assembly, ns));

            Addresses = new ClrNamespaceAddressCollection(clrNamespaceAddresses);
            XamlNamespace = xamlNamespace;            
        }

        public FullyConfiguredMapping(string name, ClrNamespaceAddressCollection addresses)
        {
            Addresses = addresses;
            XamlNamespace = name;
        }

        public ClrNamespaceAddressCollection Addresses { get; private set; }
    }

    public class ClrNamespaceAddressCollection : Collection<ClrNamespaceAddress>
    {
        public ClrNamespaceAddressCollection(IEnumerable<ClrNamespaceAddress> enumerable) : base(enumerable.ToList())
        {
        }

        public Type Get(string typeName)
        {
            var types = from mapping in this.Items
                        let t = mapping.Assembly.GetType(mapping.Namespace + "." + typeName)
                        where t != null
                        select t;

            return types.FirstOrDefault();
        }
    }
}