namespace OmniXaml
{
    public interface IXamlLoader
    {
        ConstructionResult Load(string xaml);
        ConstructionResult Load(string xaml, object rootInstance);
    }
}