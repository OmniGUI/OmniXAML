namespace OmniXaml.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class XmlnsDefinitionAttribute : Attribute
    {
        public string XmlNamespace { get; }

        public string ClrNamespace { get; }

        public string AssemblyName { get; set; }

        public XmlnsDefinitionAttribute(string xmlNamespace, string clrNamespace)
        {
            if (xmlNamespace == null)
            {
                throw new ArgumentNullException("xmlNamespace");
            }

            if (clrNamespace == null)
            {
                throw new ArgumentNullException("clrNamespace");
            }

            XmlNamespace = xmlNamespace;
            ClrNamespace = clrNamespace;
        }
    }
}
