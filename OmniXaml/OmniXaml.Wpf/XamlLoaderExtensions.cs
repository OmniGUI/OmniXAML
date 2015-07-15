namespace OmniXaml.Wpf
{
    using System.IO;

    public static class XamlLoaderExtensions
    {
        public static object LoadFromFile(this IXamlLoader coreXamlLoader, string path)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return coreXamlLoader.Load(stream);
            }
        }
    }
}