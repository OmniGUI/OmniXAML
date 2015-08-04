namespace OmniXaml
{
    using Glass;

    public static class XamlLoaderExtensions
    {
        public static object Load(this IXamlStreamLoader coreXamlStreamLoader, string xamlContent, object rootInstance = null)
        {
            using (var stream = xamlContent.ToStream())
            {
                return coreXamlStreamLoader.Load(stream, rootInstance);
            }
        }
    }
}