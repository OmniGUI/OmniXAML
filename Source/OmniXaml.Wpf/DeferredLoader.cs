namespace OmniXaml.Wpf
{
    using System.Collections.Generic;

    public class DeferredLoader : IDeferredLoader
    {
        public object Load(IEnumerable<Instruction> nodes, IRuntimeTypeSource typeContext)
        {
            return new TemplateContent(nodes, typeContext);
        }
    }
}