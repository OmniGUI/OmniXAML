namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Builder;
    using Catalogs;

    public class ContentPropertyProvider : IContentPropertyProvider
    {
        private readonly IDictionary<Type, string> registeredContentProperties = new Dictionary<Type, string>();

        public string GetContentPropertyName(Type type)
        {
            string member;
            var explictRegistration = registeredContentProperties.TryGetValue(type, out member) ? member : null;

            if (explictRegistration == null)
            {
                return ResolveFromHierarchy(type);
            }

            return explictRegistration;
        }

        private string ResolveFromHierarchy(Type type)
        {
            if (type == null)
            {
                return null;
            }

            string member;
            var baseRegistration = registeredContentProperties.TryGetValue(type, out member) ? member : null;
            if (baseRegistration != null)
            {
                return baseRegistration;
            }

            var typeToLookFor = type.GetTypeInfo().BaseType;
            return ResolveFromHierarchy(typeToLookFor);
        }

        public void AddCatalog(ContentPropertyCatalog catalog)
        {
            try
            {
                foreach (var propertyInfo in catalog.Mappings)
                {
                    registeredContentProperties.Add(propertyInfo);
                }
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException(string.Format("There has been a problem adding the catalog: {0}", ex));
            }
        }

        public void Add(ContentPropertyDefinition contentPropertyDefinition)
        {
            this.registeredContentProperties.Add(contentPropertyDefinition.OwnerType, contentPropertyDefinition.Name);
        }
    }
}