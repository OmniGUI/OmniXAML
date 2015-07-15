using System.IO;
using System.Text;

namespace OmniXaml
{
    public static class XamlLoaderExtensions
    {
        public static object Load(this IXamlLoader coreXamlLoader, string xamlContent, object rootInstance = null)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xamlContent)))
            {
                return coreXamlLoader.Load(stream, rootInstance);
            }
        }
    }
}