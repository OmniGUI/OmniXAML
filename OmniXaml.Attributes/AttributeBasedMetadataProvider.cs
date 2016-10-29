namespace OmniXaml.Attributes
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Metadata;

    public class AttributeBasedMetadataProvider : IMetadataProvider
    {
        public Metadata Get(Type type)
        {
            return new Metadata
            {
                ContentProperty = GetAttributeFromProperty<ContentAttribute, string>(type, (info, attribute) => info.Name),
                RuntimePropertyName = GetAttributeFromProperty<NameAttribute, string>(type, (info, attribute) => info.Name),
            };
        }

        private static O GetAttributeFromProperty<T, O>(Type type, Func<PropertyInfo, T, O> selector) where T : Attribute
        {
            var attributes = from prop in type.GetRuntimeProperties()
                             let attr = prop.GetCustomAttribute<T>()
                             where attr != null
                             select new { prop, attr };

            var single = attributes.SingleOrDefault();

            return single != null ? selector(single.prop, single.attr) : default(O);
        }
    }
}