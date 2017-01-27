using Zafiro.Core;

namespace OmniXaml
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using Metadata;

    public class NamescopeAnnotator : INamescopeAnnotator
    {
        private readonly IMetadataProvider metadataProvider;
        private readonly StackingLinkedList<Namescope> namescopes = new StackingLinkedList<Namescope>();
        private readonly IDictionary<object, Namescope> mappings = new ConcurrentDictionary<object, Namescope>();

        public NamescopeAnnotator(IMetadataProvider metadataProvider)
        {
            this.metadataProvider = metadataProvider;
        }

        public void TrackNewInstance(object instance)
        {
            if (metadataProvider.Get(instance.GetType()).IsNamescope)
            {
                var namescope = new Namescope(instance);
                mappings.Add(instance, namescope);
                namescopes.Push(namescope);                
            }
        }

        public void RegisterName(string name, object instance)
        {
            namescopes.CurrentValue.Register(name, instance);
        }

        public Namescope GetNamescope(object instance)
        {
            return mappings[instance];
        }

        public object Find(string name, object parent)
        {
            return GetNamescope(parent).Find(name);
        }
    }
}