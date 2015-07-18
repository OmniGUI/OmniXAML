namespace OmniXaml.Wpf
{
    using System.IO;

    public static class XamlLoaderExtensions
    {
        public static object LoadFromFile(this IXamlStreamLoader coreXamlStreamLoader, string path)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return coreXamlStreamLoader.Load(stream);
            }
        }
    }
}