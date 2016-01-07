namespace OmniXaml.Tests.Classes.Templates
{
    using System.Collections.Generic;
    using WpfLikeModel;

    public class TemplateContent
    {
        private readonly IEnumerable<Instruction> nodes;
        private readonly IRuntimeTypeSource typeContext;

        public TemplateContent(IEnumerable<Instruction> nodes, IRuntimeTypeSource typeContext)
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