namespace OmniXaml
{
    using System.Collections.Generic;
    using Ambient;

    public class TrackingContext
    {
        public TrackingContext(INamescopeAnnotator annotator, IAmbientRegistrator ambientRegistrator, IInstanceLifecycleSignaler instanceLifecycleSignaler)
        {
            Annotator = annotator;
            AmbientRegistrator = ambientRegistrator;
            InstanceLifecycleSignaler = instanceLifecycleSignaler;
        }

        public INamescopeAnnotator Annotator { get; }

        public IAmbientRegistrator AmbientRegistrator { get; }
        public IInstanceLifecycleSignaler InstanceLifecycleSignaler { get; }
        public IDictionary<string, object> Bag { get; set; } = new Dictionary<string, object>();
    }
}