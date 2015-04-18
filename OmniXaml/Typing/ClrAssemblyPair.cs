namespace OmniXaml.Typing
{
    using System.Reflection;

    public class ClrAssemblyPair
    {
        private readonly Assembly assembly;

        private readonly string ns;

        public ClrAssemblyPair(Assembly assembly, string ns)
        {
            this.assembly = assembly;
            this.ns = ns;
        }

        public Assembly Assembly => assembly;

        public string Namespace => ns;
    }
}