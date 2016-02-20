namespace OmniXaml.Tests.Common.NetCore
{
    using Parsers.ProtoParser;
    using Classes.Templates;
    using Classes.WpfLikeModel;
    using ObjectAssembler;
    using Parsers.Parser;
    using TypeConversion;

    public class DummyParserFactory : IParserFactory
    {
        private readonly IRuntimeTypeSource runtimeTypeSource;

        public DummyParserFactory(IRuntimeTypeSource runtimeTypeSource)
        {
            this.runtimeTypeSource = runtimeTypeSource;
        }

        private IParser CreateParser(IObjectAssembler objectAssemblerForUndefinedRoot)
        {
            var phaseParserKit = new PhaseParserKit(
                new ProtoInstructionParser(runtimeTypeSource),
                new InstructionParser(runtimeTypeSource),
                objectAssemblerForUndefinedRoot);

            return new XmlParser(phaseParserKit);
        }

        public IParser Create(Settings settings)
        {
            var objectAssemblerForUndefinedRoot = CreateObjectAssembler(settings);

            return CreateParser(objectAssemblerForUndefinedRoot);
        }

        private IObjectAssembler CreateObjectAssembler(Settings settings)
        {
            var topDownValueContext = new TopDownValueContext();

            var objectAssembler = new ObjectAssembler(runtimeTypeSource, new ValueContext(runtimeTypeSource, topDownValueContext), settings);

            var mapping = new DeferredLoaderMapping();
            mapping.Map<DataTemplate>(template => template.Content, new DummyDeferredLoader());

            var templateAwareObjectAssembler = new TemplateHostingObjectAssembler(objectAssembler, mapping);
            return templateAwareObjectAssembler;
        }
    }
}