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
            return new XamlObjectWriter(new XamlSchemaContext());
        }
    }
}