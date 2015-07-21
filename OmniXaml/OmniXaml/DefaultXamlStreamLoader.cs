namespace OmniXaml
{
    using System;
    using Parsers.ProtoParser.SuperProtoParser;
    using Parsers.XamlNodes;

    public class DefaultXamlStreamLoader: XamlStreamLoader
    {
        public DefaultXamlStreamLoader(WiringContext wiringContext) : base(LoaderFactory(wiringContext), new DefaultObjectAssemblerFactory(wiringContext))
        {
        }

        private static Func<IObjectAssembler, IConfiguredXamlLoader> LoaderFactory(WiringContext wiringContext)
        {
            return assembler => new ConfiguredXamlXmlLoader(new SuperProtoParser(wiringContext), new XamlNodesPullParser(wiringContext), assembler);
        }
    }
}