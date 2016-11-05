namespace OmniXaml.Metadata
{
    public interface IConstructionFragmentLoader
    {
        object Load(ConstructionNode node, IObjectBuilder builder, BuildContext context);
    }
}