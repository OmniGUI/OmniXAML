namespace Glass
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class ReflectionExtensions
    {
        // code adjusted to prevent horizontal overflow
        public static string GetFullPropertyName<T, TProperty>(this Expression<Func<T, TProperty>> propertySelector)
        {
            MemberExpression memberExp;
            if (!TryFindMemberExpression(propertySelector.Body, out memberExp))
            {
                return string.Empty;
            }

            var memberNames = new Stack<string>();
            do
            {
                memberNames.Push(memberExp.Member.Name);
            }
            while (TryFindMemberExpression(memberExp.Expression, out memberExp));

            return string.Join(".", memberNames.ToArray());
        }

        // code adjusted to prevent horizontal overflow
        private static bool TryFindMemberExpression(this Expression exp, out MemberExpression memberExp)
        {
            memberExp = exp as MemberExpression;
            if (memberExp != null)
            {
                // heyo! that was easy enough
                return true;
            }

            // if the compiler created an automatic conversion,
            // it'll look something like...
            // obj => Convert(obj.Property) [e.g., int -> object]
            // OR:
            // obj => ConvertChecked(obj.Property) [e.g., int -> long]
            // ...which are the cases checked in IsConversion
            if (IsConversion(exp) && exp is UnaryExpression)
            {
                memberExp = ((UnaryExpression)exp).Operand as MemberExpression;
                if (memberExp != null)
                {
                    return true;
                }
            }

            return false;
        }

        public static MemberExpression GetMemberExpression(this Expression expression)
        {
            MemberExpression memberExpression;
            if (TryFindMemberExpression(expression, out memberExpression))
            {
                return memberExpression;
            }

            throw new InvalidOperationException();
        }

        private static bool IsConversion(Expression exp)
        {
            return (exp.NodeType == ExpressionType.Convert || exp.NodeType == ExpressionType.ConvertChecked);
        }

        public static Action<T> GetSetterFromPropertySelector<T, TInst>(this Expression<Func<TInst, T>> sourcePropertySelector, TInst instance)
        {
            MemberExpression exp;
            if (sourcePropertySelector.Body.TryFindMemberExpression(out exp))
            {
                var propInfo = exp.Member as PropertyInfo;
                var deleg = propInfo.SetMethod.CreateDelegate(typeof(Action<T>), instance);
                var action = (Action<T>)deleg;
                return action;
            }

            throw new InvalidOperationException();
        }

        public static Action<object> GetSetterFromProperty(object instance, string propertyName)
        {
            var propInfo = instance.GetType().GetTypeInfo().GetDeclaredProperty(propertyName);
            var deleg = propInfo.SetMethod.CreateDelegate(typeof(Action<object>), instance);
            var action = (Action<object>)deleg;
            return action;            
        }

        public static Expression<Func<TIn, TOut>> CreatePropertyAccessor<TIn, TOut>(string propertyName)
        {
            var param = Expression.Parameter(typeof(TIn));
            var body = Expression.PropertyOrField(param, propertyName);
            return Expression.Lambda<Func<TIn, TOut>>(body, param);
        }

        public static dynamic GetGetterFromProperty(object instance, string propertyName)
        {
            var propInfo = instance.GetType().GetTypeInfo().GetDeclaredProperty(propertyName);
            var delegateType = typeof(Func<>).MakeGenericType(propInfo.PropertyType);
            var deleg = propInfo.GetMethod.CreateDelegate(delegateType, instance);
            
            return deleg;
        }

        private static object DynamicGenericCall(
            object ownerInstance,
            Type ownerType,
            string methodName,
            Type[] methodTypeArguments,
            object[] parameters,
            Func<IEnumerable<MethodInfo>, MethodInfo> overloadSelector = null)
        {
            var invoker = GenericMethodInvoker(ownerType, methodName, methodTypeArguments, overloadSelector);
            return invoker.Invoke(ownerInstance, parameters);
        }

        private static MethodBase GenericMethodInvoker(
            Type type,
            string methodName,
            Type[] types,
            Func<IEnumerable<MethodInfo>, MethodInfo> overloadSelector = null)
        {
            var typeInfo = type.GetTypeInfo();

            MethodInfo declaredMethod;
            if (overloadSelector != null)
            {
                var declaredMethods = typeInfo.GetDeclaredMethods(methodName);
                declaredMethod = overloadSelector(declaredMethods);
            }
            else
            {
                declaredMethod = typeInfo.GetDeclaredMethod(methodName);
            }

            return declaredMethod.MakeGenericMethod(types);
        }

        public static Delegate GetSetterFromPropertyPath(object instance, string propertyPath)
        {
            var typeInfo = instance.GetType().GetTypeInfo();
            var declaredProperty = typeInfo.GetDeclaredProperty(propertyPath);
            var methodInfo = declaredProperty.SetMethod;

            var type = typeof(Action<>).MakeGenericType(declaredProperty.PropertyType);

            return methodInfo.CreateDelegate(type, instance);
        }

        public static object GetValueOfStaticField(Type type, string name)
        {
            var fieldInfo = type.GetRuntimeField(name);

            if (fieldInfo == null)
            {
                var baseType = type.GetTypeInfo().BaseType;
                if (baseType != null)
                {
                    return GetValueOfStaticField(baseType, name);
                }
            }

            return fieldInfo?.GetValue(null);
        }

        public static IEnumerable<Type> AllExportedTypes(this IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(assembly => assembly.ExportedTypes);
        }
    }
}