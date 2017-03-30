namespace OmniXaml
{
    public interface IXamlLoader
    {
        object Load(string xaml, object rootInstance = null);
    }
}