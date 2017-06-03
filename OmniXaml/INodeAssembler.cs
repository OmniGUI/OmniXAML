namespace OmniXaml
{
    public interface INodeAssembler
    {
        void Assemble(ConstructionNode node, INodeToObjectBuilder nodeToObjectBuilder, ConstructionNode parent = null, BuilderContext context = null);
    }
}