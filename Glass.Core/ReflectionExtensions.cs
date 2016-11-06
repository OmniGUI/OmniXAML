namespace Glass.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class ReflectionExtensions
    {
        public static string GetFullPropertyName<T, TProperty>(this Expression<Func<T, TProperty>> propertySelector)
        {
            MemberExpression memberExp;

            if (!TryFindMemberExpression(propertySelector.Body, out memberExp))
                return string.Empty;
            
            var memberNames = new Stack<string>();

            do
            {
                memberNames.Push(memberExp.Member.Name);
            } while (TryFindMemberExpression(memberExp.Expression, out memberExp));


            return string.Join(".", memberNames.ToArray());
        }

        // code adjusted to prevent horizontal overflow

        private static bool TryFindMemberExpression(this Expression exp, out MemberExpression memberExp)
        {
            memberExp = exp as MemberExpression;

            if (memberExp != null)
                return true;


            // if the compiler created an automatic conversion,
            // it'll look something like...
            // obj => Convert(obj.Property) [e.g., int -> object]
            // OR:
            // obj => ConvertChecked(obj.Property) [e.g., int -> long]
            // ...which are the cases checked in IsConversion

            if (IsConversion(exp) && exp is UnaryExpression)
            {
                memberExp = ((UnaryExpression) exp).Operand as MemberExpression;

                if (memberExp != null)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsConversion(Expression exp)
        {
            return (exp.NodeType == ExpressionType.Convert) || (exp.NodeType == ExpressionType.ConvertChecked);
        }



        public static Func<object, Delegate> GetDelegateWithDefaultParameterValuesBound(this MethodInfo method)
        {
            var instanceParameter = Expression.Parameter(typeof(object));
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

            return Expression.Lambda<Func<object, Delegate>>(Expression.Lambda(
                    Expression.Call(Expression.Convert(instanceParameter, method.DeclaringType), method, arguments),
                parameterExpressions.Values), instanceParameter).Compile();
        }
    }
}