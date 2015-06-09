namespace OmniXaml.Tests.Classes.Templates
{
    using System.Collections.Generic;

    public class TemplateContent
    {
        private readonly IEnumerable<XamlNode> nodes;

        public TemplateContent(IEnumerable<XamlNode> nodes)
        {
            this.nodes = nodes;
        }

        public Grid CreateContent()
        {
            return new Grid();
        }
    }
}