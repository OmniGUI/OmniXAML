namespace OmniXaml.Typing
{
    using System.Reflection;

    internal class ClrNamespace
    {
        public Assembly TargetAssembly { get; private set; }

        public string Namespace { get; private set; }

        public ClrNamespace(Assembly targetAssembly, string ns)
        {
            TargetAssembly = targetAssembly;
            Namespace = ns;
        }     
    }
}