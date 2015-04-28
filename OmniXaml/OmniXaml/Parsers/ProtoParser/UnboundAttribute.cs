namespace OmniXaml.Parsers.ProtoParser
{
    using System;
    using Typing;

    internal class UnboundAttribute
    {
        private readonly string xmlnsDefinitionPrefix;

        private readonly string xmlnsDefinitionUri;

        public UnboundAttribute(PropertyLocator propLocator, string val)
        {
            Locator = propLocator;
            Value = val;
            Type = AttributeType.Property;

            if (!TryExtractNamespacePrefixDefinition(out xmlnsDefinitionPrefix, out xmlnsDefinitionUri))
            {
                return;
            }

            Type = AttributeType.Namespace;
        }

        public PropertyLocator Locator { get; }

        public string Value { get; }

        public AttributeType Type { get; private set; }

        public string XmlNsPrefixDefined => xmlnsDefinitionPrefix;

        public string XmlNsUriDefined => xmlnsDefinitionUri;

        private bool TryExtractNamespacePrefixDefinition(out string prefix, out string ns)
        {
            ns = string.Empty;
            prefix = string.Empty;

            if (string.Equals(Locator.Prefix, "xmlns", StringComparison.Ordinal))
            {
                ns = Value;
                prefix = !Locator.IsDotted ? Locator.PropertyName : Locator.OwnerName + "." + Locator.PropertyName;
                return true;
            }

            if (!string.IsNullOrEmpty(Locator.Prefix) || !string.Equals(Locator.PropertyName, "xmlns", StringComparison.Ordinal))
            {
                return false;
            }

            ns = Value;
            prefix = string.Empty;
            return true;
        }
    }
}