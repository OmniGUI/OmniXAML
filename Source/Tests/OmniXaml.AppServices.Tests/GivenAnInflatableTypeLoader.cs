namespace OmniXaml.AppServices.Tests
{
    using System;
    using OmniXaml.Tests.Common.NetCore;
    using Services.DotNetFx;

    public class GivenAnInflatableTypeLoader : GivenARuntimeTypeSourceNetCore
    {
        protected ITypeFactory TypeFactory { get; }

        protected GivenAnInflatableTypeLoader()
        {
            Func<ITypeFactory, ILoader> loaderFactory = inflatableTypeFactory =>
            {
                var parserFactory = new DummyParserFactory(RuntimeTypeSource);
                return new XmlLoader(parserFactory);
            };            

            TypeFactory = new DummyAutoInflatingTypeFactory(new TypeFactory(), new InflatableTranslator(), loaderFactory );
        }
    }
}