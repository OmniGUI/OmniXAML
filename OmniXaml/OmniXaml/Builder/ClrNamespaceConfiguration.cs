namespace OmniXaml.Builder
{
    using System.Collections.Generic;
    using System.Reflection;

    public class ClrNamespaceConfiguration
    {
        public List<string> Namespaces { get; }

        public ClrNamespaceConfiguration(IEnumerable<string> namespaces, string ns)
        {
            Namespaces = new List<string>(namespaces) { ns };
        }

        public ClrNamespaceConfiguration(IEnumerable<string> namespaces)
        {
            Namespaces = new List<string>(namespaces);
        }

        public ClrNamespaceConfiguration And(string ns)
        {
            return new ClrNamespaceConfiguration(Namespaces, ns);
        }

        public AssemblyConfiguration FromAssembly(Assembly assembly)
        {
            return new AssemblyConfiguration(this, assembly);
        }
    }
}