namespace OmniXaml
{
    public interface IXamlToTreeParser
    {
        ConstructionNode Parse(string xml);
    }
}