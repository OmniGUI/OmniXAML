namespace OmniXaml
{
    using System.IO;

    public interface ILoader
    {
        object Load(Stream stream);
        object Load(Stream stream, object instance);        
    }
}