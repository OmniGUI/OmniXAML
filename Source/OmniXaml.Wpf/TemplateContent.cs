namespace OmniXaml.Wpf
{
    using System.Collections.Generic;

    public class TemplateContent
    {
        public IEnumerable<Instruction> Nodes { get; }
        public IRuntimeTypeSource TypeSource { get; }

        public TemplateContent(IEnumerable<Instruction> nodes, IRuntimeTypeSource typeSource)
        {
            Nodes = nodes;
            TypeSource = typeSource;
        }
    }
}