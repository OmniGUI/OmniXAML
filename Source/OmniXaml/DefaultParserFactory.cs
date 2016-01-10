namespace OmniXaml
{
    using ObjectAssembler;
    using Parsers.Parser;
    using Parsers.ProtoParser;

    public class DefaultParserFactory : IParserFactory
    {
        private readonly IRuntimeTypeSource runtimeTypeSource;

        public DefaultParserFactory(IRuntimeTypeSource runtimeTypeSource)
        {
            this.runtimeTypeSource = runtimeTypeSource;
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

        public IParser Create(Settings settings)
        {
            var objectAssemblerForUndefinedRoot = new ObjectAssembler.ObjectAssembler(runtimeTypeSource, new TopDownValueContext(), settings);

            return CreateParser(objectAssemblerForUndefinedRoot);
        }
    }
}