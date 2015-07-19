namespace OmniXaml.Wpf
{
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;
    using System.Xaml;
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
                //var p = new TemplateContentLoader();
                //var load = p.Load(new SuperLoaderXamlXmlReader(value), new ServiceLocator(null) );
                //this.Template = (System.Windows.TemplateContent) load;

                VisualTree = ConvertTemplateContentIntoElementFactory(value);
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

    public class SuperLoaderXamlXmlReader : XamlReader
    {
        public SuperLoaderXamlXmlReader(TemplateContent value)
        {           
            SchemaContext = new XamlSchemaContext();
            //var nodeStream
        }

        public override bool Read()
        {
            throw new System.NotImplementedException();
        }

        public override XamlNodeType NodeType { get; }
        public override bool IsEof { get; }
        public override NamespaceDeclaration Namespace { get; }
        public override XamlType Type { get; }
        public override object Value { get; }
        public override XamlMember Member { get; }
        public override XamlSchemaContext SchemaContext { get; }
    }
}