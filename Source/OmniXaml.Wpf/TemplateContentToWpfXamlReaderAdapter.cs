namespace OmniXaml.Wpf
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xaml;
    using Services;
    using SystemXamlNsDeclaration = System.Xaml.NamespaceDeclaration;
    using SystemXamlType = System.Xaml.XamlType;
    using SystemXamlNodeType = System.Xaml.XamlNodeType;

    public class TemplateContentToWpfXamlReaderAdapter : XamlReader, IXamlIndexingReader
    {
        private readonly IEnumerator<XamlInstruction> nodeStream;
        private readonly TemplateContent templateContent;
        private bool hasReadSuccess;

        public TemplateContentToWpfXamlReaderAdapter(TemplateContent templateContent,
            AutoInflatingTypeFactory autoInflatingTypeFactory,
            XamlSchemaContext xamlSchemaContext)
        {
            this.templateContent = templateContent;
            SchemaContext = xamlSchemaContext;

            var hydrator = new Hydrator(autoInflatingTypeFactory.Inflatables, templateContent.Context);
            var hydratedNodes = hydrator.Hydrate(templateContent.Nodes);

            nodeStream = hydratedNodes.GetEnumerator();
        }

        public override SystemXamlNodeType NodeType => XamlTypeConversion.ToWpf(Current.InstructionType);
        public override bool IsEof => !hasReadSuccess;
        public override SystemXamlNsDeclaration Namespace => XamlTypeConversion.ToWpf(Current.NamespaceDeclaration);
        public override SystemXamlType Type => XamlTypeConversion.ToWpf(Current.XamlType, SchemaContext);
        private XamlInstruction Current => nodeStream.Current;
        public override object Value => Current.Value;
        public override XamlMember Member => XamlTypeConversion.ToWpf(Current.Member, SchemaContext);
        public override XamlSchemaContext SchemaContext { get; }

        public int Count => templateContent.Nodes.Count();
        public int CurrentIndex { get; set; } = -1;

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