namespace OmniXaml
{
    using System.IO;

    public interface ICoreXamlLoader
    {
        object Load(Stream stream);        
    }
}