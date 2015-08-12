namespace OmniXaml
{
    using System.IO;
    using Parsers;

    public interface IXamlParser : IParser<Stream, object>
    {
    }
}