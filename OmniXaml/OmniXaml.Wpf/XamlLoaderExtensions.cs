namespace OmniXaml.Wpf
{
    using System.IO;

    public static class XamlLoaderExtensions
    {
        public static object LoadFromFile(this ILoader coreLoader, string path)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return coreLoader.Load(stream);
            }
        }
    }
}