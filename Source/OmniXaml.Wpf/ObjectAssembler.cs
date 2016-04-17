namespace OmniXaml.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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
            var parsingDictionary = GetDictionary(settings);
            var valueConnectionContext = new ValueContext(typeSource, topDownValueContext, parsingDictionary);

            objectAssembler = new TemplateHostingObjectAssembler(
                new OmniXaml.ObjectAssembler.ObjectAssembler(
                    typeSource,
                    valueConnectionContext,
                    settings),
                mapping);
        }

        private static IReadOnlyDictionary<string, object> GetDictionary(Settings settings)
        {
            IReadOnlyDictionary<string, object> dict;
            if (settings != null)
            {
                dict = settings.ParsingContext;
            }
            else
            {
                dict = new ReadOnlyDictionary<string, object>(new Dictionary<string, object>());
            }

            return dict;
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