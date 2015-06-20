namespace OmniXaml
{
    using System.IO;

    public interface IXamlLoader
    {
        object Load(Stream stream);    
    }
}