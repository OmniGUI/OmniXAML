namespace OmniXaml.AppServices.Tests
{
    using System;
    using ObjectAssembler;
    using OmniXaml.Tests.Classes.Templates;
    using OmniXaml.Tests.Classes.WpfLikeModel;

    internal class DummyAssemblerFactory : IObjectAssemblerFactory
    {
        private readonly IRuntimeTypeSource runtimeTypeContext;

        public DummyAssemblerFactory(IRuntimeTypeSource runtimeTypeContext)
        {
            this.runtimeTypeContext = runtimeTypeContext;
        }

        public IObjectAssembler CreateAssembler(ObjectAssemblerSettings settings)
        {
            var mapping = new DeferredLoaderMapping();
            mapping.Map<DataTemplate>(template => template.Content, new DummyDeferredLoader());

            var objectAssembler = new ObjectAssembler(runtimeTypeContext, new TopDownValueContext(), settings);
            return new TemplateHostingObjectAssembler(objectAssembler, mapping);
        }

        public IObjectAssembler CreateAssembler()
        {
            throw new NotImplementedException();
        }
    }
}