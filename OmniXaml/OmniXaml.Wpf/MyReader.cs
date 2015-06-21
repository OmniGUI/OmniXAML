namespace OmniXaml.Wpf
{
    using System.Collections.Generic;
    using System.Windows.Controls;
    using System.Xaml;

    public class MyReader : XamlReader, IXamlIndexingReader
    {
        private readonly IEnumerator<XamlNode> nodeStream;
        private bool isEof;
        private readonly XamlSchemaContext xamlSchemaContext = new XamlSchemaContext(new[] { typeof(TextBlock).Assembly });

        public MyReader(WpfTemplateContent templateContent)
        {
            nodeStream = templateContent.Nodes.GetEnumerator();
            CurrentIndex = 0;
        }

        public override bool Read()
        {
            var moveNext = nodeStream.MoveNext();
            if (!moveNext)
            {
                isEof = true;
            }
            return moveNext;
        }

        public XamlNode Current => nodeStream.Current;

        public override System.Xaml.XamlNodeType NodeType => Current.NodeType.ToWpf();
        public override bool IsEof => isEof;
        public override NamespaceDeclaration Namespace => Current.NamespaceDeclaration.ToWpf();
        public override XamlType Type => Current.XamlType.ToWpf(xamlSchemaContext);
        public override object Value => Current.Value;
        public override XamlMember Member => Current.Member.ToWpf(xamlSchemaContext);
        public override XamlSchemaContext SchemaContext => xamlSchemaContext;
        public int Count => 1;
        public int CurrentIndex { get; set; }
    }
}