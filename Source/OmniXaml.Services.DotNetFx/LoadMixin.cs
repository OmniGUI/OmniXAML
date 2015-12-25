namespace OmniXaml.Services.DotNetFx
{
    using System.IO;

    public static class LoadMixin
    {
        public static object FromPath(this IXamlLoader loader, string path)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return loader.Load(stream);
            }
        }
    }
}
