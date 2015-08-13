namespace OmniXaml.Tests.Classes.Templates
{
    using System.Collections.Generic;

    public class DummyDeferredLoader : IDeferredLoader
    {
        public object Load(IEnumerable<XamlInstruction> nodes, IWiringContext IWiringContext)
        {
            return new TemplateContent(nodes, IWiringContext);
        }
    }
}