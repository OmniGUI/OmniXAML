namespace OmniXaml.Wpf
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Controls;
    using System.Xaml;
    using SystemXamlNsDeclaration = System.Xaml.NamespaceDeclaration;
    using SystemXamlType = System.Xaml.XamlType;
    using SystemXamlNodeType = System.Xaml.XamlNodeType;
    using XamlReader = System.Xaml.XamlReader;

    public class SuperLoaderXamlXmlReader : XamlReader, IXamlIndexingReader
    {
        private readonly IEnumerator<XamlNode> nodeStream;
        private readonly TemplateContent templateContent;
        private readonly WpfInflatableTypeFactory wpfInflatableTypeFactory;
        private bool hasReadSuccess;

        public SuperLoaderXamlXmlReader(TemplateContent templateContent, WpfInflatableTypeFactory wpfInflatableTypeFactory)
        {
            this.templateContent = templateContent;
            this.wpfInflatableTypeFactory = wpfInflatableTypeFactory;
            SchemaContext = new XamlSchemaContext();

            var inflater = new Inflater(wpfInflatableTypeFactory.Inflatables, WiringContextFactory.GetContext(wpfInflatableTypeFactory));
            var inflated = inflater.Inflate(templateContent.Nodes);

            nodeStream = inflated.GetEnumerator();
        }

        public override SystemXamlNodeType NodeType => XamlTypeConversion.ToWpf(nodeStream.Current.NodeType);
        public override bool IsEof => !hasReadSuccess;
        public override SystemXamlNsDeclaration Namespace => XamlTypeConversion.ToWpf(nodeStream.Current.NamespaceDeclaration);
        public override SystemXamlType Type => XamlTypeConversion.ToWpf(nodeStream.Current.XamlType, SchemaContext);
        public override object Value => nodeStream.Current.Value;
        public override XamlMember Member => XamlTypeConversion.ToWpf(nodeStream.Current.Member, SchemaContext);
        public override XamlSchemaContext SchemaContext { get; }
        public int Count => templateContent.Nodes.Count();
        public int CurrentIndex { get; set; }

        public override bool Read()
        {
            hasReadSuccess = nodeStream.MoveNext();
            if (hasReadSuccess)
            {
                CurrentIndex++;
            }

            return hasReadSuccess;
        }
    }
}