namespace OmniXaml.Typing
{
    public abstract class XamlName
    {
        protected XamlName(string propertyName)
        {
            PropertyName = propertyName;
        }

        protected XamlName(string prefix, string propertyName)
        {
            PropertyName = propertyName;
            Prefix = prefix ?? string.Empty;
        }

        public string PropertyName
        {
            get; private set;
        }

        public string Prefix { get; protected set; }

        public string Namespace { get; protected set; }

        public static bool ContainsDot(string name)
        {
            return name.Contains(".");
        }

        public static bool IsValidXamlName(string name)
        {
            if (name.Length == 0 || !IsValidNameStartChar(name[0]))
            {
                return false;
            }
            for (var index = 1; index < name.Length; ++index)
            {
                if (!IsValidNameChar(name[index]))
                {
                    return false;
                }
            }
            return true;
        }

        protected static bool IsValidNameStartChar(char ch)
        {
            if (!char.IsLetter(ch))
            {
                return ch == '_';
            }
            return true;
        }

        private static bool IsValidNameChar(char ch)
        {
            return IsValidNameStartChar(ch) || char.IsDigit(ch);
        }

        protected static bool IsValidQualifiedNameChar(char ch)
        {
            return ch == '.' || IsValidNameChar(ch);
        }
    }
}