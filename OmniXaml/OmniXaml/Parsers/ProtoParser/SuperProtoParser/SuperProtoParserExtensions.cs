namespace OmniXaml.Parsers.ProtoParser.SuperProtoParser
{
    using System.Collections.Generic;
    using Glass;

    internal static class SuperProtoParserExtensions
    {
        public static IEnumerable<ProtoXamlNode> Parse(this SuperProtoParser protoParser, string xml)
        {
            using (var stream = xml.ToStream())
            {
                return protoParser.Parse(stream);
            }
        }
    }
}