namespace OmniXaml.AppServices.Tests
{
    using System;
    using OmniXaml.Tests.Common.NetCore;
    using Services.DotNetFx;

    public class GivenAnInflatableTypeLoader : GivenARuntimeTypeContextNetCore
    {
        protected ITypeFactory TypeFactory { get; }

        protected GivenAnInflatableTypeLoader()
        {
            Func<ITypeFactory, IXamlLoader> loaderFactory = inflatableTypeFactory =>
            {
                var parserFactory = new DummyXamlParserFactory(TypeRuntimeTypeSource);
                return new XmlLoader(parserFactory);
            };            

            TypeFactory = new DummyAutoInflatingTypeFactory(new TypeFactory(), new InflatableTranslator(), loaderFactory );
        }
    }
}