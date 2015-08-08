namespace OmniXaml.Parsers.XamlNodes
{
    using System.Collections.Generic;

    public interface IXamlNodesPullParser
    {
        IEnumerable<XamlNode> Parse(IEnumerable<ProtoXamlNode> protoNodes);
    }
}