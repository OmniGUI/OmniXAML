namespace OmniXaml.Tests
{
    using System.Collections;
    using System.Text;

    public static class TestingExtensions
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