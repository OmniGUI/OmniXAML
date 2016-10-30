namespace OmniXaml
{
    public interface IObjectBuilder
    {
        object Create(ConstructionNode node, CreationContext creationContext);
        object Create(ConstructionNode node, object instance, CreationContext creationContext);
    }
}