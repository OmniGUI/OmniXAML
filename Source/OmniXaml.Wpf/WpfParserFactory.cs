namespace OmniXaml.Wpf
{
    using OmniXaml.ObjectAssembler;
    using Parsers.ProtoParser;
    using Parsers.XamlInstructions;

    public class WpfParserFactory : IXamlParserFactory
    {
        private readonly IWiringContext wiringContext;

        public WpfParserFactory(ITypeFactory typeFactory)
        {
            wiringContext = new WpfWiringContext(typeFactory);
        }

        public IXamlParser CreateForReadingFree()
        {
            var objectAssemblerForUndefinedRoot = GetObjectAssemblerForUndefinedRoot();

            return CreateParser(objectAssemblerForUndefinedRoot);
        }

        private IXamlParser CreateParser(IObjectAssembler objectAssemblerForUndefinedRoot)
        {
            var xamlInstructionParser = new OrderAwareXamlInstructionParser(new XamlInstructionParser(wiringContext));

            var phaseParserKit = new PhaseParserKit(
                new XamlProtoInstructionParser(wiringContext),
                xamlInstructionParser,
                objectAssemblerForUndefinedRoot);

            return new XamlXmlParser(phaseParserKit);
        }

        private IObjectAssembler GetObjectAssemblerForUndefinedRoot()
        {
            return new ObjectAssembler(wiringContext, new TopDownValueContext());
        }

        public IXamlParser CreateForReadingSpecificInstance(object rootInstance)
        {
            var objectAssemblerForUndefinedRoot = GetObjectAssemblerForSpecificRoot(rootInstance);

            return CreateParser(objectAssemblerForUndefinedRoot);
        }

        private IObjectAssembler GetObjectAssemblerForSpecificRoot(object rootInstance)
        {
            return new ObjectAssembler(wiringContext, new TopDownValueContext(), new ObjectAssemblerSettings { RootInstance = rootInstance });
        }
    }
}