namespace OmniXaml.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class XmlnsPrefixAttribute : Attribute
    {
        public string XmlNamespace { get; }

        /// <summary>Gets the recommended prefix associated with this attribute.</summary>
        /// <returns>The recommended prefix string.</returns>
        public string Prefix { get; }
      
        public XmlnsPrefixAttribute(string xmlNamespace, string prefix)
        {
            if (xmlNamespace == null)
            {
                throw new ArgumentNullException(nameof(xmlNamespace));
            }

            if (prefix == null)
            {
                throw new ArgumentNullException(nameof(prefix));
            }

            XmlNamespace = xmlNamespace;
            Prefix = prefix;
        }
    }
}