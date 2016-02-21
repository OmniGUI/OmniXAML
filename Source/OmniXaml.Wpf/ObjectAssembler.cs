namespace OmniXaml.Wpf
{
    using System;
    using OmniXaml.ObjectAssembler;
    using OmniXaml.ObjectAssembler.Commands;
    using TypeConversion;

    public class ObjectAssembler : IObjectAssembler
    {
        private readonly TemplateHostingObjectAssembler objectAssembler;

        public ObjectAssembler(IRuntimeTypeSource typeSource, ITopDownValueContext topDownValueContext, Settings settings = null)
        {
            TypeSource = typeSource;
            var mapping = new DeferredLoaderMapping();
            mapping.Map<DataTemplate>(template => template.AlternateTemplateContent, new DeferredLoader());

            var valueConnectionContext = new ValueContext(typeSource, topDownValueContext);

            objectAssembler = new TemplateHostingObjectAssembler(
                new OmniXaml.ObjectAssembler.ObjectAssembler(
                    typeSource,
                    valueConnectionContext,
                    settings),
                mapping);
        }

        public IRuntimeTypeSource TypeSource { get; }
        public ITopDownValueContext TopDownValueContext => objectAssembler.TopDownValueContext;

        public IInstanceLifeCycleListener LifecycleListener => objectAssembler.LifecycleListener;

        public object Result => objectAssembler.Result;
        public EventHandler<XamlSetValueEventArgs> XamlSetValueHandler { get; set; }

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