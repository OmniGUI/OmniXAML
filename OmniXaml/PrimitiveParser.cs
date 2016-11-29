namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    public static class PrimitiveParser
    {
        static readonly IDictionary<Type, MethodInfo> Parsers = new Dictionary<Type, MethodInfo>();

        public static bool TryParse(Type targetType, string sourceValue, out object result)
        {
            var typeInfo = targetType.GetTypeInfo();

            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

            MethodInfo converter;
            if (Parsers.TryGetValue(targetType, out converter))
            {
                var parameters = new object[] { sourceValue, null };
                var tryParse = converter.Invoke(null, parameters);
                result = parameters[1];
                return (bool)tryParse;
                
            }

            if (typeInfo.IsPrimitive && targetType != typeof(IntPtr) && targetType != typeof(UIntPtr))
            {
                var method = GetParseMethod(targetType);

                Parsers.Add(targetType, method);

                var parameters = new object[] { sourceValue, null };
                var tryParse = method.Invoke(null, parameters);
                result = parameters[1];
                return (bool)tryParse;
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
                    var hasUniqueStringArgument = parameterInfos.Length == 2 && parameterInfos.First().ParameterType == typeof(string);
                    return mi.Name == "TryParse" && hasUniqueStringArgument;
                });
        }
    }
}