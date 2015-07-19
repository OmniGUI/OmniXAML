namespace OmniXaml.Wpf
{
    using Parsers.ProtoParser.SuperProtoParser;
    using Parsers.XamlNodes;

    public class WpfXamlStreamLoader : XamlStreamLoader
    {
        public WpfXamlStreamLoader()
            : base(
                assembler => new ConfiguredXamlXmlLoader(new SuperProtoParser(WiringContextFactory.Context), new XamlNodesPullParser(WiringContextFactory.Context), assembler),
                new ObjectAssemblerFactory(WiringContextFactory.Context))
        {
        }
    }
}