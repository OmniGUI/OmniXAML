namespace OmniXaml.AppServices.Tests
{
    using System;
    using OmniXaml.Tests.Common.NetCore;
    using Services.DotNetFx;

    public class GivenAnInflatableTypeLoader : GivenAWiringContextNetCore
    {
        protected ITypeFactory TypeFactory { get; }

        protected GivenAnInflatableTypeLoader()
        {
            Func<ITypeFactory, IXamlLoader> loaderFactory = inflatableTypeFactory =>
            {
                var parserFactory = new DummyXamlParserFactory(WiringContext);
                return new XamlLoader(parserFactory);
            };            

            TypeFactory = new DummyAutoInflatingTypeFactory(new TypeFactory(), new InflatableTranslator(), loaderFactory );
        }
    }
}