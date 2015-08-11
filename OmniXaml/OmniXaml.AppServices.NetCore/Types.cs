namespace OmniXaml.AppServices.NetCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Types
    {
        public static IEnumerable<Type> FromCurrentAddDomain => AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.ExportedTypes);
    }
}