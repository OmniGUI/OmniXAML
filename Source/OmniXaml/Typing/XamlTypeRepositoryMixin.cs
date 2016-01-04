namespace OmniXaml.Typing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Attributes;
    using Glass;

    public static class XamlTypeRepositoryMixin
    {
        public static Metadata GetMetadata<T>(this IXamlTypeRepository typeRepository)
        {
            var xamlType = typeRepository.GetXamlType(typeof(T));
            return typeRepository.GetMetadata(xamlType);
        }

        public static void RegisterMetadata<T>(this IXamlTypeRepository typeRepository, GenericMetadata<T> genericMetadata)
        {
            typeRepository.RegisterMetadata(typeof(T), genericMetadata);
        }

        public static void RegisterMetadataFromAttributes(this IXamlTypeRepository typeRepository, IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                var metadata = GetMetadata(type);
                typeRepository.RegisterMetadata(type, metadata);
            }
        }

        private static Metadata GetMetadata(Type t)
        {
            var dependencies = GetDependencies(t);
            var contentProperty = GetContentProperty(t);
            var runtimePropertyName = GetRuntimePropertyName(t);

            return new Metadata { RuntimePropertyName = runtimePropertyName, PropertyDependencies = dependencies, ContentProperty = contentProperty };
        }

        private static string GetRuntimePropertyName(Type type)
        {
            var attr = type.GetTypeInfo().GetCustomAttribute<RuntimePropertyNameAttribute>();
            return attr.Name;
        }

        private static string GetContentProperty(Type type)
        {
            var attr = type.GetTypeInfo().GetCustomAttribute<ContentPropertyAttribute>();
            return attr.Name;
        }

        private static DependencyRegistrations GetDependencies(Type type)
        {
            var attrs = from member in type.GetTypeInfo().DeclaredMembers
                        let attr = member.GetCustomAttribute<DependsOnAttribute>()
                        where attr != null
                        select new { prop = member.Name, dep = attr.PropertyName };


            var registrations = new DependencyRegistrations();

            foreach (var dependsOnAttribute in attrs)
            {
                registrations.Add(new DependencyRegistration(dependsOnAttribute.prop, dependsOnAttribute.dep));
            }

            return registrations;
        }
    }
}