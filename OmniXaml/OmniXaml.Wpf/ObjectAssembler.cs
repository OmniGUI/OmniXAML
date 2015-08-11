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
            objectAssembler =
                new TemplateHostingObjectAssembler(new NewAssembler.ObjectAssembler(wiringContext, new TopDownMemberValueContext(), objectAssemblerSettings));

            objectAssembler.AddDeferredLoader<DataTemplate>(template => template.AlternateTemplateContent, new DeferredLoader());
        }        

        public object Result => objectAssembler.Result;
        public EventHandler<XamlSetValueEventArgs> XamlSetValueHandler { get; set; }
        public WiringContext WiringContext => objectAssembler.WiringContext;
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