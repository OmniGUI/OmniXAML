namespace OmniXaml.Tests.Classes
{
    using System.Collections.Generic;

    public class DummyDeferredObjectAssembler : IDeferredObjectAssembler
    {
        public object Load(IEnumerable<XamlNode> nodes)
        {
            return new TemplateContent(nodes);
        }
    }
}