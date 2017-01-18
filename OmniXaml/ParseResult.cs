namespace OmniXaml
{
    public class ParseResult
    {
        public ConstructionNode Root { get; set; }
        public IPrefixAnnotator PrefixAnnotator { get; set; }
    }
}