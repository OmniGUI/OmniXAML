namespace OmniXaml.Builder
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Attributes;

    public static class ContentProperties
    {
        public static IEnumerable<ContentPropertyDefinition> DefinedInAssemblies(IEnumerable<Assembly> assemblies)
        {
            return from assembly in assemblies
                let allTypes = assembly.DefinedTypes
                from typeInfo in allTypes
                let attribute = typeInfo.GetCustomAttribute<ContentPropertyAttribute>()
                where attribute != null
                select new ContentPropertyDefinition(typeInfo.AsType(), attribute.Name);

        }
    }
}