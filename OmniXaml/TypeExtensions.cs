namespace OmniXaml
{
    using System;
    using System.Linq;
    using System.Reflection;

    public static class TypeExtensions
    {
        public static bool IsExtension(this Type type)
        {
            return type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IMarkupExtension));
        }
    }
}