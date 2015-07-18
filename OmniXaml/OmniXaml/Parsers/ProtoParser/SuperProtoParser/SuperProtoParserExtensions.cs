namespace OmniXaml.Parsers.ProtoParser.SuperProtoParser
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    internal static class SuperProtoParserExtensions
    {
        public static IEnumerable<ProtoXamlNode> Parse(this SuperProtoParser protoParser, string xml)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                return protoParser.Parse(stream);
            }
        }
    }
}