namespace OmniXaml.Parsers.XamlNodes
{
    using System.Collections.Generic;
    using Tests;

    public class OrderAwareXamlNodesPullParser : IXamlNodesPullParser
    {
        private readonly IXamlNodesPullParser pullParser;

        public OrderAwareXamlNodesPullParser(IXamlNodesPullParser pullParser)
        {
            this.pullParser = pullParser;
        }

        public IEnumerable<XamlNode> Parse(IEnumerable<ProtoXamlNode> protoNodes)
        {
            var nodeSorter = new MemberDependencyNodeSorter();
            var originalNodes = pullParser.Parse(protoNodes);
            var enumerator = originalNodes.GetEnumerator();
            return nodeSorter.Sort(enumerator);
        }
    }
}