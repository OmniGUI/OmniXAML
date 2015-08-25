namespace OmniXaml.AppServices.NetCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Types
    {
        public static IEnumerable<Type> FromCurrentAppDomain => Assemblies.AppDomainAssemblies.SelectMany(assembly => assembly.ExportedTypes);
        public static IEnumerable<Type> FromReferencedAssemblies => Assemblies.ReferencedAssemblies.SelectMany(assembly => assembly.ExportedTypes);
        public static IEnumerable<Type> FromAppFolder => Assemblies.AssembliesInAppFolder.SelectMany(assembly => assembly.ExportedTypes);
    }
}