namespace Yuniversal.Context
{
    using System;
    using System.Reflection;
    using Windows.UI.Xaml.Controls;
    using OmniXaml;

    public class ContentPropertyProvider : IContentPropertyProvider
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
            if (typeof(TextBlock) == type)
            {
                return "Text";
            }

            return null;
        }
    }
}