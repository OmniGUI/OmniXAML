namespace OmniXaml.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;
    using System.Xaml;
    using NamespaceDeclaration = OmniXaml.NamespaceDeclaration;
    using XamlNodeType = OmniXaml.XamlNodeType;
    using XamlReader = System.Xaml.XamlReader;

    [ContentProperty("AlternateTemplateContent")]
    public class DataTemplate : System.Windows.DataTemplate
    {
        private TemplateContent alternateTemplateContent;

        // ReSharper disable once EmptyConstructor
        public DataTemplate()
        {
        }

        public TemplateContent AlternateTemplateContent
        {
            get { return alternateTemplateContent; }
            set
            {
                alternateTemplateContent = value;
                var p = new TemplateContentLoader();
                var load = p.Load(new SuperLoaderXamlXmlReader(value), new ServiceLocator(null));
                Template = (System.Windows.TemplateContent)load;
                LoadContent();

                //VisualTree = ConvertTemplateContentIntoElementFactory(value);
            }
        }

        private static FrameworkElementFactory ConvertTemplateContentIntoElementFactory(TemplateContent templateContent)
        {
            var visualTree = (DependencyObject)templateContent.Load();
            var frameworkElementFactory = new FrameworkElementFactory(visualTree.GetType());

            foreach (var dependencyProperty in DependencyObjectHelper.GetDependencyProperties(visualTree))
            {
                var binding = BindingOperations.GetBinding(visualTree, dependencyProperty);

                var value = visualTree.GetValue(dependencyProperty);
                if (binding != null)
                {
                    value = binding;
                }

                frameworkElementFactory.SetValue(dependencyProperty, value);
            }

            return frameworkElementFactory;
        }
    }

    public class SuperLoaderXamlXmlReader : XamlReader, IXamlIndexingReader
    {
        private readonly TemplateContent templateContent;
        private readonly IEnumerator<XamlNode> nodeStream;
        private bool hasReadSuccess;

        public SuperLoaderXamlXmlReader(TemplateContent templateContent)
        {
            this.templateContent = templateContent;
            SchemaContext = new XamlSchemaContext();
            nodeStream = templateContent.Nodes.GetEnumerator();
        }

        public override bool Read()
        {
            hasReadSuccess = nodeStream.MoveNext();
            if (hasReadSuccess)
            {
                CurrentIndex++;
            }

            return hasReadSuccess;
        }

        public override System.Xaml.XamlNodeType NodeType => Convert(nodeStream.Current.NodeType);

        private System.Xaml.XamlNodeType Convert(XamlNodeType nodeType)
        {
            return (System.Xaml.XamlNodeType)Enum.Parse(typeof(System.Xaml.XamlNodeType), nodeType.ToString());
        }

        public override bool IsEof => !hasReadSuccess;
        public override System.Xaml.NamespaceDeclaration Namespace => Convert(nodeStream.Current.NamespaceDeclaration);

        private System.Xaml.NamespaceDeclaration Convert(NamespaceDeclaration namespaceDeclaration)
        {
            return new System.Xaml.NamespaceDeclaration(namespaceDeclaration.Namespace, namespaceDeclaration.Prefix);
        }

        public override XamlType Type => Convert(nodeStream.Current.XamlType);

        private XamlType Convert(Typing.XamlType xamlType)
        {
            return new XamlType(xamlType.UnderlyingType, SchemaContext);
        }

        public override object Value => nodeStream.Current.Value;
        public override XamlMember Member => Convert(nodeStream.Current.Member);

        private XamlMember Convert(Typing.XamlMember member)
        {
            var declaringType = Convert(member.DeclaringType);
            var xamlMember = member.IsAttachable == false ? declaringType.GetMember(member.Name) : declaringType.GetAttachableMember(member.Name);
            return xamlMember;
        }

        public override XamlSchemaContext SchemaContext { get; }
        public int Count => templateContent.Nodes.Count();
        public int CurrentIndex { get; set; }
    }
}