namespace OmniXaml.Tests.XamlObjectWriterTests
{
    using System;
    using System.Collections.Generic;
    using Typing;

    public class XamlReaderMock : IXamlReader
    {
        private readonly IEnumerator<XamlNode> nodeStream;

        public XamlReaderMock(IEnumerator<XamlNode> nodeStream)
        {
            this.nodeStream = nodeStream;
            SetNode(nodeStream.Current);
        }

        private void SetNode(XamlNode current)
        {
            NodeType = current.NodeType;
            Member = current.Member;
            Namespace = current.NamespaceDeclaration;
            Type = current.XamlType;
            Value = current.Value;
        }

        public XamlNodeType NodeType { get; private set; }
        public bool IsEof { get; private set; }
        public XamlType Type { get; private set; }
        public XamlMember Member { get; private set; }
        public NamespaceDeclaration Namespace { get; private set; }
        public object Value { get; private set; }
        public bool Read()
        {
            var moveNext = nodeStream.MoveNext();

            if (moveNext)
            {
                SetNode(nodeStream.Current);
            }

            return moveNext;
        }

        public void Skip()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public IXamlReader ReadSubtree()
        {
            throw new NotImplementedException();
        }
    }
}