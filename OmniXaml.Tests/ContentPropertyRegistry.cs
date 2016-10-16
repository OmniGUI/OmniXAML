namespace OmniXaml.Tests
{
    using System;
    using Model;

    internal class ContentPropertyRegistry : IContentPropertyRegistry
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