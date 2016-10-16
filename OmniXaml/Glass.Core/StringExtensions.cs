namespace OmniXaml.Glass.Core
{
    using System.Collections.Generic;
    using System.Linq;

    public static class StringExtensions
    {
        public static string AsString(this IEnumerable<char> array)
        {
            return new string(array.ToArray());
        }

        public static IEnumerable<string> Split(this string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length/chunkSize)
                .Select(i => str.Substring(i*chunkSize, chunkSize));
        }
    }
}