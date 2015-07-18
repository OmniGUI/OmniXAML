namespace OmniXaml.Wpf
{
    using System.Collections.Generic;

    public class DeferredObjectAssembler : IDeferredObjectAssembler
    {
        public object Load(IEnumerable<XamlNode> nodes, WiringContext context)
        {
            return new TemplateContent(nodes, context);
        }
    }
}