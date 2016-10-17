namespace OmniXaml
{
    public interface IObjectBuilder
    {
        object Create(ConstructionNode node);
    }
}