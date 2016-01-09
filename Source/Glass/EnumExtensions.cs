namespace Glass
{
    using System;
    using System.Reflection;

    public static class EnumExtensions
    {
        public static bool TryParse(Type enumType, string value, out object result)
        {
            try
            {
                var tryParse = Enum.Parse(enumType, value);
                result = tryParse;
                return true;
            }
            catch
            {
                result = 0;
                return false;
            }
        }
    }
}