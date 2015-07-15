namespace OmniXaml
{
    using System.IO;

    public interface ICoreXamlLoader
    {
        object Load(Stream stream);        
    }

    public interface IXamlLoader
    {
        object Load(Stream stream);
        object Load(Stream stream, object rootInstance);
    }
}