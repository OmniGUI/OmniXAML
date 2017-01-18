using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Glass.Core
{
    public static class MethodFinder
    {
        public static MethodInfo GetAddMethod(string name, Type owner, params Type[] parameterTypes)
        {
            var typeInfo = owner.GetTypeInfo();

            var fromInterfaces = typeInfo.ImplementedInterfaces
                .SelectMany(x => x.GetRuntimeMethods());

            var fromType = owner.GetRuntimeMethods();

            var allMethods = fromInterfaces.Concat(fromType);

            var matching = from method in allMethods
                where method.Name == name
                where HasCorrectArgumentTypes(method, parameterTypes)
                where !method.IsStatic
                select method;

            return matching.FirstOrDefault();
        }

        private static bool HasCorrectArgumentTypes(MethodBase method, IEnumerable<Type> desiredTypes)
        {
            var actualTypes = method.GetParameters()
                .Select(info => info.ParameterType)
                .DefaultIfEmpty();

            var typePairs = desiredTypes.Zip(actualTypes, (desired, actual) => new {Desired = desired, Actual = actual});

            return typePairs.All(arg => arg.Actual != null && arg.Actual.GetTypeInfo().IsAssignableFrom(arg.Desired.GetTypeInfo()));
        }

        private static bool HasCorrectParameterCount(MethodInfo method)
        {
            return method.GetParameters().Length == 1;
        }
    }
}