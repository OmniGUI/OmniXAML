namespace OmniXaml.Services.DotNetFx
{
    using System.IO;
    using ObjectAssembler;

    public static class LoadMixin
    {
        public static object FromPath(this ILoader loader, string path)
        {
            return FromPath(loader, path, new Settings());
        }

        public static object FromPath(this ILoader loader, string path, Settings settings)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return loader.Load(stream, settings);
            }
        }
    }
}
