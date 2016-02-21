namespace OmniXaml.Tests.Classes.Templates
{
    using System.Collections.Generic;
    using WpfLikeModel;

    public class TemplateContent
    {
        private readonly IEnumerable<Instruction> nodes;
        private readonly IRuntimeTypeSource typeSource;

        public TemplateContent(IEnumerable<Instruction> nodes, IRuntimeTypeSource typeSource)
        {
            this.nodes = nodes;
            this.typeSource = typeSource;
        }

        public Grid CreateContent()
        {
            return new Grid();
        }
    }
}