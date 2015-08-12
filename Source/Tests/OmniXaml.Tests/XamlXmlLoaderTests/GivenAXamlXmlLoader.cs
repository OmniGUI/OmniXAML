namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using Common;
    using OmniXaml.Parsers.ProtoParser;
    using OmniXaml.Parsers.XamlNodes;

    public class GivenAXamlXmlLoader : GivenAWiringContext
    {
        protected GivenAXamlXmlLoader()
        {
            XamlLoader = new XamlLoader(new XamlProtoInstructionParser(WiringContext),
                new XamlInstructionParser(WiringContext),
                new DefaultObjectAssemblerFactory(WiringContext));
        }

        protected XamlLoader XamlLoader { get; }
    }
}
