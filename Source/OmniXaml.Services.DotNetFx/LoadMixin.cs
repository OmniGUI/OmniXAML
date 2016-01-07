namespace OmniXaml.Services.DotNetFx
{
    using System.IO;

    public static class LoadMixin
    {
        public static object FromPath(this ILoader loader, string path)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return loader.Load(stream);
            }
        }
    }
}
