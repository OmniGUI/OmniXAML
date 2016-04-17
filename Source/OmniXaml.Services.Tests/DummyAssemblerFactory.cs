namespace OmniXaml.Services.Tests
{
    using System;
    using ObjectAssembler;
    using OmniXaml.Tests.Classes.Templates;
    using OmniXaml.Tests.Classes.WpfLikeModel;
    using TypeConversion;

    internal class DummyAssemblerFactory : IObjectAssemblerFactory
    {
        private readonly IRuntimeTypeSource runtimeTypeSource;

        public DummyAssemblerFactory(IRuntimeTypeSource runtimeTypeSource)
        {
            this.runtimeTypeSource = runtimeTypeSource;
        }

        public IObjectAssembler CreateAssembler(Settings settings)
        {
            var mapping = new DeferredLoaderMapping();
            mapping.Map<DataTemplate>(template => template.Content, new DummyDeferredLoader());

            var topDownValueContext = new TopDownValueContext();
            var valueConnectionContext = new ValueContext(runtimeTypeSource, topDownValueContext, settings.ParsingContext);
            var objectAssembler = new ObjectAssembler(runtimeTypeSource, valueConnectionContext, settings);
            return new TemplateHostingObjectAssembler(objectAssembler, mapping);
        }

        public IObjectAssembler CreateAssembler()
        {
            throw new NotImplementedException();
        }
    }
}