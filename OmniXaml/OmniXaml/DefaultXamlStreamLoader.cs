namespace OmniXaml
{
    using System;
    using Parsers.ProtoParser;
    using Parsers.XamlNodes;

    public class DefaultXamlStreamLoader: XamlStreamLoader
    {
        public DefaultXamlStreamLoader(WiringContext wiringContext) : base(LoaderFactory(wiringContext), new DefaultObjectAssemblerFactory(wiringContext))
        {
        }

        private static Func<IObjectAssembler, IConfiguredXamlLoader> LoaderFactory(WiringContext wiringContext)
        {
            return assembler => new ConfiguredXamlXmlLoader(new XamlProtoInstructionParser(wiringContext), new XamlInstructionParser(wiringContext), assembler);
        }
    }
}