namespace OmniXaml.Builder
{
    using System.Collections.Generic;

    public class Namespace
    {
        public static ClrNamespaceConfiguration CreateMapFor(string ns)
        {
            return new ClrNamespaceConfiguration(new List<string>(), ns);
        }
    }
}