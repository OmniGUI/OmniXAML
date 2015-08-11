namespace OmniXaml
{
    using Glass;

    public static class XamlLoaderExtensions
    {
        public static object Load(this IXamlLoader coreXamlLoader, string xamlContent, object rootInstance = null)
        {
            using (var stream = xamlContent.ToStream())
            {
                return coreXamlLoader.Load(stream, rootInstance);
            }
        }
    }
}