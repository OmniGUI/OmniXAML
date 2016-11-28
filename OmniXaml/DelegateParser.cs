namespace OmniXaml
{
    using System;
    using System.Linq;
    using System.Reflection;

    public static class DelegateParser
    {
        public static bool TryParse(string str, Type targetType, object delegateParent, out object result)
        {
            if (typeof(Delegate).GetTypeInfo().IsAssignableFrom(targetType.GetTypeInfo()))
            {                
                var callbackMethodInfo = delegateParent
                    .GetType()
                    .GetRuntimeMethods()
                    .First(method => method.Name.Equals(str));

                result = callbackMethodInfo.CreateDelegate(targetType, delegateParent);
                return true;
            }

            result = null;
            return false;
        }
    }
}