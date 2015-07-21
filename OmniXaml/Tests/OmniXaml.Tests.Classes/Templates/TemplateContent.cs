namespace OmniXaml.Tests.Classes.Templates
{
    using System.Collections.Generic;
    using WpfLikeModel;

    public class TemplateContent
    {
        private readonly IEnumerable<XamlNode> nodes;
        private readonly WiringContext wiringContext;

        public TemplateContent(IEnumerable<XamlNode> nodes, WiringContext wiringContext)
        {
            this.nodes = nodes;
            this.wiringContext = wiringContext;
        }

        public Grid CreateContent()
        {
            return new Grid();
        }
    }
}