namespace OmniXaml.Attributes
{
    using System;
    using System.Reflection;
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
                FragmentLoaderInfo = type.GetAttributeFromProperty<FragmentLoaderAttribute, FragmentLoaderInfo>((info, attribute) => GetFragmentLoaderInfo(type, info, attribute))
            };
        }

        private static FragmentLoaderInfo GetFragmentLoaderInfo(Type type, PropertyInfo propInfo, FragmentLoaderAttribute attribute)
        {
            var constructionFragmentLoader = (IConstructionFragmentLoader) Activator.CreateInstance(attribute.FragmentLoader);

            return new FragmentLoaderInfo
            {
                Loader = constructionFragmentLoader, 
                Type = type,
                PropertyName = propInfo.Name,
            };
        }
    }
}