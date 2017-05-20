namespace OmniXaml.Services
{
    public interface INodeToObjectBuilder
    {
        object Build(ConstructionNode node);
    }
}