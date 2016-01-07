namespace OmniXaml.Tests.Classes.Templates
{
    using System.Collections.Generic;

    public class DummyDeferredLoader : IDeferredLoader
    {
        public object Load(IEnumerable<Instruction> nodes, IRuntimeTypeSource typeContext)
        {
            return new TemplateContent(nodes, typeContext);
        }
    }
}