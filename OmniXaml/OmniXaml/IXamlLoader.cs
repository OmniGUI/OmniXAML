namespace OmniXaml
{
    using System.IO;

    public interface IXamlLoader
    {
        object Load(Stream stream);

        // ReSharper disable once UnusedMember.Global
        object Load(Stream stream, object rootInstance);
    }
}