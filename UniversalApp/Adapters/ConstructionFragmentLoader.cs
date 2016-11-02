namespace Yuniversal.Adapters
{
    using OmniXaml;
    using OmniXaml.Metadata;

    public class ConstructionFragmentLoader : IConstructionFragmentLoader
    {
        public object Load(ConstructionNode node, IObjectBuilder builder, TrackingContext trackingContext)
        {
            return new TemplateContent(node, builder, trackingContext);
        }
    }
}