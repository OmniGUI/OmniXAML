namespace OmniXaml
{
    using System.Collections.Generic;
    using Typing;

    public class XamlReader : IXamlReader
    {
        private readonly IEnumerator<XamlNode> nodeStream;

        private XamlNode current;

        private readonly XamlNode endOfStreamNode = new XamlNode(XamlNode.InternalNodeType.EndOfStream);

        public XamlReader(IEnumerable<XamlNode> xamlNodes)
        {
            nodeStream = xamlNodes.GetEnumerator();
        }

        public XamlNodeType NodeType => current.NodeType;

        public bool IsEof => current.IsEof;
        public XamlType Type => current.XamlType;
        public XamlMember Member => current.Member;
        public NamespaceDeclaration Namespace => current.NamespaceDeclaration;
        public object Value => current.Value;

        public bool Read()
        {
            while (nodeStream.MoveNext())
            {
                current = nodeStream.Current;
                if (current.NodeType == XamlNodeType.None)
                {
                    if (current.IsEof)
                    {
                        return !IsEof;
                    }
                }

                if (current.NodeType != XamlNodeType.None)
                {
                    return !IsEof;
                }
            }

            current = endOfStreamNode;

            return !IsEof;
        }
    }
}