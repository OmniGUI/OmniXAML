namespace OmniXaml.Wpf
{
    using System.Xaml;

    public class XamlSchemaContentProvider : IXamlSchemaContextProvider
    {
        public XamlSchemaContentProvider(XamlSchemaContext schemaContext)
        {
            SchemaContext = schemaContext;
        }

        public XamlSchemaContext SchemaContext { get; }
    }
}