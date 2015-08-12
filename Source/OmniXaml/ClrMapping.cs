namespace OmniXaml
{
    using System.Reflection;

    public class ClrMapping
    {
        public Assembly Assembly { get; }
        public string XamlNamespace { get; }
        public string ClrNamespace { get; }

        public ClrMapping(Assembly assembly, string xamlNamespace, string clrNamespace)
        {
            Assembly = assembly;
            XamlNamespace = xamlNamespace;
            ClrNamespace = clrNamespace;            
        }
    }
}