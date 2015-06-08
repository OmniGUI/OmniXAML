namespace OmniXaml
{
    using System;
    using Builder;
    using Catalogs;

    public interface IContentPropertyProvider
    {
        string GetContentPropertyName(Type type);

        void AddCatalog(ContentPropertyCatalog catalog);
        void Add(ContentPropertyDefinition contentPropertyDefinition);
    }
}