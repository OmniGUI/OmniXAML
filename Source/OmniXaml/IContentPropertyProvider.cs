namespace OmniXaml
{
    using System;
    using Builder;

    public interface IContentPropertyProvider
    {
        string GetContentPropertyName(Type type);

        void Add(ContentPropertyDefinition contentPropertyDefinition);
    }
}