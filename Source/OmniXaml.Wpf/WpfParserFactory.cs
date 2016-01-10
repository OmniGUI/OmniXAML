namespace OmniXaml.Wpf
{
    using OmniXaml.ObjectAssembler;
    using Parsers.Parser;
    using Parsers.ProtoParser;

    public class WpfParserFactory : IParserFactory
    {
        private readonly IRuntimeTypeSource runtimeTypeSource;

        public WpfParserFactory()
        {
            runtimeTypeSource = new WpfRuntimeTypeSource();
        }

        private IParser CreateParser(IObjectAssembler objectAssemblerForUndefinedRoot)
        {
            var xamlInstructionParser = new OrderAwareInstructionParser(new InstructionParser(runtimeTypeSource));

            var phaseParserKit = new PhaseParserKit(
                new ProtoInstructionParser(runtimeTypeSource),
                xamlInstructionParser,
                objectAssemblerForUndefinedRoot);

            return new XmlParser(phaseParserKit);
        }

        private IObjectAssembler GetObjectAssemblerForUndefinedRoot()
        {
            return new ObjectAssembler(runtimeTypeSource, new TopDownValueContext());
        }

        public IParser Create(Settings settings)
        {
            var objectAssemblerForUndefinedRoot = new ObjectAssembler(runtimeTypeSource, new TopDownValueContext(), settings);

            return CreateParser(objectAssemblerForUndefinedRoot);
        }

        private IObjectAssembler GetObjectAssemblerForSpecificRoot(object rootInstance)
        {
            return new ObjectAssembler(runtimeTypeSource, new TopDownValueContext(), new Settings { RootInstance = rootInstance });
        }
    }
}