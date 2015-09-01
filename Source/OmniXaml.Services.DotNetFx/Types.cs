namespace OmniXaml.Services.DotNetFx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class Types
    {
        public static IEnumerable<Type> FromCurrentAppDomain => GetTypesFromNonDynamicAssemblies(Assemblies.AppDomainAssemblies);

        public static IEnumerable<Type> FromReferencedAssemblies => GetTypesFromNonDynamicAssemblies(Assemblies.ReferencedAssemblies);

        public static IEnumerable<Type> FromAppFolder => GetTypesFromNonDynamicAssemblies(Assemblies.AssembliesInAppFolder);

        private static IEnumerable<Type> GetTypesFromNonDynamicAssemblies(IEnumerable<Assembly> assemblies)
        {
            return assemblies.Where(assembly => !assembly.IsDynamic).SelectMany(assembly => assembly.ExportedTypes);
        }
    }
}