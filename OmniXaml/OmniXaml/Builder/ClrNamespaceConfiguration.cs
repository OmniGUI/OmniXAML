namespace OmniXaml.Builder
{
    using System.Collections.Generic;
    using System.Reflection;

    public class ClrNamespaceConfiguration
    {
        public List<string> Namespaces { get; }

        public ClrNamespaceConfiguration(IEnumerable<string> list, string ns)
        {
            Namespaces = new List<string>(list) { ns };
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