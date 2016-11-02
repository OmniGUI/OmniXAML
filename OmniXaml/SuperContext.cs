namespace OmniXaml
{
    public class SuperContext
    {
        public SuperContext(TrackingContext trackingContext, ObjectBuilderContext objectBuilderContext)
        {
            TrackingContext = trackingContext;
            ObjectBuilderContext = objectBuilderContext;
        }

        public TrackingContext TrackingContext { get; }

        public ObjectBuilderContext ObjectBuilderContext { get; }
    }
}