namespace OmniXaml.Wpf
{
    using System.Collections.Generic;

    public class TemplateContent
    {
        public IEnumerable<Instruction> Nodes { get; }
        public IRuntimeTypeSource TypeContext { get; }

        public TemplateContent(IEnumerable<Instruction> nodes, IRuntimeTypeSource typeContext)
        {
            Nodes = nodes;
            TypeContext = typeContext;
        }
    }
}