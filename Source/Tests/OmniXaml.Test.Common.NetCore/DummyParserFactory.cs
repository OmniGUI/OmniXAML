namespace OmniXaml.Tests.Common.NetCore
{
    using Parsers.ProtoParser;
    using Classes.Templates;
    using Classes.WpfLikeModel;
    using ObjectAssembler;
    using Parsers.Parser;

    public class DummyParserFactory : IParserFactory
    {
        private readonly IRuntimeTypeSource runtimeTypeSource;

        public DummyParserFactory(IRuntimeTypeSource runtimeTypeSource)
        {
            this.runtimeTypeSource = runtimeTypeSource;
        }

        public IParser CreateForReadingFree()
        {
            var objectAssemblerForUndefinedRoot = GetObjectAssemblerForUndefinedRoot();

            return CreateParser(objectAssemblerForUndefinedRoot);
        }

        private IParser CreateParser(IObjectAssembler objectAssemblerForUndefinedRoot)
        {
            var phaseParserKit = new PhaseParserKit(
                new ProtoInstructionParser(runtimeTypeSource),
                new InstructionParser(runtimeTypeSource),
                objectAssemblerForUndefinedRoot);

            return new XmlParser(phaseParserKit);
        }

        private ObjectAssembler GetObjectAssemblerForUndefinedRoot()
        {
            return new ObjectAssembler(runtimeTypeSource, new TopDownValueContext());
        }

        public IParser CreateForReadingSpecificInstance(object rootInstance)
        {
            var objectAssemblerForUndefinedRoot = GetObjectAssemblerForSpecificRoot(rootInstance);

            return CreateParser(objectAssemblerForUndefinedRoot);
        }

        public IParser Create(ObjectAssemblerSettings settings)
        {
            var objectAssemblerForUndefinedRoot = CreateObjectAssembler(settings);

            return CreateParser(objectAssemblerForUndefinedRoot);
        }

        private IObjectAssembler CreateObjectAssembler(ObjectAssemblerSettings settings)
        {
            var objectAssembler = new ObjectAssembler(runtimeTypeSource, new TopDownValueContext(), settings);

            var mapping = new DeferredLoaderMapping();
            mapping.Map<DataTemplate>(template => template.Content, new DummyDeferredLoader());

            var templateAwareObjectAssembler = new TemplateHostingObjectAssembler(objectAssembler, mapping);
            return templateAwareObjectAssembler;
        }

        private IObjectAssembler GetObjectAssemblerForSpecificRoot(object rootInstance)
        {
            var objectAssembler = new ObjectAssembler(runtimeTypeSource, new TopDownValueContext(), new ObjectAssemblerSettings { RootInstance = rootInstance });

            var mapping = new DeferredLoaderMapping();
            mapping.Map<DataTemplate>(template => template.Content, new DummyDeferredLoader());

            var templateAwareObjectAssembler = new TemplateHostingObjectAssembler(objectAssembler, mapping);            
            return templateAwareObjectAssembler;
        }
    }
}