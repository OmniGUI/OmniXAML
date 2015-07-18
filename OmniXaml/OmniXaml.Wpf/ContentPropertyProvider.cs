namespace OmniXaml.Wpf
{
    using System;
    using System.Reflection;
    using System.Windows.Markup;
    using Builder;
    using Catalogs;

    internal class ContentPropertyProvider : IContentPropertyProvider
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