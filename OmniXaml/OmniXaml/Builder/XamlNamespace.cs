namespace OmniXaml.Builder
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Attributes;

    public class XamlNamespace
    {
        private readonly AddressPack addressPack;
        private readonly string name;

        public XamlNamespace(string name, AddressPack addressPack)
        {
            this.name = name;
            this.addressPack = addressPack;
        }

        public AddressPack Addresses => addressPack;

        public string Name => name;

        public static IEnumerable<XamlNamespace> DefinedInAssemblies(IEnumerable<Assembly> assemblies)
        {
            var namespaces = from a in assemblies
                let attributes = a.GetCustomAttributes<XmlnsDefinitionAttribute>()
                from byNamespace in attributes.GroupBy(arg => arg.XmlNamespace)
                let name = byNamespace.Key
                let clrNamespaces = byNamespace.Select(arg => arg.ClrNamespace)
                select Map(name)
                    .With(
                        new[]
                        {
                            Route.Assembly(a)
                                .WithNamespaces(clrNamespaces)
                        });

            return namespaces;
        }

        public static AssemblyNameConfig Map(string root)
        {            
            return new AssemblyNameConfig(root);
        }
    }
}