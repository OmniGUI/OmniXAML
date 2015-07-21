namespace OmniXaml.AppServices.Tests
{
    using System;
    using System.Collections.ObjectModel;
    using NetCore;
    using OmniXaml.Tests;
    using OmniXaml.Tests.Classes.WpfLikeModel;
    using Parsers.ProtoParser.SuperProtoParser;
    using Parsers.XamlNodes;

    public class GivenAnInflatableTypeLoader : GivenAWiringContext
    {
        protected InflatableTypeFactory Inflatable { get; }

        protected GivenAnInflatableTypeLoader()
        {
            Inflatable = new InflatableTypeFactory(new TypeFactory(), new NetCoreResourceProvider(), new NetCoreTypeToUriLocator(), LoaderFactory)
            {
                Inflatables = new Collection<Type> {typeof (Window), typeof (UserControl)}
            };
        }

        private IXamlStreamLoader LoaderFactory(InflatableTypeFactory inflatableTypeFactory)
        {
            return
                new XamlStreamLoader(
                    assembler => new ConfiguredXamlXmlLoader(new SuperProtoParser(WiringContext), new XamlNodesPullParser(WiringContext), assembler),
                    new DummyAssemblerFactory(WiringContext));
        }
    }
}