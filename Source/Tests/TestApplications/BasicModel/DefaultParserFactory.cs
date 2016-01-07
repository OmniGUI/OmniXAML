namespace SampleOmniXAML
{
    using OmniXaml;
    using OmniXaml.ObjectAssembler;
    using OmniXaml.Parsers.ProtoParser;
    using OmniXaml.Parsers.XamlInstructions;

    public class DefaultParserFactory : IXamlParserFactory
    {
        private readonly IRuntimeTypeSource typeContext;

        public DefaultParserFactory()
        {
        }

        public DefaultParserFactory(IRuntimeTypeSource typeContext)
        {
            this.typeContext = typeContext;
        }

        public IXamlParser CreateForReadingFree()
        {
            var objectAssemblerForUndefinedRoot = GetObjectAssemblerForUndefinedRoot();

            return CreateParser(objectAssemblerForUndefinedRoot);
        }

        private IXamlParser CreateParser(IObjectAssembler objectAssemblerForUndefinedRoot)
        {
            var xamlInstructionParser = new OrderAwareXamlInstructionParser(new XamlInstructionParser(typeContext));

            var phaseParserKit = new PhaseParserKit(
                new XamlProtoInstructionParser(typeContext),
                xamlInstructionParser,
                objectAssemblerForUndefinedRoot);

            return new XmlParser(phaseParserKit);
        }

        private IObjectAssembler GetObjectAssemblerForUndefinedRoot()
        {
            return new ObjectAssembler(typeContext, new TopDownValueContext());
        }

        public IXamlParser CreateForReadingSpecificInstance(object rootInstance)
        {
            var objectAssemblerForUndefinedRoot = GetObjectAssemblerForSpecificRoot(rootInstance);

            return CreateParser(objectAssemblerForUndefinedRoot);
        }

        private IObjectAssembler GetObjectAssemblerForSpecificRoot(object rootInstance)
        {
            return new ObjectAssembler(typeContext, new TopDownValueContext(), new ObjectAssemblerSettings { RootInstance = rootInstance });
        }
    }
}