namespace OmniXaml
{
    using System.Collections.Generic;
    using System.IO;
    using Glass.Core;
    using ObjectAssembler;
    using Parsers;
    using Parsers.ProtoParser;

    public static class LoadMixin
    {
        public static IEnumerable<ProtoInstruction> Parse(this IParser<IXmlReader, IEnumerable<ProtoInstruction>> parser, string xml)
        {
            using (var stream = new StringReader(xml))
            {
                return parser.Parse(new XmlCompatibilityReader(stream));
            }
        }

        public static object FromString(this ILoader loader, string xml)
        {
            return FromString(loader, xml, new Settings());
        }

        public static object FromString(this ILoader loader, string xml, Settings settings)
        {
            using (var stream = xml.FromUTF8ToStream())
            {
                return loader.Load(stream, settings);
            }
        }

        public static object FromString(this ILoader loader, string xml, object instance)
        {
            using (var stream = xml.FromUTF8ToStream())
            {
                return loader.Load(stream, new Settings { RootInstance = instance });
            }
        }
    }
}