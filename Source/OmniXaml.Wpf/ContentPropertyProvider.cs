namespace OmniXaml.Wpf
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Windows.Markup;
    using Builder;

    internal class ContentPropertyProvider : IContentPropertyProvider
    {
        public string GetContentPropertyName(Type type)
        {
            return type.GetTypeInfo().GetCustomAttribute<ContentPropertyAttribute>().Name;
        }

        public void Add(ContentPropertyDefinition item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<ContentPropertyDefinition> GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}