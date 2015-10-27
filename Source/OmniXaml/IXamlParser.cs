namespace OmniXaml
{
    using System.IO;
    using Parsers;
    using Parsers.ProtoParser;

    public interface IXamlParser : IParser<IXmlReader, object>
    {
    }
}