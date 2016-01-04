namespace OmniXaml.Tests.XamlXmlLoaderTests
{
    using System;
    using Common.NetCore;

    public class GivenAXamlXmlLoader : GivenAWiringContextNetCore
    {
        protected GivenAXamlXmlLoader()
        {
            XamlLoader = new XamlXmlLoader(new DummyXamlParserFactory(TypeContext));
        }

        protected XamlXmlLoader XamlLoader { get; }
    }
}
