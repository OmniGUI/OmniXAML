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
        public IPrefixAnnotator PrefixAnnotator { get; set; }

        public IAmbientRegistrator AmbientRegistrator { get; }
        public IInstanceLifecycleSignaler InstanceLifecycleSignaler { get; }
        public IDictionary<string, object> Bag { get; set; } = new Dictionary<string, object>();
        public ConstructionNode CurrentNode { get; set; }
        public IPrefixedTypeResolver PrefixedTypeResolver { get; set; }
        public ConstructionNode Root { get; set; }
    }
}