namespace OmniXaml.Tests.Classes.Templates
{
    using System.Collections.Generic;
    using WpfLikeModel;

    public class TemplateContent
    {
        private readonly IEnumerable<XamlInstruction> nodes;
        private readonly ITypeContext typeContext;

        public TemplateContent(IEnumerable<XamlInstruction> nodes, ITypeContext typeContext)
        {
            this.nodes = nodes;
            this.typeContext = typeContext;
        }

        public Grid CreateContent()
        {
            return new Grid();
        }
    }
}