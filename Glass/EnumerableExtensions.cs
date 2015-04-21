namespace Glass
{
    using System.Collections;
    using System.Text;

    public static class EnumerableExtensions
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
    }
}