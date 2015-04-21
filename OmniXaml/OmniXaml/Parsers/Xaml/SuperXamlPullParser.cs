namespace OmniXaml.Parsers.Xaml
{
    using System.Collections.Generic;
    using ProtoParser;

    public class SuperXamlPullParser
    {
        public SuperXamlPullParser()
        {
        }

        public IEnumerable<XamlNode> Parse(IEnumerable<ProtoXamlNode> protoNodes)
        {
            foreach (var protoXamlNode in protoNodes)
            {
                foreach (var xamlNode in ProcessProtoNode(protoXamlNode))
                {
                    yield return xamlNode;
                }
            }
            
        }

        private IEnumerable<XamlNode> ProcessProtoNode(ProtoXamlNode protoXamlNode)
        {
            switch (protoXamlNode.NodeType)
            {
                case ProtoNodeType.PrefixDefinition:
                    yield return new XamlNode(XamlNodeType.NamespaceDeclaration, new NamespaceDeclaration(protoXamlNode.Namespace, protoXamlNode.Prefix));
                    break;
                case ProtoNodeType.EmptyElement:

                    var type = protoXamlNode.XamlType;

                    yield return new XamlNode(XamlNodeType.StartObject, type);
                    yield return new XamlNode(XamlNodeType.EndObject);

                    break;
                default:
                    yield break;                    
            }
        }
    }
}