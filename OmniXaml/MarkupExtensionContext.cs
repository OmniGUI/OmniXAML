namespace OmniXaml
{
    public class MarkupExtensionContext
    {

        public StaticContext StaticContext { get; }
        public Assignment Assignment { get; }
        public ITypeDirectory TypeDirectory { get; }
        public TrackingContext TrackingContext { get; set; }

        public MarkupExtensionContext(Assignment assignment, StaticContext staticContext, ITypeDirectory directory, TrackingContext trackingContext)
        {
            TypeDirectory = directory;
            TrackingContext = trackingContext;
            StaticContext = staticContext;
            Assignment = assignment;
        }
    }
}