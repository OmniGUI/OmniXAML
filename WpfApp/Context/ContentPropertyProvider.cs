namespace WpfApplication1.Context
{
    using System;
    using System.Reflection;
    using System.Windows.Markup;
    using OmniXaml;

    internal class ContentPropertyProvider : IContentPropertyProvider
    {
        public string GetContentProperty(Type type)
        {
            var contentPropertyAttribute = type.GetTypeInfo().GetCustomAttribute<ContentPropertyAttribute>();
            return contentPropertyAttribute?.Name;
        }
    }
}