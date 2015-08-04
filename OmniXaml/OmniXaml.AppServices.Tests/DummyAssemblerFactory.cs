namespace OmniXaml.AppServices.Tests
{
    using System;
    using Assembler;
    using NewAssembler;
    using OmniXaml.Tests.Classes.Templates;
    using OmniXaml.Tests.Classes.WpfLikeModel;

    internal class DummyAssemblerFactory : IObjectAssemblerFactory
    {
        private readonly WiringContext wiringContext;

        public DummyAssemblerFactory(WiringContext wiringContext)
        {
            this.wiringContext = wiringContext;
        }

        public IObjectAssembler CreateAssembler(ObjectAssemblerSettings settings)
        {
            var templateHostingObjectAssembler = new TemplateHostingObjectAssembler(new SuperObjectAssembler(wiringContext, new TopDownMemberValueContext(), settings));
            templateHostingObjectAssembler.AddDeferredLoader<DataTemplate>(template => template.Content, new DummyDeferredLoader());
            return templateHostingObjectAssembler;
        }

        public IObjectAssembler CreateAssembler()
        {
            throw new NotImplementedException();
        }
    }
}