namespace OmniXaml
{
    public interface IObjectBuilder
    {
        object Create(ConstructionNode node, BuildContext buildContext);
        object Create(ConstructionNode node, object instance, BuildContext buildContext);
    }
}