namespace OmniXaml.Wpf
{
    using System.Collections.Generic;
    using System.IO;
    using Parsers.ProtoParser;
    using Parsers.XamlNodes;

    public class WpfXamlLoader : XamlLoader
    {
        public WpfXamlLoader(ITypeFactory typeFactory)
            : base(
                assembler => new XamlXmlParser(GetProtoParser(typeFactory), GetPullParser(typeFactory), assembler),
                new ObjectAssemblerFactory(WpfWiringContextFactory.GetContext(typeFactory)))
        {
        }

        private static IProtoParser GetProtoParser(ITypeFactory typeFactory)
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