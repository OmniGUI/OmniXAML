namespace OmniXaml
{
    using System;
    using Catalogs;

    public interface IContentPropertyProvider
    {
        string GetContentPropertyName(Type type);

        void AddCatalog(ContentPropertyCatalog catalog);
    }
}