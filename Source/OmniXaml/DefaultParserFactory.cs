namespace OmniXaml
{
    using System.Collections.Generic;
    using ObjectAssembler;
    using Parsers.Parser;
    using Parsers.ProtoParser;
    using TypeConversion;

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
            var topDownValueContext = new TopDownValueContext();
            var objectAssemblerForUndefinedRoot = new ObjectAssembler.ObjectAssembler(runtimeTypeSource,new ValueContext(runtimeTypeSource, topDownValueContext, new Dictionary<string, object>()),
                settings);

            return CreateParser(objectAssemblerForUndefinedRoot);
        }
    }
}