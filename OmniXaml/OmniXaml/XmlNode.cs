namespace OmniXaml
{
    using System.Collections.Generic;
    using System.Xml;

    public class XmlNode
    {
        private readonly string name;

        private readonly string xamlNamespace;

        private readonly object value;

        private readonly IDictionary<string, string> attributes;

        private readonly XmlNodeType nodeType;

        public XmlNode(string name, string xamlNamespace, object value, IDictionary<string, string> attributes, XmlNodeType nodeType)
        {
            this.name = name;
            this.xamlNamespace = xamlNamespace;
            this.value = value;
            this.attributes = attributes;
            this.nodeType = nodeType;
        }

        public string Name => name;

        public string XamlNamespace => xamlNamespace;

        public object Value => value;

        public IDictionary<string, string> Attributes => attributes;

        public XmlNodeType NodeType => nodeType;

        public bool IsCollapsed { get; set; }
    }
}