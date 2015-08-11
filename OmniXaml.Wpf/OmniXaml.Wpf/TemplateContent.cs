namespace OmniXaml.Wpf
{
    using System.Collections.Generic;

    public class TemplateContent
    {
        public IEnumerable<XamlInstruction> Nodes { get; }
        public WiringContext Context { get; }

        public TemplateContent(IEnumerable<XamlInstruction> nodes, WiringContext context)
        {
            Nodes = nodes;
            Context = context;
        }
    }
}