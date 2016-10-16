namespace Yuniversal.Context
{
    using System;
    using System.Reflection;
    using Windows.UI.Xaml.Controls;
    using OmniXaml;

    public class ContentPropertyRegistry : IContentPropertyRegistry
    {
        public string GetContentProperty(Type type)
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