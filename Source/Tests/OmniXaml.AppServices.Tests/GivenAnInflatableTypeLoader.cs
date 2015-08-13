namespace OmniXaml.AppServices.Tests
{
    using System;
    using System.Collections.ObjectModel;
    using NetCore;
    using OmniXaml.Tests.Classes.WpfLikeModel;
    using OmniXaml.Tests.Common.NetCore;

    public class GivenAnInflatableTypeLoader : GivenAWiringContextNetCore
    {
        protected InflatableTypeFactory Inflatable { get; }

        protected GivenAnInflatableTypeLoader()
        {
            Func<InflatableTypeFactory, IXamlLoader> loaderFactory = inflatableTypeFactory =>
            {
                var parserFactory = new DummyXamlParserFactory(WiringContext);
                return new XamlLoader(parserFactory);
            };

            Inflatable = new InflatableTypeFactory(new TypeFactory(), new InflatableTranslator(), loaderFactory)
            {
                Inflatables = new Collection<Type> {typeof (Window), typeof (UserControl)}
            };
        }
    }
}