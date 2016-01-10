namespace SampleOmniXAML
{
    using OmniXaml;
    using OmniXaml.ObjectAssembler;
    using OmniXaml.Parsers.Parser;
    using OmniXaml.Parsers.ProtoParser;

    public class DefaultParserFactory : IParserFactory
    {
        private readonly IRuntimeTypeSource typeSource;

        public DefaultParserFactory()
        {
        }

        public DefaultParserFactory(IRuntimeTypeSource typeSource)
        {
            this.typeSource = typeSource;
        }

        public IParser CreateForReadingFree()
        {
            var objectAssemblerForUndefinedRoot = GetObjectAssemblerForUndefinedRoot();

            return CreateParser(objectAssemblerForUndefinedRoot);
        }

        private IParser CreateParser(IObjectAssembler objectAssemblerForUndefinedRoot)
        {
            var xamlInstructionParser = new OrderAwareInstructionParser(new InstructionParser(typeSource));

            var phaseParserKit = new PhaseParserKit(
                new ProtoInstructionParser(typeSource),
                xamlInstructionParser,
                objectAssemblerForUndefinedRoot);

            return new XmlParser(phaseParserKit);
        }

        private IObjectAssembler GetObjectAssemblerForUndefinedRoot()
        {
            return new ObjectAssembler(typeSource, new TopDownValueContext());
        }

        public IParser CreateForReadingSpecificInstance(object rootInstance)
        {
            var objectAssemblerForUndefinedRoot = GetObjectAssemblerForSpecificRoot(rootInstance);

            return CreateParser(objectAssemblerForUndefinedRoot);
        }

        public IParser Create(ObjectAssemblerSettings settings)
        {
            var objectAssemblerForUndefinedRoot = new ObjectAssembler(typeSource, new TopDownValueContext(), new ObjectAssemblerSettings { RootInstance = settings.RootInstance });

            return CreateParser(objectAssemblerForUndefinedRoot);
        }

        private IObjectAssembler GetObjectAssemblerForSpecificRoot(object rootInstance)
        {
            return new ObjectAssembler(typeSource, new TopDownValueContext(), new ObjectAssemblerSettings { RootInstance = rootInstance });
        }
    }
}