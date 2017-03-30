namespace OmniXaml.Services
{
    public interface IObjectBuilder
    {
        object Inflate(ConstructionNode ctNode);
    }
}