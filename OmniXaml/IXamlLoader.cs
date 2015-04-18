namespace OmniXaml
{
    using System.IO;

    public interface IXamlLoader
    {
        object Load(Stream stream);

        object Load(Stream stream, object rootInstance);

        object Load(IXamlReader reader, object rootObject = null);
    }
}