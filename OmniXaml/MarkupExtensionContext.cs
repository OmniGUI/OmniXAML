namespace OmniXaml
{
    public class MarkupExtensionContext
    {

        public ConstructionContext ConstructionContext { get; }
        public Assignment Assignment { get; }
        public ITypeDirectory TypeDirectory { get; }
        public TrackingContext TrackingContext { get; set; }

        public MarkupExtensionContext(Assignment assignment, ConstructionContext constructionContext, ITypeDirectory directory, TrackingContext trackingContext)
        {
            TypeDirectory = directory;
            TrackingContext = trackingContext;
            ConstructionContext = constructionContext;
            Assignment = assignment;
        }
    }
}