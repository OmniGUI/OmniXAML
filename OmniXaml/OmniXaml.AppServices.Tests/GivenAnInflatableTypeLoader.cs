namespace OmniXaml.AppServices.Tests
{
    using System;
    using System.Collections.ObjectModel;
    using NetCore;
    using OmniXaml.Tests;
    using OmniXaml.Tests.Classes.WpfLikeModel;
    using OmniXaml.Tests.Common;
    using Parsers.ProtoParser;
    using Parsers.XamlNodes;

    public class GivenAnInflatableTypeLoader : GivenAWiringContext
    {
        protected InflatableTypeFactory Inflatable { get; }

        protected GivenAnInflatableTypeLoader()
        {
            Inflatable = new InflatableTypeFactory(new TypeFactory(), new InflatableTranslator(), LoaderFactory)
            {
                Inflatables = new Collection<Type> {typeof (Window), typeof (UserControl)}
            };
        }

        private IXamlLoader LoaderFactory(InflatableTypeFactory inflatableTypeFactory)
        {
            return
                new XamlLoader(
                    assembler => new XamlXmlParser(new XamlProtoInstructionParser(WiringContext), new XamlInstructionParser(WiringContext), assembler),
                    new DummyAssemblerFactory(WiringContext));
        }
    }
}