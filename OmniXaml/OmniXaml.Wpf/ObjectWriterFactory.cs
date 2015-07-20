namespace OmniXaml.Wpf
{
    using System.Xaml;

    public class ObjectWriterFactory : IXamlObjectWriterFactory
    {
        public XamlObjectWriterSettings GetParentSettings()
        {
            return new XamlObjectWriterSettings();
        }

        public XamlObjectWriter GetXamlObjectWriter(XamlObjectWriterSettings settings)
        {
            var xamlSchemaContext = new XamlSchemaContext();
            var xamlObjectWriter = new XamlObjectWriter(xamlSchemaContext, settings);
            return xamlObjectWriter;
        }
    }
}