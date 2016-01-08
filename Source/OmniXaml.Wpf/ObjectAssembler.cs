namespace OmniXaml.Wpf
{
    using System;
    using OmniXaml.ObjectAssembler;
    using OmniXaml.ObjectAssembler.Commands;

    public class ObjectAssembler : IObjectAssembler
    {
        public IRuntimeTypeSource TypeSource { get; }
        public ITopDownValueContext TopDownValueContext => objectAssembler.TopDownValueContext;
        private readonly TemplateHostingObjectAssembler objectAssembler;

        public ObjectAssembler(IRuntimeTypeSource typeSource, ITopDownValueContext topDownValueContext, ObjectAssemblerSettings objectAssemblerSettings = null)
        {
            TypeSource = typeSource;
            var mapping = new DeferredLoaderMapping();
            mapping.Map<DataTemplate>(template => template.AlternateTemplateContent, new DeferredLoader());

            objectAssembler = new TemplateHostingObjectAssembler(new OmniXaml.ObjectAssembler.ObjectAssembler(typeSource, topDownValueContext, objectAssemblerSettings), mapping);            
        }        

        public object Result => objectAssembler.Result;
        public EventHandler<XamlSetValueEventArgs> XamlSetValueHandler { get; set; }

        public InstanceLifeCycleHandler InstanceLifeCycleHandler
        {
            get { return objectAssembler.InstanceLifeCycleHandler; }
            set { objectAssembler.InstanceLifeCycleHandler = value; }
        }

        public void Process(Instruction instruction)
        {
            objectAssembler.Process(instruction);
        }

        public void OverrideInstance(object instance)
        {
            objectAssembler.OverrideInstance(instance);
        }      
    }
}