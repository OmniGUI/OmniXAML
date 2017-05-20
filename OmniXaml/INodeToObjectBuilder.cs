namespace OmniXaml
{
    public interface INodeToObjectBuilder
    {
        object Build(ConstructionNode node);
    }
}