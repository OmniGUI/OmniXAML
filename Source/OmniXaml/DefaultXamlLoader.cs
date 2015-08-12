namespace OmniXaml
{
    using System;
    using Parsers.ProtoParser;
    using Parsers.XamlNodes;

    public class DefaultXamlLoader: XamlLoader
    {
        public DefaultXamlLoader(WiringContext wiringContext) : base(LoaderFactory(wiringContext), new DefaultObjectAssemblerFactory(wiringContext))
        {
        }

        private static Func<IObjectAssembler, IXamlParser> LoaderFactory(WiringContext wiringContext)
        {
            return assembler => new XamlXmlParser(new XamlProtoInstructionParser(wiringContext), new XamlInstructionParser(wiringContext), assembler);
        }
    }
}