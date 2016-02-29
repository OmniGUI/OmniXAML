namespace XamlViewer.ViewModels
{
    public static class StringExtensions
    {
        public static string GetFirstNChars(this string str, int max)
        {
            if (str.Length <= max)
            {
                return str;
            }
            return str.Substring(0, max) + "…";
        }
    }
}