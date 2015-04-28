namespace OmniXaml.Typing
{
    public class XamlQualifiedName : XamlName
    {
        public XamlQualifiedName(string prefix, string propertyName)
            : base(prefix, propertyName)
        {
        }

        private static bool IsNameValid(string name)
        {
            if (name.Length == 0 || !IsValidNameStartChar(name[0]))
            {
                return false;
            }
            for (var index = 1; index < name.Length; ++index)
            {
                if (!IsValidQualifiedNameChar(name[index]))
                {
                    return false;
                }
            }
            return true;
        }        

        public static bool TryParse(string longName, out string prefix, out string name)
        {
            var startIndex = 0;
            var length = longName.IndexOf(':');
            prefix = string.Empty;
            name = string.Empty;
            if (length != -1)
            {
                prefix = longName.Substring(startIndex, length);
                if (string.IsNullOrEmpty(prefix) || !IsNameValid(prefix))
                {
                    return false;
                }
                startIndex = length + 1;
            }
            name = startIndex == 0 ? longName : longName.Substring(startIndex);
            return !string.IsNullOrEmpty(name);
        }
    }
}