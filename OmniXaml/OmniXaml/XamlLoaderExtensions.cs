using System.IO;
using System.Text;

namespace OmniXaml
{
    public static class XamlLoaderExtensions
    {
        public static object Load(this IXamlStreamLoader coreXamlStreamLoader, string xamlContent, object rootInstance = null)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xamlContent)))
            {
                return coreXamlStreamLoader.Load(stream, rootInstance);
            }
        }
    }
}