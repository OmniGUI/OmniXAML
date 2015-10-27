namespace OmniXaml
{
    using System.Collections.Generic;
    using System.IO;
    using Parsers;
    using Parsers.ProtoParser;

    public static class ProtoParserExtensions
    {
        public static IEnumerable<ProtoXamlInstruction> Parse(this IParser<IXmlReader, IEnumerable<ProtoXamlInstruction>> parser, string xml)
        {
            using (var stream = new StringReader(xml))
            {
                return parser.Parse(new XmlCompatibilityReader(stream));
            }
        }
    }

    public static class XamlXmlLoaderExtensions
    {
        public static object Load(this IXamlXmlLoader loader, string xml)
        {
            using (var stream = new StringReader(xml))
            {
                return loader.Load(new XmlCompatibilityReader(stream));
            }
        }
    }
}