namespace OmniXaml.Wpf
{
    using System.Windows;
    using System.Windows.Markup;
    using System.Xaml;

    [ContentProperty("AlternateTemplateContent")]
    // ReSharper disable once ClassNeverInstantiated.Global
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
            // ReSharper disable once UnusedMember.Global
            set
            {
                alternateTemplateContent = value;
                var loader = new TemplateContentLoader();
                var reader = new TemplateContentToWpfXamlReaderAdapter(value, new WpfTypeFactory(), new XamlSchemaContext());
                var template = loader.Load(reader, new ServiceLocator(null));
                Template = (System.Windows.TemplateContent)template;
                LoadContent();
            }
        }
    }
}