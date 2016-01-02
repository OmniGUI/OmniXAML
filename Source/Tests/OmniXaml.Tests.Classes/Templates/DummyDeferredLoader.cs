namespace OmniXaml.Tests.Classes.Templates
{
    using System.Collections.Generic;

    public class DummyDeferredLoader : IDeferredLoader
    {
        public object Load(IEnumerable<XamlInstruction> nodes, ITypeContext typeContext)
        {
            return new TemplateContent(nodes, typeContext);
        }
    }
}