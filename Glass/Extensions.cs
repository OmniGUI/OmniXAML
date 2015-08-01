namespace Glass
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public static class Extensions
    {
        public static string ToString(this IEnumerable items)
        {
            var builder = new StringBuilder();

            foreach (var xamlNodeType in items)
            {
                builder.Append(" ·" + xamlNodeType + "\n");
            }

            return builder.ToString();
        }

        public static Tuple<string, string> Dicotomize(this string str, char ch)
        {
            var indexOfChar = str.IndexOf(ch);

            if (indexOfChar < 0)
            {
                return new Tuple<string, string>(str, null);
            }

            var leftPart = str.Substring(0, indexOfChar);
            indexOfChar++;
            var rightPart = str.Substring(indexOfChar, str.Length - indexOfChar);

            return new Tuple<string, string>(leftPart, rightPart);
        }

        public static Stream ToStream(this string str)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(str));
        }

        public static void AddOrReplace<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }
    }
}