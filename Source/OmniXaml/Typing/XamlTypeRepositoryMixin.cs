namespace OmniXaml.Typing
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Attributes;
    using Glass;

    public static class XamlTypeRepositoryMixin
    {
        public static Metadata GetMetadata<T>(this IXamlTypeRepository typeRepository)
        {
            return typeRepository.GetMetadata(typeof(T));
        }

        public static void RegisterMetadata<T>(this IXamlTypeRepository typeRepository, GenericMetadata<T> genericMetadata)
        {
            typeRepository.RegisterMetadata(typeof(T), genericMetadata);
        }

        public static void RegisterMetadataFromAttributes(this IXamlTypeRepository typeRepository, IEnumerable<Type> types)
        {
            var gatheredDependencies = types.GatherAttributesFromMembers<DependsOnAttribute, Tuple<PropertyInfo, string>>((propertyInfo, attribute) => new Tuple<PropertyInfo, string>(propertyInfo, attribute.PropertyName));
            foreach (var tuple in gatheredDependencies)
            {
                var metadata = new Metadata();
                var type = tuple.Item1.DeclaringType;
                var property = tuple.Item1.Name;
                var dependsOn = tuple.Item2;
                metadata.SetMemberDependency(property, dependsOn);
                typeRepository.RegisterMetadata(type, metadata);
            }
        }
    }
}