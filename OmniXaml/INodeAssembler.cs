namespace OmniXaml
{
    public interface INodeAssembler
    {
        void Assemble(ConstructionNode node, ConstructionNode parent = null);
    }
}