namespace OmniXaml
{
    public interface IObjectBuilder
    {
        object Create(ConstructionNode node, TrackingContext trackingContext);
        object Create(ConstructionNode node, object instance, TrackingContext trackingContext);
    }
}