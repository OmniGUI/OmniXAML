namespace WpfApplication1.Context
{
    public interface IXamlLoader
    {
        object Load(string xaml);
        object Load(string xaml, object intance);
    }
}