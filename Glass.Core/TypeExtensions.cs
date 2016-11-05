namespace Glass.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class TypeExtensions
    {
        public static bool IsCollection(this Type type)
        {
            if (type == typeof(string))
            {
                return false;
            }

            var typeInfo = type.GetTypeInfo();
            return typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(typeInfo);
        }

        public static TSelector GetAttributeFromType<TAttribute, TSelector>(this Type type, Func<TAttribute, TSelector> selector) where TAttribute : Attribute
        {
            var attributeFromType = type.GetTypeInfo().GetCustomAttribute<TAttribute>();
            return attributeFromType != null ? selector(attributeFromType) : default(TSelector);
        }

        public static TSelector GetAttributeFromProperty<TAttribute, TSelector>(this Type type, Func<PropertyInfo, TAttribute, TSelector> selector) where TAttribute : Attribute
        {
            var attributes = from prop in type.GetRuntimeProperties()
                let attr = prop.GetCustomAttribute<TAttribute>()
                where attr != null
                select new { prop, attr };

            var single = attributes.SingleOrDefault();

            return single != null ? selector(single.prop, single.attr) : default(TSelector);
        }

        public static IEnumerable<TSelector> GetAttributesFromProperties<TAttribute, TSelector>(this Type type, Func<PropertyInfo, TAttribute, TSelector> selector) where TAttribute : Attribute
        {
            var attributes = from prop in type.GetRuntimeProperties()
                             let attr = prop.GetCustomAttribute<TAttribute>()
                             where attr != null
                             select new { prop, attr };

            return attributes.Select(arg => selector(arg.prop, arg.attr));
        }
    }
}