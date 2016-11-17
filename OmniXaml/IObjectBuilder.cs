namespace OmniXaml
{
    public interface IObjectBuilder
    {
        object Inflate(ConstructionNode node, BuildContext buildContext);
        object Inflate(ConstructionNode node, object instance, BuildContext buildContext);
    }
}