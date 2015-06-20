using System.IO;
using System.Text;

namespace OmniXaml
{
    public static class XamlLoaderExtensions
    {
        public static object Load(this IXamlLoader xamlLoader, string xamlContent)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xamlContent)))
            {
                return xamlLoader.Load(stream);
            }
        }        
    }
}