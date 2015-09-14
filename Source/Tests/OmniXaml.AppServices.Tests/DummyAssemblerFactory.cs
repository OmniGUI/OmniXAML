namespace OmniXaml.AppServices.Tests
{
    using System;
    using ObjectAssembler;
    using OmniXaml.Tests.Classes.Templates;
    using OmniXaml.Tests.Classes.WpfLikeModel;

    internal class DummyAssemblerFactory : IObjectAssemblerFactory
    {
        private readonly IWiringContext wiringContext;

        public DummyAssemblerFactory(IWiringContext wiringContext)
        {
            this.wiringContext = wiringContext;
        }

        public IObjectAssembler CreateAssembler(ObjectAssemblerSettings settings)
        {
            var mapping = new DeferredLoaderMapping();
            mapping.Map<DataTemplate>(template => template.Content, new DummyDeferredLoader());

            var objectAssembler = new ObjectAssembler(wiringContext, new TopDownValueContext(), settings);
            return new TemplateHostingObjectAssembler(objectAssembler, mapping);
        }

        public IObjectAssembler CreateAssembler()
        {
            throw new NotImplementedException();
        }
    }
}