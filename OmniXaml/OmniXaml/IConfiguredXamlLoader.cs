namespace OmniXaml
{
    using System.IO;

    public interface IConfiguredXamlLoader
    {
        object Load(Stream stream);        
    }
}