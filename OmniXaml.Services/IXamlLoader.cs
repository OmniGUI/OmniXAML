namespace OmniXaml.Services
{
    public interface IXamlLoader
    {
        object Load(string xaml, object intance = null);
    }
}