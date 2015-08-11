namespace OmniXaml
{
    using System.IO;

    public interface IXamlLoader
    {
        object Load(Stream stream);
        object Load(Stream stream, object rootInstance);
    }
}