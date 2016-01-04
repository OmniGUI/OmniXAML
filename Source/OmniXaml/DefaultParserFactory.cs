namespace OmniXaml
{
    using ObjectAssembler;
    using Parsers.ProtoParser;
    using Parsers.XamlInstructions;

    public class DefaultParserFactory : IXamlParserFactory
    {
        private readonly IWiringContext wiringContext;

        public DefaultParserFactory(IWiringContext wiringContext)
        {
            this.wiringContext = wiringContext;
        }

        public IXamlParser CreateForReadingFree()
        {
            var objectAssemblerForUndefinedRoot = GetObjectAssemblerForUndefinedRoot();

            return CreateParser(objectAssemblerForUndefinedRoot);
        }

        private IXamlParser CreateParser(IObjectAssembler objectAssemblerForUndefinedRoot)
        {
            var xamlInstructionParser = new OrderAwareXamlInstructionParser(new XamlInstructionParser(wiringContext.TypeContext));

            var phaseParserKit = new PhaseParserKit(
                new XamlProtoInstructionParser(wiringContext.TypeContext),
                xamlInstructionParser,
                objectAssemblerForUndefinedRoot);

            return new XamlXmlParser(phaseParserKit);
        }

        private IObjectAssembler GetObjectAssemblerForUndefinedRoot()
        {
            return new ObjectAssembler.ObjectAssembler(wiringContext.TypeContext, new TopDownValueContext());
        }

        public IXamlParser CreateForReadingSpecificInstance(object rootInstance)
        {
            var objectAssemblerForUndefinedRoot = GetObjectAssemblerForSpecificRoot(rootInstance);

            return CreateParser(objectAssemblerForUndefinedRoot);
        }

        private IObjectAssembler GetObjectAssemblerForSpecificRoot(object rootInstance)
        {
            return new ObjectAssembler.ObjectAssembler(wiringContext.TypeContext, new TopDownValueContext(), new ObjectAssemblerSettings { RootInstance = rootInstance });
        }
    }
}