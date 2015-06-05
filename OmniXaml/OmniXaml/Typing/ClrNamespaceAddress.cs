namespace OmniXaml.Typing
{
    using System.Reflection;

    public class ClrNamespaceAddress
    {
        private readonly Assembly assembly;

        private readonly string ns;

        public ClrNamespaceAddress(Assembly assembly, string ns)
        {
            this.assembly = assembly;
            this.ns = ns;
        }

        public Assembly Assembly => assembly;

        public string Namespace => ns;
    }
}