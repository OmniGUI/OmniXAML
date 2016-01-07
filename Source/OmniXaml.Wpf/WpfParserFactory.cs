namespace OmniXaml.Wpf
{
    using OmniXaml.ObjectAssembler;
    using Parsers.ProtoParser;
    using Parsers.XamlInstructions;

    public class WpfParserFactory : IXamlParserFactory
    {
        private readonly IRuntimeTypeSource runtimeTypeContext;

        public WpfParserFactory()
        {
            runtimeTypeContext = new WpfRuntimeTypeSource();
        }

        public IXamlParser CreateForReadingFree()
        {
            var objectAssemblerForUndefinedRoot = GetObjectAssemblerForUndefinedRoot();

            return CreateParser(objectAssemblerForUndefinedRoot);
        }

        private IXamlParser CreateParser(IObjectAssembler objectAssemblerForUndefinedRoot)
        {
            var xamlInstructionParser = new OrderAwareXamlInstructionParser(new XamlInstructionParser(runtimeTypeContext));

            var phaseParserKit = new PhaseParserKit(
                new XamlProtoInstructionParser(runtimeTypeContext),
                xamlInstructionParser,
                objectAssemblerForUndefinedRoot);

            return new XmlParser(phaseParserKit);
        }

        private IObjectAssembler GetObjectAssemblerForUndefinedRoot()
        {
            return new ObjectAssembler(runtimeTypeContext, new TopDownValueContext());
        }

        public IXamlParser CreateForReadingSpecificInstance(object rootInstance)
        {
            var objectAssemblerForUndefinedRoot = GetObjectAssemblerForSpecificRoot(rootInstance);

            return CreateParser(objectAssemblerForUndefinedRoot);
        }

        private IObjectAssembler GetObjectAssemblerForSpecificRoot(object rootInstance)
        {
            return new ObjectAssembler(runtimeTypeContext, new TopDownValueContext(), new ObjectAssemblerSettings { RootInstance = rootInstance });
        }
    }
}