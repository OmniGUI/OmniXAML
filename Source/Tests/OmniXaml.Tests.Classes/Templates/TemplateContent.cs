namespace OmniXaml.Tests.Classes.Templates
{
    using System.Collections.Generic;
    using WpfLikeModel;

    public class TemplateContent
    {
        private readonly IEnumerable<XamlInstruction> nodes;
        private readonly IWiringContext IWiringContext;

        public TemplateContent(IEnumerable<XamlInstruction> nodes, IWiringContext IWiringContext)
        {
            this.nodes = nodes;
            this.IWiringContext = IWiringContext;
        }

        public Grid CreateContent()
        {
            return new Grid();
        }
    }
}