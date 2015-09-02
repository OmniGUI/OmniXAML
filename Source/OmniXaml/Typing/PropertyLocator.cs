namespace OmniXaml.Typing
{
    public class PropertyLocator : XamlName
    {
        public readonly XamlName Owner;

        private PropertyLocator(XamlName owner, string prefix, string propertyName)
            : base(propertyName)
        {
            if (owner != null)
            {
                Owner = owner;
                Prefix = owner.Prefix ?? string.Empty;
            }
            else
            {
                Prefix = prefix ?? string.Empty;
            }
        }     

        public string OwnerName
        {
            get
            {
                if (!IsDotted)
                {
                    return string.Empty;
                }
                return Owner.PropertyName;
            }
        }

        public bool IsDotted => Owner != null;

        public bool IsNsPrefixDefinition => Prefix == "xmlns" || PropertyName == "xmlns";

        public static PropertyLocator Parse(string longName)
        {
            if (string.IsNullOrEmpty(longName))
            {
                return null;
            }

            string prefix;
            string qualifiedName;

            if (!XamlQualifiedName.TryParse(longName, out prefix, out qualifiedName))
            {
                return null;
            }

            var startIndex = 0;
            var part1 = string.Empty;
            var length = qualifiedName.IndexOf('.');
            if (length != -1)
            {
                part1 = qualifiedName.Substring(startIndex, length);

                if (string.IsNullOrEmpty(part1))
                {
                    return null;
                }

                startIndex = length + 1;
            }
            var part2 = startIndex == 0 ? qualifiedName : qualifiedName.Substring(startIndex);
            XamlQualifiedName xamlQualifiedName = null;
            if (!string.IsNullOrEmpty(part1))
            {
                xamlQualifiedName = new XamlQualifiedName(prefix, part1);
            }

            return new PropertyLocator(xamlQualifiedName, prefix, part2);
        }

        public static PropertyLocator Parse(string longName, string namespaceUri)
        {
            var xamlPropertyName = Parse(longName);
            xamlPropertyName.Namespace = namespaceUri;
            return xamlPropertyName;
        }
    }    
}