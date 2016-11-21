namespace OmniXaml
{
    public interface IXamlToTreeParser
    {
        ParseResult Parse(string xml, IPrefixAnnotator annotator);
    }

    public class ParseResult
    {
        public ConstructionNode Root { get; set; }
        public IPrefixAnnotator PrefixAnnotator { get; set; }
    }
}