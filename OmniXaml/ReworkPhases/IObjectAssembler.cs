namespace OmniXaml.ReworkPhases
{
    public interface IObjectAssembler
    {
        void Assemble(ConstructionNode node, ConstructionNode parent = null);
    }
}