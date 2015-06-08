namespace TestApplication.WpfAdaptation
{
    using System;
    using System.Reflection;
    using System.Windows.Markup;
    using OmniXaml;
    using OmniXaml.Builder;
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

        public void Add(ContentPropertyDefinition contentPropertyDefinition)
        {
            throw new NotImplementedException();
        }
    }
}