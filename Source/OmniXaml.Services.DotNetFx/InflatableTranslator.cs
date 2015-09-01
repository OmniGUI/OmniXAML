namespace OmniXaml.Services.DotNetFx
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public class InflatableTranslator : IInflatableTranslator
    {
        public Stream GetInflationSourceStream(Type type)
        {
            var uri = GetUriFor(type);
            var absoluteUri = new Uri(Assembly.GetExecutingAssembly().Location, UriKind.Absolute);
            var finalUri = new Uri(absoluteUri, uri);
            return new FileStream(finalUri.LocalPath, FileMode.Open);
        }

        public Type GetTypeFor(Uri uri)
        {
            var withExt = uri.OriginalString;
            var lastSlash = withExt.LastIndexOf("/", StringComparison.Ordinal);
            var innerNs = withExt.Substring(0, lastSlash);
            var fileName = withExt.Substring(lastSlash + 1, withExt.Length - lastSlash - 1);

            var className = fileName.Substring(0, fileName.LastIndexOf('.'));


            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            var type = from assembly in assemblies
                let t = assembly.GetType(GetName(assembly, innerNs, className))
                where t != null
                select t;

            return type.First();
        }

        private static Uri GetUriFor(Type type)
        {
            if (type.Namespace != null)
            {
                var toRemove = type.Assembly.GetName().Name;
                var substracted = toRemove.Length < type.Namespace.Length ? type.Namespace.Remove(0, toRemove.Length + 1) : "";
                var replace = substracted.Replace('.', Path.PathSeparator);
                if (replace != string.Empty)
                {
                    replace = replace + "/";
                }
                return new Uri(replace + type.Name + ".xaml", UriKind.Relative);
            }

            return null;
        }

        private static string GetName(Assembly assembly, string innerNs, string className)
        {
            var ns = assembly.GetName().Name + "." + innerNs;
            var fullLocator = ns + "." + className;
            return fullLocator;
        }
    }
}