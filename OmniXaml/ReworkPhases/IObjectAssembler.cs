namespace OmniXaml.ReworkPhases
{
    public interface IObjectAssembler
    {
        InflatedNode Assemble(ConstructionNode node);
    }
}