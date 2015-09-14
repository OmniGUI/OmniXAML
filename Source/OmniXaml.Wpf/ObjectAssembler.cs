namespace OmniXaml.Wpf
{
    using System;
    using OmniXaml.ObjectAssembler;
    using OmniXaml.ObjectAssembler.Commands;

    public class ObjectAssembler : IObjectAssembler
    {
        private readonly TemplateHostingObjectAssembler objectAssembler;

        public ObjectAssembler(IWiringContext wiringContext, ITopDownValueContext topDownValueContext, ObjectAssemblerSettings objectAssemblerSettings = null)
        {
            var mapping = new DeferredLoaderMapping();
            mapping.Map<DataTemplate>(template => template.AlternateTemplateContent, new DeferredLoader());

            objectAssembler = new TemplateHostingObjectAssembler(new OmniXaml.ObjectAssembler.ObjectAssembler(wiringContext, topDownValueContext, objectAssemblerSettings), mapping);            
        }        

        public object Result => objectAssembler.Result;
        public EventHandler<XamlSetValueEventArgs> XamlSetValueHandler { get; set; }
        public IWiringContext WiringContext => objectAssembler.WiringContext;
        public void Process(XamlInstruction instruction)
        {
            objectAssembler.Process(instruction);
        }

        public void OverrideInstance(object instance)
        {
            objectAssembler.OverrideInstance(instance);
        }
    }
}