namespace OmniXaml
{
    using System;
    using Parsers.ProtoParser;
    using Parsers.XamlNodes;

    public class DefaultXamlLoader: XamlLoader
    {
        public DefaultXamlLoader(IWiringContext IWiringContext) : base(LoaderFactory(IWiringContext), new DefaultObjectAssemblerFactory(IWiringContext))
        {
        }

        private static Func<IObjectAssembler, IXamlParser> LoaderFactory(IWiringContext IWiringContext)
        {
            return assembler => new XamlXmlParser(new XamlProtoInstructionParser(IWiringContext), new XamlInstructionParser(IWiringContext), assembler);
        }
    }
}