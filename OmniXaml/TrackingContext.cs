namespace OmniXaml
{
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
    }
}