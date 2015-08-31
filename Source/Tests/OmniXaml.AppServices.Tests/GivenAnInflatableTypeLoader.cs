namespace OmniXaml.AppServices.Tests
{
    using System;
    using NetCore;
    using OmniXaml.Tests.Common.NetCore;

    public class GivenAnInflatableTypeLoader : GivenAWiringContextNetCore
    {
        protected InflatableTypeFactory Inflatable { get; }

        protected GivenAnInflatableTypeLoader()
        {
            Func<ITypeFactory, IXamlLoader> loaderFactory = inflatableTypeFactory =>
            {
                var parserFactory = new DummyXamlParserFactory(WiringContext);
                return new XamlLoader(parserFactory);
            };            

            Inflatable = new DummyInflatableTypeFactory(new TypeFactory(), new InflatableTranslator(), loaderFactory );
        }
    }
}