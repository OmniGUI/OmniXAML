namespace OmniXaml.Services
{
    public interface IObjectBuilder
    {
        object Build(ConstructionNode node);
    }
}