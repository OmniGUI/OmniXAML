namespace AvaloniaApp.Context
{
    using System;
    using System.Linq;
    using System.Reflection;
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
            var contentProperty = type.GetRuntimeProperties()
               .First(info => info.GetCustomAttribute(typeof(Avalonia.Metadata.ContentAttribute)) != null);

            return contentProperty.Name;
        }
    }
}