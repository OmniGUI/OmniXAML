namespace OmniXaml
{
    using System.Collections.Generic;
    using Ambient;

    public interface IBuildContext
    {
        INamescopeAnnotator NamescopeAnnotator { get; }
        IPrefixAnnotator PrefixAnnotator { get; set; }
        IAmbientRegistrator AmbientRegistrator { get; }
        IInstanceLifecycleSignaler InstanceLifecycleSignaler { get; }
        IDictionary<string, object> Bag { get; set; }
        ConstructionNode CurrentNode { get; set; }
        IPrefixedTypeResolver PrefixedTypeResolver { get; set; }
        ConstructionNode Root { get; set; }
    }
}