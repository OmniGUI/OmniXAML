namespace OmniXaml
{
    using System.IO;

    public interface IXamlStreamLoader
    {
        object Load(Stream stream);
        object Load(Stream stream, object rootInstance);
    }
}