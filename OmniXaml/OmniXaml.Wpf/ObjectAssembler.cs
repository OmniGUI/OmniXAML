namespace OmniXaml.Wpf
{
    using System;
    using Assembler;
    using NewAssembler;

    public class ObjectAssembler : IObjectAssembler
    {
        private readonly TemplateHostingObjectAssembler objectAssembler;

        public ObjectAssembler(WiringContext wiringContext, ObjectAssemblerSettings objectAssemblerSettings = null)
        {
            objectAssembler = new TemplateHostingObjectAssembler(new SuperObjectAssembler(wiringContext, objectAssemblerSettings));
            objectAssembler.DeferredAssembler<DataTemplate>(template => template.AlternateTemplateContent, new DeferredObjectAssembler());
        }        

        public object Result => objectAssembler.Result;
        public EventHandler<XamlSetValueEventArgs> XamlSetValueHandler { get; set; }
        public WiringContext WiringContext => objectAssembler.WiringContext;
        public void Process(XamlNode node)
        {
            objectAssembler.Process(node);
        }

        public void OverrideInstance(object instance)
        {
            objectAssembler.OverrideInstance(instance);
        }
    }
}