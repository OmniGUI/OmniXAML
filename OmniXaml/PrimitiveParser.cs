namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class PrimitiveParser
    {
        static readonly IDictionary<Type, MethodInfo> Parsers = new Dictionary<Type, MethodInfo>();

        public static bool TryParse(Type targetType, string sourceValue, out object result)
        {
            var typeInfo = targetType.GetTypeInfo();


            MethodInfo converter;
            if (Parsers.TryGetValue(targetType, out converter))
            {
                result = converter.Invoke(null, new object[] { sourceValue });
                return true;
            }

            if (typeInfo.IsPrimitive && targetType != typeof(IntPtr) && targetType != typeof(UIntPtr))
            {
                var method = GetParseMethod(targetType);
                result = method.Invoke(null, new object[] { sourceValue });
                Parsers.Add(targetType, method);
                return true;
            }

            result = null;
            return false;
        }

        private static MethodInfo GetParseMethod(Type targetType)
        {
            return targetType.GetRuntimeMethods().First(
                mi =>
                {
                    var parameterInfos = mi.GetParameters();
                    var hasUniqueStringArgument = parameterInfos.Length == 1 && parameterInfos.First().ParameterType == typeof(string);
                    return mi.Name == "Parse" && hasUniqueStringArgument;
                });
        }
    }
}