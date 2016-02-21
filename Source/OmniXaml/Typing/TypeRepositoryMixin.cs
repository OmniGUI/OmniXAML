namespace OmniXaml.Typing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Attributes;

    public static class TypeRepositoryMixin
    {
        public static Metadata GetMetadata<T>(this ITypeFeatureProvider typeRepository)
        {
            return typeRepository.GetMetadata(typeof(T));
        }

        public static void RegisterMetadata<T>(this ITypeFeatureProvider featureProvider, GenericMetadata<T> genericMetadata)
        {
            featureProvider.RegisterMetadata(typeof(T), genericMetadata);
        }

        public static void FillFromAttributes(this ITypeFeatureProvider featureProvider, IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                var metadata = GetMetadata(type);
                featureProvider.RegisterMetadata(type, metadata);
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
            var attr = type.GetTypeInfo()
                .DeclaredMembers
                .FirstOrDefault(info => info.GetCustomAttribute<NameAttribute>() != null);

            return attr?.Name;
        }

        private static string GetContentProperty(Type type)
        {
            var attr = type.GetTypeInfo().GetCustomAttribute<ContentPropertyAttribute>();
            return attr?.Name;
        }

        private static DependencyRegistrations GetDependencies(Type type)
        {
            var attrs = from member in type.GetRuntimeProperties()
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