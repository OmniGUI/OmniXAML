namespace OmniXaml.Typing
{
    public abstract class XamlName
    {
        public const char PlusSign = '+';

        public const char UnderScore = '_';

        public const char Dot = '.';

        protected XamlName()
            : this(string.Empty)
        {
        }

        public XamlName(string propertyName)
        {
            PropertyName = propertyName;
        }

        public XamlName(string prefix, string propertyName)
        {
            PropertyName = propertyName;
            this.Prefix = prefix ?? string.Empty;
        }

        public string PropertyName
        {
            get; protected set;
        }

        public abstract string ScopedName { get; }

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

        public static bool IsValidNameStartChar(char ch)
        {
            if (!char.IsLetter(ch))
            {
                return ch == 95;
            }
            return true;
        }

        public static bool IsValidNameChar(char ch)
        {
            if (IsValidNameStartChar(ch) || char.IsDigit(ch))
            {
                return true;
            }
            return false;
        }

        public static bool IsValidQualifiedNameChar(char ch)
        {
            if (ch != 46)
            {
                return IsValidNameChar(ch);
            }
            return true;
        }

        public static bool IsValidQualifiedNameCharPlus(char ch)
        {
            if (!IsValidQualifiedNameChar(ch))
            {
                return ch == 43;
            }
            return true;
        }
    }
}