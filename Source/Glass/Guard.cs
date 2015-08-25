using System;
using System.Runtime.CompilerServices;

namespace Glass
{
    public static class Guard
    {
        public static void ThrowIfNull(object argumentValue, string argumentName, [CallerMemberName] string methodName = null)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName, $"{methodName} requires you to provide a value for {argumentName}");
            }
        }

        public static void ThrowIfNull<TException>(this object item, Func<TException> createException) where TException : Exception
        {
            CheckForCondition(item, p => p == null, createException);
        }

        public static void CheckForCondition<T, TException>(this T item, Predicate<T> p, Func<TException> createException) where TException : Exception
        {
            if (p(item)) { throw createException(); }
        }
    }
}