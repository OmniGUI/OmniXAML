namespace Glass.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

        public static IEnumerable<Type> AllExportedTypes(this IEnumerable<Assembly> assemblies)
        {
            return assemblies
                .Where(assembly => !assembly.IsDynamic)
                .SelectMany(assembly => assembly.ExportedTypes);
        }

        public static bool IsNullable(this Type type)
        {
            if (!type.GetTypeInfo().IsValueType)
            {
                return true; // ref-type
            }

            if (Nullable.GetUnderlyingType(type) != null)
            {
                return true; // Nullable<T>
            }

            return false; // value-type
        }

    }
}