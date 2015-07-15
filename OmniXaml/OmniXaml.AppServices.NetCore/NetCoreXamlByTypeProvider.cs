namespace OmniXaml.AppServices.NetCore
{
    using System;
    using System.IO;

    public class NetCoreXamlByTypeProvider : IXamlByTypeProvider
    {
        public Uri GetUriFor(Type type)
        {
            if (type.Namespace != null)
            {
                var toRemove = type.Assembly.GetName().Name;
                var substracted = type.Namespace.Remove(0, toRemove.Length + 1);
                var replace = substracted.Replace('.', Path.PathSeparator);
                if (replace != string.Empty)
                {
                    replace = replace + "/";
                }
                return new Uri(replace + type.Name + ".xaml", UriKind.Relative);
            }

            return null;
        }
    }
}
