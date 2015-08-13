namespace OmniXaml.Wpf
{
    using System.Collections.Generic;

    public class TemplateContent
    {
        public IEnumerable<XamlInstruction> Nodes { get; }
        public IWiringContext Context { get; }

        public TemplateContent(IEnumerable<XamlInstruction> nodes, IWiringContext context)
        {
            Nodes = nodes;
            Context = context;
        }
    }
}