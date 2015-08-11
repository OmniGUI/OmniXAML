namespace OmniXaml.Wpf
{
    using System.Collections.Generic;
    using System.IO;
    using Parsers.ProtoParser;
    using Parsers.XamlNodes;

    public class WpfXamlStreamLoader : XamlStreamLoader
    {
        public WpfXamlStreamLoader(ITypeFactory typeFactory)
            : base(
                assembler => new ConfiguredXamlXmlLoader(GetProtoParser(typeFactory), GetPullParser(typeFactory), assembler),
                new ObjectAssemblerFactory(WpfWiringContextFactory.GetContext(typeFactory)))
        {
        }

        private static IParser<Stream, IEnumerable<ProtoXamlInstruction>> GetProtoParser(ITypeFactory typeFactory)
        {
            return new XamlProtoInstructionParser(WpfWiringContextFactory.GetContext(typeFactory));
        }

        private static IXamlInstructionParser GetPullParser(ITypeFactory typeFactory)
        {
            var xamlNodesPullParser = new XamlInstructionParser(WpfWiringContextFactory.GetContext(typeFactory));
            return new OrderAwareXamlInstructionParser(xamlNodesPullParser);
        }
    }
}