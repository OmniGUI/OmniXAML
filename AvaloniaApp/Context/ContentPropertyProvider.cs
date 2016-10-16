namespace AvaloniaApp.Context
{
    using System;
    using System.Linq;
    using System.Reflection;

    internal class ContentPropertyRegistry : OmniXaml.IContentPropertyRegistry
    {
        public string GetContentProperty(Type type)
        {
            var contentProperty = type.GetRuntimeProperties()
                .First(info => info.GetCustomAttribute(typeof(Avalonia.Metadata.ContentAttribute)) != null);
                
            return contentProperty.Name;
        }
    }
}