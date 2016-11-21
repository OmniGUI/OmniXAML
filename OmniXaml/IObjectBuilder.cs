namespace OmniXaml
{
    public interface IObjectBuilder
    {
        object Inflate(ConstructionNode node, BuildContext buildContext, object instance = null);
    }
}