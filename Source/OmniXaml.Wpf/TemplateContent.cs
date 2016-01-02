namespace OmniXaml.Wpf
{
    using System.Collections.Generic;

    public class TemplateContent
    {
        public IEnumerable<XamlInstruction> Nodes { get; }
        public ITypeContext TypeContext { get; }

        public TemplateContent(IEnumerable<XamlInstruction> nodes, ITypeContext typeContext)
        {
            Nodes = nodes;
            TypeContext = typeContext;
        }
    }
}