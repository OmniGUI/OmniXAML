namespace OmniXaml.Tests.Classes.Templates
{
    using System.Collections.Generic;
    using WpfLikeModel;

    public class TemplateContent
    {
        private readonly IEnumerable<XamlInstruction> nodes;
        private readonly WiringContext wiringContext;

        public TemplateContent(IEnumerable<XamlInstruction> nodes, WiringContext wiringContext)
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