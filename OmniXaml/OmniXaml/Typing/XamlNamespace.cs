namespace OmniXaml.Typing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class XamlNamespace
    {
        private readonly string xamlNamespaceUri;
        private readonly IXamlTypeRepository typeRepository;

        private readonly HashSet<ClrAssemblyPair> mappings = new HashSet<ClrAssemblyPair>();

        public XamlNamespace(string xamlNamespaceUri)
        {
            this.xamlNamespaceUri = xamlNamespaceUri;
        }

        public XamlNamespace(string xamlNamespaceUri, IEnumerable<ClrAssemblyPair> clrAssemblyPair) : this(xamlNamespaceUri)
        {
            mappings = new HashSet<ClrAssemblyPair>(clrAssemblyPair);
        }

        public string NamespaceUri => xamlNamespaceUri;

        public XamlType GetXamlType(string typeName)
        {
            var firstType = (from clrAssemblyPair in mappings
                let clrNs = clrAssemblyPair.Namespace
                select clrAssemblyPair.Assembly.GetType(clrNs + "." + typeName)).FirstOrDefault(
                    type => type != null);

            if (firstType == null)
            {
                return null;
            }

            return XamlType.Builder.Create(firstType, typeRepository);
        }

        public void AddMapping(ClrAssemblyPair clrAssemblyPair)
        {
            mappings.Add(clrAssemblyPair);
        }

        public Type Get(string name)
        {
            var types = from mapping in mappings
                        let t = mapping.Assembly.GetType(mapping.Namespace + "." + name)
                        where t != null
                        select t;

            return types.FirstOrDefault();
        }
    }
}