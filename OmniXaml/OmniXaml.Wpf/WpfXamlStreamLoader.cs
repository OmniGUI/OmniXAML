namespace OmniXaml.Wpf
{
    using Parsers.ProtoParser.SuperProtoParser;
    using Parsers.XamlNodes;

    public class WpfXamlStreamLoader : XamlStreamLoader
    {
        public WpfXamlStreamLoader(ITypeFactory typeFactory)
            : base(
                assembler => new ConfiguredXamlXmlLoader(new SuperProtoParser(WpfWiringContextFactory.GetContext(typeFactory)), GetPullParser(typeFactory), assembler),
                new ObjectAssemblerFactory(WpfWiringContextFactory.GetContext(typeFactory)))
        {
        }

        private static IXamlNodesPullParser GetPullParser(ITypeFactory typeFactory)
        {
            var xamlNodesPullParser = new XamlNodesPullParser(WpfWiringContextFactory.GetContext(typeFactory));
            return new OrderAwareXamlNodesPullParser(xamlNodesPullParser);
        }
    }
}