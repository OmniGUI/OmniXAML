namespace OmniXaml.Glass
{
    using System.Collections.Generic;
    using System.Linq;

    public static class StringExtensions
    {
        public static string AsString(this IEnumerable<char> array)
        {
            return new string(array.ToArray());
        }
    }
}