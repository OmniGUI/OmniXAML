namespace OmniXaml.Wpf
{
    using System.Collections.Generic;

    public class TemplateContent
    {
        public IEnumerable<XamlNode> Nodes { get; }
        public WiringContext Context { get; }

        public TemplateContent(IEnumerable<XamlNode> nodes, WiringContext context)
        {
            Nodes = nodes;
            Context = context;
        }
    }
}