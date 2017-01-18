namespace OmniXaml
{
    public interface IXamlToTreeParser
    {
        ParseResult Parse(string xml, IPrefixAnnotator annotator);
    }
}