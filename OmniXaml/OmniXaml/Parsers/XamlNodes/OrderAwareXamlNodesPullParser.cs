namespace OmniXaml.Parsers.XamlNodes
{
    using System.Collections.Generic;

    public class OrderAwareXamlNodesPullParser : IXamlNodesPullParser
    {
        private readonly IXamlNodesPullParser pullParser;

        public OrderAwareXamlNodesPullParser()
        {
        }

        public OrderAwareXamlNodesPullParser(IXamlNodesPullParser pullParser)
        {
            this.pullParser = pullParser;
        }

        public IEnumerable<XamlNode> Parse(IEnumerable<ProtoXamlNode> protoNodes)
        {
            return pullParser.Parse(protoNodes);
        }
    }
}