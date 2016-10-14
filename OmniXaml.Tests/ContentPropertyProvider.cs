namespace OmniXaml.Tests
{
    using System;
    using Model;

    internal class ContentPropertyProvider : IContentPropertyProvider
    {
        public string GetContentProperty(Type type)
        {
            if (type == typeof(TextBlock))
            {
                return "Text";
            }

            if (type == typeof(Window))
            {
                return "Content";
            }

            return null;
        }
    }
}