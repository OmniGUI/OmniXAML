namespace OmniXaml.Tests.Classes.Templates
{
    using System.Collections.Generic;

    public class DummyDeferredObjectAssembler : IDeferredObjectAssembler
    {
        public object Load(IEnumerable<XamlNode> nodes, WiringContext wiringContext)
        {
            return new TemplateContent(nodes, wiringContext);
        }
    }
}