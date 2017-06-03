namespace OmniXaml.Metadata
{
    public interface IConstructionFragmentLoader
    {
        object Load(ConstructionNode node, INodeToObjectBuilder builder, BuilderContext context);
    }
}