namespace Glass.Core
{
    using System;

    public static class EnumExtensions
    {
        public static bool TryParse(Type enumType, string value, out object result)
        {
            if (Enum.IsDefined(enumType, value))
            {
                result = Enum.Parse(enumType, value);
                return true;
            }

            result = null;
            return false;
        }
    }
}