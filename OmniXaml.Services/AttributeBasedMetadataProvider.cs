namespace OmniXaml.Services
{
    using System;
    using System.Reflection;
    using Attributes;
    using Glass.Core;
    using Metadata;

    public class AttributeBasedMetadataProvider : IMetadataProvider
    {
        public Metadata Get(Type type)
        {
            return new Metadata
            {
                ContentProperty = type.GetAttributeFromProperty<ContentAttribute, string>((info, attribute) => info.Name),
                RuntimePropertyName = type.GetAttributeFromProperty<NameAttribute, string>((info, attribute) => info.Name),
                IsNamescope = type.GetAttributeFromType<NamescopeAttribute, NamescopeAttribute>(attribute => attribute) != null,
                FragmentLoaderInfo = type.GetAttributeFromProperty<FragmentLoaderAttribute, FragmentLoaderInfo>((info, attribute) => GetFragmentLoaderInfo(type, info, attribute)),
                PropertyDependencies = GetDependencyRegistrations(type),
            };
        }

        private DependencyRegistrations GetDependencyRegistrations(Type type)
        {
            var regs = type.GetAttributesFromProperties<DependsOnAttribute, DependencyRegistration>(GetDependencyRegistration);
            return new DependencyRegistrations(regs);
        }

        private DependencyRegistration GetDependencyRegistration(PropertyInfo info, DependsOnAttribute attribute)
        {
            return new DependencyRegistration
            {
                PropertyName = info.Name,
                DependsOn = attribute.Dependency,
            };
        }

        private static FragmentLoaderInfo GetFragmentLoaderInfo(Type type, MemberInfo memberInfo, FragmentLoaderAttribute attribute)
        {
            var constructionFragmentLoader = (IConstructionFragmentLoader) Activator.CreateInstance(attribute.FragmentLoader);

            return new FragmentLoaderInfo
            {
                Loader = constructionFragmentLoader, 
                Type = type,
                PropertyName = memberInfo.Name,
            };
        }
    }
}