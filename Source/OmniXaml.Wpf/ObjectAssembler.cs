namespace OmniXaml.Wpf
{
    using System;
    using OmniXaml.ObjectAssembler;
    using OmniXaml.ObjectAssembler.Commands;

    public class ObjectAssembler : IObjectAssembler
    {
        public ITypeContext TypeContext { get; set; }
        private readonly TemplateHostingObjectAssembler objectAssembler;

        public ObjectAssembler(ITypeContext typeContext, ITopDownValueContext topDownValueContext, ObjectAssemblerSettings objectAssemblerSettings = null)
        {
            TypeContext = typeContext;
            var mapping = new DeferredLoaderMapping();
            mapping.Map<DataTemplate>(template => template.AlternateTemplateContent, new DeferredLoader());

            objectAssembler = new TemplateHostingObjectAssembler(new OmniXaml.ObjectAssembler.ObjectAssembler(typeContext, topDownValueContext, objectAssemblerSettings), mapping);            
        }        

        public object Result => objectAssembler.Result;
        public EventHandler<XamlSetValueEventArgs> XamlSetValueHandler { get; set; }

        public InstanceLifeCycleHandler InstanceLifeCycleHandler
        {
            get { return objectAssembler.InstanceLifeCycleHandler; }
            set { objectAssembler.InstanceLifeCycleHandler = value; }
        }

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