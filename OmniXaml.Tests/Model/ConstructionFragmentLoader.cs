namespace OmniXaml.Tests.Model
{
    using Metadata;

    public class ConstructionFragmentLoader : IConstructionFragmentLoader
    {
        public object Load(ConstructionNode node, IObjectBuilder builder, TrackingContext trackingContext)
        {
            return new TemplateContent(node, builder, trackingContext);
        }
    }
}