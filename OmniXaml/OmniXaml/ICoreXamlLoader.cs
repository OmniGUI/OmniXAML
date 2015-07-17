namespace OmniXaml
{
    using System.IO;

    public interface ICoreXamlLoader
    {
        object Load(Stream stream);        
    }

    public interface ILoader
    {
        object Load(Stream stream);
        object Load(Stream stream, object rootInstance);
    }
}