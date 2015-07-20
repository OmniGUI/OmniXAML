namespace OmniXaml.Wpf
{
    using System.Windows;
    using System.Windows.Markup;

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
                var p = new TemplateContentLoader();
                var load = p.Load(new SuperLoaderXamlXmlReader(value), new ServiceLocator(null));
                Template = (System.Windows.TemplateContent)load;
                LoadContent();
            }
        }
    }
}