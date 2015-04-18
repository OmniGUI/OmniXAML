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
    }
}