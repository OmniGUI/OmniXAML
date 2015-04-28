namespace OmniXaml
{
    internal class XamlElement
    {
        public XamlElement(string name, string @namespace)
        {
            Name = name;
            Namespace = @namespace;
        }

        public XamlNodeType XamlNodeType { get; set; }

        public string Name { get; set; }

        public string Namespace { get; set; }
    }
}