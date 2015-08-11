namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using OmniXaml.Parsers.ProtoParser;
    using OmniXaml.Parsers.XamlNodes;
    using Tests;

    public class GivenAXamlXmlLoader : GivenAWiringContext
    {
        protected GivenAXamlXmlLoader()
        {
            XamlStreamLoader = new XamlStreamLoader(new XamlProtoInstructionParser(WiringContext),
                new XamlInstructionParser(WiringContext),
                new DefaultObjectAssemblerFactory(WiringContext));
        }

        protected XamlStreamLoader XamlStreamLoader { get; }
    }
}
