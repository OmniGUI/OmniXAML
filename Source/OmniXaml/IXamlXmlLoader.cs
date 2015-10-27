namespace OmniXaml
{
    using Parsers.ProtoParser;

    public interface IXamlXmlLoader
    {
        object Load(IXmlReader reader);
        object Load(IXmlReader reader, object rootInstance);
    }
}