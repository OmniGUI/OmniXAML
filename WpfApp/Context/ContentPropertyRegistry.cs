namespace WpfApplication1.Context
{
    using System;
    using System.Reflection;
    using System.Windows.Markup;
    using OmniXaml.Metadata;

    public class MetadataProvider : IMetadataProvider
    {
        public Metadata Get(Type type)
        {
            return new Metadata
            {
                ContentProperty = GetContentProperty(type),
            };
        }

        private string GetContentProperty(Type type)
        {
            var contentPropertyAttribute = type.GetTypeInfo().GetCustomAttribute<ContentPropertyAttribute>();
            return contentPropertyAttribute?.Name;
        }
    }
}