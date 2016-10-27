namespace OmniXaml
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using Glass.Core.Glass.Core;

    public class NamescopeAnnotator : INamescopeAnnotator
    {
        private readonly StackingLinkedList<Namescope> namescopes = new StackingLinkedList<Namescope>();
        private readonly IDictionary<object, Namescope> mappings = new ConcurrentDictionary<object, Namescope>();

        public void NewInstance(object instance)
        {
            if (instance.GetType().Name == "Window")
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