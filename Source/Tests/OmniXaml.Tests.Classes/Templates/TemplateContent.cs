namespace OmniXaml.Tests.Classes.Templates
{
    using System.Collections.Generic;
    using WpfLikeModel;

    public class TemplateContent
    {
        private readonly IEnumerable<XamlInstruction> nodes;
        private readonly IWiringContext wiringContext;

        public TemplateContent(IEnumerable<XamlInstruction> nodes, IWiringContext wiringContext)
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