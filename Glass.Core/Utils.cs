namespace Glass.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class Utils
    {
        public static void UniversalAdd(object collection, object item)
        {
            var addMethod = collection.GetType().GetTypeInfo().ImplementedInterfaces.SelectMany(x => x.GetRuntimeMethods()).First(n => n.Name == "Add");
            if (addMethod == null || addMethod.GetParameters().Length != 1)
            {
                // handle your error
                return;
            }
            ParameterInfo parameter = addMethod.GetParameters().First();
            if (parameter.ParameterType.GetTypeInfo().IsAssignableFrom(item.GetType().GetTypeInfo()))
            {
                addMethod.Invoke(collection, new[] { item });
            }
            else
            {
                // handle your error
            }
        }

        public static Delegate GetDelegateWithDefaultParameterValuesBound(object instance, MethodInfo method)
        {
            var methodParameters = method.GetParameters();
            var parameterExpressions = new Dictionary<int, ParameterExpression>();
            var arguments = new List<Expression>(methodParameters.Length);
            for (int i = 0; i < methodParameters.Length; i++)
            {
                if (methodParameters[i].HasDefaultValue)
                {
                    arguments.Add(Expression.Constant(methodParameters[i].DefaultValue));
                }
                else
                {
                    parameterExpressions.Add(i, Expression.Parameter(methodParameters[i].ParameterType));
                    arguments.Add(parameterExpressions[i]);
                }
            }

            return Expression.Lambda(Expression.Call(Expression.Constant(instance), method, arguments), parameterExpressions.Values).Compile();
        }
    }
}