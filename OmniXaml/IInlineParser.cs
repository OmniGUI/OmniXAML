namespace OmniXaml
{
    public interface IInlineParser
    {

        bool CanParse(string inline);
        ConstructionNode Parse(string inline);
    }
}