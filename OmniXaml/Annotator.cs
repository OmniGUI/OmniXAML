namespace OmniXaml
{
    using System.Collections.Generic;
    using Metadata;
    using Tests;

    public class Annotator
    {
        private readonly IMetadataProvider metadataProvider;

        public Annotator(IMetadataProvider metadataProvider)
        {
            this.metadataProvider = metadataProvider;            
        }

        private readonly IDictionary<object, NamescopeX> instancesInNamescopes = new Dictionary<object, NamescopeX>();
        
        public void TrackInstance(string name, object instance)
        {
            instancesInNamescopes.Add(instance, CurrentNamescope);

            if (name!=null)
            {
                CurrentNamescope.Add(name, instance);
            }

            if (IsNamescope(instance))
            {
                CurrentNamescope = CurrentNamescope.AddNamescope();
            }
        }

        private bool IsNamescope(object instance)
        {
            return metadataProvider.Get(instance.GetType()).IsNamescope;
        }

        public void RegisterInsideCurrentNamescope(string name, object instance)
        {
            instancesInNamescopes.Add(instance, CurrentNamescope);
            CurrentNamescope.Add(name, instance);            
        }

        public NamescopeX GetNamescope(object instance)
        {
            return instancesInNamescopes[instance];
        }

        public NamescopeX CurrentNamescope { get; private set; } = new NamescopeX();
    }
}