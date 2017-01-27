namespace OmniXaml
{
    using System;
    using Zafiro.Core;

    public class StringUtils
    {
        public static Tuple<string, string> GetPrefixAndType(string prefixedType)
        {
            if (prefixedType.Contains(":"))
            {
                return prefixedType.Dicotomize(':');
            }
            else
            {
                return new Tuple<string, string>(String.Empty, prefixedType);
            }
        }
    }
}