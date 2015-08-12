namespace OmniXaml.Parsers.ProtoParser
{
    using System.IO;
    using Glass;

    public static class ProtoXamlInstructionExtensions
    {
        public static TOutput Parse<TOutput>(this IParser<Stream, TOutput> parser, string xml)
        {
            using (var stream = xml.ToStream())
            {
                return parser.Parse(stream);
            }
        }
    }
}