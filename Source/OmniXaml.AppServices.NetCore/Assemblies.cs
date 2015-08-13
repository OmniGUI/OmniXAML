namespace OmniXaml.AppServices.NetCore
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public static class Assemblies
    {
        public static IEnumerable<Assembly> ReferencedAssemblies => Assembly.GetCallingAssembly().GetReferencedAssemblies().Select(Assembly.Load);
        public static IEnumerable<Assembly> AppDomainAssemblies => AppDomain.CurrentDomain.GetAssemblies();
        public static IEnumerable<Assembly> AssembliesInAppFolder
        {
            get
            {
                var entryAssembly = Assembly.GetExecutingAssembly();
                var path = entryAssembly.Location;
                var folder = Path.GetDirectoryName(path);
                var directory = new DirectoryInfo(folder);

                var assemblies = new Collection<Assembly>();

                foreach (var file in directory.GetFiles("*.dll"))
                {
                    try
                    {
                        assemblies.Add(Assembly.LoadFile(file.FullName));
                    }
                    catch
                    {                        
                    }
                }

                return assemblies;
            }
        }
    }
}