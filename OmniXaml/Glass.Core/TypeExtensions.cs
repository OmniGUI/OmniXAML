namespace OmniXaml.Glass.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
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
    }
}