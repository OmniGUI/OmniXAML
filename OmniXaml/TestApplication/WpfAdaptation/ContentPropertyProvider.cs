namespace TestApplication.WpfAdaptation
{
    using System;
    using System.Reflection;
    using System.Windows.Markup;
    using OmniXaml;
    using OmniXaml.Catalogs;

    public class ContentPropertyProvider : IContentPropertyProvider
    {
        public string GetContentPropertyName(Type type)
        {
            return type.GetTypeInfo().GetCustomAttribute<ContentPropertyAttribute>().Name;
        }

        public void AddCatalog(ContentPropertyCatalog catalog)
        {
            throw new NotImplementedException();
        }
    }
}