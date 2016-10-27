namespace OmniXaml
{
    public interface IObjectBuilder
    {
        object Create(ConstructionNode node, INamescopeAnnotator annotator);
        object Create(ConstructionNode node, object instance, INamescopeAnnotator annotator);
    }
}