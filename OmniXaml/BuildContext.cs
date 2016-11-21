namespace OmniXaml
{
    using System.Collections.Generic;
    using Ambient;

    public class BuildContext
    {
        public BuildContext(INamescopeAnnotator namescopeAnnotator, IAmbientRegistrator ambientRegistrator, IInstanceLifecycleSignaler instanceLifecycleSignaler)
        {
            NamescopeAnnotator = namescopeAnnotator;
            AmbientRegistrator = ambientRegistrator;
            InstanceLifecycleSignaler = instanceLifecycleSignaler;
        }

        public INamescopeAnnotator NamescopeAnnotator { get; }

        public IAmbientRegistrator AmbientRegistrator { get; }
        public IInstanceLifecycleSignaler InstanceLifecycleSignaler { get; }
        public IDictionary<string, object> Bag { get; set; } = new Dictionary<string, object>();
        public ConstructionNode CurrentNode { get; set; }
    }
}