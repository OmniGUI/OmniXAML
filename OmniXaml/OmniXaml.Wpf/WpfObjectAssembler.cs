namespace OmniXaml.Wpf
{
    using System;
    using Assembler;
    using NewAssembler;

    public class WpfObjectAssembler : IObjectAssembler
    {
        private readonly TemplateHostingObjectAssembler objectAssembler;

        public WpfObjectAssembler(WiringContext wiringContext)
        {
            objectAssembler = new TemplateHostingObjectAssembler(new ObjectAssembler(wiringContext));
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