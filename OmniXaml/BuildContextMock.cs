namespace OmniXaml
{
    using System.Collections.Generic;
    using Ambient;

    public class BuildContextMock : IBuildContext
    {
        public INamescopeAnnotator NamescopeAnnotator { get; }
        public IPrefixAnnotator PrefixAnnotator { get; set; }
        public IAmbientRegistrator AmbientRegistrator { get; }
        public IInstanceLifecycleSignaler InstanceLifecycleSignaler { get; }
        public IDictionary<string, object> Bag { get; set; }
        public ConstructionNode CurrentNode { get; set; }
        public IPrefixedTypeResolver PrefixedTypeResolver { get; set; }
        public ConstructionNode Root { get; set; }
    }
}