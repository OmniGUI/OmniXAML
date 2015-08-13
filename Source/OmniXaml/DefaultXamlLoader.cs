namespace OmniXaml
{
    using System;
    using Parsers.ProtoParser;
    using Parsers.XamlNodes;

    public class DefaultXamlLoader: XamlLoader
    {
        public DefaultXamlLoader(IWiringContext wiringContext) : base(LoaderFactory(wiringContext), new DefaultObjectAssemblerFactory(wiringContext))
        {
        }

        private static Func<IObjectAssembler, IXamlParser> LoaderFactory(IWiringContext wiringContext)
        {
            return assembler => new XamlXmlParser(new XamlProtoInstructionParser(wiringContext), new XamlInstructionParser(wiringContext), assembler);
        }
    }
}