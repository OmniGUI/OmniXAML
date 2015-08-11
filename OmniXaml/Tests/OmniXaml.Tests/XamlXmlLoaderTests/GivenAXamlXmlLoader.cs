namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using OmniXaml.Parsers.ProtoParser;
    using OmniXaml.Parsers.XamlNodes;
    using Tests;

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
