namespace OmniXaml
{
    public class ValueContext
    {

        public ObjectBuilderContext ObjectBuilderContext { get; }
        public Assignment Assignment { get; }
        public ITypeDirectory TypeDirectory { get; }
        public TrackingContext TrackingContext { get; set; }

        public ValueContext(Assignment assignment, ObjectBuilderContext objectBuilderContext, ITypeDirectory directory, TrackingContext trackingContext)
        {
            TypeDirectory = directory;
            TrackingContext = trackingContext;
            ObjectBuilderContext = objectBuilderContext;
            Assignment = assignment;
        }
    }
}