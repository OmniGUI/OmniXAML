namespace OmniXaml
{
    public class SuperContext
    {
        public SuperContext(TrackingContext trackingContext, StaticContext staticContext)
        {
            TrackingContext = trackingContext;
            StaticContext = staticContext;
        }

        public TrackingContext TrackingContext { get; }

        public StaticContext StaticContext { get; }
    }
}