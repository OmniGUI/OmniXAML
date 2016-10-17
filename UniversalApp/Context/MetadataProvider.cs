namespace Yuniversal.Context
{
    using System;
    using System.Reflection;
    using Windows.UI.Xaml.Controls;
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
            if (typeof(Panel).IsAssignableFrom(type))
            {
                return "Children";
            }
            if (typeof(Page).IsAssignableFrom(type))
            {
                return "Content";
            }
            if (typeof(ContentControl).IsAssignableFrom(type))
            {
                return "Content";
            }
            if (typeof(ItemsControl).IsAssignableFrom(type))
            {
                return "Items";
            }
            if (typeof(TextBlock) == type)
            {
                return "Text";
            }
            if (typeof(TextBox) == type)
            {
                return "Text";
            }

            return null;
        }
    }
}