namespace OmniXaml
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using Attributes;
    using Builder;
    using Glass;

    public class ContentPropertyProvider : IContentPropertyProvider
    {
        private readonly IDictionary<Type, ContentPropertyDefinition> registeredContentProperties = new Dictionary<Type, ContentPropertyDefinition>();

        public string GetContentPropertyName(Type type)
        {
            ContentPropertyDefinition member;
            var explictRegistration = registeredContentProperties.TryGetValue(type, out member) ? member : null;

            if (explictRegistration == null)
            {
                return ResolveFromHierarchy(type);
            }

            return explictRegistration.Name;
        }

        private string ResolveFromHierarchy(Type type)
        {
            while (true)
            {
                if (type == null)
                {
                    return null;
                }

                ContentPropertyDefinition member;
                var baseRegistration = registeredContentProperties.TryGetValue(type, out member) ? member : null;
                if (baseRegistration != null)
                {
                    return baseRegistration.Name;
                }

                var typeToLookFor = type.GetTypeInfo().BaseType;
                type = typeToLookFor;
            }
        }

        public void Add(ContentPropertyDefinition item)
        {
            registeredContentProperties.Add(item.OwnerType, item);
        }

        public static IContentPropertyProvider FromAttributes(IEnumerable<Type> types)
        {
            var contentPropertyProvider = new ContentPropertyProvider();

            var defs = Extensions.GatherAttributes<ContentPropertyAttribute, ContentPropertyDefinition>(
                types,
                (type, attribute) => new ContentPropertyDefinition(type, attribute.Name));

            contentPropertyProvider.AddAll(defs);
            return contentPropertyProvider;
        }

        public IEnumerator<ContentPropertyDefinition> GetEnumerator()
        {
            return registeredContentProperties.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}