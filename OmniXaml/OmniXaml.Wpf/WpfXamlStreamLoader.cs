namespace OmniXaml.Wpf
{
    using Parsers.ProtoParser.SuperProtoParser;
    using Parsers.XamlNodes;

    public class WpfXamlStreamLoader : XamlStreamLoader
    {
        public WpfXamlStreamLoader(ITypeFactory typeFactory)
            : base(
                assembler => new ConfiguredXamlXmlLoader(new SuperProtoParser(WiringContextFactory.GetContext(typeFactory)), new XamlNodesPullParser(WiringContextFactory.GetContext(typeFactory)), assembler),
                new ObjectAssemblerFactory(WiringContextFactory.GetContext(typeFactory)))
        {
        }
    }
}