namespace OmniXaml.Tests.Classes.Templates
{
    using System.Collections.Generic;

    public class DummyDeferredLoader : IDeferredLoader
    {
        public object Load(IEnumerable<XamlInstruction> nodes, IWiringContext wiringContext)
        {
            return new TemplateContent(nodes, wiringContext);
        }
    }
}