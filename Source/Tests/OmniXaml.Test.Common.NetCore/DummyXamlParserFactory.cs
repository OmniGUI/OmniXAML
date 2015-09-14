namespace OmniXaml.Tests.Common.NetCore
{
    using Parsers.ProtoParser;
    using Classes.Templates;
    using Classes.WpfLikeModel;
    using ObjectAssembler;
    using Parsers.XamlInstructions;

    public class DummyXamlParserFactory : IXamlParserFactory
    {
        private readonly IWiringContext wiringContext;

        public DummyXamlParserFactory(IWiringContext wiringContext)
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
            var phaseParserKit = new PhaseParserKit(
                new XamlProtoInstructionParser(wiringContext),
                new XamlInstructionParser(wiringContext),
                objectAssemblerForUndefinedRoot);

            return new XamlXmlParser(phaseParserKit);
        }

        private ObjectAssembler GetObjectAssemblerForUndefinedRoot()
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
            var objectAssembler = new ObjectAssembler(wiringContext, new TopDownValueContext(), new ObjectAssemblerSettings { RootInstance = rootInstance });

            var mapping = new DeferredLoaderMapping();
            mapping.Map<DataTemplate>(template => template.Content, new DummyDeferredLoader());

            var templateAwareObjectAssembler = new TemplateHostingObjectAssembler(objectAssembler, mapping);            
            return templateAwareObjectAssembler;
        }
    }
}