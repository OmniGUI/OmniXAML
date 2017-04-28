namespace OmniXaml.Services
{
    public interface IFullObjectBuilder
    {
        object Build(ConstructionNode node);
    }
}